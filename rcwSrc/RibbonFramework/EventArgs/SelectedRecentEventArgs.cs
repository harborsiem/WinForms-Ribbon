using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;

namespace WinForms.Ribbon
{
    //If the user selected an item then we get the key RibbonProperties.SelectedItem
    /// <summary>
    /// The EventArgs for RibbonRecentItems
    /// </summary>
    public sealed class SelectedRecentEventArgs : EventArgs
    {
        new internal static SelectedRecentEventArgs Empty => new SelectedRecentEventArgs(null);

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="selected"></param>
        private SelectedRecentEventArgs(SelectedItem<RecentItemsPropertySet>? selected)
        {
            SelectedItem = selected;
        }

        /// <summary>
        /// SelectedRecentItem, can be null
        /// </summary>
        public SelectedItem<RecentItemsPropertySet>? SelectedItem { get; private set; }

        /// <summary>
        /// Creates a SelectedRecentEventArgs from ExecuteEventArgs of a RibbonRecentItems event
        /// </summary>
        /// <param name="sender">Parameters from event: sender = RibbonControl</param>
        /// <param name="e">Parameters from event: ExecuteEventArgs</param>
        /// <returns></returns>
        public static SelectedRecentEventArgs? Create(object sender, ExecuteEventArgs e)
        {
            if (!(sender is RibbonRecentItems recentItems))
                throw new ArgumentException("Not a RibbonRecentItems", nameof(sender));
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            if (e.Key.HasValue && e.CurrentValue.HasValue)
            {
                return Create(recentItems, e.Key.Value, e.CurrentValue.Value, e.CommandExecutionProperties);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ribbonRecentItems"></param>
        /// <param name="key"></param>
        /// <param name="currentValue"></param>
        /// <param name="commandExecutionProperties"></param>
        /// <returns></returns>
        internal static unsafe SelectedRecentEventArgs Create(RibbonRecentItems ribbonRecentItems, in PROPERTYKEY key, in PROPVARIANT currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            SelectedItem<RecentItemsPropertySet>? selectedRecentItem = null;
            if (key == RibbonProperties.SelectedItem)
            {
                // get selected item index
                uint uSelectedItemIndex = (uint)currentValue; //PropVariantToUInt32
                int selectedItemIndex = (int)uSelectedItemIndex;
                RecentItemsPropertySet propSet = ribbonRecentItems.RecentItems[selectedItemIndex];

                selectedRecentItem = new SelectedItem<RecentItemsPropertySet>(selectedItemIndex, propSet);
            }
            SelectedRecentEventArgs e = new SelectedRecentEventArgs(selectedRecentItem);
            return e;
        }
    }
}
