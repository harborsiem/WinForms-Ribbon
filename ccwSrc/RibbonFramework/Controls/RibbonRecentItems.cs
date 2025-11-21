//*****************************************************************************
//
//  File:       RibbonRecentItems.cs
//
//  Contents:   Helper class that wraps a ribbon recent items.
//
//*****************************************************************************

using System;
using System.Collections.Generic;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a ribbon recent items.
    /// </summary>
    public sealed class RibbonRecentItems : RibbonStripItem,
        IRecentItemsPropertiesProvider,
        IKeytipPropertiesProvider,
        IExecuteEventsProvider
    {
        private static readonly EventKey s_SelectedChangedKey = new EventKey();
        private static readonly EventKey s_PinnedChangedKey = new EventKey();
        private KeytipPropertiesProvider _keytipPropertiesProvider;
        private RecentItemsPropertiesProvider _recentItemsPropertiesProvider;
        private ExecuteEventsProvider _executeEventsProvider;

        /// <summary>
        /// Initializes a new instance of the Ribbon RecentItems
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonRecentItems(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            AddPropertiesProvider(_recentItemsPropertiesProvider = new RecentItemsPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_keytipPropertiesProvider = new KeytipPropertiesProvider(ribbon, commandId, this));

            AddEventsProvider(_executeEventsProvider = new ExecuteEventsProvider(this));
        }

        /// <summary>
        /// Invalidate RecentItems
        /// </summary>
        public unsafe void Invalidate()
        {
            HRESULT hr;
            if (Ribbon.Framework != null)
            {
                fixed (PROPERTYKEY* pKeyRecentItems = &RibbonProperties.RecentItems)
                    hr = Ribbon.Framework->InvalidateUICommand(CommandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pKeyRecentItems);
            }
        }

        /// <summary>
        /// Put a RecentItemsPropertySet at top of the list.
        /// Strategy is descriped in Microsoft Windows Ribbon
        /// </summary>
        /// <param name="propertySet"></param>
        /// <returns>successful if true</returns>
        public bool PutElement(RecentItemsPropertySet propertySet)
        {
            int maxCount = MaxCount;
            if (maxCount == -1)
                maxCount = 10; //default value in xml
            int count = RecentItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (propertySet == RecentItems[i])
                {
                    RecentItems.RemoveAt(i);
                    count--;
                    break;
                }
            }
            if (count + 1 > maxCount)
            {
                //Strategie for removing an item
                for (int i = count - 1; i >= 0; i--)
                {
                    if (!RecentItems[i].Pinned)
                    {
                        RecentItems.RemoveAt(i);
                        count--;
                        break;
                    }
                }
            }
            if (count < maxCount)
            {
                RecentItems.Insert(0, propertySet);
                //Invalidate();
                return true;
            }
            return false;
        }

        #region IRecentItemsPropertiesProvider Members

        /// <summary>
        /// This property contains the list of the recent items.
        /// </summary>
        public IList<RecentItemsPropertySet> RecentItems
        {
            get
            {
                return _recentItemsPropertiesProvider.RecentItems;
            }
        }

        /// <summary>
        /// This property contains the maximum count of recent items.
        /// This is configured in the RibbonMarkup file
        /// The value is available after first showing the file menu
        /// Init value = -1
        /// </summary>
        public int MaxCount
        {
            get
            {
                return _recentItemsPropertiesProvider.MaxCount;
            }
        }

        #endregion

        #region IKeytipPropertiesProvider Members

        /// <summary>
        /// The keytip or key sequence that is used to access the command using the Alt key.
        /// This keytip appears when the user presses the Alt key to navigate the ribbon.
        /// The Ribbon Framework will automatically apply keytips to every command.
        /// However, if you want more control over the keytips used, you can specify them yourself.
        /// A keytip is not limited to a single character.
        /// </summary>
        public string? Keytip
        {
            get
            {
                return _keytipPropertiesProvider.Keytip;
            }
            set
            {
                _keytipPropertiesProvider.Keytip = value;
            }
        }

        #endregion

        #region IExecuteEventsProvider Members

        /// <summary>
        /// Event provider similar to a Click event.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IExecuteEventsProvider.ExecuteEvent
        {
            add
            {
                _executeEventsProvider.ExecuteEvent += value;
            }
            remove
            {
                _executeEventsProvider.ExecuteEvent -= value;
            }
        }

        #endregion

        /// <summary>
        /// Event provider similar to a SelectedItemChanged event.
        /// </summary>
        public event EventHandler<SelectedRecentEventArgs>? SelectedChanged
        {
            add { EventSet.Add(s_SelectedChangedKey, value); }
            remove { EventSet.Remove(s_SelectedChangedKey, value); }
        }

        /// <summary>
        /// Event provider when Pinned changed by UI user.
        /// </summary>
        public event EventHandler<PinnedChangedEventArgs>? PinnedChanged
        {
            add { EventSet.Add(s_PinnedChangedKey, value); }
            remove { EventSet.Remove(s_PinnedChangedKey, value); }
        }

        private protected override unsafe HRESULT OnExecute(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            if (*key == RibbonProperties.SelectedItem)
            {
                SelectedRecentEventArgs eventArgs = SelectedRecentEventArgs.Create(this, *key, *currentValue, commandExecutionProperties);
                EventSet.Raise(s_SelectedChangedKey, this, eventArgs);
            }
            if (*key == RibbonProperties.RecentItems)
            {
                PinnedChangedEventArgs eventArgs = PinnedChangedEventArgs.Create(this, *key, *currentValue, commandExecutionProperties);
                EventSet.Raise(s_PinnedChangedKey, this, eventArgs);
            }
            return HRESULT.S_OK;
        }
    }
}
