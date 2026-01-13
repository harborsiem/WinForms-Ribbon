//*****************************************************************************
//
//  File:       ContextAvailablePropertiesProvider.cs
//
//  Contents:   Definition for context available properties provider 
//
//*****************************************************************************

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for context available properties interface
    /// </summary>
    public interface IContextAvailablePropertiesProvider
    {
        /// <summary>
        /// Context available property
        /// </summary>
        ContextAvailability ContextAvailable { get; set; }
    }

    /// <summary>
    /// Implementation of IContextAvailablePropertiesProvider
    /// </summary>
    public sealed class ContextAvailablePropertiesProvider : BasePropertiesProvider, IContextAvailablePropertiesProvider
    {
        /// <summary>
        /// ContextAvailablePropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        public ContextAvailablePropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.ContextAvailable);
        }

        private UI_CONTEXTAVAILABILITY? _contextAvailable;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.ContextAvailable)
            {
                if (_contextAvailable.HasValue)
                {
                    newValue = (PROPVARIANT)(uint)_contextAvailable.Value; //InitPropVariantFromUInt32
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region IContextAvailablePropertiesProvider Members

        /// <summary>
        /// Context available property
        /// </summary>
        public unsafe ContextAvailability ContextAvailable
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar;
                    HRESULT hr;
                    using var framework = _ribbon.Framework.GetInterface();
                    fixed (PROPERTYKEY* pKeyContextAvailable = &RibbonProperties.ContextAvailable)
                        hr = framework.Value->GetUICommandProperty(_commandId, pKeyContextAvailable, &propvar);
                    if (hr.Succeeded)
                    {
                        uint result;
                        if (propvar.vt == Windows.Win32.System.Variant.VARENUM.VT_I4) //@ seems to be a bug in UIRibbon
                            result = (uint)(int)propvar;
                        else
                            result = (uint)propvar; //PropVariantToUInt32
                        UI_CONTEXTAVAILABILITY retResult = (UI_CONTEXTAVAILABILITY)result;
                        return (ContextAvailability)retResult;
                    }
                }

                return (ContextAvailability)_contextAvailable.GetValueOrDefault();
            }
            set
            {
                _contextAvailable = (UI_CONTEXTAVAILABILITY)value;
                if (_ribbon.Framework != null)
                {
                    PROPVARIANT propvar = (PROPVARIANT)(uint)value; //InitPropVariantFromUInt32
                    HRESULT hr;
                    using var framework = _ribbon.Framework.GetInterface();
                    fixed (PROPERTYKEY* pKeyContextAvailable = &RibbonProperties.ContextAvailable)
                        hr = framework.Value->SetUICommandProperty(_commandId, pKeyContextAvailable, &propvar);
                }
            }
        }

        #endregion
    }
}
