using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    internal sealed unsafe class UIApplication : IUIApplication.Interface, IUICommandHandler.Interface, IManagedWrapper<IUIApplication, IUICommandHandler>
    {
        private RibbonStrip _ribbon;

        public UIApplication(RibbonStrip ribbon)
        {
            _ribbon = ribbon;
        }

        /// <summary>
        /// Implementation of IUICommandHandler.Execute
        /// Responds to execute events on Commands bound to the Command handler
        /// </summary>
        /// <param name="commandId">the command that has been executed</param>
        /// <param name="verb">the mode of execution</param>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        /// <remarks>This method is used internally by the RibbonStrip class and should not be called by the user.</remarks>
        HRESULT IUICommandHandler.Interface.Execute(uint commandId, Windows.Win32.UI.Ribbon.UI_EXECUTIONVERB verb, Windows.Win32.UI.Shell.PropertiesSystem.PROPERTYKEY* key, Windows.Win32.System.Com.StructuredStorage.PROPVARIANT* currentValue, Windows.Win32.UI.Ribbon.IUISimplePropertySet* commandExecutionProperties)
        {
#if DEBUG
            Debug.WriteLine(string.Format("Execute verb: {0} for command {1}", verb, commandId));
#endif
            RibbonStripItem? ribbonItem;
            if (_ribbon.TryGetRibbonControlById(commandId, out ribbonItem))
            {
                ICommandHandler commands = (ribbonItem as ICommandHandler)!;
                return commands.Execute(verb, key, currentValue, commandExecutionProperties);
            }

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Implementation of IUICommandHandler.UpdateProperty
        /// Responds to property update requests from the Windows Ribbon (Ribbon) framework. 
        /// </summary>
        /// <param name="commandId">The ID for the Command, which is specified in the Markup resource file</param>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        /// <remarks>This method is used internally by the RibbonStrip class and should not be called by the user.</remarks>
        HRESULT IUICommandHandler.Interface.UpdateProperty(uint commandId, Windows.Win32.UI.Shell.PropertiesSystem.PROPERTYKEY* key, Windows.Win32.System.Com.StructuredStorage.PROPVARIANT* currentValue, Windows.Win32.System.Com.StructuredStorage.PROPVARIANT* newValue)
        {
#if DEBUG
            Debug.WriteLine(string.Format("UpdateProperty key: {0} for command {1}", RibbonProperties.GetPropertyKeyName(*key), commandId));
#endif
            RibbonStripItem? ribbonItem;
            if (_ribbon.TryGetRibbonControlById(commandId, out ribbonItem))
            {
                ICommandHandler commands = (ribbonItem as ICommandHandler)!;
                return commands.UpdateProperty(*key, currentValue, out *newValue);
            }
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Called for each Command specified in the Windows Ribbon framework markup to bind 
        /// the Command to an IUICommandHandler. 
        /// </summary>
        /// <param name="commandId">The ID for the Command, which is specified in the markup resource file.</param>
        /// <param name="typeID">The Command type that is associated with a specific control.</param>
        /// <param name="commandHandler">When this method returns, contains the address of a pointer to an
        /// IUICommandHandler object. This object is a host application Command handler that is bound to one or 
        /// more Commands.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
        HRESULT IUIApplication.Interface.OnCreateUICommand(uint commandId, Windows.Win32.UI.Ribbon.UI_COMMANDTYPE typeID, Windows.Win32.UI.Ribbon.IUICommandHandler** commandHandler)
        {
            RibbonStripItem? ribbonItem;
            if (_ribbon.TryGetRibbonControlById(commandId, out ribbonItem))
            {
                if (ribbonItem != null)
                    ribbonItem.RaiseCreateUICommand(commandId, typeID);
            }
            *commandHandler = ComHelpers.GetComScope<IUICommandHandler>(this);
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Called for each Command specified in the Windows Ribbon framework markup when the 
        /// application window is destroyed. 
        /// </summary>
        /// <param name="commandId">The ID for the Command, which is specified in the markup resource file.</param>
        /// <param name="typeID">The Command type that is associated with a specific control.</param>
        /// <param name="commandHandler">A pointer to an IUICommandHandler object. This value can be null.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
        HRESULT IUIApplication.Interface.OnDestroyUICommand(uint commandId, Windows.Win32.UI.Ribbon.UI_COMMANDTYPE typeID, Windows.Win32.UI.Ribbon.IUICommandHandler* commandHandler)
        {
            RibbonStripItem? ribbonItem;
            if (_ribbon.TryGetRibbonControlById(commandId, out ribbonItem))
            {
                if (ribbonItem != null)
                    ribbonItem.RaiseDestroyUICommand(commandId, typeID);
            }
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Called when the state of a View changes
        /// </summary>
        /// <param name="viewId">The ID for the View. Only a value of 0 is valid.</param>
        /// <param name="typeID">The UI_VIEWTYPE hosted by the application.</param>
        /// <param name="view">A pointer to the View interface.</param>
        /// <param name="verb">The UI_VIEWVERB (or action) performed by the View.</param>
        /// <param name="uReasonCode">Not defined.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
        HRESULT IUIApplication.Interface.OnViewChanged(uint viewId, Windows.Win32.UI.Ribbon.UI_VIEWTYPE typeID, Windows.Win32.System.Com.IUnknown* view, Windows.Win32.UI.Ribbon.UI_VIEWVERB verb, int uReasonCode)
        {
            HRESULT hr = HRESULT.E_FAIL;

            // Checks to see if the view that was changed was a Ribbon view.
            if (typeID == UI_VIEWTYPE.UI_VIEWTYPE_RIBBON)
            {
                using ComScope<IUIRibbon> uiRibbonScope = ComScope<IUIRibbon>.QueryFrom(view);
                switch (verb)
                {
                    // The view was newly created
                    case UI_VIEWVERB.UI_VIEWVERB_CREATE:
                        if (!uiRibbonScope.IsNull)
                        {
                            _ribbon.BeginInvoke(new MethodInvoker(_ribbon.OnViewCreated));
                            hr = HRESULT.S_OK;
                        }
                        break;

                    // The view has been resized. For the Ribbon view, the application should
                    // call GetHeight to determine the height of the ribbon.
                    case UI_VIEWVERB.UI_VIEWVERB_SIZE:
                        uint uRibbonHeight;
                        // Call to the framework to determine the desired height of the Ribbon.
                        hr = uiRibbonScope.Value->GetHeight(&uRibbonHeight);

                        if (hr.Failed)
                        {
                            // error
                        }
                        else
                        {
                            _ribbon.Height = (int)uRibbonHeight;
                            _ribbon.BeginInvoke(new MethodInvoker(_ribbon.OnRibbonHeightChanged));
                        }
                        break;

                    // The view was destroyed.
                    case UI_VIEWVERB.UI_VIEWVERB_DESTROY:

                        if (!uiRibbonScope.IsNull)
                        {
                            _ribbon.Invoke(new MethodInvoker(_ribbon.OnViewDestroy));
                            hr = HRESULT.S_OK;
                        }
                        break;

                    default:
                        break;
                }
            }
            return hr;
        }
    }
}
