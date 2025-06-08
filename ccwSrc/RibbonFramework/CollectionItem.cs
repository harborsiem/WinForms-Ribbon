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
        /// This is a <see cref="IRibbonControl"/> which contains UICollections
        /// <see cref="RibbonComboBox"/>, <see cref="RibbonQuickAccessToolbar"/> and 
        /// all Galleries (<see cref="RibbonDropDownGallery"/>, <see cref="RibbonInRibbonGallery"/>, <see cref="RibbonSplitButtonGallery"/>) 
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
