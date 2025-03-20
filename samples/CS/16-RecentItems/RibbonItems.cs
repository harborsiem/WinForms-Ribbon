using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        private IList<RecentItemsPropertySet> _recentItems;

        public void Init()
        {
            RecentItems.SelectedChanged += new EventHandler<SelectedRecentEventArgs>(_recentItems_ExecuteEvent);
            RecentItems.PinnedChanged += RecentItems_PinnedChanged;
        }

        private void RecentItems_PinnedChanged(object sender, PinnedChangedEventArgs e)
        {
            {
                for (int i = 0; i < e.ChangedPinnedIndices.Count; ++i)
                {
                    RecentItemsPropertySet propSet = RecentItems.RecentItems[i];
                    string label = propSet.Label;
                    string labelDescription = propSet.LabelDescription;
                    bool pinned = propSet.Pinned;
                }
            }
        }

        private void InitRecentItems()
        {
            // prepare list of recent items
            _recentItems = RecentItems.RecentItems;
            _recentItems.Add(new RecentItemsPropertySet()
            {
                Label = "Recent item 1",
                LabelDescription = "Recent item 1 description",
                Pinned = true
            });
            _recentItems.Add(new RecentItemsPropertySet()
            {
                Label = "Recent item 2",
                LabelDescription = "Recent item 2 description",
                Pinned = false
            });
        }

        void _recentItems_ExecuteEvent(object sender, SelectedRecentEventArgs e)
        {
            int selectedItem = e.SelectedItem.SelectedItemIndex;
            RecentItemsPropertySet propertySet = e.SelectedItem.PropertySet;
            string label = propertySet.Label;
            string labelDescription = propertySet.LabelDescription;
            bool pinned = propertySet.Pinned;
            int maxCount = RecentItems.MaxCount;
            MessageBox.Show("Selected Recent index: " + e.SelectedItem.SelectedItemIndex + Environment.NewLine + "MaxCount: " + maxCount);
        }

        public void Load()
        {
            InitRecentItems();
        }
    }
}
