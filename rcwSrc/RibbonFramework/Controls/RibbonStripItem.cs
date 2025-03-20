using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Base class for all ribbon control helper classes, provides 
    /// common functionality for ribbon controls.
    /// </summary>
    public abstract class RibbonStripItem : IRibbonControl, ICommandHandler
    {
        private protected static readonly EventKey s_CreateUICommandKey = new EventKey();
        private protected static readonly EventKey s_DestroyUICommandKey = new EventKey();
        private protected static readonly EventKey s_PreviewKey = new EventKey();
        private protected static readonly EventKey s_CancelPreviewKey = new EventKey();

        private readonly EventSet _eventSet;

        private string? _name;

        /// <summary>
        /// reference for parent ribbon class
        /// </summary>
        private RibbonStrip _ribbon;

        /// <summary>
        /// reference for parent ribbon class
        /// </summary>
        public RibbonStrip Ribbon => _ribbon;

        /// <summary>
        /// ribbon control command id
        /// </summary>
        private uint _commandId;

        /// <summary>
        /// map of registered properties for this ribbon control
        /// </summary>
        private protected Dictionary<PROPERTYKEY, IPropertiesProvider> _mapProperties = new Dictionary<PROPERTYKEY, IPropertiesProvider>();

        /// <summary>
        /// map of registered events for this ribbon control
        /// </summary>
        private protected Dictionary<UI_EXECUTIONVERB, IEventsProvider> _mapEvents = new Dictionary<UI_EXECUTIONVERB, IEventsProvider>();

        private protected EventSet EventSet => _eventSet;

        /// <summary>
        /// RibbonStripItem ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">command id attached to this control</param>
        protected RibbonStripItem(RibbonStrip ribbon, uint commandId)
        {
            _ribbon = ribbon;
            _commandId = commandId;
            _eventSet = new EventSet();

            ribbon.AddRibbonControl(this);
        }

        /// <summary>
        /// Register a properties provider with this ribbon control
        /// </summary>
        /// <param name="propertiesProvider">properties provider</param>
        private protected void AddPropertiesProvider(IPropertiesProvider propertiesProvider)
        {
            foreach (PROPERTYKEY propertyKey in propertiesProvider.SupportedProperties)
            {
                _mapProperties[propertyKey] = propertiesProvider;
            }
        }

        /// <summary>
        /// Register an events provider with this ribbon control
        /// </summary>
        /// <param name="eventsProvider">events provider</param>
        private protected void AddEventsProvider(IEventsProvider eventsProvider)
        {
            foreach (UI_EXECUTIONVERB verb in eventsProvider.SupportedEvents)
            {
                _mapEvents[verb] = eventsProvider;
            }
        }

        /// <summary>
        /// Handles IUICommandHandler.Execute function for this ribbon control, called by ExecuteEventsProvider
        /// </summary>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        internal virtual unsafe HRESULT OnExecute(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Handles IUICommandHandler.Execute function for this ribbon control, called by PreviewEventsProvider
        /// </summary>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <param name="cancel">cancel = true: CancelPreview</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        internal virtual unsafe HRESULT OnPreview(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties, bool cancel)
        {
            return HRESULT.S_OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        internal HRESULT EventExceptionHandler(Exception ex)
        {
            ThreadExceptionEventArgs e = new ThreadExceptionEventArgs(ex);
            if (Ribbon.OnRibbonEventException(this, e))
                return HRESULT.E_FAIL;
            Environment.FailFast(ex.StackTrace);
            return HRESULT.E_ABORT;
        }

        /// <summary>
        /// The name of RibbonStripItem
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    return string.Empty;
                return _name!;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _name = null;
                else
                    _name = value;
            }
        }

        #region IRibbonControl Members

        /// <summary>
        /// The Command Id, member of IRibbonControl
        /// </summary>
        public uint CommandId
        {
            get
            {
                return _commandId;
            }
        }

        private protected virtual unsafe HRESULT ExecuteImpl(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            // check if verb is registered with this ribbon control
            if (_mapEvents.ContainsKey(verb))
            {
                // find events provider
                IEventsProvider eventsProvider = _mapEvents[verb];

                // delegates execution to events provider
                return eventsProvider.Execute(verb, key, currentValue, commandExecutionProperties);
            }

            Debug.WriteLine(string.Format("Class {0} does not support verb: {1}.", GetType(), verb));
            return HRESULT.E_NOTIMPL;
        }

        /// <summary>
        /// Handles IUICommandHandler.Execute function for this ribbon control
        /// </summary>
        /// <param name="verb">the mode of execution</param>
        /// <param name="key">the property that has changed</param>
        /// <param name="currentValue">the new value of the property that has changed</param>
        /// <param name="commandExecutionProperties">additional data for this execution</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        unsafe HRESULT ICommandHandler.Execute(UI_EXECUTIONVERB verb, PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            return ExecuteImpl(verb, key, currentValue, commandExecutionProperties);
        }

        private protected virtual unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            fixed (PROPVARIANT* newValueLocal = &newValue) { }
            // check if property is registered with this ribbon control
            if (_mapProperties.ContainsKey(key))
            {
                // find property provider
                IPropertiesProvider propertiesProvider = _mapProperties[key];

                // delegates execution to property provider
                return propertiesProvider.UpdateProperty(key, currentValue, out newValue);
            }

            Debug.WriteLine(string.Format("Class {0} does not support property: {1}.", GetType(), RibbonProperties.GetPropertyKeyName(key)));
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for this ribbon control
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        unsafe HRESULT ICommandHandler.UpdateProperty(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            return UpdatePropertyImpl(key, currentValue, out newValue);
        }

        /// <summary>
        /// Gets or sets the object that contains data about the control
        /// </summary>
        public object? Tag { get; set; }

        /// <summary>
        /// The CommandType of the Control
        /// If the CommandType is UI_CommandType.Unknown (UI_COMMANDTYPE.UI_COMMANDTYPE_UNKNOWN) then the Control is not initialized by the Framework
        /// </summary>
        public CommandType CommandType { get; internal set; }

        #endregion

        /// <summary>
        /// Set the RESID's in ctor to get the strings after view is created.
        /// For unused parameters 0 must be set.
        /// </summary>
        /// <param name="keytip"></param>
        /// <param name="label"></param>
        /// <param name="labelDescription"></param>
        /// <param name="tooltipTitle"></param>
        /// <param name="tooltipDescription"></param>
        public void SetResIds(ushort keytip, ushort label, ushort labelDescription, ushort tooltipTitle, ushort tooltipDescription)
        {
            bool exist;
            IPropertiesProvider? iProvider;
            if (keytip >= 2)
            {
                exist = _mapProperties.TryGetValue(RibbonProperties.Keytip, out iProvider);
                if (exist && iProvider is KeytipPropertiesProvider keytipProvider)
                {
                    keytipProvider.KeytipResId = keytip;
                }
            }
            if (label >= 2)
            {
                exist = _mapProperties.TryGetValue(RibbonProperties.Label, out iProvider);
                if (exist && iProvider is LabelPropertiesProvider labelProvider)
                {
                    labelProvider.LabelResId = label;
                }
            }
            if (labelDescription >= 2)
            {
                exist = _mapProperties.TryGetValue(RibbonProperties.LabelDescription, out iProvider);
                if (exist && iProvider is LabelDescriptionPropertiesProvider labelDescProvider)
                {
                    labelDescProvider.LabelDescriptionResId = labelDescription;
                }
            }
            exist = _mapProperties.TryGetValue(RibbonProperties.TooltipTitle, out iProvider);
            if (exist && iProvider is TooltipPropertiesProvider provider)
            {
                if (tooltipTitle >= 2)
                {
                    provider.TooltipTitleResId = tooltipTitle;
                }
                if (tooltipDescription >= 2)
                {
                    provider.TooltipDescriptionResId = tooltipDescription;
                }
            }
        }

        /// <summary>
        /// Method is called by IUIApplication.OnCreateUICommand
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="typeID"></param>
        internal void BaseCreateUICommand(uint commandId, UI_COMMANDTYPE typeID)
        {
            CommandType = (CommandType)typeID;
            Ribbon.Invoke((MethodInvoker)delegate
            {
                OnCreateUICommand(commandId, typeID);
            });
            //OnCreateUICommand(commandId, typeID);
        }

        /// <summary>
        /// Method is called by IUIApplication.OnDestroyUICommand
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="typeID"></param>
        internal void BaseDestroyUICommand(uint commandId, UI_COMMANDTYPE typeID)
        {
            Ribbon.Invoke((MethodInvoker)delegate
            {
                OnDestroyUICommand(commandId, typeID);
            });
            //OnDestroyUICommand(commandId, typeID);
        }

        private protected virtual void OnCreateUICommand(uint commandId, UI_COMMANDTYPE typeID)
        {
            EventSet.Raise(s_CreateUICommandKey, this, EventArgs.Empty);
        }

        private protected virtual void OnDestroyUICommand(uint commandId, UI_COMMANDTYPE typeID)
        {
            EventSet.Raise(s_DestroyUICommandKey, this, EventArgs.Empty);
        }

        internal void OnViewCreated()
        {
            uint resId;
            bool exist;
            IPropertiesProvider? iProvider;
            exist = _mapProperties.TryGetValue(RibbonProperties.Keytip, out iProvider);
            if (exist && iProvider is KeytipPropertiesProvider keyTipProvider)
            {
                resId = keyTipProvider.KeytipResId;
                if (resId >= 2)
                    keyTipProvider.InitKeytip(_ribbon.LoadString(resId));
            }
            exist = _mapProperties.TryGetValue(RibbonProperties.Label, out iProvider);
            if (exist && iProvider is LabelPropertiesProvider labelProvider)
            {
                resId = labelProvider.LabelResId;
                if (resId >= 2)
                    labelProvider.InitLabel(_ribbon.LoadString(resId));
            }
            exist = _mapProperties.TryGetValue(RibbonProperties.LabelDescription, out iProvider);
            if (exist && iProvider is LabelDescriptionPropertiesProvider labelDescProvider)
            {
                resId = labelDescProvider.LabelDescriptionResId;
                if (resId >= 2)
                    labelDescProvider.InitLabelDescription(_ribbon.LoadString(resId));
            }
            exist = _mapProperties.TryGetValue(RibbonProperties.TooltipTitle, out iProvider);
            if (exist && iProvider is TooltipPropertiesProvider provider)
            {
                resId = provider.TooltipTitleResId;
                if (resId >= 2)
                    provider.InitTooltipTitle(_ribbon.LoadString(resId));

                resId = provider.TooltipDescriptionResId;
                if (resId >= 2)
                    provider.InitTooltipDescription(_ribbon.LoadString(resId));
            }
        }

        /// <summary>
        /// Control is created by the Ribbon Framework
        /// </summary>
        public event EventHandler<EventArgs> CreateUICommand
        {
            add { _eventSet.Add(s_CreateUICommandKey, value); }
            remove { _eventSet.Remove(s_CreateUICommandKey, value); }
        }

        /// <summary>
        /// Control is destroyed by the Ribbon Framework
        /// </summary>
        public event EventHandler<EventArgs> DestroyUICommand
        {
            add { _eventSet.Add(s_DestroyUICommandKey, value); }
            remove { _eventSet.Remove(s_DestroyUICommandKey, value); }
        }
    }
}
