//*****************************************************************************
//
//  File:       SpinnerPropertiesProvider.cs
//
//  Contents:   Definition for spinner properties provider 
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
    /// Definition for spinner properties provider interface
    /// </summary>
    public interface ISpinnerPropertiesProvider
    {
        /// <summary>
        /// Decimal value property
        /// </summary>
        decimal DecimalValue { get; set; }

        /// <summary>
        /// Increment property
        /// </summary>
        decimal Increment { get; set; }

        /// <summary>
        /// Max value property
        /// </summary>
        decimal MaxValue { get; set; }

        /// <summary>
        /// Min value property
        /// </summary>
        decimal MinValue { get; set; }

        /// <summary>
        /// Decimal places property
        /// </summary>
        uint DecimalPlaces { get; set; }

        /// <summary>
        /// Format string property
        /// </summary>
        string FormatString { get; set; }
    }

    /// <summary>
    /// Implementation of ISpinnerPropertiesProvider
    /// </summary>
    public sealed unsafe class SpinnerPropertiesProvider : BasePropertiesProvider, ISpinnerPropertiesProvider
    {
        /// <summary>
        /// SpinnerPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        public SpinnerPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.DecimalValue);
            _supportedProperties.Add(RibbonProperties.Increment);
            _supportedProperties.Add(RibbonProperties.MaxValue);
            _supportedProperties.Add(RibbonProperties.MinValue);
            _supportedProperties.Add(RibbonProperties.DecimalPlaces);
            _supportedProperties.Add(RibbonProperties.FormatString);
        }

        private decimal? _decimalValue;
        private decimal? _increment;
        private decimal? _maxValue;
        private decimal? _minValue;
        private uint? _decimalPlaces;
        private string _formatString;

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
            if (key == RibbonProperties.DecimalValue)
            {
                if (_decimalValue.HasValue)
                {
                    newValue = (PROPVARIANT)_decimalValue.Value; //UIInitPropertyFromDecimal
                    _decimalValue = null;
                }
            }
            else if (key == RibbonProperties.Increment)
            {
                if (_increment.HasValue)
                {
                    newValue = (PROPVARIANT)_increment.Value; //UIInitPropertyFromDecimal
                }
            }
            else if (key == RibbonProperties.MaxValue)
            {
                if (_maxValue.HasValue)
                {
                    newValue = (PROPVARIANT)_maxValue.Value; //UIInitPropertyFromDecimal
                }
            }
            else if (key == RibbonProperties.MinValue)
            {
                if (_minValue.HasValue)
                {
                    newValue = (PROPVARIANT)_minValue.Value; //UIInitPropertyFromDecimal
                }
            }
            else if (key == RibbonProperties.DecimalPlaces)
            {
                if (_decimalPlaces.HasValue)
                {
                    newValue = (PROPVARIANT)_decimalPlaces.Value;
                }
            }
            else if (key == RibbonProperties.FormatString)
            {
                if (_formatString != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_formatString, out newValue);
                }
            }

            return HRESULT.S_OK;
        }

        #region ISpinnerPropertiesProvider Members

        /// <summary>
        /// Decimal value property
        /// </summary>
        public decimal DecimalValue
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    HRESULT hr;
                    fixed (PROPERTYKEY* pDecimalValue = &RibbonProperties.DecimalValue)
                        hr = _ribbon.Framework->GetUICommandProperty(_commandId, pDecimalValue, &propvar);
                    if (hr.Succeeded)
                    {
                        decimal decValue = (decimal)propvar; //UIPropertyToDecimal
                        return decValue;
                    }
                }

                return _decimalValue.GetValueOrDefault();
            }
            set
            {
                _decimalValue = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = (PROPVARIANT)value; //UIInitPropertyFromDecimal
                    HRESULT hr;
                    fixed (PROPERTYKEY* pDecimalValue = &RibbonProperties.DecimalValue)
                        hr = _ribbon.Framework->SetUICommandProperty(_commandId, pDecimalValue, &propvar);
                }
            }
        }

        /// <summary>
        /// Increment property
        /// </summary>
        public decimal Increment
        {
            get
            {
                return _increment.GetValueOrDefault();
            }
            set
            {
                _increment = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pIncrement = &RibbonProperties.Increment)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pIncrement);
                }
            }
        }

        /// <summary>
        /// Max value property
        /// </summary>
        public decimal MaxValue
        {
            get
            {
                return _maxValue.GetValueOrDefault();
            }
            set
            {
                _maxValue = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pMaxValue = &RibbonProperties.MaxValue)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pMaxValue);
                }
            }
        }

        /// <summary>
        /// Min value property
        /// </summary>
        public decimal MinValue
        {
            get
            {
                return _minValue.GetValueOrDefault();
            }
            set
            {
                _minValue = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pMinValue = &RibbonProperties.MinValue)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pMinValue);
                }
            }
        }

        /// <summary>
        /// Decimal places property
        /// </summary>
        public uint DecimalPlaces
        {
            get
            {
                return _decimalPlaces.GetValueOrDefault();
            }
            set
            {
                _decimalPlaces = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pDecimalPlaces = &RibbonProperties.DecimalPlaces)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pDecimalPlaces);
                }
            }
        }

        /// <summary>
        /// Format string property
        /// </summary>
        public string FormatString
        {
            get
            {
                return _formatString;
            }
            set
            {
                _formatString = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pFormatString = &RibbonProperties.FormatString)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pFormatString);
                }
            }
        }

        #endregion
    }
}
