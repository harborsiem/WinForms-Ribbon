//*****************************************************************************
//
//  File:       IEventsProvider.cs
//
//  Contents:   Interface for components that provides events 
//
//*****************************************************************************

using System.Collections.Generic;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Interface for components that provides events
    /// </summary>
    internal interface IEventsProvider
    {
        /// <summary>
        /// Get supported "execution verbs", or events
        /// </summary>
        IList<UI_EXECUTIONVERB> SupportedEvents { get; }

        /// <summary>
        /// Handles IUICommandHandler.Execute function for supported events
        /// </summary>
        /// <param name="verb">the mode of execution</param>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        unsafe HRESULT Execute(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties);
    }
}
