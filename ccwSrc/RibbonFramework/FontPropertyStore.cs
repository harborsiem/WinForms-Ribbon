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
    public sealed unsafe class FontPropertyStore
    {
        private IPropertyStore* _cpPropertyStore;

        /// <summary>
        /// Initializes a new instance of the FontPropertyStore
        /// </summary>
        /// <param name="cpPropertyStore">Font properties</param>
        internal FontPropertyStore(IPropertyStore* cpPropertyStore)
        {
            if (cpPropertyStore == null)
            {
                throw new ArgumentNullException(nameof(cpPropertyStore), "Parameter cannot be null.");
            }
            _cpPropertyStore = cpPropertyStore;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~FontPropertyStore()
        {
            Dispose(false);
        }

        /// <summary>
        /// The selected font family name.
        /// </summary>
        public unsafe string Family
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_Family = &RibbonProperties.FontProperties_Family)
                    hr = _cpPropertyStore->GetValue(pFontProperties_Family, &propvar);
                PWSTR pwstr;
                hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
                string result = new string(pwstr); // pwstr.ToString();
                PInvoke.CoTaskMemFree(pwstr);
                propvar.Clear(); //PropVariantClear
                return result;
            }
        }

        /// <summary>
        /// The size of the font.
        /// </summary>
        public decimal Size
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_Size = &RibbonProperties.FontProperties_Size)
                    hr = _cpPropertyStore->GetValue(pFontProperties_Size, &propvar);
                decimal decValue = (decimal)propvar; //UIPropertyToDecimal
                return decValue;
            }
        }

        /// <summary>
        /// Flag that indicates whether bold is selected.
        /// </summary>
        public unsafe UI_FontProperties Bold
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_Bold = &RibbonProperties.FontProperties_Bold)
                    hr = _cpPropertyStore->GetValue(pFontProperties_Bold, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                return (UI_FontProperties)retResult;
            }
        }

        /// <summary>
        /// Flag that indicates whether italic is selected.
        /// </summary>
        public unsafe UI_FontProperties Italic
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_Italic = &RibbonProperties.FontProperties_Italic)
                    hr = _cpPropertyStore->GetValue(pFontProperties_Italic, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                return (UI_FontProperties)retResult;
            }
        }

        /// <summary>
        /// Flag that indicates whether underline is selected.
        /// </summary>
        public unsafe UI_FontUnderline Underline
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_Underline = &RibbonProperties.FontProperties_Underline)
                    hr = _cpPropertyStore->GetValue(pFontProperties_Underline, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTUNDERLINE retResult = (UI_FONTUNDERLINE)result;
                return (UI_FontUnderline)retResult;
            }
        }

        /// <summary>
        /// Flag that indicates whether strikethrough is selected
        /// (sometimes called Strikeout).
        /// </summary>
        public unsafe UI_FontProperties Strikethrough
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_Strikethrough = &RibbonProperties.FontProperties_Strikethrough)
                    hr = _cpPropertyStore->GetValue(pFontProperties_Strikethrough, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTPROPERTIES retResult = (UI_FONTPROPERTIES)result;
                return (UI_FontProperties)retResult;
            }
        }

        /// <summary>
        /// Contains the text color if ForegroundColorType is set to RGB.
        /// The FontControl helper class expose this property as a .NET Color
        /// and handles internally the conversion to and from COLORREF structure.
        /// </summary>
        public unsafe Color ForegroundColor
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_ForegroundColor = &RibbonProperties.FontProperties_ForegroundColor)
                    hr = _cpPropertyStore->GetValue(pFontProperties_ForegroundColor, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                Color retResult = ColorTranslator.FromWin32((int)result);
                return retResult;
            }
        }

        /// <summary>
        /// The text color type. Valid values are RGB and Automatic. 
        /// If RGB is selected, the user should get the color from the ForegroundColor property. 
        /// If Automatic is selected the user should use SystemColors.WindowText.
        /// </summary>
        public unsafe UI_SwatchColorType ForegroundColorType
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_ForegroundColorType = &RibbonProperties.FontProperties_ForegroundColorType)
                    hr = _cpPropertyStore->GetValue(pFontProperties_ForegroundColorType, &propvar);
                uint result;
                if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                    result = (uint)(int)propvar;
                else
                    result = (uint)propvar; //PropVariantToUInt32
                UI_SWATCHCOLORTYPE retResult = (UI_SWATCHCOLORTYPE)result;
                return (UI_SwatchColorType)retResult;
            }
        }

        /// <summary>
        /// Indicated whether the "Grow Font" or "Shrink Font" buttons were pressed.
        /// </summary>
        public unsafe UI_FontDeltaSize? DeltaSize
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_DeltaSize = &RibbonProperties.FontProperties_DeltaSize)
                    hr = _cpPropertyStore->GetValue(pFontProperties_DeltaSize, &propvar);
                if (hr.Succeeded)
                {
                    if (propvar.IsEmpty)
                        return null;
                    uint result = (uint)propvar; //PropVariantToUInt32
                    UI_FONTDELTASIZE? retResult = (UI_FONTDELTASIZE?)(uint?)result;
                    return (UI_FontDeltaSize?)retResult;
                }
                return null;
            }
        }

        /// <summary>
        /// Contains the background color if BackgroundColorType is set to RGB.
        /// The FontControl helper class expose this property as a .NET Color
        /// and handles internally the conversion to and from COLORREF structure.
        /// </summary>
        public unsafe Color BackgroundColor
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_BackgroundColor = &RibbonProperties.FontProperties_BackgroundColor)
                    hr = _cpPropertyStore->GetValue(pFontProperties_BackgroundColor, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                Color retResult = ColorTranslator.FromWin32((int)result);
                return retResult;
            }
        }

        /// <summary>
        /// The background color type. Valid values are RGB and NoColor. 
        /// If RGB is selected, the user should get the color from the BackgroundColor property.
        /// If NoColor is selected the user should use SystemColors.Window.
        /// </summary>
        public unsafe UI_SwatchColorType BackgroundColorType
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_BackgroundColorType = &RibbonProperties.FontProperties_BackgroundColorType)
                    hr = _cpPropertyStore->GetValue(pFontProperties_BackgroundColorType, &propvar);
                uint result;
                if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                    result = (uint)(int)propvar;
                else
                    result = (uint)propvar; //PropVariantToUInt32
                UI_SWATCHCOLORTYPE retResult = (UI_SWATCHCOLORTYPE)result;
                return (UI_SwatchColorType)retResult;
            }
        }

        /// <summary>
        /// Flag that indicates which one of the Subscript
        /// and Superscript buttons are selected, if any.
        /// </summary>
        public unsafe UI_FontVerticalPosition VerticalPositioning
        {
            get
            {
                PROPVARIANT propvar;
                HRESULT hr;
                fixed (PROPERTYKEY* pFontProperties_VerticalPositioning = &RibbonProperties.FontProperties_VerticalPositioning)
                    hr = _cpPropertyStore->GetValue(pFontProperties_VerticalPositioning, &propvar);
                uint result = (uint)propvar; //PropVariantToUInt32
                UI_FONTVERTICALPOSITION retResult = (UI_FONTVERTICALPOSITION)result;
                return (UI_FontVerticalPosition)retResult;
            }
        }

        private void Dispose(bool disposing)
        {
            if (_cpPropertyStore != null)
            {
                
                _cpPropertyStore->Release();
                //_cpPropertyStore = null;
            }
        }
    }
}
