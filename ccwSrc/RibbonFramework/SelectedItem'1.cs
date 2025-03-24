using System;
using System.Collections.Generic;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Infos about the selected Item
    /// </summary>
    /// <typeparam name="T">RecentItemsPropertySet, GalleryItemPropertySet</typeparam>
    public sealed class SelectedItem<T> where T : class
    {
        internal SelectedItem(int selectedItemIndex, T propertySet)
        {
            SelectedItemIndex = selectedItemIndex;
            PropertySet = propertySet;
        }

        /// <summary>
        /// The selected Item index
        /// </summary>
        public readonly int SelectedItemIndex;

        /// <summary>
        /// The selected PropertySet
        /// </summary>
        public readonly T PropertySet;
    }
}
