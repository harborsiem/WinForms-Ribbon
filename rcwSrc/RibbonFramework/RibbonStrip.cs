//*****************************************************************************
//
//  File:       RibbonStrip.cs
//
//  Contents:   Class which is used as a façade for the Windows Ribbon 
//              Framework. In charge of initialization and communication with 
//              the Windows Ribbon Framework.
//
//*****************************************************************************

using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.System.Com;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Main class for using the Windows Ribbon Framework in a .NET application
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RibbonStrip), "Ribbon16.bmp")]
    public sealed class RibbonStrip : Control
    {
        private const string NotInitialized = $"{nameof(RibbonStrip)} not initialized";
        private const string NotSupported = "Not supported by this Windows version";
        private static readonly Guid IIDGuidIUIContextualUI = typeof(IUIContextualUI).GUID;
        private static readonly Guid IIDGuidIUIRibbon = typeof(IUIRibbon).GUID;

        private static readonly EventKey s_EventRibbonEventException = new EventKey();
        private static readonly EventKey s_ViewCreatedKey = new EventKey();
        private static readonly EventKey s_ViewDestroyKey = new EventKey();
        private static readonly EventKey s_RibbonHeightKey = new EventKey();

        //@ Size for designer
        /// <summary>
        /// The default height is 147, but here we have to use default height - Top Non Client Area (31)
        /// </summary>
        protected override Size DefaultSize => new Size(base.DefaultSize.Width, 116);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="specified"></param>
        /// <inheritdoc cref="SetBoundsCore"/>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (Util.DesignMode)
                height = DefaultSize.Height;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private readonly EventSet _eventSet = new EventSet();
        private Dictionary<uint, RibbonStripItem> _mapRibbonStripItems = new Dictionary<uint, RibbonStripItem>();
        private IUIImageFromBitmap? _cpIUIImageFromBitmap;
        private UIApplication? _uIApplication;
        private QatSetting? _qatSetting;
        private MarkupHandler? _markupHandler;
        private ShortcutHandler? _shortcutHandler;
        private string? _markupResource;
        private Dictionary<ushort, MarkupResIds>? _allMarkupResIds;

        internal EventSet EventSet => _eventSet;

        /// <summary>
        /// Get EventLogger object which implements IUIEventLogger.
        /// Only available in Windows 8, 10, 11. Can be null.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventLogger? EventLogger { get; private set; }

        //private RibbonShortcutTable _ribbonShortcutTable;
        private string? _shortcutTableResourceName;

        /// <summary>
        /// is a reference to an embedded resource file
        /// in the application assembly. The (xml)-file contains
        /// shortcut keys.
        /// </summary>
        [Category("RibbonBehavior"), Description("The embedded resource (xml)-file contains shortcut keys.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(null)]
        public string? ShortcutTableResourceName
        {
            get { return _shortcutTableResourceName; }
            set
            {
                _shortcutTableResourceName = value;
                CheckInitialize();
            }
        }

        /// <summary>
        /// Initializes a new instance of the Ribbon
        /// </summary>
        public RibbonStrip()
        {
            base.Dock = DockStyle.Top;

            if (Util.DesignMode)
                return;

            this.SetStyle(ControlStyles.UserPaint, false);
            this.SetStyle(ControlStyles.Opaque, true);
        }

        #region Form Windows State change bug workaround

        private Form? _form;
        private FormWindowState _previousWindowState;
        private int _previousNormalHeight;
        private int _preserveHeight;

        private void Ribbon_ParentChanged(object sender, EventArgs e)
        {
            var parent = this.Parent;
            if (parent == null)
            {
                RegisterForm(null);
                return;
            }
            var form = parent as Form;
            if (form == null)
                throw new ApplicationException($"Parent of {nameof(RibbonStrip)} does not derive from Form class.");

            RegisterForm(form);
        }

        private void RegisterForm(Form? form)
        {
            if (_form != null)
                _form.SizeChanged -= new EventHandler(Form_SizeChanged);

            _form = form;

            if (_form == null)
                return;

            _form.SizeChanged += new EventHandler(Form_SizeChanged);
        }

#pragma warning disable CS8602

        private void Form_SizeChanged(object? sender, EventArgs e)
        {
            if (_previousWindowState != FormWindowState.Normal
                && _form.WindowState == FormWindowState.Normal
                && _previousNormalHeight != 0)
            {
                _preserveHeight = _previousNormalHeight;
                _form.BeginInvoke(new System.Windows.Forms.MethodInvoker(RestoreHeight));
            }

            if (_form.WindowState == FormWindowState.Normal)
                _previousNormalHeight = _form.Height;
            _previousWindowState = _form.WindowState;
        }

        private void RestoreHeight()
        {
            _form.Height = _preserveHeight;
        }
#pragma warning restore CS8602

        #endregion Form Windows State change bug workaround

        /// <summary>
        ///  Inheriting classes should override this method to find out when the handle has been created.
        ///  Call base.OnHandleCreated first.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            CheckInitialize();
            //if (Application.IsDarkModeEnabled)
            //{
            //    SetDarkModeRibbon(true);
            //}
        }

        /// <summary>
        ///  Inheriting classes should override this method to find out when the
        ///  handle is about to be destroyed.
        ///  Call base.OnHandleDestroyed last.
        /// </summary>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            DestroyFramework();
            base.OnHandleDestroyed(e);
        }

        /// <summary>
        ///  Call base.OnParentChanged last.
        /// </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            var parent = this.Parent;
            if (parent == null)
            {
                RegisterForm(null);
                return;
            }
            var form = parent as Form;
            if (form == null)
                throw new ApplicationException($"Parent of {nameof(RibbonStrip)} does not derive from Form class.");

            RegisterForm(form);

            base.OnParentChanged(e);
        }

        /// <summary>
        /// Only Dock.Top possible
        /// </summary>
        [DefaultValue(typeof(DockStyle), "Top")]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
            }
        }

        /// <summary>
        /// Don't use
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            //set
            //{
            //    base.Text = value;
            //}
        }

        /// <summary>
        /// A settings name like qat.xml is welcome
        /// </summary>
        [Category("RibbonBehavior"), Description("Stores the QuickAccess settings to the app specific LocalAppData.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(null)]
        public string? QatSettingsFile { get; set; }

        /// <summary>
        /// This is the Name parameter used for the UICC Compiler.
        /// Default value is APPLICATION or leave it empty.
        /// </summary>
        [Category("RibbonBehavior"), Description("This is the Name parameter used for the UICC Compiler. Default value is APPLICATION or leave it empty.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(null)]
        public string? ResourceIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// is a reference to an embedded resource file
        /// in the application assembly. The RibbonMarkup.ribbon file.
        /// </summary>
        [Category("RibbonBehavior"), Description("Is a reference to an embedded resource file in the application assembly. The RibbonMarkup.ribbon file.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string? MarkupResource
        {
            get { return _markupResource; }
            set
            {
                _markupResource = value;
                CheckInitialize();
            }
        }

        /// <summary>
        /// Is a reference to an embedded resource header file
        /// in the application assembly. The RibbonMarkup.h file.
        /// Don't use symbols for Keytip, LableTitle, ...
        /// In a later version we want to get informations from MarkupResource by parsing the header file for Id's and find strings and UIImage.
        /// </summary>
        [Category("RibbonBehavior"), Description("Is a reference to an embedded resource header file in the application assembly. The RibbonMarkup.h file.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(null)]
        public string? MarkupHeader { get; set; }

        /// <summary>
        /// Don't use
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr WindowHandle
        {
            get
            {
                var form = this.Parent as Form;
                return (form == null) ? IntPtr.Zero : form.Handle;
            }
        }

        private void CheckInitialize()
        {
            if (Util.DesignMode)
                return;

            if (IsInitialized)
                return;

            if (string.IsNullOrEmpty(MarkupResource))
                throw new ApplicationException($"'{nameof(MarkupResource)}' not set");
            //return;

            var form = this.Parent as Form;
            if (form == null)
                return;

            if (!form.IsHandleCreated)
                return;

            var assembly = form.GetType().Assembly;
            _markupHandler = new MarkupHandler(assembly, this);
            _allMarkupResIds = MarkupHandler.ParseHeader(MarkupHeader, assembly);

            InitFramework(_markupHandler.ResourceIdentifier, _markupHandler.MarkupDllHandle);

            if (Framework != null && _uIApplication != null)
            {
                _shortcutHandler = new ShortcutHandler(this, (IUICommandHandler)_uIApplication);
                _shortcutHandler.TryCreateShortcutTable(assembly);
            }
        }

        /// <summary>
        /// Draws only in Design mode
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ControlPaint.DrawContainerGrabHandle(e.Graphics, this.ClientRectangle);
        }

        /// <summary>
        /// Check if ribbon framework has been initialized
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInitialized
        {
            get
            {
                return (Framework != null);
            }
        }

        internal HRESULT SaveSettingsToStreamInternal(Stream stream)
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            HRESULT hr;
            if (uiRibbon != null)
            {
                StreamAdapter adapter = new StreamAdapter(stream);
                hr = uiRibbon.SaveSettingsToStream(adapter);
                Marshal.ReleaseComObject(uiRibbon);
                return hr;
            }
            return HRESULT.E_POINTER;
        }

        /// <summary>
        /// The SaveSettingsToStream method is useful for persisting ribbon state, such as Quick Access Toolbar (QAT) items, across application instances.
        /// </summary>
        /// <param name="stream"></param>
        public void SaveSettingsToStream(Stream stream)
        {
            if (!IsInitialized)
            {
                return;
            }

            SaveSettingsToStreamInternal(stream);
        }

        internal HRESULT LoadSettingsFromStreamInternal(Stream stream)
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            HRESULT hr;
            if (uiRibbon != null)
            {
                StreamAdapter adapter = new StreamAdapter(stream);
                hr = uiRibbon.LoadSettingsFromStream(adapter);
                Marshal.ReleaseComObject(uiRibbon);
                return hr;
            }
            return HRESULT.E_POINTER;
        }

        /// <summary>
        /// The LoadSettingsFromStream method is useful for persisting ribbon state, such as Quick Access Toolbar (QAT) items, across application instances.
        /// </summary>
        /// <param name="stream"></param>
        public void LoadSettingsFromStream(Stream stream)
        {
            if (!IsInitialized)
            {
                return;
            }

            LoadSettingsFromStreamInternal(stream);
        }

        /// <summary>
        /// Get ribbon framework object
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal IUIFramework? Framework { get; private set; }

        /// <summary>
        /// Create ribbon framework object
        /// </summary>
        /// <returns>ribbon framework object</returns>
        private static IUIFramework? CreateRibbonFramework()
        {
            try
            {
                return new UIRibbonFramework() as IUIFramework;
            }
            catch (COMException exception)
            {
                throw new PlatformNotSupportedException("The ribbon framework couldn't be found on this system.", exception);
            }
        }

        /// <summary>
        /// Create image-from-bitmap factory object
        /// </summary>
        /// <returns>image-from-bitmap factory object</returns>
        internal static IUIImageFromBitmap CreateImageFromBitmapFactory()
        {
            IUIImageFromBitmap cpIUIImageFromBitmap = (new UIRibbonImageFromBitmapFactory() as IUIImageFromBitmap)!;
            return cpIUIImageFromBitmap;
        }

        internal IUIImageFromBitmap? CpIUIImageFromBitmap => _cpIUIImageFromBitmap;

        /// <summary>
        /// Initialize ribbon framework
        /// </summary>
        /// <param name="resourceIdentifier">Identifier of the ribbon resource</param>
        /// <param name="hInstance">Pointer to HINSTANCE of module where we can find ribbon resource</param>
        private unsafe void InitFramework(string? resourceIdentifier, HMODULE hInstance)
        {
            HRESULT hr;
            // create ribbon framework object
            Framework = CreateRibbonFramework();
            _cpIUIImageFromBitmap = CreateImageFromBitmapFactory();

            if (Framework == null)
                return;

            // create ribbon application object
            _uIApplication = new UIApplication(this);
            _qatSetting = new QatSetting(this, QatSettingsFile);

            // init ribbon framework
            HWND hwnd = new HWND(this.WindowHandle);
            hr = Framework.Initialize(hwnd, (IUIApplication)_uIApplication);
            hr.ThrowOnFailure();

            // load ribbon ui
            fixed (char* resourceIdentifierLocal = resourceIdentifier)
            {
                hr = Framework.LoadUI(hInstance, resourceIdentifierLocal);
            }

            hr.ThrowOnFailure();

            if (!(Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor <= 1 || Environment.OSVersion.Version.Major < 6))
            {
                IUIEventingManager? cpIUIEventingManager = Framework as IUIEventingManager;
                if (cpIUIEventingManager != null)
                {
                    EventLogger = new EventLogger(this);
                    cpIUIEventingManager = null;
                }
            }
        }

        /// <summary>
        /// Destroy ribbon framework
        /// </summary>
        private void DestroyFramework()
        {
            if (Framework != null)
            {
                EventLogger?.Destroy();

                // destroy ribbon framework
                Framework.Destroy();
                int refCount = Marshal.ReleaseComObject(Framework);
                Debug.WriteLine("RefCount Framework: " +  refCount);

                // remove reference to framework object
                Framework = null;
            }

            // Unregister event handlers
            RegisterForm(null);

            _markupHandler?.Dispose();
            _shortcutHandler?.Dispose();

            if (_cpIUIImageFromBitmap != null)
            {
                // remove reference to imageFromBitmap object
                Marshal.ReleaseComObject(_cpIUIImageFromBitmap);
                _cpIUIImageFromBitmap = null;
            }

            if (_uIApplication != null)
            {
                // remove references to ribbon items
                _mapRibbonStripItems.Clear();
                // remove reference to application object
                IUIApplication uiApplication = (IUIApplication)_uIApplication;
                uiApplication = null!;
                IUICommandHandler uiCommandHandler = (IUICommandHandler)_uIApplication;
                uiCommandHandler = null!;
            }
        }

        /// <summary>
        /// Get ribbon background color
        /// </summary>
        public unsafe UI_HSBCOLOR GetBackgroundColor()
        {
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            PROPVARIANT propvar;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            propertyStore.GetValue(RibbonProperties.GlobalBackgroundColor, out propvar);
            uint background = (uint)propvar; //PropVariantToUInt32
            UI_HSBCOLOR retResult = (UI_HSBCOLOR)background;
            propertyStore = null!;
            return retResult;
        }

        /// <summary>
        /// Set ribbon background color
        /// </summary>
        public void SetBackgroundColor(UI_HSBCOLOR value)
        {
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            uint color = value.Value;
            PROPVARIANT propvar = (PROPVARIANT)color; //InitPropVariantFromUInt32

            // set ribbon color
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            propertyStore.SetValue(RibbonProperties.GlobalBackgroundColor, propvar);

            propertyStore.Commit();
            propertyStore = null!;
        }

        /// <summary>
        /// Get ribbon highlight color
        /// </summary>
        public unsafe UI_HSBCOLOR GetHighlightColor()
        {
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            PROPVARIANT propvar;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            propertyStore.GetValue(RibbonProperties.GlobalHighlightColor, out propvar);
            uint highlight = (uint)propvar; //PropVariantToUInt32
            UI_HSBCOLOR retResult = (UI_HSBCOLOR)highlight;
            propertyStore = null!;
            return retResult;
        }

        /// <summary>
        /// Set ribbon highlight color
        /// </summary>
        public void SetHighlightColor(UI_HSBCOLOR value)
        {
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            uint color = value.Value;
            PROPVARIANT propvar = (PROPVARIANT)color; //InitPropVariantFromUInt32

            // set ribbon color
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            propertyStore.SetValue(RibbonProperties.GlobalHighlightColor, propvar);

            propertyStore.Commit();
            propertyStore = null!;
        }


        /// <summary>
        /// Get ribbon text color
        /// </summary>
        public unsafe UI_HSBCOLOR GetTextColor()
        {
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            PROPVARIANT propvar;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            propertyStore.GetValue(RibbonProperties.GlobalTextColor, out propvar);
            uint text = (uint)propvar; //PropVariantToUInt32
            UI_HSBCOLOR retResult = (UI_HSBCOLOR)text;
            propertyStore = null!;
            return retResult;
        }

        /// <summary>
        /// Set ribbon text color
        /// </summary>
        public void SetTextColor(UI_HSBCOLOR value)
        {
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            uint color = value.Value;
            PROPVARIANT propvar = (PROPVARIANT)color; //InitPropVariantFromUInt32

            // set ribbon color
            HRESULT hr;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            hr = propertyStore.SetValue(RibbonProperties.GlobalTextColor, propvar);

            propertyStore.Commit();
            propertyStore = null!;
        }

        /// <summary>
        /// Set current application modes
        /// </summary>
        /// <param name="modesArray">array of modes to set</param>
        /// <remarks>Unlisted modes will be unset</remarks>
        public void SetModes(params byte[] modesArray)
        {
            if (modesArray == null || modesArray.Length == 0)
                throw new ArgumentNullException(nameof(modesArray));
            // check that ribbon is initialized
            if (Framework == null)
            {
                return;
            }

            // calculate compact modes value
            int compactModes = 0;
            for (int i = 0; i < modesArray.Length; ++i)
            {
                if (modesArray[i] >= 32)
                {
                    throw new ArgumentException("Modes should range between 0 to 31.");
                }

                compactModes |= (1 << modesArray[i]);
            }

            // set modes
            Framework.SetModes(compactModes);
        }

        /// <summary>
        /// Shows a predefined context popup in a specific location
        /// </summary>
        /// <param name="contextPopupID">commandId for the context popup</param>
        /// <param name="x">X in screen coordinates</param>
        /// <param name="y">Y in screen coordinates</param>
        public unsafe void ShowContextPopup(uint contextPopupID, int x, int y)
        {
            // check that ribbon is initialized
            if (Framework == null)
            {
                return;
            }

            HRESULT hr;
            void* voidPtr;
            object contextualUI;

            hr = Framework.GetView(contextPopupID, IIDGuidIUIContextualUI, out voidPtr);
            if (hr.Succeeded)
            {
                if ((IntPtr)voidPtr != IntPtr.Zero)
                {
                    contextualUI = Marshal.GetObjectForIUnknown((IntPtr)voidPtr);
                    IUIContextualUI? cpIUIContextualUI = contextualUI as IUIContextualUI;
                    if (cpIUIContextualUI != null)
                    {
                        cpIUIContextualUI.ShowAtLocation(x, y);
                        Marshal.ReleaseComObject(cpIUIContextualUI);
                    }
                }
            }
            else
            {
                Marshal.ThrowExceptionForHR((int)hr);
            }
        }

        /// <summary>
        /// Get color of application button
        /// </summary>
        public unsafe UI_HSBCOLOR GetApplicationButtonColor()
        {
            // check that ribbon is initialized
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            HRESULT hr;
            PROPVARIANT propvar;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            hr = propertyStore.GetValue(RibbonProperties.ApplicationButtonColor, out propvar);
            propertyStore = null!;
            if (hr.Succeeded)
            {
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_HSBCOLOR retResult = (UI_HSBCOLOR)result;
                return retResult;
            }
            throw new NotSupportedException(NotSupported);
        }

        /// <summary>
        /// Set color of application button
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool SetApplicationButtonColor(UI_HSBCOLOR value)
        {
            // check that ribbon is initialized
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }
            uint hsb = value.Value;
            PROPVARIANT propvar = (PROPVARIANT)hsb; //InitPropVariantFromUInt32
            HRESULT hr;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            hr = propertyStore.SetValue(RibbonProperties.ApplicationButtonColor, propvar);
            if (hr.Succeeded)
            {
                hr = propertyStore.Commit();
                propertyStore = null!;
                return hr.Succeeded;
            }
            propertyStore = null!;
            return false;
        }

        /// <summary>
        /// Get the DarkModeRibbon PropertyKey
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public unsafe bool GetDarkModeRibbon()
        {
            // check that ribbon is initialized
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            HRESULT hr;
            PROPVARIANT propvar;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            hr = propertyStore.GetValue(RibbonProperties.DarkModeRibbon, out propvar);
            propertyStore = null!;
            if (hr.Succeeded)
            {
                bool result = (bool)propvar; //PropVariantToBoolean
                return result;
            }
            return false;
        }

        /// <summary>
        /// Set the DarkModeRibbon PropertyKey
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool SetDarkModeRibbon(bool value)
        {
            // check that ribbon is initialized
            if (Framework == null)
            {
                throw new InvalidOperationException(NotInitialized);
            }

            PROPVARIANT propvar;
            propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
            HRESULT hr;
            IPropertyStore propertyStore = (IPropertyStore)Framework;
            hr = propertyStore.SetValue(RibbonProperties.DarkModeRibbon, propvar);
            if (hr.Succeeded)
            {
                hr = propertyStore.Commit();
                propertyStore = null!;
                return hr.Succeeded;
            }
            propertyStore = null!;
            return false;
        }


#pragma warning disable CS8602 //_application has null check by Initialized
        /// <summary>
        /// Specifies whether the ribbon is in a collapsed or expanded state
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Minimized
        {
            get
            {
                // check that ribbon is initialized
                if (Framework == null)
                {
                    return false;
                }
                return GetMinimized();
            }
            set
            {
                // check that ribbon is initialized
                if (Framework == null)
                {
                    return;
                }
                SetMinimized(value);
            }
        }

        /// <summary>
        /// Specifies whether the ribbon user interface (UI) is in a visible or hidden state
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Viewable
        {
            get
            {
                // check that ribbon is initialized
                if (Framework == null)
                {
                    return false;
                }
                return GetViewable();
            }
            set
            {
                // check that ribbon is initialized
                if (Framework == null)
                {
                    return;
                }
                SetViewable(value);
            }
        }

        /// <summary>
        /// Specifies whether the quick access toolbar is docked at the top or at the bottom
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ControlDock QuickAccessToolbarDock
        {
            get
            {
                // check that ribbon is initialized
                if (Framework == null)
                {
                    return (ControlDock)UI_CONTROLDOCK.UI_CONTROLDOCK_TOP;
                }
                return (ControlDock)GetQuickAccessToolbarDock();
            }
            set
            {
                // check that ribbon is initialized
                if (Framework == null)
                {
                    return;
                }
                SetQuickAccessToolbarDock((UI_CONTROLDOCK)value);
            }
        }
#pragma warning restore CS8602

        /// <summary>
        /// Adds a ribbon control to the internal map
        /// </summary>
        /// <param name="ribbonControl">ribbon control to add</param>
        internal void AddRibbonControl(RibbonStripItem ribbonControl)
        {
            _mapRibbonStripItems.Add(ribbonControl.CommandId, ribbonControl);
        }

        internal bool OnRibbonEventException(object sender, ThreadExceptionEventArgs args)
        {
            EventHandler<ThreadExceptionEventArgs>? eh = Events[s_EventRibbonEventException] as EventHandler<ThreadExceptionEventArgs>;
            if (eh != null)
            {
                eh(sender, args);
                return true;
            }
            return false;
        }

        /// <summary>
        /// User can handle untrapped Exceptions in the other events of the Ribbon
        /// </summary>
        [Category("RibbonEvent"), Description("User can handle untrapped Exceptions in the other events of the Ribbon")]
        public event EventHandler<ThreadExceptionEventArgs>? RibbonEventException
        {
            add
            {
                Events.AddHandler(s_EventRibbonEventException, value);
            }
            remove
            {
                Events.RemoveHandler(s_EventRibbonEventException, value);
            }
        }

        /// <summary>
        /// Event fires when the View is created
        /// </summary>
        [Category("RibbonEvent"), Description("Event fires when the View is created")]
        public event EventHandler? ViewCreated
        {
            add
            {
                EventSet.Add(s_ViewCreatedKey, value);
                //Events.AddHandler(s_ViewCreatedKey, value);
            }
            remove
            {
                EventSet.Remove(s_ViewCreatedKey, value);
                //Events.RemoveHandler(s_ViewCreatedKey, value);
            }
        }

        internal void OnViewCreated()
        {
            if (_allMarkupResIds != null)
            {
                foreach (var pair in _mapRibbonStripItems)
                {
                    RibbonStripItem item = pair.Value;
                    if (_allMarkupResIds.TryGetValue((ushort)item.CommandId, out var resIds))
                    {
                        item.ResourceIds = resIds;
                    }
                }
            }
            _qatSetting!.Load();

            EventSet.Raise(s_ViewCreatedKey, this, EventArgs.Empty);
            //EventHandler? eh = Events[s_ViewCreatedKey] as EventHandler;
            //if (eh != null)
            //    eh(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event fires when the View is in destroy
        /// </summary>
        [Category("RibbonEvent"), Description("Event fires when the View is in destroy")]
        public event EventHandler? ViewDestroy
        {
            add
            {
                EventSet.Add(s_ViewDestroyKey, value);
                //Events.AddHandler(s_ViewDestroyKey, value);
            }
            remove
            {
                EventSet.Remove(s_ViewDestroyKey, value);
                //Events.RemoveHandler(s_ViewDestroyKey, value);
            }
        }

        internal void OnViewDestroy()
        {
            _qatSetting!.Save();
            EventSet.Raise(s_ViewDestroyKey, this, EventArgs.Empty);
            //EventHandler? eh = Events[s_ViewDestroyKey] as EventHandler;
            //if (eh != null)
            //    eh(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event fires when the Ribbon height changed
        /// </summary>
        [Category("RibbonEvent"), Description("Event fires when the Ribbon height changed")]
        public event EventHandler? RibbonHeightChanged
        {
            add
            {
                EventSet.Add(s_RibbonHeightKey, value);
                //Events.AddHandler(s_RibbonHeightKey, value);
            }
            remove
            {
                EventSet.Remove(s_RibbonHeightKey, value);
                //Events.RemoveHandler(s_RibbonHeightKey, value);
            }
        }

        internal void OnRibbonHeightChanged()
        {
            EventSet.Raise(s_RibbonHeightKey, this, EventArgs.Empty);
            //EventHandler? eh = Events[s_RibbonHeightKey] as EventHandler;
            //if (eh != null)
            //    eh(this, EventArgs.Empty);
        }

        internal HMODULE MarkupHandleInternal
        {
            get
            {
                if (_markupHandler == null)
                    return HMODULE.Null;
                return _markupHandler.MarkupDllHandle;
            }
        }

        /// <summary>
        /// Returns the Dll HMODULE Handle for the culture specific RibbonMarkup.ribbon file.
        /// One can use this handle to get Strings, Bitmaps.
        /// </summary>
        /// <returns>The Dll Handle of the Ribbon resource</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr MarkupHandle
        {
            get
            {
                return MarkupHandleInternal;
            }
        }

        /// <summary>
        /// Get the control by commandId
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns>IRibbonControl</returns>
        /// <exception cref="ArgumentException"></exception>
        public IRibbonControl GetRibbonControlById(uint commandId)
        {
            bool result = _mapRibbonStripItems.TryGetValue(commandId, out RibbonStripItem? item);
            if (result)
                return item!;
            throw new ArgumentException("Not found", nameof(commandId));
        }

        internal bool TryGetRibbonControlById(uint commandId, out RibbonStripItem? item)
        {
            bool result = _mapRibbonStripItems.TryGetValue(commandId, out item);
            return result;
        }

        /// <summary>
        /// Load a string from Ribbon resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal unsafe string? LoadString(uint id)
        {
            if (MarkupHandle == HMODULE.Null)
                return null;
            Span<char> stackBuffer = stackalloc char[512];
            int length;
            fixed (char* bufferlocal = stackBuffer)
                length = PInvoke.LoadString(MarkupHandleInternal, id, bufferlocal, stackBuffer.Length);
            if (length == 0)
                return string.Empty;
            string result;
            if (length < stackBuffer.Length)
            {
                fixed (char* bufferlocal = stackBuffer)
                    result = new string(bufferlocal, 0, length);
                return result; // new string(stackBuffer[..length]);
            }
            char[] buffer = ArrayPool<char>.Shared.Rent(length + 1);
            fixed (char* bufferlocal = buffer)
                length = PInvoke.LoadString(MarkupHandleInternal, id, bufferlocal, buffer.Length);
            result = new string(buffer, 0, length);
            ArrayPool<char>.Shared.Return(buffer);
            return result;
        }

        /// <summary>
        /// Specifies whether the ribbon is in a collapsed or expanded state
        /// </summary>
        private unsafe bool GetMinimized()
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            if (uiRibbon != null)
            {
                PROPVARIANT propvar;
                IPropertyStore propstore = (IPropertyStore)uiRibbon;
                HRESULT hr = propstore.GetValue(RibbonProperties.Minimized, out propvar);
                propstore = null!;
                Marshal.ReleaseComObject(uiRibbon);
                bool result = (bool)propvar; //PropVariantToBoolean
                return result;
            }
            else
                return false;
        }

        /// <summary>
        /// Specifies whether the ribbon is in a collapsed or expanded state
        /// </summary>
        private void SetMinimized(bool value)
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            if (uiRibbon != null)
            {
                PROPVARIANT propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
                IPropertyStore propstore = (IPropertyStore)uiRibbon;
                HRESULT hr = propstore.SetValue(RibbonProperties.Minimized, propvar);
                hr = propstore.Commit();
                propstore = null!;
                Marshal.ReleaseComObject(uiRibbon);
            }
        }

        /// <summary>
        /// Specifies whether the ribbon user interface (UI) is in a visible or hidden state
        /// </summary>
        private unsafe bool GetViewable()
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            if (uiRibbon != null)
            {
                PROPVARIANT propvar;
                IPropertyStore propstore = (IPropertyStore)uiRibbon;
                HRESULT hr = propstore.GetValue(RibbonProperties.Viewable, out propvar);
                propstore = null!;
                Marshal.ReleaseComObject(uiRibbon);
                bool result = (bool)propvar; //PropVariantToBoolean
                return result;
            }
            else
                return false;
        }

        /// <summary>
        /// Specifies whether the ribbon user interface (UI) is in a visible or hidden state
        /// </summary>
        private void SetViewable(bool value)
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            if (uiRibbon != null)
            {
                PROPVARIANT propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
                IPropertyStore propstore = (IPropertyStore)uiRibbon;
                HRESULT hr = propstore.SetValue(RibbonProperties.Viewable, propvar);
                hr = propstore.Commit();
                propstore = null!;
                Marshal.ReleaseComObject(uiRibbon);
            }
        }

        /// <summary>
        /// Specifies whether the quick access toolbar is docked at the top or at the bottom
        /// </summary>
        private unsafe UI_CONTROLDOCK GetQuickAccessToolbarDock()
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            if (uiRibbon != null)
            {
                PROPVARIANT propvar;
                IPropertyStore propstore = (IPropertyStore)uiRibbon;
                HRESULT hr = propstore.GetValue(RibbonProperties.QuickAccessToolbarDock, out propvar);
                propstore = null!;
                Marshal.ReleaseComObject(uiRibbon);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_CONTROLDOCK retResult = (UI_CONTROLDOCK)result;
                return retResult;
            }
            else
                return UI_CONTROLDOCK.UI_CONTROLDOCK_TOP;
        }

        /// <summary>
        /// Specifies whether the quick access toolbar is docked at the top or at the bottom
        /// </summary>
        private void SetQuickAccessToolbarDock(UI_CONTROLDOCK value)
        {
            IUIRibbon uiRibbon = GetIUIRibbon();
            if (uiRibbon != null)
            {
                PROPVARIANT propvar = (PROPVARIANT)(uint)value; //InitPropVariantFromUInt32
                IPropertyStore propstore = (IPropertyStore)uiRibbon;
                HRESULT hr = propstore.SetValue(RibbonProperties.QuickAccessToolbarDock, propvar);
                hr = propstore.Commit();
                propstore = null!;
                Marshal.ReleaseComObject(uiRibbon);
            }
        }

        private unsafe IUIRibbon GetIUIRibbon()
        {
            HRESULT hr;
            void* voidPtr;
            object uiRibbon;

            hr = Framework.GetView(0, IIDGuidIUIRibbon, out voidPtr);
            if (hr.Succeeded)
            {
                if ((IntPtr)voidPtr != IntPtr.Zero)
                {
                    uiRibbon = Marshal.GetObjectForIUnknown((IntPtr)voidPtr);
                    return (uiRibbon as IUIRibbon)!;
                }
            }
            return null!;
        }

        ///// <summary>
        ///// Get a Bitmap from an IUIImage
        ///// </summary>
        ///// <param name="uiImage"></param>
        ///// <returns></returns>
        //public static unsafe Bitmap GetBitmap(IUIImage uiImage)
        //{
        //    if (uiImage == null)
        //        throw new ArgumentNullException(nameof(uiImage));
        //    HBITMAP hBitmap;
        //    uiImage.GetBitmap(&hBitmap);
        //    // Create the BITMAP structure and get info from our nativeHBitmap
        //    //this is a workaround because GDI+ did it not correct for 32 bit Bitmaps
        //    BITMAP bitmapStruct = new BITMAP();
        //    int bitmapSize = Marshal.SizeOf(bitmapStruct);
        //    int size = PInvoke.GetObject(hBitmap, bitmapSize, &bitmapStruct);
        //    //if (size != bitmapSize)
        //    //    return null;
        //    Bitmap managedBitmap;
        //    if (bitmapStruct.bmBitsPixel == 32)
        //    {
        //        // Create the managed bitmap using the pointer to the pixel data of the native HBitmap
        //        managedBitmap = new Bitmap(
        //            bitmapStruct.bmWidth, bitmapStruct.bmHeight, bitmapStruct.bmWidthBytes, PixelFormat.Format32bppArgb, (IntPtr)bitmapStruct.bmBits);
        //        if (bitmapStruct.bmHeight > 0)
        //            managedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
        //    }
        //    else
        //    {
        //        managedBitmap = Bitmap.FromHbitmap(hBitmap);
        //    }
        //    return managedBitmap;
        //}

        //public static unsafe Bitmap? GetBitmap1(IUIImage uiImage)
        //{
        //    if (uiImage == null)
        //        throw new ArgumentNullException(nameof(uiImage));
        //    HBITMAP hBitmap;
        //    uiImage.GetBitmap(&hBitmap);
        //    Bitmap? managedBitmap = null;
        //    IWICImagingFactory? factory = new PInvoke.WICImagingFactory() as IWICImagingFactory;
        //    if (factory != null)
        //    {
        //        factory.CreateBitmapFromHBITMAP(hBitmap, HPALETTE.Null, WICBitmapAlphaChannelOption.WICBitmapUseAlpha, out IWICBitmap wicBitmap);
        //        wicBitmap.GetSize(out uint width1, out uint height1);
        //        WICRect rect = new WICRect() { X = 0, Y = 0, Width = (int)width1, Height = (int)height1 };
        //        wicBitmap.Lock(null, (uint)WICBitmapLockFlags.WICBitmapLockRead, out IWICBitmapLock ppILock);
        //        ppILock.GetStride(out uint stride);
        //        ppILock.GetPixelFormat(out Guid format);
        //        ppILock.GetSize(out uint width, out uint height);
        //        byte* pData;
        //        ppILock.GetDataPointer(out uint bufferSize, &pData);
        //        factory.CreateComponentInfo(format, out IWICComponentInfo ppIInfo);
        //        IWICPixelFormatInfo formatInfo = ppIInfo as IWICPixelFormatInfo;
        //        IWICPixelFormatInfo2 formatInfo2 = ppIInfo as IWICPixelFormatInfo2;
        //        if (formatInfo != null)
        //        {
        //            formatInfo.GetBitsPerPixel(out uint bpp);
        //            formatInfo.GetChannelCount(out uint channelCount);
        //            BOOL pfSupportsTransparency = false;
        //            if (formatInfo2 != null)
        //            {
        //                formatInfo2.SupportsTransparency(out pfSupportsTransparency);
        //            }
        //            //formatInfo.GetChannelMask((200, friendly, out uint chActual);
        //            managedBitmap = new Bitmap((int)width, (int)height, (int)stride, PixelFormat.Format32bppArgb, (IntPtr)pData);
        //            //PixelFormat pf = PixelFormat.
        //        }

        //    }
        //    return managedBitmap;
        //}

        //public static unsafe Bitmap? GetBitmap2(HBITMAP hBitmap)
        //{
        //    //if (uiImage == null)
        //    //    throw new ArgumentNullException(nameof(uiImage));
        //    //HBITMAP hBitmap;
        //    //uiImage.GetBitmap(&hBitmap);
        //    Bitmap? managedBitmap = null;
        //    IWICImagingFactory? factory = new PInvoke.WICImagingFactory() as IWICImagingFactory;
        //    if (factory != null)
        //    {
        //        factory.CreateBitmapFromHBITMAP(hBitmap, HPALETTE.Null, WICBitmapAlphaChannelOption.WICBitmapUsePremultipliedAlpha, out IWICBitmap wicBitmap);
        //        wicBitmap.GetSize(out uint width1, out uint height1);
        //        WICRect rect = new WICRect() { X = 0, Y = 0, Width = (int)width1, Height = (int)height1 };
        //        wicBitmap.Lock(null, (uint)WICBitmapLockFlags.WICBitmapLockRead, out IWICBitmapLock ppILock);
        //        ppILock.GetStride(out uint stride);
        //        ppILock.GetPixelFormat(out Guid format);
        //        ppILock.GetSize(out uint width, out uint height);
        //        byte* pData;
        //        ppILock.GetDataPointer(out uint bufferSize, &pData);
        //        factory.CreateComponentInfo(format, out IWICComponentInfo ppIInfo);
        //        IWICPixelFormatInfo formatInfo = ppIInfo as IWICPixelFormatInfo;
        //        IWICPixelFormatInfo2 formatInfo2 = ppIInfo as IWICPixelFormatInfo2;
        //        if (formatInfo != null)
        //        {
        //            formatInfo.GetBitsPerPixel(out uint bpp);
        //            formatInfo.GetChannelCount(out uint channelCount);
        //            BOOL pfSupportsTransparency = false;
        //            if (formatInfo2 != null)
        //            {
        //                formatInfo2.SupportsTransparency(out pfSupportsTransparency);
        //            }
        //            string friendlyName = null;
        //            char[] chars = new char[200];
        //            fixed (char* charsLocal = chars)
        //            {
        //                PWSTR friendly = new PWSTR(charsLocal);
        //                formatInfo.GetFriendlyName(200, friendly, out uint chActual);
        //                if (chActual > 0)
        //                    friendlyName = new string(charsLocal, 0, (int)chActual - 1);
        //            }
        //            PixelFormat pf = GetPixelFormat(format);
        //            managedBitmap = new Bitmap((int)width, (int)height, (int)stride, pf, (IntPtr)pData);
        //        }

        //    }
        //    return managedBitmap;
        //}
        ////Format1bppIndexed  DEFINE_GUID(GUID_WICPixelFormat1bppIndexed, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x01);
        ////Format4bppIndexed  DEFINE_GUID(GUID_WICPixelFormat4bppIndexed, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x03);
        ////Format8bppIndexed  DEFINE_GUID(GUID_WICPixelFormat8bppIndexed, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x04);
        ////Format16bppArgb1555  DEFINE_GUID(GUID_WICPixelFormat16bppBGRA5551, 0x05ec7c2b, 0xf1e6, 0x4961, 0xad, 0x46, 0xe1, 0xcc, 0x81, 0x0a, 0x87, 0xd2);
        ////Format16bppGrayScale  DEFINE_GUID(GUID_WICPixelFormat16bppGray,   0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x0b);
        ////Format16bppRgb555  DEFINE_GUID(GUID_WICPixelFormat16bppBGR555, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x09);
        ////Format16bppRgb565  DEFINE_GUID(GUID_WICPixelFormat16bppBGR565, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x0a);
        ////Format24bppRgb  DEFINE_GUID(GUID_WICPixelFormat24bppBGR, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x0c);
        ////Format32bppRgb  DEFINE_GUID(GUID_WICPixelFormat32bppBGR,   0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x0e);
        ////Format32bppArgb  DEFINE_GUID(GUID_WICPixelFormat32bppBGRA,  0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x0f);
        ////Format32bppPArgb  DEFINE_GUID(GUID_WICPixelFormat32bppPBGRA, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x10);
        ////Format48bppRgb  DEFINE_GUID(GUID_WICPixelFormat48bppBGR, 0xe605a384, 0xb468, 0x46ce, 0xbb, 0x2e, 0x36, 0xf1, 0x80, 0xe6, 0x43, 0x13);
        ////Format64bppArgb  DEFINE_GUID(GUID_WICPixelFormat64bppRGBA,  0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x16);
        ////DEFINE_GUID(GUID_WICPixelFormat64bppBGRA,  0x1562ff7c, 0xd352, 0x46f9, 0x97, 0x9e, 0x42, 0x97, 0x6b, 0x79, 0x22, 0x46);

        ////Format64bppPArgb  DEFINE_GUID(GUID_WICPixelFormat64bppPRGBA, 0x6fddc324, 0x4e03, 0x4bfe, 0xb1, 0x85, 0x3d, 0x77, 0x76, 0x8d, 0xc9, 0x17);
        ////DEFINE_GUID(GUID_WICPixelFormat64bppPBGRA, 0x8c518e8e, 0xa4ec, 0x468b, 0xae, 0x70, 0xc9, 0xa3, 0x5a, 0x9c, 0x55, 0x30);

        //private static PixelFormat GetPixelFormat(in Guid guid)
        //{
        //    if (PInvoke.GUID_WICPixelFormat32bppBGR == guid)
        //        return PixelFormat.Format32bppRgb;
        //    if (PInvoke.GUID_WICPixelFormat32bppBGRA == guid)
        //        return PixelFormat.Format32bppArgb;
        //    if (PInvoke.GUID_WICPixelFormat32bppPBGRA == guid)
        //        return PixelFormat.Format32bppPArgb;
        //    if (PInvoke.GUID_WICPixelFormat1bppIndexed == guid)
        //        return PixelFormat.Format1bppIndexed;
        //    if (PInvoke.GUID_WICPixelFormat4bppIndexed == guid)
        //        return PixelFormat.Format4bppIndexed;
        //    if (PInvoke.GUID_WICPixelFormat8bppIndexed == guid)
        //        return PixelFormat.Format8bppIndexed;
        //    if (PInvoke.GUID_WICPixelFormat24bppBGR == guid)
        //        return PixelFormat.Format24bppRgb;
        //    if (PInvoke.GUID_WICPixelFormat16bppGray == guid)
        //        return PixelFormat.Format16bppGrayScale;
        //    if (PInvoke.GUID_WICPixelFormat16bppBGR555 == guid)
        //        return PixelFormat.Format16bppRgb555;
        //    if (PInvoke.GUID_WICPixelFormat16bppBGR565 == guid)
        //        return PixelFormat.Format16bppRgb565;
        //    if (PInvoke.GUID_WICPixelFormat48bppBGR == guid)
        //        return PixelFormat.Format48bppRgb;
        //    if (PInvoke.GUID_WICPixelFormat64bppBGRA == guid)
        //        return PixelFormat.Format64bppArgb;
        //    if (PInvoke.GUID_WICPixelFormat64bppPBGRA == guid)
        //        return PixelFormat.Format64bppPArgb;
        //    if (PInvoke.GUID_WICPixelFormat16bppBGRA5551 == guid)
        //        return PixelFormat.Format16bppArgb1555;
        //    return PixelFormat.Undefined;
        //}

        //public unsafe IUIImage Dummy(string filename)
        //{
        //    HBITMAP hbm = HBITMAP.Null;
        //    HRESULT hr;
        //    PCWSTR pCWSTR = new PCWSTR();
        //    string path = Path.GetFullPath(filename);
        //    //only Bitmap files possible, no V5 Bitmap
        //    hbm = (HBITMAP)(IntPtr)PInvoke.LoadImage(PInvoke.GetModuleHandle(pCWSTR), path, GDI_IMAGE_TYPE.IMAGE_BITMAP, 0, 0, IMAGE_FLAGS.LR_CREATEDIBSECTION | IMAGE_FLAGS.LR_LOADFROMFILE);
        //    if (hbm == HBITMAP.Null)
        //    {
        //        hr = PInvoke.HRESULT_FROM_WIN32((WIN32_ERROR)Marshal.GetLastWin32Error());
        //        return null;
        //    }
        //    IUIImage uiImage;
        //    _imageFromBitmap.CreateImage(hbm, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out uiImage);

        //    return uiImage;

        //}

        //public unsafe void dummy2()
        //{
        //    int cx, cy;
        //    cx = cy = 16;

        //    BITMAPINFO bmi = new BITMAPINFO();
        //    bmi.bmiHeader = new BITMAPINFOHEADER();
        //    bmi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
        //    bmi.bmiHeader.biWidth = cx;
        //    bmi.bmiHeader.biHeight = cy;
        //    bmi.bmiHeader.biPlanes = 1;
        //    bmi.bmiHeader.biBitCount = 32;
        //    bmi.bmiHeader.biCompression = 0x0;

        //    void* pBits;
        //    HBITMAP hBitmap = PInvoke.CreateDIBSection(HDC.Null, bmi, DIB_USAGE.DIB_RGB_COLORS, out pBits, HANDLE.Null, 0);

        //    if (hBitmap != HBITMAP.Null)
        //    {
        //        //unsafe
        //        {
        //            int* ppBits = (int*)pBits;

        //            for (int y = 0; y < cy; y++)
        //            {
        //                for (int x = 0; x < cx; x++)
        //                {
        //                    int bAlpha = x * x * 255 / cx / cx;
        //                    int dw = (bAlpha << 24) | (bAlpha << 16) | bAlpha;

        //                    ppBits[y * cx + x] = dw;
        //                }
        //            }
        //        }

        //    }
        //}
    }
}
