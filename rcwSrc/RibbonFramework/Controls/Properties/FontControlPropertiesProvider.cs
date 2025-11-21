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
        /// <param name="ribbon">Parent RibbonStrip</param>
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Family = &RibbonProperties.FontProperties_Family)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Family, in propvar);
                    }
                    else
                    {
                        UIPropVariant.UIInitPropertyFromString(_family, out propvar);
                        fixed (PROPERTYKEY* pKeyFontProperties_Family = &RibbonProperties.FontProperties_Family)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Family, in propvar);
                        propvar.Clear(); //PropVariantClear
                    }

                    // set size
                    if (_size.HasValue)
                    {
                        propvar = (PROPVARIANT)_size.Value; //UIInitPropertyFromDecimal
                        fixed (PROPERTYKEY* pKeyFontProperties_Size = &RibbonProperties.FontProperties_Size)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Size, in propvar);
                    }

                    // set bold
                    if (_bold.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_bold.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_Bold = &RibbonProperties.FontProperties_Bold)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Bold, in propvar);
                    }

                    // set italic
                    if (_italic.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_italic.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_Italic = &RibbonProperties.FontProperties_Italic)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Italic, in propvar);
                    }

                    // set underline
                    if (_underline.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_underline.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_Underline = &RibbonProperties.FontProperties_Underline)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Underline, in propvar);
                    }

                    // set strikethrough
                    if (_strikethrough.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_strikethrough.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_Strikethrough = &RibbonProperties.FontProperties_Strikethrough)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_Strikethrough, in propvar);
                    }

                    // set foregroundColor
                    if (_foregroundColor.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)ColorTranslator.ToWin32(_foregroundColor.Value); //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColor = &RibbonProperties.FontProperties_ForegroundColor)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_ForegroundColor, in propvar);
                    }

                    // set foregroundColorType
                    if (_foregroundColorType.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_foregroundColorType.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColorType = &RibbonProperties.FontProperties_ForegroundColorType)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_ForegroundColorType, in propvar);
                    }

                    // set backgroundColor
                    if (_backgroundColor.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)ColorTranslator.ToWin32(_backgroundColor.Value); //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColor = &RibbonProperties.FontProperties_BackgroundColor)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_BackgroundColor, in propvar);
                    }

                    // set backgroundColorType
                    if (_backgroundColorType.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_backgroundColorType.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColorType = &RibbonProperties.FontProperties_BackgroundColorType)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_BackgroundColorType, in propvar);
                    }

                    // set verticalPositioning
                    if (_verticalPositioning.HasValue)
                    {
                        propvar = (PROPVARIANT)(uint)_verticalPositioning.Value; //InitPropVariantFromUInt32
                        fixed (PROPERTYKEY* pKeyFontProperties_VerticalPositioning = &RibbonProperties.FontProperties_VerticalPositioning)
                            cpFontPropertyStore.SetValue(pKeyFontProperties_VerticalPositioning, in propvar);
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
        private unsafe IPropertyStore? FontProperties
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    fixed (PROPERTYKEY* pKeyFontProperties = &RibbonProperties.FontProperties)
                        hr = _ribbon.Framework.GetUICommandProperty(_commandId, pKeyFontProperties, out propvar);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Family = &RibbonProperties.FontProperties_Family)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_Family, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Size = &RibbonProperties.FontProperties_Size)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_Size, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Bold = &RibbonProperties.FontProperties_Bold)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_Bold, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Italic = &RibbonProperties.FontProperties_Italic)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_Italic, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Underline = &RibbonProperties.FontProperties_Underline)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_Underline, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_Strikethrough = &RibbonProperties.FontProperties_Strikethrough)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_Strikethrough, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColorType = &RibbonProperties.FontProperties_ForegroundColorType)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_ForegroundColorType, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_SWATCHCOLORTYPE swatchColorType = (UI_SWATCHCOLORTYPE)result;

                        switch (swatchColorType)
                        {
                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB:
                                PROPVARIANT propForegroundColor;
                                fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColor = &RibbonProperties.FontProperties_ForegroundColor)
                                    hr = cpIPropertyStore.GetValue(pKeyFontProperties_ForegroundColor, out propForegroundColor);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColorType = &RibbonProperties.FontProperties_BackgroundColorType)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_BackgroundColorType, out propvar);
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_SWATCHCOLORTYPE swatchColorType = (UI_SWATCHCOLORTYPE)result;

                        switch (swatchColorType)
                        {
                            case UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB:
                                PROPVARIANT propBackgroundColor;
                                fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColor = &RibbonProperties.FontProperties_BackgroundColor)
                                    hr = cpIPropertyStore.GetValue(pKeyFontProperties_BackgroundColor, out propBackgroundColor);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
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
                        fixed (PROPERTYKEY* pKeyFontProperties_VerticalPositioning = &RibbonProperties.FontProperties_VerticalPositioning)
                            hr = cpIPropertyStore.GetValue(pKeyFontProperties_VerticalPositioning, out propvar);
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
                    HRESULT hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_ALLPROPERTIES, (PROPERTYKEY*)null);
                }
            }
        }

        #endregion
    }
}
