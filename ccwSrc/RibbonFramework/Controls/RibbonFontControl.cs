//*****************************************************************************
//
//  File:       RibbonFontControl.cs
//
//  Contents:   Helper class that wraps a ribbon font control.
//
//*****************************************************************************

using System.Drawing;
using System;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a ribbon font control.
    /// </summary>
    public sealed class RibbonFontControl : RibbonStripItem,
        IFontControlPropertiesProvider,
        IEnabledPropertiesProvider,
        IKeytipPropertiesProvider,
        IExecuteEventsProvider,
        IPreviewEventsProvider
    {
        private static readonly EventKey s_FontChangedKey = new EventKey();
        private FontControlPropertiesProvider _fontControlPropertiesProvider;
        private EnabledPropertiesProvider _enabledPropertiesProvider;
        private KeytipPropertiesProvider _keytipPropertiesProvider;
        private ExecuteEventsProvider _executeEventsProvider;
        private PreviewEventsProvider _previewEventsProvider;

        /// <summary>
        /// Initializes a new instance of the Ribbon FontControl
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonFontControl(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            AddPropertiesProvider(_fontControlPropertiesProvider = new FontControlPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_enabledPropertiesProvider = new EnabledPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_keytipPropertiesProvider = new KeytipPropertiesProvider(ribbon, commandId, this));

            AddEventsProvider(_executeEventsProvider = new ExecuteEventsProvider(this));
            AddEventsProvider(_previewEventsProvider = new PreviewEventsProvider(this));
        }

        #region IFontControlPropertiesProvider Members

        /// <summary>
        /// The selected font family name.
        /// </summary>
        public string Family
        {
            get
            {
                return _fontControlPropertiesProvider.Family;
            }
            set
            {
                _fontControlPropertiesProvider.Family = value;
            }
        }

        /// <summary>
        /// The size of the font.
        /// </summary>
        public decimal Size
        {
            get
            {
                return _fontControlPropertiesProvider.Size;
            }
            set
            {
                _fontControlPropertiesProvider.Size = value;
            }
        }

        /// <summary>
        /// Flag that indicates whether bold is selected.
        /// </summary>
        public FontProperties Bold
        {
            get
            {
                return _fontControlPropertiesProvider.Bold;
            }
            set
            {
                _fontControlPropertiesProvider.Bold = value;
            }
        }

        /// <summary>
        /// Flag that indicates whether italic is selected.
        /// </summary>
        public FontProperties Italic
        {
            get
            {
                return _fontControlPropertiesProvider.Italic;
            }
            set
            {
                _fontControlPropertiesProvider.Italic = value;
            }
        }

        /// <summary>
        /// Flag that indicates whether underline is selected.
        /// </summary>
        public FontUnderline Underline
        {
            get
            {
                return _fontControlPropertiesProvider.Underline;
            }
            set
            {
                _fontControlPropertiesProvider.Underline = value;
            }
        }

        /// <summary>
        /// Flag that indicates whether strikethrough is selected
        /// (sometimes called Strikeout).
        /// </summary>
        public FontProperties Strikethrough
        {
            get
            {
                return _fontControlPropertiesProvider.Strikethrough;
            }
            set
            {
                _fontControlPropertiesProvider.Strikethrough = value;
            }
        }

        /// <summary>
        /// Contains the text color if ForegroundColorType is set to RGB.
        /// The FontControl helper class expose this property as a .NET Color
        /// and handles internally the conversion to and from COLORREF structure.
        /// </summary>
        public Color ForegroundColor
        {
            get
            {
                return _fontControlPropertiesProvider.ForegroundColor;
            }
            set
            {
                _fontControlPropertiesProvider.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Contains the background color if BackgroundColorType is set to RGB.
        /// The FontControl helper class expose this property as a .NET Color
        /// and handles internally the conversion to and from COLORREF structure.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return _fontControlPropertiesProvider.BackgroundColor;
            }
            set
            {
                _fontControlPropertiesProvider.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Flag that indicates which one of the Subscript
        /// and Superscript buttons are selected, if any.
        /// </summary>
        public FontVerticalPosition VerticalPositioning
        {
            get
            {
                return _fontControlPropertiesProvider.VerticalPositioning;
            }
            set
            {
                _fontControlPropertiesProvider.VerticalPositioning = value;
            }
        }

        #endregion

        /// <summary>
        /// System.Drawing.FontStyle. A combination of some properties
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                FontStyle fontStyle = FontStyle.Regular;
                if ((UI_FONTPROPERTIES)this.Bold == UI_FONTPROPERTIES.UI_FONTPROPERTIES_SET)
                {
                    fontStyle |= FontStyle.Bold;
                }
                if ((UI_FONTPROPERTIES)this.Italic == UI_FONTPROPERTIES.UI_FONTPROPERTIES_SET)
                {
                    fontStyle |= FontStyle.Italic;
                }
                if ((UI_FONTUNDERLINE)this.Underline == UI_FONTUNDERLINE.UI_FONTUNDERLINE_SET)
                {
                    fontStyle |= FontStyle.Underline;
                }
                if ((UI_FONTPROPERTIES)this.Strikethrough == UI_FONTPROPERTIES.UI_FONTPROPERTIES_SET)
                {
                    fontStyle |= FontStyle.Strikeout;
                }
                return fontStyle;
            }
        }

        ///// <summary>
        ///// The RichTextBox SelectionCharOffset calculated from VerticalPositioning
        ///// </summary>
        //public int SelectionCharOffset
        //{
        //    get
        //    {   // set subscript / superscript
        //        switch (this.VerticalPositioning)
        //        {
        //            case FontVerticalPosition.NotSet:
        //            case FontVerticalPosition.NotAvailable:
        //                return 0;

        //            case FontVerticalPosition.SuperScript:
        //                return 10;

        //            case FontVerticalPosition.SubScript:
        //                return -10;
        //        }
        //        return 0;
        //    }
        //}

        #region IEnabledPropertiesProvider Members

        /// <summary>
        /// Get or set the Enabled state.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabledPropertiesProvider.Enabled;
            }
            set
            {
                _enabledPropertiesProvider.Enabled = value;
            }
        }

        #endregion

        #region IKeytipPropertiesProvider Members

        /// <summary>
        /// The keytip or key sequence that is used to access the command using the Alt key.
        /// This keytip appears when the user presses the Alt key to navigate the ribbon.
        /// The Ribbon Framework will automatically apply keytips to every command.
        /// However, if you want more control over the keytips used, you can specify them yourself.
        /// A keytip is not limited to a single character.
        /// </summary>
        public string? Keytip
        {
            get
            {
                return _keytipPropertiesProvider.Keytip;
            }
            set
            {
                _keytipPropertiesProvider.Keytip = value;
            }
        }

        #endregion

        #region IExecuteEventsProvider Members

        /// <summary>
        /// Event provider similar to a "Selected Changed" event.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IExecuteEventsProvider.ExecuteEvent
        {
            add
            {
                _executeEventsProvider.ExecuteEvent += value;
            }
            remove
            {
                _executeEventsProvider.ExecuteEvent -= value;
            }
        }

        #endregion

        #region IPreviewEventsProvider Members

        /// <summary>
        /// Event provider for a preview.
        /// This is when the mouse enters the control.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IPreviewEventsProvider.PreviewEvent
        {
            add
            {
                _previewEventsProvider.PreviewEvent += value;
            }
            remove
            {
                _previewEventsProvider.PreviewEvent -= value;
            }
        }

        /// <summary>
        /// Event provider when the preview is cancelled.
        /// This is when the mouse leaves the control.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IPreviewEventsProvider.CancelPreviewEvent
        {
            add
            {
                _previewEventsProvider.CancelPreviewEvent += value;
            }
            remove
            {
                _previewEventsProvider.CancelPreviewEvent -= value;
            }
        }

        #endregion

        /// <summary>
        /// Event provider when Font properties changed.
        /// </summary>
        public event EventHandler<FontControlEventArgs>? FontChanged
        {
            add { EventSet.Add(s_FontChangedKey, value); }
            remove { EventSet.Remove(s_FontChangedKey, value); }
        }

        /// <summary>
        /// Event provider for Preview.
        /// </summary>
        public event EventHandler<FontControlEventArgs>? Preview
        {
            add { EventSet.Add(s_PreviewKey, value); }
            remove { EventSet.Remove(s_PreviewKey, value); }
        }

        /// <summary>
        /// Event provider for CancelPreview.
        /// </summary>
        public event EventHandler<FontControlEventArgs>? CancelPreview
        {
            add { EventSet.Add(s_CancelPreviewKey, value); }
            remove { EventSet.Remove(s_CancelPreviewKey, value); }
        }

        private protected override unsafe HRESULT OnExecute(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            FontControlEventArgs eventArgs = FontControlEventArgs.Create(*key, *currentValue, commandExecutionProperties)!;
            EventSet.Raise(s_FontChangedKey, this, eventArgs);
            return HRESULT.S_OK;
        }

        private protected override unsafe HRESULT OnPreview(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties, bool cancel)
        {
            FontControlEventArgs eventArgs = FontControlEventArgs.Create(*key, *currentValue, commandExecutionProperties)!;
            if (cancel)
            {
                EventSet.Raise(s_CancelPreviewKey, this, eventArgs);
            }
            else
            {
                EventSet.Raise(s_PreviewKey, this, eventArgs);
            }
            return HRESULT.S_OK;
        }

    }
}
