using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;

namespace WinForms.Ribbon
{
    /// <summary>
    /// The EventArgs for RibbonFontControl
    /// </summary>
    public sealed class FontControlEventArgs : EventArgs
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="currentFontStore"></param>
        /// <param name="changedFontValues"></param>
        private FontControlEventArgs(FontPropertyStore? currentFontStore, Dictionary<FontPropertiesEnum, object> changedFontValues)
        {
            CurrentFontStore = currentFontStore;
            ChangedFontValues = changedFontValues;
        }

        /// <summary>
        /// Current Font Properties Store
        /// </summary>
        public FontPropertyStore? CurrentFontStore { get; private set; }

        /// <summary>
        /// The changed values, can be null
		/// Key is an enum for the Font control properties defined at the end of this class
		/// like Family, Size, ...
        /// </summary>
        public Dictionary<FontPropertiesEnum, object> ChangedFontValues { get; private set; }

        /// <summary>
        /// Creates a FontControlEventArgs from ExecuteEventArgs of a RibbonFontControl event
        /// </summary>
        /// <param name="sender">Parameter from event: sender</param>
        /// <param name="e">Parameters from event: ExecuteEventArgs</param>
        /// <returns></returns>
        internal static FontControlEventArgs? Create(object sender, ExecuteEventArgs e)
        {
            if (!(sender is RibbonFontControl))
                throw new ArgumentException("Not a RibbonFontControl", nameof(sender));
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            if (e.Key.HasValue && e.CurrentValue.HasValue)
            {
                return Create(e.Key.Value, e.CurrentValue.Value, e.CommandExecutionProperties);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentValue"></param>
        /// <param name="commandExecutionProperties"></param>
        /// <returns></returns>
        internal static unsafe FontControlEventArgs Create(in PROPERTYKEY key, in PROPVARIANT currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            HRESULT hr;
            FontPropertyStore? fontStore = null;
            if (key == RibbonProperties.FontProperties)
            {
                IPropertyStore cpPropertyStore;
                UIPropVariant.UIPropertyToInterface<IPropertyStore>(RibbonProperties.FontProperties, currentValue, out cpPropertyStore!);
                //currentValue.Clear(); //PropVariantClear ??? => no
                fontStore = new FontPropertyStore(cpPropertyStore);
                Marshal.ReleaseComObject(cpPropertyStore);
            }
            Dictionary<FontPropertiesEnum, object> keys = new Dictionary<FontPropertiesEnum, object>();
            PROPVARIANT propvar;
            if (commandExecutionProperties != null)
            {
                fixed (PROPERTYKEY* pKeyFontProperties_ChangedProperties = &RibbonProperties.FontProperties_ChangedProperties)
                    hr = commandExecutionProperties.GetValue(pKeyFontProperties_ChangedProperties, out propvar);
                if (propvar.vt != VARENUM.VT_EMPTY)
                {
                    IPropertyStore cpPropertyStore;
                    UIPropVariant.UIPropertyToInterface<IPropertyStore>(RibbonProperties.FontProperties_ChangedProperties, propvar, out cpPropertyStore!);
                    propvar.Clear(); //PropVariantClear
                    Solution11(cpPropertyStore, keys);
                    Marshal.ReleaseComObject(cpPropertyStore);
                }
            }
            FontControlEventArgs e = new FontControlEventArgs(fontStore, keys);
            return e;
        }

        private static unsafe void Solution11(IPropertyStore cpPropertyStore, Dictionary<FontPropertiesEnum, object> keys)
        {
            HRESULT hr;
            uint count;
            PROPVARIANT propvar = PROPVARIANT.Empty;
            object objValue;
            uint uintResult;

            cpPropertyStore.GetCount(out count);
            for (uint i = 0; i < count; i++)
            {
                PROPERTYKEY key;
                hr = cpPropertyStore.GetAt(i, &key);
                if (key == RibbonProperties.FontProperties_Family)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Family = &RibbonProperties.FontProperties_Family)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_Family, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(propvar, out pwstr);
                        objValue = pwstr.ToStringAndCoTaskMemFree()!;
                        propvar.Clear(); //PropVariantClear
                        keys.Add(FontPropertiesEnum.Family, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Size)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Size = &RibbonProperties.FontProperties_Size)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_Size, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        decimal decValue = (decimal)propvar; //UIPropertyToDecimal
                        keys.Add(FontPropertiesEnum.Size, decValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Bold)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Bold = &RibbonProperties.FontProperties_Bold)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_Bold, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (FontProperties)(UI_FONTPROPERTIES)uintResult;
                        keys.Add(FontPropertiesEnum.Bold, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Italic)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Italic = &RibbonProperties.FontProperties_Italic)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_Italic, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (FontProperties)(UI_FONTPROPERTIES)uintResult;
                        keys.Add(FontPropertiesEnum.Italic, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Underline)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Underline = &RibbonProperties.FontProperties_Underline)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_Underline, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (FontUnderline)(UI_FONTUNDERLINE)uintResult;
                        keys.Add(FontPropertiesEnum.Underline, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Strikethrough)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Strikethrough = &RibbonProperties.FontProperties_Strikethrough)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_Strikethrough, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (FontProperties)(UI_FONTPROPERTIES)uintResult;
                        keys.Add(FontPropertiesEnum.Strikethrough, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_ForegroundColor)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColor = &RibbonProperties.FontProperties_ForegroundColor)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_ForegroundColor, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = ColorTranslator.FromWin32((int)uintResult);
                        keys.Add(FontPropertiesEnum.ForegroundColor, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_BackgroundColor)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColor = &RibbonProperties.FontProperties_BackgroundColor)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_BackgroundColor, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = ColorTranslator.FromWin32((int)uintResult);
                        keys.Add(FontPropertiesEnum.BackgroundColor, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_ForegroundColorType)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_ForegroundColorType = &RibbonProperties.FontProperties_ForegroundColorType)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_ForegroundColorType, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                            uintResult = (uint)(int)propvar;
                        else
                            uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (SwatchColorType)(UI_SWATCHCOLORTYPE)uintResult;
                        keys.Add(FontPropertiesEnum.ForegroundColorType, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_BackgroundColorType)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_BackgroundColorType = &RibbonProperties.FontProperties_BackgroundColorType)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_BackgroundColorType, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                            uintResult = (uint)(int)propvar;
                        else
                            uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (SwatchColorType)(UI_SWATCHCOLORTYPE)uintResult;
                        keys.Add(FontPropertiesEnum.BackgroundColorType, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_VerticalPositioning)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_VerticalPositioning = &RibbonProperties.FontProperties_VerticalPositioning)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_VerticalPositioning, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        uintResult = (uint)propvar; //PropVariantToUInt32
                        objValue = (FontVerticalPosition)(UI_FONTVERTICALPOSITION)uintResult;
                        keys.Add(FontPropertiesEnum.VerticalPositioning, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_DeltaSize)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_DeltaSize = &RibbonProperties.FontProperties_DeltaSize)
                        hr = cpPropertyStore.GetValue(pKeyFontProperties_DeltaSize, out propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        if (!propvar.IsEmpty)
                        {
                            uintResult = (uint)propvar; //PropVariantToUInt32
                            objValue = (FontDeltaSize)(UI_FONTDELTASIZE)uintResult;
                            keys.Add(FontPropertiesEnum.DeltaSize, objValue);
                        }
                    }
                }
            }
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public enum FontPropertiesEnum
    {
        None = 0,
        Family = 301,
        Size,
        Bold,
        Italic,
        Underline,
        Strikethrough,
        VerticalPositioning,
        ForegroundColor,
        BackgroundColor,
        ForegroundColorType,
        BackgroundColorType,
        ChangedProperties,
        DeltaSize,
    }
#pragma warning restore CS1591
}
