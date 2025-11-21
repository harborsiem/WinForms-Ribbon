using System;
using System.Collections.Generic;
using System.Text;
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
        internal static unsafe FontControlEventArgs? Create(object sender, ExecuteEventArgs e)
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
        internal static unsafe FontControlEventArgs Create(in PROPERTYKEY key, in PROPVARIANT currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            HRESULT hr;
            FontPropertyStore? fontStore = null;
            if (key == RibbonProperties.FontProperties)
            {
                IPropertyStore* cpPropertyStore;
                UIPropVariant.UIPropertyToInterface<IPropertyStore>(RibbonProperties.FontProperties, currentValue, out cpPropertyStore);
                //uint refCount;
                //cpPropertyStore->AddRef();
                //refCount = cpPropertyStore->Release();
                //Debug.WriteLine("Font IPropertyStore " + refCount);

                //refCount = 3 here. (native Framework + PROPVARIANT currentValue + UIPropertyToInterface cpPropertyStore)
                //refCount from (native Framework + PROPVARIANT) have to be released by Framework

                //currentValue.Clear(); //PropVariantClear ??? => no
                fontStore = new FontPropertyStore(cpPropertyStore);
                cpPropertyStore->Release();
            }
            Dictionary<FontPropertiesEnum, object> keys = new Dictionary<FontPropertiesEnum, object>();
            PROPVARIANT propvar;
            if (commandExecutionProperties != null)
            {
                fixed (PROPERTYKEY* pKeyFontProperties_ChangedProperties = &RibbonProperties.FontProperties_ChangedProperties)
                    hr = commandExecutionProperties->GetValue(pKeyFontProperties_ChangedProperties, &propvar);
                if (propvar.vt != VARENUM.VT_EMPTY)
                {
                    IPropertyStore* cpPropertyStore;
                    UIPropVariant.UIPropertyToInterface<IPropertyStore>(RibbonProperties.FontProperties_ChangedProperties, propvar, out cpPropertyStore);
                    propvar.Clear(); //PropVariantClear
                    Solution11(cpPropertyStore, keys);
                    cpPropertyStore->Release();
                }
            }
            FontControlEventArgs e = new FontControlEventArgs(fontStore, keys);
            return e;
        }

        //private static unsafe ChangedFontProperties Solution0(IPropertyStore store)
        //{
        //    HRESULT hr;
        //    uint count;
        //    PROPVARIANT propvar = PROPVARIANT.Empty;
        //    store.GetCount(out count);
        //    if (count == 0)
        //        return null;
        //    ChangedFontProperties changedFontProps = new ChangedFontProperties();
        //    for (uint i = 0; i < count; i++)
        //    {
        //        PROPERTYKEY key;
        //        hr = store.GetAt(i, &key);
        //        if (key == RibbonProperties.FontProperties_Family)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Family, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                PWSTR pwstr;
        //                hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
        //                changedFontProps.Family = new string(pwstr); // pwstr.ToString();
        //                PInvoke.CoTaskMemFree(pwstr);
        //                propvar.Clear(); //PropVariantClear
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Size)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Size, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.Size = (decimal)propvar;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Bold)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Bold, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.Bold = (FontProperties)(uint)propvar;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Italic)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Italic, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.Italic = (FontProperties)(uint)propvar;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Underline)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Underline, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.Underline = (FontUnderline)(uint)propvar;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Strikethrough)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Strikethrough, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.Strikethrough = (FontProperties)(uint)propvar;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_ForegroundColor)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_ForegroundColor, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.ForegroundColor = ColorTranslator.FromWin32((int)(uint)propvar);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_BackgroundColor)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_BackgroundColor, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.BackgroundColor = ColorTranslator.FromWin32((int)(uint)propvar);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_ForegroundColorType)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_ForegroundColorType, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                SwatchColorType colorType;
        //                if (propvar.vt == VARENUM.VT_I4)
        //                    colorType = (SwatchColorType)(int)propvar;
        //                else
        //                    colorType = (SwatchColorType)(uint)propvar;
        //                changedFontProps.ForegroundColorType = colorType;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_BackgroundColorType)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_BackgroundColorType, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                SwatchColorType colorType;
        //                if (propvar.vt == VARENUM.VT_I4)
        //                    colorType = (SwatchColorType)(int)propvar;
        //                else
        //                    colorType = (SwatchColorType)(uint)propvar;
        //                changedFontProps.BackgroundColorType = colorType;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_VerticalPositioning)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_VerticalPositioning, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.VerticalPositioning = (FontVerticalPosition)(uint)propvar;
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_DeltaSize)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_DeltaSize, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                changedFontProps.DeltaSize = (FontDeltaSize)(uint)propvar;
        //            }
        //        }
        //    }
        //    return changedFontProps;
        //}

        //private static unsafe void Solution1(IPropertyStore store, Dictionary<string, object> keys)
        //{
        //    HRESULT hr;
        //    uint count;
        //    PROPVARIANT propvar = PROPVARIANT.Empty;
        //    object objValue;
        //    store.GetCount(out count);
        //    for (uint i = 0; i < count; i++)
        //    {
        //        PROPERTYKEY key;
        //        hr = store.GetAt(i, &key);
        //        if (key == RibbonProperties.FontProperties_Family)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Family, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                PWSTR pwstr;
        //                hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
        //                objValue = new string(pwstr); // pwstr.ToString();
        //                PInvoke.CoTaskMemFree(pwstr);
        //                propvar.Clear(); //PropVariantClear
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Family), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Size)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Size, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (decimal)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Size), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Bold)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Bold, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (FontProperties)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Bold), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Italic)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Italic, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (FontProperties)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Italic), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Underline)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Underline, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (FontUnderline)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Underline), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_Strikethrough)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_Strikethrough, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (FontProperties)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Strikethrough), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_ForegroundColor)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_ForegroundColor, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = ColorTranslator.FromWin32((int)(uint)propvar);
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_ForegroundColor), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_BackgroundColor)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_BackgroundColor, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = ColorTranslator.FromWin32((int)(uint)propvar);
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_BackgroundColor), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_ForegroundColorType)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_ForegroundColorType, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                if (propvar.vt == VARENUM.VT_I4)
        //                    objValue = (SwatchColorType)(int)propvar;
        //                else
        //                    objValue = (SwatchColorType)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_ForegroundColorType), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_BackgroundColorType)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_BackgroundColorType, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                if (propvar.vt == VARENUM.VT_I4)
        //                    objValue = (SwatchColorType)(int)propvar;
        //                else
        //                    objValue = (SwatchColorType)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_BackgroundColorType), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_VerticalPositioning)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_VerticalPositioning, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (FontVerticalPosition)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_VerticalPositioning), objValue);
        //            }
        //        }
        //        else if (key == RibbonProperties.FontProperties_DeltaSize)
        //        {
        //            hr = store.GetValue(RibbonProperties.FontProperties_DeltaSize, out propvar);
        //            if (hr == HRESULT.S_OK)
        //            {
        //                objValue = (FontDeltaSize)(uint)propvar;
        //                keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_DeltaSize), objValue);
        //            }
        //        }
        //    }
        //}

        private static unsafe void Solution11(IPropertyStore* cpPropertyStore, Dictionary<FontPropertiesEnum, object> keys)
        {
            HRESULT hr;
            uint count;
            PROPVARIANT propvar = PROPVARIANT.Empty;
            object objValue;
            uint uintResult;

            cpPropertyStore->GetCount(&count);
            for (uint i = 0; i < count; i++)
            {
                PROPERTYKEY key;
                hr = cpPropertyStore->GetAt(i, &key);
                if (key == RibbonProperties.FontProperties_Family)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Family = &RibbonProperties.FontProperties_Family)
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_Family, &propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
                        objValue = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        keys.Add(FontPropertiesEnum.Family, objValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Size)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Size = &RibbonProperties.FontProperties_Size)
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_Size, &propvar);
                    if (hr == HRESULT.S_OK)
                    {
                        decimal decValue = (decimal)propvar; //UIPropertyToDecimal
                        keys.Add(FontPropertiesEnum.Size, decValue);
                    }
                }
                else if (key == RibbonProperties.FontProperties_Bold)
                {
                    fixed (PROPERTYKEY* pKeyFontProperties_Bold = &RibbonProperties.FontProperties_Bold)
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_Bold, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_Italic, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_Underline, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_Strikethrough, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_ForegroundColor, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_BackgroundColor, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_ForegroundColorType, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_BackgroundColorType, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_VerticalPositioning, &propvar);
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
                        hr = cpPropertyStore->GetValue(pKeyFontProperties_DeltaSize, &propvar);
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

        //private static unsafe void Solution2(IPropertyStore store, Dictionary<string, object> keys)
        //{
        //    HRESULT hr;
        //    object objValue;
        //    PROPVARIANT propvar = PROPVARIANT.Empty;
        //    hr = store.GetValue(RibbonProperties.FontProperties_BackgroundColor, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = ColorTranslator.FromWin32((int)(uint)propvar);
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_BackgroundColor), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_BackgroundColorType, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
        //            objValue = (SwatchColorType)(int)propvar;
        //        else
        //            objValue = (SwatchColorType)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_BackgroundColorType), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_Bold, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (FontProperties)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Bold), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_DeltaSize, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (FontDeltaSize)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_DeltaSize), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_Family, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        PWSTR pwstr;
        //        hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
        //        objValue = new string(pwstr); // pwstr.ToString();
        //        PInvoke.CoTaskMemFree(pwstr);
        //        propvar.Clear(); //PropVariantClear
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Family), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_ForegroundColor, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = ColorTranslator.FromWin32((int)(uint)propvar);
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_ForegroundColor), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_ForegroundColorType, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        if (propvar.vt == VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
        //            objValue = (SwatchColorType)(int)propvar;
        //        else
        //            objValue = (SwatchColorType)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_ForegroundColorType), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_Italic, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (FontProperties)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Italic), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_Size, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (decimal)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Size), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_Strikethrough, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (FontProperties)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Strikethrough), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_Underline, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (FontUnderline)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_Underline), objValue);
        //    }
        //    hr = store.GetValue(RibbonProperties.FontProperties_VerticalPositioning, out propvar);
        //    if (hr == HRESULT.S_OK)
        //    {
        //        objValue = (FontVerticalPosition)(uint)propvar;
        //        keys.Add(RibbonProperties.GetPropertyKeyName(RibbonProperties.FontProperties_VerticalPositioning), objValue);
        //    }
        //}

        ///// <summary>
        ///// Helper class that contains changed font properties.
        ///// Only changed properties have a non null value.
        ///// </summary>
        //public sealed class ChangedFontProperties
        //{
        //    /// <summary>
        //    /// The selected font family name.
        //    /// </summary>
        //    public string Family { get; internal set; }

        //    /// <summary>
        //    /// The size of the font.
        //    /// </summary>
        //    public decimal? Size { get; internal set; }

        //    /// <summary>
        //    /// Flag that indicates whether bold is selected.
        //    /// </summary>
        //    public FontProperties? Bold { get; internal set; }

        //    /// <summary>
        //    /// Flag that indicates whether italic is selected.
        //    /// </summary>
        //    public FontProperties? Italic { get; internal set; }

        //    /// <summary>
        //    /// Flag that indicates whether underline is selected.
        //    /// </summary>
        //    public FontUnderline? Underline { get; internal set; }

        //    /// <summary>
        //    /// Flag that indicates whether strikethrough is selected
        //    /// (sometimes called Strikeout).
        //    /// </summary>
        //    public FontProperties? Strikethrough { get; internal set; }

        //    /// <summary>
        //    /// Contains the text color if ForegroundColorType is set to RGB.
        //    /// The FontControl helper class expose this property as a .NET Color
        //    /// and handles internally the conversion to and from COLORREF structure.
        //    /// </summary>
        //    public Color? ForegroundColor { get; internal set; }

        //    /// <summary>
        //    /// The text color type. Valid values are RGB and Automatic. 
        //    /// If RGB is selected, the user should get the color from the ForegroundColor property. 
        //    /// If Automatic is selected the user should use SystemColors.WindowText.
        //    /// </summary>
        //    public SwatchColorType? ForegroundColorType { get; internal set; }

        //    /// <summary>
        //    /// Indicated whether the "Grow Font" or "Shrink Font" buttons were pressed.
        //    /// </summary>
        //    public FontDeltaSize? DeltaSize { get; internal set; }

        //    /// <summary>
        //    /// Contains the background color if BackgroundColorType is set to RGB.
        //    /// The FontControl helper class expose this property as a .NET Color
        //    /// and handles internally the conversion to and from COLORREF structure.
        //    /// </summary>
        //    public Color? BackgroundColor { get; internal set; }

        //    /// <summary>
        //    /// The background color type. Valid values are RGB and NoColor. 
        //    /// If RGB is selected, the user should get the color from the BackgroundColor property.
        //    /// If NoColor is selected the user should use SystemColors.Window.
        //    /// </summary>
        //    public SwatchColorType? BackgroundColorType { get; internal set; }

        //    /// <summary>
        //    /// Flag that indicates which one of the Subscript
        //    /// and Superscript buttons are selected, if any.
        //    /// </summary>
        //    public FontVerticalPosition? VerticalPositioning { get; internal set; }
        //}
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
