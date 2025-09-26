//*****************************************************************************
//
//  File:       BasePropertiesProvider.cs
//
//  Contents:   Base class for all properties provider classes.
//              provides common members like: _ribbon, _commandID 
//              and _supportedProperties.
//
//*****************************************************************************

using System.Collections.Generic;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32;
using System.Buffers;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Base class for all properties provider classes.
    /// provides common members like: _ribbon, _commandId 
    /// and _supportedProperties.
    /// </summary>
    public abstract class BasePropertiesProvider : IPropertiesProvider
    {
        /// <summary>
        /// reference for parent RibbonStrip class
        /// </summary>
        protected readonly RibbonStrip _ribbon;

        /// <summary>
        /// ribbon control command id
        /// </summary>
        protected readonly uint _commandId;

        /// <summary>
        /// list of supported properties
        /// </summary>
        private protected List<PROPERTYKEY> _supportedProperties = new List<PROPERTYKEY>();

        /// <summary>
        /// BasePropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">command id attached to this control</param>
        protected BasePropertiesProvider(RibbonStrip ribbon, uint commandId)
        {
            _ribbon = ribbon;
            _commandId = commandId;
        }

        #region IPropertiesProvider Members

        /// <summary>
        /// Get supported properties
        /// </summary>
        IList<PROPERTYKEY> IPropertiesProvider.SupportedProperties
        {
            get
            {
                return _supportedProperties;
            }
        }

        private protected abstract unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue);

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        unsafe HRESULT IPropertiesProvider.UpdateProperty(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            return UpdatePropertyImpl(key, currentValue, out newValue);
            //fixed (PROPVARIANT* newValueLocal = &newValue) { }
            //return HRESULT.S_OK;
        }

        #endregion

    }
}
