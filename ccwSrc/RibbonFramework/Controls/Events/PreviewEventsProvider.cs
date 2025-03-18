//*****************************************************************************
//
//  File:       PreviewEventsProvider.cs
//
//  Contents:   definition for preview events provider 
//
//*****************************************************************************

using System;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for Preview and CancelPreview events provider
    /// </summary>
    internal interface IPreviewEventsProvider
    {
        /// <summary>
        /// Preview event
        /// </summary>
        event EventHandler<ExecuteEventArgs>? PreviewEvent;

        /// <summary>
        /// Cancel Preview event
        /// </summary>
        event EventHandler<ExecuteEventArgs>? CancelPreviewEvent;
    }

    /// <summary>
    /// Implementation of IPreviewEventsProvider
    /// </summary>
    internal sealed unsafe class PreviewEventsProvider : BaseEventsProvider, IPreviewEventsProvider
    {
        public PreviewEventsProvider(RibbonStripItem ribbonItem) : base(ribbonItem)
        {
            ((IEventsProvider)this).SupportedEvents.Add(UI_EXECUTIONVERB.UI_EXECUTIONVERB_PREVIEW);
            ((IEventsProvider)this).SupportedEvents.Add(UI_EXECUTIONVERB.UI_EXECUTIONVERB_CANCELPREVIEW);
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
            switch (verb)
            {
                case UI_EXECUTIONVERB.UI_EXECUTIONVERB_PREVIEW:
                    try
                    {
                        _ribbonItem.Ribbon.Invoke((MethodInvoker)delegate
                        {
                            _ribbonItem.OnPreview(key, currentValue, commandExecutionProperties, false);
                        });
                        //_ribbonItem.OnPreview(key, currentValue, commandExecutionProperties, false);
                        if (PreviewEvent != null)
                        {
                            PreviewEvent(_ribbonItem, new ExecuteEventArgs(key, currentValue, commandExecutionProperties));
                        }
                    }
                    catch (Exception ex)
                    {
                        return _ribbonItem.EventExceptionHandler(ex);
                    }
                    break;

                case UI_EXECUTIONVERB.UI_EXECUTIONVERB_CANCELPREVIEW:
                    try
                    {
                        _ribbonItem.Ribbon.Invoke((MethodInvoker)delegate
                        {
                            _ribbonItem.OnPreview(key, currentValue, commandExecutionProperties, true);
                        });
                        //_ribbonItem.OnPreview(key, currentValue, commandExecutionProperties, true);
                        if (CancelPreviewEvent != null)
                        {
                            CancelPreviewEvent(_ribbonItem, new ExecuteEventArgs(key, currentValue, commandExecutionProperties));
                        }
                    }
                    catch (Exception ex)
                    {
                        return _ribbonItem.EventExceptionHandler(ex);
                    }
                    break;
            }
            return HRESULT.S_OK;
        }

        #region IPreviewEventsProvider Members

        /// <summary>
        /// Preview event
        /// </summary>
        public event EventHandler<ExecuteEventArgs>? PreviewEvent;

        /// <summary>
        /// Cancel Preview event
        /// </summary>
        public event EventHandler<ExecuteEventArgs>? CancelPreviewEvent;

        #endregion
    }
}
