//*****************************************************************************
//
//  File:       StringValuePropertiesProvider.cs
//
//  Contents:   Definition for string value Properties provider 
//
//*****************************************************************************

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for string value properties provider interface
    /// </summary>
    public interface IStringValuePropertiesProvider
    {
        /// <summary>
        /// String value property
        /// </summary>
        string StringValue { get; set; }
    }

    /// <summary>
    /// Implementation of IStringValuePropertiesProvider
    /// </summary>
    public sealed unsafe class StringValuePropertiesProvider : BasePropertiesProvider, IStringValuePropertiesProvider
    {
        /// <summary>
        /// StringValuePropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        public StringValuePropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.StringValue);
        }

        private string _stringValue;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.StringValue)
            {
                if (_stringValue != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_stringValue, out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region IStringValuePropertiesProvider Members

        /// <summary>
        /// String value property
        /// </summary>
        public unsafe string StringValue
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    HRESULT hr;
                    fixed (PROPERTYKEY* pStringValue = &RibbonProperties.StringValue)
                        hr = _ribbon.Framework->GetUICommandProperty(_commandId, pStringValue, &propvar);
                    if (hr.Succeeded)
                    {
                        PWSTR pwstr;
                        hr = UIPropVariant.UIPropertyToStringAlloc(&propvar, &pwstr);
                        string result = new string(pwstr); // pwstr.ToString();
                        PInvoke.CoTaskMemFree(pwstr);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return _stringValue;
            }
            set
            {
                _stringValue = value;

                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = PROPVARIANT.Empty;
                    if (!string.IsNullOrWhiteSpace(_stringValue))
                    {
                        UIPropVariant.UIInitPropertyFromString(_stringValue, out propvar);
                    }
                    HRESULT hr;
                    fixed (PROPERTYKEY* pStringValue = &RibbonProperties.StringValue)
                        hr = _ribbon.Framework->SetUICommandProperty(_commandId, pStringValue, &propvar);
                    propvar.Clear(); //PropVariantClear
                }
            }
        }

        #endregion
    }
}
