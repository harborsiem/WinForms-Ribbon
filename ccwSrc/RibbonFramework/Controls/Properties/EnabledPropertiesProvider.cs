//*****************************************************************************
//
//  File:       EnabledPropertiesProvider.cs
//
//  Contents:   Definition for enabled properties provider 
//
//*****************************************************************************

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for enabled properties provider interface
    /// </summary>
    public interface IEnabledPropertiesProvider
    {
        /// <summary>
        /// Enabled property
        /// </summary>
        bool Enabled { get; set; }
    }

    /// <summary>
    /// Implementation of IEnabledPropertiesProvider
    /// </summary>
    public sealed class EnabledPropertiesProvider : BasePropertiesProvider, IEnabledPropertiesProvider
    {
        /// <summary>
        /// EnabledPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        public EnabledPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.Enabled);
        }

        private bool? _enabled;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.Enabled)
            {
                if (_enabled.HasValue)
                {
                    newValue = (PROPVARIANT)_enabled.Value; //UIInitPropertyFromBoolean
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region IEnabledPropertiesProvider Members

        /// <summary>
        /// Enabled property
        /// </summary>
        public unsafe bool Enabled
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    HRESULT hr;
                    using var framework = _ribbon.Framework.GetInterface();
                    fixed (PROPERTYKEY* pKeyEnabled = &RibbonProperties.Enabled)
                        hr = framework.Value->GetUICommandProperty(_commandId, pKeyEnabled, &propvar);
                    if (hr.Succeeded)
                    {
                        bool result = (bool)propvar; //PropVariantToBoolean
                        return result;
                    }
                    if (_enabled == null)
                        _enabled = true;
                }

                return _enabled.GetValueOrDefault();
            }
            set
            {
                _enabled = value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    propvar = (PROPVARIANT)value; //UIInitPropertyFromBoolean
                    HRESULT hr;
                    using var framework = _ribbon.Framework.GetInterface();
                    fixed (PROPERTYKEY* pKeyEnabled = &RibbonProperties.Enabled)
                        hr = framework.Value->SetUICommandProperty(_commandId, pKeyEnabled, &propvar);
                }
            }
        }

        #endregion
    }
}
