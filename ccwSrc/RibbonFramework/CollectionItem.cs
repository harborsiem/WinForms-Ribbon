using System;
using System.Collections.Generic;
using System.Text;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class for sender parameter in UICollectionChangedEvent.ChangedEvent 
    /// </summary>
    public class CollectionItem
    {
        /// <summary>
        /// This is a RibbonStrip item which contains UICollections
        /// RibbonComboBox, RibbonQuickAccessToolbar and all Galleries (DropDown, InRibbon, SplitButton) 
        /// </summary>
        public readonly object Sender;

        /// <summary>
        /// The CollectionType (Categories or ItemsSource)
        /// </summary>
        public readonly CollectionType CollectionType;

        internal CollectionItem(IRibbonControl ribbonItem, CollectionType collectionType)
        {
            Sender = ribbonItem;
            CollectionType = collectionType;
        }
    }
}
