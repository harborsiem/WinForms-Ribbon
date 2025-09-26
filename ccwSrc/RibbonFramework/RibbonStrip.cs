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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Buffers;
using System.IO;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Main class for using the Windows Ribbon Framework in a .NET application
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RibbonStrip), "Ribbon16.bmp")]
    public sealed unsafe class RibbonStrip : Control
    {
        private const string NotInitialized = $"{nameof(RibbonStrip)} not initialized";
        private const string NotSupported = "Not supported by this Windows version";

        private static readonly EventKey s_RibbonEventExceptionKey = new EventKey();
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
        private IUIFramework* _cpIUIFramework;
        private IUIImageFromBitmap* _cpIUIImageFromBitmap;
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
        /// Initializes a new instance of the RibbonStrip
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
                _shortcutHandler = new ShortcutHandler(this, (IUICommandHandler.Interface)_uIApplication);
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
            using ComScope<IUIRibbon> uiRibbonScope = GetIUIRibbon();
            if (!uiRibbonScope.IsNull)
            {
                using var pStream = ComHelpers.GetComScope<IStream>(new ComManagedStream(stream));
                HRESULT hr = uiRibbonScope.Value->SaveSettingsToStream(pStream);
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
            using ComScope<IUIRibbon> uiRibbonScope = GetIUIRibbon();
            if (!uiRibbonScope.IsNull)
            {
                using var pStream = ComHelpers.GetComScope<IStream>(new ComManagedStream(stream));
                HRESULT hr = uiRibbonScope.Value->LoadSettingsFromStream(pStream);
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
        internal unsafe IUIFramework* Framework => _cpIUIFramework;

        /// <summary>
        /// Create ribbon framework object
        /// </summary>
        /// <returns>ribbon framework object</returns>
        private static void CreateRibbonFramework(IUIFramework** ppFramework)
        {
            try
            {
                HRESULT hr;
                Guid CLSID_UIRibbonFramework = typeof(UIRibbonFramework).GUID;
                hr = PInvoke.CoCreateInstance(
                    &CLSID_UIRibbonFramework,
                    null,
                    CLSCTX.CLSCTX_INPROC_SERVER,
                    IID.Get<IUIFramework>(),
                    (void**)ppFramework).ThrowOnFailure();
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
        private static void CreateImageFromBitmapFactory(IUIImageFromBitmap** ppImageFromBitmap)
        {
            Guid CLSID_UIRibbonImageFromBitmapFactory = typeof(UIRibbonImageFromBitmapFactory).GUID;
            PInvoke.CoCreateInstance(
                &CLSID_UIRibbonImageFromBitmapFactory,
                null,
                CLSCTX.CLSCTX_ALL, //.CLSCTX_INPROC_SERVER,
                IID.Get<IUIImageFromBitmap>(),
                (void**)ppImageFromBitmap).ThrowOnFailure();
        }

        internal IUIImageFromBitmap* CpIUIImageFromBitmap => _cpIUIImageFromBitmap;

        /// <summary>
        /// Initialize ribbon framework
        /// </summary>
        /// <param name="resourceIdentifier">Identifier of the ribbon resource</param>
        /// <param name="hInstance">Pointer to HINSTANCE of module where we can find ribbon resource</param>
        private unsafe void InitFramework(string? resourceIdentifier, HMODULE hInstance)
        {
            HRESULT hr;
            // create ribbon framework object
            fixed (IUIFramework** ppFramework = &_cpIUIFramework)
                CreateRibbonFramework(ppFramework);
            fixed (IUIImageFromBitmap** ppImageFromBitmap = &_cpIUIImageFromBitmap)
                CreateImageFromBitmapFactory(ppImageFromBitmap);

            if (Framework == null)
                return;

            // create ribbon application object
            _uIApplication = new UIApplication(this);
            _qatSetting = new QatSetting(this, QatSettingsFile);

            // init ribbon framework
            HWND hwnd = new HWND(this.WindowHandle);
            using ComScope<IUIApplication> cpIUIApplication = ComHelpers.GetComScope<IUIApplication>(_uIApplication);
            hr = Framework->Initialize(hwnd, cpIUIApplication);
            hr.ThrowOnFailure();

            // load ribbon ui
            fixed (char* resourceIdentifierLocal = resourceIdentifier)
            {
                hr = Framework->LoadUI(hInstance, resourceIdentifierLocal);
            }

            hr.ThrowOnFailure();

            if (!(Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor <= 1 || Environment.OSVersion.Version.Major < 6))
            {
#pragma warning disable CA1416
                using ComScope<IUIEventingManager> cpEventingManager = ComScope<IUIEventingManager>.QueryFrom(Framework);
                if (!cpEventingManager.IsNull)
                    EventLogger = new EventLogger(this);
#pragma warning restore CA1416
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
                Framework->Destroy();
                IUIFramework* localFramework = _cpIUIFramework;
                // remove reference to framework object
                _cpIUIFramework = null;
                uint refCount = localFramework->Release();
                Debug.WriteLine("Destroy IUIFramework refCount: " + refCount.ToString());
            }

            // Unregister event handlers
            RegisterForm(null);

            _markupHandler?.Dispose();

            _shortcutHandler?.Dispose();

            if (_cpIUIImageFromBitmap != null)
            {
                // remove reference to imageFromBitmap object
                IUIImageFromBitmap* localImageFromBitmap = _cpIUIImageFromBitmap;
                _cpIUIImageFromBitmap = null;
                uint refCount = localImageFromBitmap->Release();
                Debug.WriteLine("Destroy IUIImageFromBitmap refCount: " + refCount.ToString());
            }

            // remove references to ribbon items
            _mapRibbonStripItems.Clear();

            //@Todo: Do we need GC.KeepAlive
            //GC.KeepAlive(_uIApplication);
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            PROPVARIANT propvar;
            fixed (PROPERTYKEY* pGlobalBackgroundColor = &RibbonProperties.GlobalBackgroundColor)
                cpPropertyStore.Value->GetValue(pGlobalBackgroundColor, &propvar);
            uint background = (uint)propvar; //PropVariantToUInt32
            UI_HSBCOLOR retResult = (UI_HSBCOLOR)background;
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            // set ribbon color
            fixed (PROPERTYKEY* pGlobalBackgroundColor = &RibbonProperties.GlobalBackgroundColor)
                cpPropertyStore.Value->SetValue(pGlobalBackgroundColor, &propvar);

            cpPropertyStore.Value->Commit();
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            PROPVARIANT propvar;
            fixed (PROPERTYKEY* pGlobalHighlightColor = &RibbonProperties.GlobalHighlightColor)
                cpPropertyStore.Value->GetValue(pGlobalHighlightColor, &propvar);
            uint highlight = (uint)propvar; //PropVariantToUInt32
            UI_HSBCOLOR retResult = (UI_HSBCOLOR)highlight;
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            // set ribbon color
            fixed (PROPERTYKEY* pGlobalHighlightColor = &RibbonProperties.GlobalHighlightColor)
                cpPropertyStore.Value->SetValue(pGlobalHighlightColor, &propvar);

            cpPropertyStore.Value->Commit();
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            PROPVARIANT propvar;
            fixed (PROPERTYKEY* pGlobalTextColor = &RibbonProperties.GlobalTextColor)
                cpPropertyStore.Value->GetValue(pGlobalTextColor, &propvar);
            uint text = (uint)propvar; //PropVariantToUInt32
            UI_HSBCOLOR retResult = (UI_HSBCOLOR)text;
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            // set ribbon color
            HRESULT hr;
            fixed (PROPERTYKEY* pGlobalTextColor = &RibbonProperties.GlobalTextColor)
                hr = cpPropertyStore.Value->SetValue(pGlobalTextColor, &propvar);

            cpPropertyStore.Value->Commit();
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            HRESULT hr;
            PROPVARIANT propvar;
            fixed (PROPERTYKEY* pApplicationButtonColor = &RibbonProperties.ApplicationButtonColor)
                hr = cpPropertyStore.Value->GetValue(pApplicationButtonColor, &propvar);
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
            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            HRESULT hr;
            fixed (PROPERTYKEY* pApplicationButtonColor = &RibbonProperties.ApplicationButtonColor)
                hr = cpPropertyStore.Value->SetValue(pApplicationButtonColor, &propvar);
            if (hr.Succeeded)
            {
                hr = cpPropertyStore.Value->Commit();
                return hr.Succeeded;
            }
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

            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            HRESULT hr;
            PROPVARIANT propvar;
            fixed (PROPERTYKEY* pDarkModeRibbon = &RibbonProperties.DarkModeRibbon)
                hr = cpPropertyStore.Value->GetValue(pDarkModeRibbon, &propvar);
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
            using ComScope<IPropertyStore> cpPropertyStore = ComScope<IPropertyStore>.QueryFrom(Framework);
            HRESULT hr;
            fixed (PROPERTYKEY* pDarkModeRibbon = &RibbonProperties.DarkModeRibbon)
                hr = cpPropertyStore.Value->SetValue(pDarkModeRibbon, &propvar);
            if (hr.Succeeded)
            {
                hr = cpPropertyStore.Value->Commit();
                return (hr.Succeeded);
            }
            return false;
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
            Framework->SetModes(compactModes);
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
            IUIContextualUI* cpContextualUI;
            hr = Framework->GetView(contextPopupID, IID.Get<IUIContextualUI>(), (void**)&cpContextualUI);
            if (hr.Succeeded)
            {
                using var contextualUIScope = new ComScope<IUIContextualUI>(cpContextualUI);
                if (!contextualUIScope.IsNull)
                {
                    contextualUIScope.Value->ShowAtLocation(x, y);
                }
            }
            else
            {
                Marshal.ThrowExceptionForHR((int)hr);
            }
        }

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
            EventHandler<ThreadExceptionEventArgs>? eh = Events[s_RibbonEventExceptionKey] as EventHandler<ThreadExceptionEventArgs>;
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
                Events.AddHandler(s_RibbonEventExceptionKey, value);
            }
            remove
            {
                Events.RemoveHandler(s_RibbonEventExceptionKey, value);
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
            }
            remove
            {
                EventSet.Remove(s_ViewCreatedKey, value);
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
            _qatSetting?.Load();

            EventSet.Raise(s_ViewCreatedKey, this, EventArgs.Empty);
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
            }
            remove
            {
                EventSet.Remove(s_ViewDestroyKey, value);
            }
        }

        internal void OnViewDestroy()
        {
            _qatSetting?.Save();
            EventSet.Raise(s_ViewDestroyKey, this, EventArgs.Empty);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_cpIUIImageFromBitmap != null)
            {
                IUIImageFromBitmap* localImageFromBitmap = _cpIUIImageFromBitmap;
                _cpIUIImageFromBitmap = null;
                localImageFromBitmap->Release();
            }
            if (Framework != null)
            {
                IUIFramework* localFramework = Framework;
                _cpIUIFramework = null;
                localFramework->Release();
            }
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
            }
            remove
            {
                EventSet.Remove(s_RibbonHeightKey, value);
            }
        }

        internal void OnRibbonHeightChanged()
        {
            EventSet.Raise(s_RibbonHeightKey, this, EventArgs.Empty);
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
            using var uiRibbonScope = new UIRibbonScope(this);
            if (!uiRibbonScope.IsNull)
            {
                HRESULT hr;
                PROPVARIANT propvar;
                fixed (PROPERTYKEY* pMinimized = &RibbonProperties.Minimized)
                    hr = uiRibbonScope.PropertyStoreScope.Value->GetValue(pMinimized, &propvar);
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
            using var uiRibbonScope = new UIRibbonScope(this);
            if (!uiRibbonScope.IsNull)
            {
                HRESULT hr;
                PROPVARIANT propvar;
                propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
                fixed (PROPERTYKEY* pMinimized = &RibbonProperties.Minimized)
                    hr = uiRibbonScope.PropertyStoreScope.Value->SetValue(pMinimized, &propvar);
                if (hr.Succeeded)
                    hr = uiRibbonScope.PropertyStoreScope.Value->Commit();
            }
        }

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
        private unsafe bool GetViewable()
        {
            using var uiRibbonScope = new UIRibbonScope(this);
            if (!uiRibbonScope.IsNull)
            {
                HRESULT hr;
                PROPVARIANT propvar;
                fixed (PROPERTYKEY* pViewable = &RibbonProperties.Viewable)
                    hr = uiRibbonScope.PropertyStoreScope.Value->GetValue(pViewable, &propvar);
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
            using var uiRibbonScope = new UIRibbonScope(this);
            if (!uiRibbonScope.IsNull)
            {
                HRESULT hr;
                PROPVARIANT propvar;
                propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
                fixed (PROPERTYKEY* pViewable = &RibbonProperties.Viewable)
                    hr = uiRibbonScope.PropertyStoreScope.Value->SetValue(pViewable, &propvar);
                if (hr.Succeeded)
                    hr = uiRibbonScope.PropertyStoreScope.Value->Commit();
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
        private unsafe UI_CONTROLDOCK GetQuickAccessToolbarDock()
        {
            using var uiRibbonScope = new UIRibbonScope(this);
            if (!uiRibbonScope.IsNull)
            {
                HRESULT hr;
                PROPVARIANT propvar;
                fixed (PROPERTYKEY* pQuickAccessToolbarDock = &RibbonProperties.QuickAccessToolbarDock)
                    hr = uiRibbonScope.PropertyStoreScope.Value->GetValue(pQuickAccessToolbarDock, &propvar);
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
            using var uiRibbonScope = new UIRibbonScope(this);
            if (!uiRibbonScope.IsNull)
            {
                HRESULT hr;
                PROPVARIANT propvar = (PROPVARIANT)(uint)value; //InitPropVariantFromUInt32
                fixed (PROPERTYKEY* pQuickAccessToolbarDock = &RibbonProperties.QuickAccessToolbarDock)
                    hr = uiRibbonScope.PropertyStoreScope.Value->SetValue(pQuickAccessToolbarDock, &propvar);
                if (hr.Succeeded)
                    hr = uiRibbonScope.PropertyStoreScope.Value->Commit();
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

        private ComScope<IUIRibbon> GetIUIRibbon()
        {
            HRESULT hr;
            ComScope<IUIRibbon> uiRibbonScope = new ComScope<IUIRibbon>(null);
            hr = Framework->GetView(0, IID.Get<IUIRibbon>(), (void**)&uiRibbonScope);
            return uiRibbonScope;
        }
    }
}
