//*****************************************************************************
//
//  File:       IPropertiesProvider.cs
//
//  Contents:   Interface for components that provides properties 
//
//*****************************************************************************

using System.Collections.Generic;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Interface for components that provides properties
    /// </summary>
    internal interface IPropertiesProvider
    {
        /// <summary>
        /// Get supported properties
        /// </summary>
        IList<PROPERTYKEY> SupportedProperties { get; }

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        unsafe HRESULT UpdateProperty(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue);
    }
}
