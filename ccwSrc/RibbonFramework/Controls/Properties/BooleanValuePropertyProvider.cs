//*****************************************************************************
//
//  File:       BooleanValuePropertyProvider.cs
//
//  Contents:   Definition for boolean value properties provider 
//
//*****************************************************************************

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for boolean value properties provider interface
    /// </summary>
    public interface IBooleanValuePropertyProvider
    {
        /// <summary>
        /// Boolean value property
        /// </summary>
        bool BooleanValue { get; set; }
    }

    /// <summary>
    /// Implementation of IBooleanValuePropertyProvider
    /// </summary>
    public sealed class BooleanValuePropertyProvider : BasePropertiesProvider, IBooleanValuePropertyProvider
    {
        /// <summary>
        /// BooleanValuePropertyProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        public BooleanValuePropertyProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.BooleanValue);
        }

        private bool? _booleanValue;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.BooleanValue)
            {
                if (_booleanValue.HasValue)
                {
                    newValue = (PROPVARIANT)_booleanValue.Value; //UIInitPropertyFromBoolean
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region IBooleanValuePropertyProvider Members

        /// <summary>
        /// Boolean value property
        /// </summary>
        public unsafe bool BooleanValue
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    HRESULT hr;
                    fixed (PROPERTYKEY* pKeyBooleanValue = &RibbonProperties.BooleanValue)
                        hr = _ribbon.Framework->GetUICommandProperty(_commandId, pKeyBooleanValue, &propvar);
                    if (hr.Succeeded)
                    {
                        bool result = (bool)propvar; //PropVariantToBoolean
                        return result;
                    }
                }
                return _booleanValue.GetValueOrDefault();
            }
            set
            {
                _booleanValue = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
                    HRESULT hr;
                    fixed (PROPERTYKEY* pKeyBooleanValue = &RibbonProperties.BooleanValue)
                        hr = _ribbon.Framework->SetUICommandProperty(_commandId, pKeyBooleanValue, &propvar);
                }
            }
        }

        #endregion
    }
}
