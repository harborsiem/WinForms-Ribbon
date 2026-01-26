//*****************************************************************************
//
//  File:       FontPropertyStore.cs
//
//  Contents:   Helper class that wraps an IPropertyStore interface that 
//              contains font properties
//
//*****************************************************************************

using System;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Variant;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps an IPropertyStore interface that 
    /// contains font properties
    /// </summary>
    public sealed class FontPropertyStore
    {
        /// <summary>
        /// Initializes a new instance of the FontPropertyStore
        /// </summary>
        /// <param name="cpPropertyStore">Font properties</param>
        internal FontPropertyStore(ComScope<IPropertyStore> cpPropertyStore)
        {
            if (cpPropertyStore.IsNull)
            {
                throw new ArgumentNullException(nameof(cpPropertyStore), "Parameter cannot be null.");
            }
            Family = GetFamily(cpPropertyStore);
            Size = GetSize(cpPropertyStore);
            Bold = GetBold(cpPropertyStore);
            Italic = GetItalic(cpPropertyStore);
            Underline = GetUnderline(cpPropertyStore);
            Strikethrough = GetStrikethrough(cpPropertyStore);
            ForegroundColor = GetForegroundColor(cpPropertyStore);
            ForegroundColorType = GetForegroundColorType(cpPropertyStore);
            DeltaSize = GetDeltaSize(cpPropertyStore);
            BackgroundColor = GetBackgroundColor(cpPropertyStore);
            BackgroundColorType = GetBackgroundColorType(cpPropertyStore);
            VerticalPositioning = GetVerticalPositioning(cpPropertyStore);
        }

        /// <summary>
        /// The selected font family name.
        /// </summary>
        public readonly string Family;
        private unsafe string GetFamily(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_Family = &RibbonProperties.FontProperties_Family)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_Family, &propvar);
                PWSTR pwstr;
                hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
                string? result = pwstr.ToStringAndCoTaskMemFree();
                propvar.Clear(); //PropVariantClear
                return result!;
        }

        /// <summary>
        /// The size of the font.
        /// </summary>
        public readonly decimal Size;
        private unsafe decimal GetSize(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_Size = &RibbonProperties.FontProperties_Size)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_Size, &propvar);
                decimal decValue = (decimal)propvar; //UIPropertyToDecimal
                return decValue;
        }

        /// <summary>
        /// Flag that indicates whether bold is selected.
        /// </summary>
        public readonly FontProperties Bold;
        private unsafe FontProperties GetBold(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_Bold = &RibbonProperties.FontProperties_Bold)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_Bold, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                return (FontProperties)retResult;
        }

        /// <summary>
        /// Flag that indicates whether italic is selected.
        /// </summary>
        public readonly FontProperties Italic;
        private unsafe FontProperties GetItalic(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_Italic = &RibbonProperties.FontProperties_Italic)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_Italic, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                return (FontProperties)retResult;
        }

        /// <summary>
        /// Flag that indicates whether underline is selected.
        /// </summary>
        public readonly FontUnderline Underline;
        private unsafe FontUnderline GetUnderline(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_Underline = &RibbonProperties.FontProperties_Underline)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_Underline, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTUNDERLINE retResult = (UI_FONTUNDERLINE)result;
                return (FontUnderline)retResult;
        }

        /// <summary>
        /// Flag that indicates whether strikethrough is selected
        /// (sometimes called Strikeout).
        /// </summary>
        public readonly FontProperties Strikethrough;
        private unsafe FontProperties GetStrikethrough(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_Strikethrough = &RibbonProperties.FontProperties_Strikethrough)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_Strikethrough, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                return (FontProperties)retResult;
        }

        /// <summary>
        /// Contains the text color if ForegroundColorType is set to RGB.
        /// The FontControl helper class expose this property as a .NET Color
        /// and handles internally the conversion to and from COLORREF structure.
        /// </summary>
        public readonly Color ForegroundColor;
        private unsafe Color GetForegroundColor(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColor = &RibbonProperties.FontProperties_ForegroundColor)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_ForegroundColor, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                Color retResult = ColorTranslator.FromWin32((int)result);
                return retResult;
        }

        /// <summary>
        /// The text color type. Valid values are RGB and Automatic. 
        /// If RGB is selected, the user should get the color from the ForegroundColor property. 
        /// If Automatic is selected the user should use SystemColors.WindowText.
        /// </summary>
        public readonly SwatchColorType ForegroundColorType;
        private unsafe SwatchColorType GetForegroundColorType(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColorType = &RibbonProperties.FontProperties_ForegroundColorType)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_ForegroundColorType, &propvar);
                uint result;
                if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                    result = (uint)(int)propvar;
                else
                    result = (uint)propvar; //PropVariantToUInt32
                UI_SWATCHCOLORTYPE retResult = (UI_SWATCHCOLORTYPE)result;
                return (SwatchColorType)retResult;
        }

        /// <summary>
        /// Indicated whether the "Grow Font" or "Shrink Font" buttons were pressed.
        /// </summary>
        public readonly FontDeltaSize? DeltaSize;
        private unsafe FontDeltaSize? GetDeltaSize(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_DeltaSize = &RibbonProperties.FontProperties_DeltaSize)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_DeltaSize, &propvar);
                if (hr.Succeeded)
                {
                    if (propvar.IsEmpty)
                        return null;
                    uint result = (uint)propvar; //PropVariantToUInt32
                    UI_FONTDELTASIZE? retResult = (UI_FONTDELTASIZE?)(uint?)result;
                    return (FontDeltaSize?)retResult;
                }
                return null;
        }

        /// <summary>
        /// Contains the background color if BackgroundColorType is set to RGB.
        /// The FontControl helper class expose this property as a .NET Color
        /// and handles internally the conversion to and from COLORREF structure.
        /// </summary>
        public readonly Color BackgroundColor;
        private unsafe Color GetBackgroundColor(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColor = &RibbonProperties.FontProperties_BackgroundColor)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_BackgroundColor, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                Color retResult = ColorTranslator.FromWin32((int)result);
                return retResult;
        }

        /// <summary>
        /// The background color type. Valid values are RGB and NoColor. 
        /// If RGB is selected, the user should get the color from the BackgroundColor property.
        /// If NoColor is selected the user should use SystemColors.Window.
        /// </summary>
        public readonly SwatchColorType BackgroundColorType;
        private unsafe SwatchColorType GetBackgroundColorType(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColorType = &RibbonProperties.FontProperties_BackgroundColorType)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_BackgroundColorType, &propvar);
                uint result;
                if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                    result = (uint)(int)propvar;
                else
                    result = (uint)propvar; //PropVariantToUInt32
                UI_SWATCHCOLORTYPE retResult = (UI_SWATCHCOLORTYPE)result;
                return (SwatchColorType)retResult;
        }

        /// <summary>
        /// Flag that indicates which one of the Subscript
        /// and Superscript buttons are selected, if any.
        /// </summary>
        public readonly FontVerticalPosition VerticalPositioning;
        private unsafe FontVerticalPosition GetVerticalPositioning(ComScope<IPropertyStore> cpPropertyStore)
        {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pKeyFontProperties_VerticalPositioning = &RibbonProperties.FontProperties_VerticalPositioning)
                    hr = cpPropertyStore.Value->GetValue(pKeyFontProperties_VerticalPositioning, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTVERTICALPOSITION retResult = (UI_FONTVERTICALPOSITION)result;
                return (FontVerticalPosition)retResult;
        }
    }
}
