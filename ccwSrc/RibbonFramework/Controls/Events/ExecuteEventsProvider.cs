#define unusedActivate
//*****************************************************************************
//
//  File:       ExecuteEventsProvider.cs
//
//  Contents:   Definition for execute events provider 
//
//*****************************************************************************

using System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for Execute events provider
    /// </summary>
    internal interface IExecuteEventsProvider
    {
        /// <summary>
        /// Execute event
        /// </summary>
        event EventHandler<ExecuteEventArgs>? ExecuteEvent;
    }

    /// <summary>
    /// Implementation of IExecuteEventsProvider
    /// </summary>
    internal sealed class ExecuteEventsProvider : BaseEventsProvider, IExecuteEventsProvider
    {
        private readonly static EventKey s_ExecuteProviderKey = new EventKey();

        /// <summary>
        /// Initializes a new instance of the ExecuteEventsProvider
        /// </summary>
        /// <param name="ribbonItem"></param>
        public ExecuteEventsProvider(RibbonStripItem ribbonItem) : base(ribbonItem)
        {
            ((IEventsProvider)this).SupportedEvents.Add(UI_EXECUTIONVERB.UI_EXECUTIONVERB_EXECUTE);
        }

        /// <summary>
        /// Handles IUICommandHandler.Execute function for supported events
        /// </summary>
        /// <param name="verb">the mode of execution</param>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT ExecuteImpl(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            if (verb == UI_EXECUTIONVERB.UI_EXECUTIONVERB_EXECUTE)
            {
                _ribbonItem.RaiseExecute(key, currentValue, commandExecutionProperties);
#if InternalEvent
                try
                {
                //    _ribbonItem.Ribbon.Invoke((MethodInvoker)delegate
                //    {
                //        _ribbonItem.OnExecute(key, currentValue, commandExecutionProperties);
                //    });
                    _ribbonItem.EventSet.Raise(s_ExecuteProviderKey, _ribbonItem, new ExecuteEventArgs(key, currentValue, commandExecutionProperties));
                }
                catch (Exception ex)
                {
                    return _ribbonItem.EventExceptionHandler(ex);
                }
#endif
            }

            return HRESULT.S_OK;
        }

        #region IExecuteEventsProvider Members

        /// <summary>
        /// Execute event
        /// </summary>
        public event EventHandler<ExecuteEventArgs>? ExecuteEvent
        {
            add { _ribbonItem.EventSet.Add(s_ExecuteProviderKey, value); }
            remove { _ribbonItem.EventSet.Remove(s_ExecuteProviderKey, value); }
        }

        #endregion
    }
}
