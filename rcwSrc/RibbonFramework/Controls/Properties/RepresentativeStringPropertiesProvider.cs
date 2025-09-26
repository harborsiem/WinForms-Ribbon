//*****************************************************************************
//
//  File:       RepresentativeStringPropertiesProvider.cs
//
//  Contents:   Definition for representative string properties provider 
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
    /// Definition for representative string properties provider interface
    /// </summary>
    public interface IRepresentativeStringPropertiesProvider
    {
        /// <summary>
        /// Representative string property
        /// </summary>
        string RepresentativeString { get; set; }
    }

    /// <summary>
    /// Implementation of IRepresentativeStringPropertiesProvider
    /// </summary>
    public sealed class RepresentativeStringPropertiesProvider : BasePropertiesProvider, IRepresentativeStringPropertiesProvider
    {
        /// <summary>
        /// RepresentativeStringPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        public RepresentativeStringPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        { 
            // add supported properties
            _supportedProperties.Add(RibbonProperties.RepresentativeString);
        }

        private string _representativeString;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.RepresentativeString)
            {
                if (_representativeString != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_representativeString, out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region IRepresentativeStringPropertiesProvider Members

        /// <summary>
        /// Representative string property
        /// </summary>
        public string RepresentativeString
        {
            get
            {
                return _representativeString;
            }
            set
            {
                _representativeString = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, RibbonProperties.RepresentativeString);
                }
            }
        }

        #endregion
    }
}
