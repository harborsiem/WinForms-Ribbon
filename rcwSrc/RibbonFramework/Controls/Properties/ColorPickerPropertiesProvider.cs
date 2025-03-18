//*****************************************************************************
//
//  File:       ColorPickerPropertiesProvider.cs
//
//  Contents:   Definition for color picker properties provider 
//
//*****************************************************************************

using System;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using System.Runtime.InteropServices;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for color picker properties provider interface
    /// </summary>
    public interface IColorPickerPropertiesProvider
    {
        /// <summary>
        /// Automatic color label property
        /// </summary>
        string AutomaticColorLabel { get; set; }

        /// <summary>
        /// Color property
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Color type property
        /// </summary>
        UI_SwatchColorType ColorType { get; set; }

        /// <summary>
        /// More colors label property
        /// </summary>
        string MoreColorsLabel { get; set; }

        /// <summary>
        /// No color label property
        /// </summary>
        string NoColorLabel { get; set; }

        /// <summary>
        /// Recent colors category label property
        /// </summary>
        string RecentColorsCategoryLabel { get; set; }

        /// <summary>
        /// Standard colors property
        /// </summary>
        Color[] StandardColors { get; set; }

        /// <summary>
        /// Standard colors category label property
        /// </summary>
        string StandardColorsCategoryLabel { get; set; }

        /// <summary>
        /// Standard colors tooltips property
        /// </summary>
        string[] StandardColorsTooltips { get; set; }

        /// <summary>
        /// Theme colors property
        /// </summary>
        Color[] ThemeColors { get; set; }

        /// <summary>
        /// Theme colors category label property
        /// </summary>
        string ThemeColorsCategoryLabel { get; set; }

        /// <summary>
        /// Theme colors tooltips property
        /// </summary>
        string[] ThemeColorsTooltips { get; set; }
    }

    /// <summary>
    /// Implementation of IColorPickerPropertiesProvider
    /// </summary>
    public sealed class ColorPickerPropertiesProvider : BasePropertiesProvider, IColorPickerPropertiesProvider
    {
        /// <summary>
        /// ColorPickerPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        public ColorPickerPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.AutomaticColorLabel);
            _supportedProperties.Add(RibbonProperties.Color);
            _supportedProperties.Add(RibbonProperties.ColorType);
            _supportedProperties.Add(RibbonProperties.MoreColorsLabel);
            _supportedProperties.Add(RibbonProperties.NoColorLabel);
            _supportedProperties.Add(RibbonProperties.RecentColorsCategoryLabel);
            _supportedProperties.Add(RibbonProperties.StandardColors);
            _supportedProperties.Add(RibbonProperties.StandardColorsCategoryLabel);
            _supportedProperties.Add(RibbonProperties.StandardColorsTooltips);
            _supportedProperties.Add(RibbonProperties.ThemeColors);
            _supportedProperties.Add(RibbonProperties.ThemeColorsCategoryLabel);
            _supportedProperties.Add(RibbonProperties.ThemeColorsTooltips);
        }

        private string _automaticColorLabel;
        private Color? _color;
        private UI_SWATCHCOLORTYPE? _colorType;
        private string _moreColorsLabel;
        private string _noColorLabel;
        private string _recentColorsCategoryLabel;
        private Color[] _standardColors;
        private string _standardColorsCategoryLabel;
        private string[] _standardColorsTooltips;
        private Color[] _themeColors;
        private string _themeColorsCategoryLabel;
        private string[] _themeColorsTooltips;

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
            if (key == RibbonProperties.AutomaticColorLabel)
            {
                if (_automaticColorLabel != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_automaticColorLabel, out newValue);
                }
            }
            else if (key == RibbonProperties.Color)
            {
                if (_color.HasValue)
                {
                    newValue = (PROPVARIANT)(uint)ColorTranslator.ToWin32(_color.Value); //InitPropVariantFromUInt32
                }
            }
            else if (key == RibbonProperties.ColorType)
            {
                if (_colorType.HasValue)
                {
                    newValue = (PROPVARIANT)(uint)_colorType.Value; //InitPropVariantFromUInt32
                }
            }
            else if (key == RibbonProperties.MoreColorsLabel)
            {
                if (_moreColorsLabel != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_moreColorsLabel, out newValue);
                }
            }
            else if (key == RibbonProperties.NoColorLabel)
            {
                if (_noColorLabel != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_noColorLabel, out newValue);
                }
            }
            else if (key == RibbonProperties.RecentColorsCategoryLabel)
            {
                if (_recentColorsCategoryLabel != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_recentColorsCategoryLabel, out newValue);
                }
            }
            else if (key == RibbonProperties.StandardColors)
            {
                if (_standardColors != null)
                {
                    int[] intStandardColors = Array.ConvertAll<Color, int>(_standardColors, new Converter<Color, int>(ColorTranslator.ToWin32));
                    uint[] uintStandardColors = Array.ConvertAll<int, uint>(intStandardColors, new Converter<int, uint>(Convert.ToUInt32));
                    fixed (uint* prgn = uintStandardColors)
                        PInvoke.InitPropVariantFromUInt32Vector(prgn, (uint)uintStandardColors.Length, out newValue);
                }
            }
            else if (key == RibbonProperties.StandardColorsCategoryLabel)
            {
                if (_standardColorsCategoryLabel != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_standardColorsCategoryLabel, out newValue);
                }
            }
            else if (key == RibbonProperties.StandardColorsTooltips)
            {
                if (_standardColorsTooltips != null)
                {
                    HRESULT hr;
                    hr = PROPVARIANT.InitPropVariantFromStringVector(_standardColorsTooltips, out newValue);
                }
            }
            else if (key == RibbonProperties.ThemeColors)
            {
                if (_themeColors != null)
                {
                    int[] intThemeColors = Array.ConvertAll<Color, int>(_themeColors, new Converter<Color, int>(ColorTranslator.ToWin32));
                    uint[] uintThemeColors = Array.ConvertAll<int, uint>(intThemeColors, new Converter<int, uint>(Convert.ToUInt32));
                    fixed (uint* prgn = uintThemeColors)
                        PInvoke.InitPropVariantFromUInt32Vector(prgn, (uint)uintThemeColors.Length, out newValue);
                }
            }
            else if (key == RibbonProperties.ThemeColorsCategoryLabel)
            {
                if (_themeColorsCategoryLabel != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_themeColorsCategoryLabel, out newValue);
                }
            }
            else if (key == RibbonProperties.ThemeColorsTooltips)
            {
                if (_themeColorsTooltips != null)
                {
                    HRESULT hr;
                    hr = PROPVARIANT.InitPropVariantFromStringVector(_themeColorsTooltips, out newValue);
                }
            }

            return HRESULT.S_OK;
        }

        #region IColorPickerPropertiesProvider Members

        /// <summary>
        /// Automatic color label property
        /// </summary>
        public unsafe string AutomaticColorLabel
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.AutomaticColorLabel, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _automaticColorLabel;
            }
            set
            {
                _automaticColorLabel = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    if (!string.IsNullOrWhiteSpace(_automaticColorLabel))
                    {
                        UIPropVariant.UIInitPropertyFromString(_automaticColorLabel, out propvar);
                    }
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.AutomaticColorLabel, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Color property
        /// </summary>
        public unsafe Color Color
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.Color, out propvar);
                    if (hr.Succeeded)
                    {
                        uint result = (uint)propvar; //PropVariantToUInt32
                        Color retResult = ColorTranslator.FromWin32((int)result);
                        propvar.Clear(); //PropVariantClear
                        return retResult;
                    }
                }

                return _color.GetValueOrDefault();
            }
            set
            {
                _color = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    propvar = (PROPVARIANT)(uint)ColorTranslator.ToWin32(value); //InitPropVariantFromUInt32
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.Color, propvar);
                }
            }
        }

        /// <summary>
        /// Color type property
        /// </summary>
        public unsafe UI_SwatchColorType ColorType
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.ColorType, out propvar);
                    if (hr.Succeeded)
                    {
                        uint result = (uint)propvar; //PropVariantToUInt32
                        UI_SWATCHCOLORTYPE retResult = (UI_SWATCHCOLORTYPE)result;
                        return (UI_SwatchColorType)retResult;
                    }
                }

                return (UI_SwatchColorType)_colorType.GetValueOrDefault();
            }
            set
            {
                _colorType = (UI_SWATCHCOLORTYPE)value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    propvar = (PROPVARIANT)(uint)value; //InitPropVariantFromUInt32
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.ColorType, propvar);
                }
            }
        }

        /// <summary>
        /// More colors label property
        /// </summary>
        public unsafe string MoreColorsLabel
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.MoreColorsLabel, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _moreColorsLabel;
            }
            set
            {
                _moreColorsLabel = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;    
                    if (!string.IsNullOrWhiteSpace(_moreColorsLabel))
                    {
                        UIPropVariant.UIInitPropertyFromString(_moreColorsLabel, out propvar);
                    }
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.MoreColorsLabel, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// No color label property
        /// </summary>
        public unsafe string NoColorLabel
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.NoColorLabel, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr);
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _noColorLabel;
            }
            set
            {
                _noColorLabel = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    if (!string.IsNullOrWhiteSpace(_noColorLabel))
                    {
                        UIPropVariant.UIInitPropertyFromString(_noColorLabel, out propvar);
                    }
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.NoColorLabel, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Recent colors category label property
        /// </summary>
        public unsafe string RecentColorsCategoryLabel
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.RecentColorsCategoryLabel, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _recentColorsCategoryLabel;
            }
            set
            {
                _recentColorsCategoryLabel = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    if (!string.IsNullOrWhiteSpace(_recentColorsCategoryLabel))
                    {
                        UIPropVariant.UIInitPropertyFromString(_recentColorsCategoryLabel, out propvar);
                    }
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.RecentColorsCategoryLabel, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Standard colors property
        /// </summary>
        public unsafe Color[] StandardColors
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.StandardColors, out propvar);
                    if (hr.Succeeded)
                    {
                        uint pcElem;
                        uint* pprgn = null;
                        hr = PInvoke.PropVariantToUInt32VectorAlloc(propvar, &pprgn, &pcElem);
                        int[] intStandardColors = new ReadOnlySpan<int>((int*)pprgn, (int)pcElem).ToArray();
                        PInvoke.CoTaskMemFree(pprgn);

                        //int[] intStandardColors = Array.ConvertAll<uint, int>(uintStandardColors, new Converter<uint, int>(Convert.ToInt32));
                        Color[] colorStandardColors = Array.ConvertAll<int, Color>(intStandardColors, new Converter<int, Color>(ColorTranslator.FromWin32));
                        propvar.Clear(); //PropVariantClear
                        return colorStandardColors;
                    }
                }

                return _standardColors;
            }
            set
            {
                _standardColors = value;
                if (_ribbon.Framework != null)
                {
                    int[] intStandardColors = Array.ConvertAll<Color, int>(_standardColors, new Converter<Color, int>(ColorTranslator.ToWin32));
                    uint[] uintStandardColors = Array.ConvertAll<int, uint>(intStandardColors, new Converter<int, uint>(Convert.ToUInt32));

                    PROPVARIANT propvar;
                    fixed (uint* prgn = uintStandardColors)
                        PInvoke.InitPropVariantFromUInt32Vector(prgn, (uint)uintStandardColors.Length, out propvar);

                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.StandardColors, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Standard colors category label property
        /// </summary>
        public unsafe string StandardColorsCategoryLabel
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.StandardColorsCategoryLabel, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _standardColorsCategoryLabel;
            }
            set
            {
                _standardColorsCategoryLabel = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    if (!string.IsNullOrWhiteSpace(_standardColorsCategoryLabel))
                    {
                        UIPropVariant.UIInitPropertyFromString(_standardColorsCategoryLabel, out propvar);
                    }
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.StandardColorsCategoryLabel, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Standard colors tooltips property
        /// </summary>
        public unsafe string[] StandardColorsTooltips
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.StandardColorsTooltips, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR* pprgsz;
                        uint pcElem;
                        hr = PInvoke.PropVariantToStringVectorAlloc(propvar, &pprgsz, &pcElem);
                        ReadOnlySpan<PWSTR> span = new ReadOnlySpan<PWSTR>(pprgsz, (int)pcElem);
                        string[] result = new string[span.Length];
                        for (int i = 0; i < result.Length; i++)
                            result[i] = new string(span[i]); //span[i].ToString();

                        for (uint u = 0; u < pcElem; u++)
                        {
                            PInvoke.CoTaskMemFree(pprgsz[u]);
                        }
                        PInvoke.CoTaskMemFree(pprgsz);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _standardColorsTooltips;
            }
            set
            {
                _standardColorsTooltips = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    PROPVARIANT.InitPropVariantFromStringVector(value, out propvar);
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.StandardColorsTooltips, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Theme colors property
        /// </summary>
        public unsafe Color[] ThemeColors
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.ThemeColors, out propvar);
                    if (hr.Succeeded)
                    {
                        uint pcElem;
                        uint* pprgn = null;
                        hr = PInvoke.PropVariantToUInt32VectorAlloc(propvar, &pprgn, &pcElem);
                        int[] intThemeColors = new ReadOnlySpan<int>((int*)pprgn, (int)pcElem).ToArray();
                        PInvoke.CoTaskMemFree(pprgn);

                        Color[] colorThemeColors = Array.ConvertAll<int, Color>(intThemeColors, new Converter<int, Color>(ColorTranslator.FromWin32));
                        propvar.Clear(); //PropVariantClear
                        return colorThemeColors;
                    }
                }

                return _themeColors;
            }
            set
            {
                _themeColors = value;
                if (_ribbon.Framework != null)
                {
                    int[] intThemeColors = Array.ConvertAll<Color, int>(_themeColors, new Converter<Color, int>(ColorTranslator.ToWin32));
                    uint[] uintThemeColors = Array.ConvertAll<int, uint>(intThemeColors, new Converter<int, uint>(Convert.ToUInt32));

                    PROPVARIANT propvar;
                    fixed (uint* prgn = uintThemeColors)
                        PInvoke.InitPropVariantFromUInt32Vector(prgn, (uint)uintThemeColors.Length, out propvar);

                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.ThemeColors, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Theme colors category label property
        /// </summary>
        public unsafe string ThemeColorsCategoryLabel
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.ThemeColorsCategoryLabel, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _themeColorsCategoryLabel;
            }
            set
            {
                _themeColorsCategoryLabel = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    if (!string.IsNullOrWhiteSpace(_themeColorsCategoryLabel))
                    {
                        UIPropVariant.UIInitPropertyFromString(_themeColorsCategoryLabel, out propvar);
                    }
                    HRESULT hr;
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.ThemeColorsCategoryLabel, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        /// <summary>
        /// Theme colors tooltips property
        /// </summary>
        public unsafe string[] ThemeColorsTooltips
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    hr = _ribbon.Framework.GetUICommandProperty(_commandId, RibbonProperties.ThemeColorsTooltips, out propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR* pprgsz;
                        uint pcElem;
                        hr = PInvoke.PropVariantToStringVectorAlloc(propvar, &pprgsz, &pcElem);
                        ReadOnlySpan<PWSTR> span = new ReadOnlySpan<PWSTR>(pprgsz, (int)pcElem);
                        string[] result = new string[span.Length];
                        for (int i = 0; i < result.Length; i++)
                            result[i] = new string(span[i]); //span[i].ToString();

                        for (uint u = 0; u < pcElem; u++)
                        {
                            PInvoke.CoTaskMemFree(pprgsz[u]);
                        }
                        PInvoke.CoTaskMemFree(pprgsz);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _themeColorsTooltips;
            }
            set
            {
                _themeColorsTooltips = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    PROPVARIANT.InitPropVariantFromStringVector(value, out propvar);
                    hr = _ribbon.Framework.SetUICommandProperty(_commandId, RibbonProperties.ThemeColorsTooltips, propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        #endregion
    }
}
