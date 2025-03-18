//*****************************************************************************
//
//  File:       CollectionChangedEventArgs.cs
//
//  Contents:   UI collection changed event arguments
//
//*****************************************************************************

using System;
using System.Runtime.InteropServices;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    /// <summary>
    /// 
    /// </summary>
    public sealed unsafe class CollectionChangedEventArgs : EventArgs
    {
        private UI_COLLECTIONCHANGE _action;
        private int _oldIndex;
        private int _newIndex;

        /// <summary>
        /// EventArgs when UICollection changed
        /// </summary>
        /// <param name="action"></param>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        internal CollectionChangedEventArgs(UI_COLLECTIONCHANGE action, uint oldIndex, uint newIndex)
        {
            _action = action;
            _oldIndex = (int)oldIndex;
            _newIndex = (int)newIndex;
        }

        /// <summary>
        /// Collection change action
        /// </summary>
        public UI_CollectionChange Action
        {
            get
            {
                return (UI_CollectionChange)_action;
            }
        }

        /// <summary>
        /// The old index
        /// </summary>
        public int OldIndex
        {
            get
            {
                return _oldIndex;
            }
        }

        ///// <summary>
        ///// The old item
        ///// </summary>
        //public object OldItem
        //{
        //    get
        //    {
        //        return _oldItem;
        //    }
        //}

        /// <summary>
        /// The new Index
        /// </summary>
        public int NewIndex
        {
            get
            {
                return _newIndex;
            }
        }

        ///// <summary>
        ///// The new Item
        ///// </summary>
        //public object NewItem
        //{
        //    get
        //    {
        //        return _newItem;
        //    }
        //}

        ///// <summary>
        ///// @Todo
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <returns></returns>
        //public AbstractPropertySet GetPropertySet(object sender)
        //{
        //    if (sender is CollectionItem item)
        //    {
        //        if (Action == UI_CollectionChange.Insert)
        //        {
        //            if (item.CollectionType == CollectionType.QatItemsSource)
        //            {
        //                RibbonQuickAccessToolbar toolbar = (RibbonQuickAccessToolbar)item.Sender;
        //                QatCommandPropertySet set = toolbar.QatItemsSource[NewIndex];
        //            }
        //        }

        //    }
        //    return null;

        //}
    }
}
