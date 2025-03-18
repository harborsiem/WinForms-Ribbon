//*****************************************************************************
//
//  File:       BaseEventsProvider.cs
//
//  Contents:   Base class for all events provider classes.
//              provides common members like: SupportedEvents.
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
    /// Base class for all events provider classes.
    /// provides common members like: SupportedEvents.
    /// </summary>
    internal abstract class BaseEventsProvider : IEventsProvider
    {
        /// <summary>
        /// list of supported events
        /// </summary>
        private List<UI_EXECUTIONVERB> _supportedEvents = new List<UI_EXECUTIONVERB>();

        /// <summary>
        /// 
        /// </summary>
        protected readonly RibbonStripItem _ribbonItem;

        /// <summary>
        /// Initializes a new instance of the BaseEventsProvider
        /// </summary>
        protected BaseEventsProvider(RibbonStripItem ribbonItem)
        {
            _ribbonItem = ribbonItem;
        }

        #region IEventsProvider Members

        /// <summary>
        /// Get supported "execution verbs", or events
        /// </summary>
        IList<UI_EXECUTIONVERB> IEventsProvider.SupportedEvents
        {
            get
            {
                return _supportedEvents;
            }
        }

        private protected abstract unsafe HRESULT ExecuteImpl(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties);

        /// <summary>
        /// Handles IUICommandHandler.Execute function for supported events
        /// </summary>
        /// <param name="verb">the mode of execution</param>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        unsafe HRESULT IEventsProvider.Execute(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            return ExecuteImpl(verb, key, currentValue, commandExecutionProperties);
        }

        #endregion
    }
}
