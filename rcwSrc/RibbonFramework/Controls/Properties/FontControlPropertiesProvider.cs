//*****************************************************************************
//
//  File:       FontControlPropertiesProvider.cs
//
//  Contents:   Definition for font control properties provider 
//
//*****************************************************************************

using System;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for font control properties provider interface
    /// </summary>
    public interface IFontControlPropertiesProvider
    {
        /// <summary>
        /// Family property
        /// </summary>
        string Family { get; set; }

        /// <summary>
        /// Size property
        /// </summary>
        decimal Size { get; set; }

        /// <summary>
        /// Bold property
        /// </summary>
        FontProperties Bold { get; set; }

        /// <summary>
        /// Italic property
        /// </summary>
        FontProperties Italic { get; set; }

        /// <summary>
        /// Underline property
        /// </summary>
        FontUnderline Underline { get; set; }

        /// <summary>
        /// Strikethrough property
        /// </summary>
        FontProperties Strikethrough { get; set; }

        /// <summary>
        /// Foreground color property
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Background color property
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Vertical positioning property
        /// </summary>
        FontVerticalPosition VerticalPositioning { get; set; }
    }

    /// <summary>
    /// Implementation of IFontControlPropertiesProvider
    /// </summary>
    public sealed class FontControlPropertiesProvider : BasePropertiesProvider, IFontControlPropertiesProvider
    {
        /// <summary>
        /// FontControlPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        public FontControlPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.FontProperties);
        }

        private string _family;
        private decimal? _size;
        private UI_FONTPROPERTIES? _bold;
        private UI_FONTPROPERTIES? _italic;
        private UI_FONTUNDERLINE? _underline;
        private UI_FONTPROPERTIES? _strikethrough;
        private Color? _foregroundColor;
        private UI_SWATCHCOLORTYPE? _foregroundColorType;
        private Color? _backgroundColor;
        private UI_SWATCHCOLORTYPE? _backgroundColorType;
        private UI_FONTVERTICALPOSITION? _verticalPositioning;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            fixed (PROPVARIANT* newValueLocal = &newValue) { }
            if (key == RibbonProperties.FontProperties)
            {
                if (currentValue is not null)
                {
                    // get current font properties
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    IPropertyStore cpFontPropertyStore;
                    UIPropVariant.UIPropertyToInterface<IPropertyStore>(RibbonProperties.FontProperties, *currentValue, out cpFontPropertyStore!);
                    //(*currentValue).Clear(); //PropVariantClear ???

                    // set family
                    if ((_family == null) || (_family.Trim() == string.Empty))
                    {
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Family, in propvar);
                    }
                    else
                    {
                        UIPropVariant.UIInitPropertyFromString(_family, out propvar);
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Family, in propvar);
                        propvar.Clear(); //PropVariantClear
                    }

                    // set size
                    if (_size.HasValue)
                    {
                        propvar = (PROPVARIANT)_size.Value; //UIInitPropertyFromDecimal
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Size, in propvar);
                    }

                    // set bold
                    if (_bold.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_bold.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Bold, in propvar);
                    }

                    // set italic
                    if (_italic.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_italic.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Italic, in propvar);
                    }

                    // set underline
                    if (_underline.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_underline.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Underline, in propvar);
                    }

                    // set strikethrough
                    if (_strikethrough.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_strikethrough.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_Strikethrough, in propvar);
                    }

                    // set foregroundColor
                    if (_foregroundColor.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)ColorTranslator.ToWin32(_foregroundColor.Value); //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_ForegroundColor, in propvar);
                    }

                    // set foregroundColorType
                    if (_foregroundColorType.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_foregroundColorType.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_ForegroundColorType, in propvar);
                    }

                    // set backgroundColor
                    if (_backgroundColor.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)ColorTranslator.ToWin32(_backgroundColor.Value); //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_BackgroundColor, in propvar);
                    }

                    // set backgroundColorType
                    if (_backgroundColorType.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_backgroundColorType.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_BackgroundColorType, in propvar);
                    }

                    // set verticalPositioning
                    if (_verticalPositioning.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_verticalPositioning.Value; //InitPropVariantFromUInt32
                        cpFontPropertyStore.SetValue(RibbonProperties.FontProperties_VerticalPositioning, in propvar);
                    }

                    // set new font properties
                    UIPropVariant.UIInitPropertyFromInterface(RibbonProperties.FontProperties, cpFontPropertyStore, out newValue);

                }
            }

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Font properties property
        /// </summary>
        private IPropertyStore? FontProperties
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.FontProperties, out propvar);
                    if (hr.Succeeded)
                    {
                        IPropertyStore result;
                        UIPropVariant.UIPropertyToInterface<IPropertyStore>(RibbonProperties.FontProperties, propvar, out result!);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return null;
            }
        }

        #region IFontControlPropertiesProvider Members

        /// <summary>
        /// Family property
        /// </summary>
        public unsafe string Family
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_Family, out propvar);
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _family;
            }
            set
            {
                _family = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Size property
        /// </summary>
        public unsafe decimal Size
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_Size, out propvar);
                        decimal decValue = (decimal)propvar; //UIPropertyToDecimal
                        return decValue;
                    }
                }

                return _size.GetValueOrDefault();
            }
            set
            {
                _size = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Bold property
        /// </summary>
        public unsafe FontProperties Bold
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_Bold, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                        return (FontProperties)retResult;
                    }
                }

                return (FontProperties)_bold.GetValueOrDefault(UI_FONTPROPERTIES.UI_FONTPROPERTIES_NOTAVAILABLE);
            }
            set
            {
                _bold = (UI_FONTPROPERTIES)value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Italic property
        /// </summary>
        public unsafe FontProperties Italic
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_Italic, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                        return (FontProperties)retResult;
                    }
                }

                return (FontProperties)_italic.GetValueOrDefault(UI_FONTPROPERTIES.UI_FONTPROPERTIES_NOTAVAILABLE);
            }
            set
            {
                _italic = (UI_FONTPROPERTIES)value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Underline property
        /// </summary>
        public unsafe FontUnderline Underline
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_Underline, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_FONTUNDERLINE retResult = (UI_FONTUNDERLINE)result;
                        return (FontUnderline)retResult;
                    }
                }

                return (FontUnderline)_underline.GetValueOrDefault(UI_FONTUNDERLINE.UI_FONTUNDERLINE_NOTAVAILABLE);
            }
            set
            {
                _underline = (UI_FONTUNDERLINE)value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Strikethrough property
        /// </summary>
        public unsafe FontProperties Strikethrough
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_Strikethrough, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                        return (FontProperties)retResult;
                    }
                }

                return (FontProperties)_strikethrough.GetValueOrDefault(UI_FONTPROPERTIES.UI_FONTPROPERTIES_NOTAVAILABLE);
            }
            set
            {
                _strikethrough = (UI_FONTPROPERTIES)value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Foreground color property
        /// </summary>
        public unsafe Color ForegroundColor
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_ForegroundColorType, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_SWATCHCOLORTYPE swatchColorType = (UI_SWATCHCOLORTYPE)result;

                        switch (swatchColorType)
                        {
                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB:
                                PROPVARIANT propForegroundColor;
                                hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_ForegroundColor, out propForegroundColor);
                                uint resultRGB = (uint)propForegroundColor; //PropVariantToUInt32
                                Color retResult = ColorTranslator.FromWin32((int)resultRGB);
                                return retResult;

                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_AUTOMATIC:
                                return SystemColors.WindowText;

                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_NOCOLOR:
                                throw new NotSupportedException("NoColor is not a valid value for ForegroundColor property in FontControl.");
                        }

                        return SystemColors.WindowText;
                    }
                }

                return _foregroundColor.GetValueOrDefault(SystemColors.WindowText);
            }
            set
            {
                _foregroundColor = value;
                _foregroundColorType = UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB;

                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Background color property
        /// </summary>
        public unsafe Color BackgroundColor
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_BackgroundColorType, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_SWATCHCOLORTYPE swatchColorType = (UI_SWATCHCOLORTYPE)result;

                        switch (swatchColorType)
                        {
                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB:
                                PROPVARIANT propBackgroundColor;
                                hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_BackgroundColor, out propBackgroundColor);
                                uint resultRGB = (uint)propBackgroundColor; //PropVariantToUInt32
                                Color retResult = ColorTranslator.FromWin32((int)resultRGB);
                                return retResult;

                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_AUTOMATIC:
                                throw new NotSupportedException("Automatic is not a valid value for BackgroundColor property in FontControl.");

                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_NOCOLOR:
                                return SystemColors.Window;
                        }

                        return SystemColors.Window;
                    }
                }

                return _backgroundColor.GetValueOrDefault(SystemColors.Window);
            }
            set
            {
                _backgroundColor = value;
                _backgroundColorType = UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB;

                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        /// <summary>
        /// Vertical positioning property
        /// </summary>
        public unsafe FontVerticalPosition VerticalPositioning
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    IPropertyStore? cpIPropertyStore = FontProperties;
                    if (cpIPropertyStore != null)
                    {
                        HRESULT hr;
                        PROPVARIANT propvar;
                        hr = cpIPropertyStore.GetValue(RibbonProperties.FontProperties_VerticalPositioning, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_FONTVERTICALPOSITION retResult = (UI_FONTVERTICALPOSITION)result;
                        return (FontVerticalPosition)retResult;
                    }
                }

                return (FontVerticalPosition)_verticalPositioning.GetValueOrDefault(UI_FONTVERTICALPOSITION.UI_FONTVERTICALPOSITION_NOTAVAILABLE);
            }
            set
            {
                _verticalPositioning = (UI_FONTVERTICALPOSITION)value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, null);
                }
            }
        }

        #endregion
    }
}
