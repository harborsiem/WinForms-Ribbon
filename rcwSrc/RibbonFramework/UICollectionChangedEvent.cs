//*****************************************************************************
//
//  File:       UICollectionChangedEvent.cs
//
//  Contents:   Helper class that exposes an OnChanged event for a given 
//              IUICollection instance.
//
//*****************************************************************************

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that exposes an OnChanged event for a given 
    /// IUICollection instance.
    /// </summary>
    public sealed class UICollectionChangedEvent<T> : IUICollectionChangedEvent where T : AbstractPropertySet, new()
    {
        private static readonly Guid IIDGuidIUICollectionChangedEvent = typeof(IUICollectionChangedEvent).GUID;
        private readonly UICollection<T> _collection;
        private int _cookie;

        internal UICollectionChangedEvent(UICollection<T> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Attach to an IUICollection object events
        /// </summary>
        internal void Attach()
        {
            if (_cookie != 0)
            {
                Detach();
            }

            _cookie = RegisterComEvent();
        }

        /// <summary>
        /// Detach from the previous IUICollection object events
        /// </summary>
        internal void Detach()
        {
            if (_cookie != 0)
            {
                UnregisterComEvent(_cookie);
                _cookie = 0;
            }
        }

        private IConnectionPoint? GetConnectionPoint()
        {
            // get connection point container from IUICollection
            IConnectionPointContainer cpIConnectionPointContainer = (IConnectionPointContainer)_collection.CpIUICollection;

            // get connection point for IUICollectionChangedEvent
            Guid guid = IIDGuidIUICollectionChangedEvent;
            IConnectionPoint? cpIConnectionPoint;
            cpIConnectionPointContainer.FindConnectionPoint(ref guid, out cpIConnectionPoint);

            return cpIConnectionPoint;
        }

        private int RegisterComEvent()
        {
            IConnectionPoint? cpIConnectionPoint = GetConnectionPoint();

            int cookie = 0;
            cpIConnectionPoint?.Advise(this, out cookie);

            return cookie;
        }

        private void UnregisterComEvent(int cookie)
        {
            IConnectionPoint? cpIConnectionPoint = GetConnectionPoint();

            cpIConnectionPoint?.Unadvise(cookie);
        }

        #region IUICollectionChangedEvent Members

        HRESULT IUICollectionChangedEvent.OnChanged(UI_COLLECTIONCHANGE action, uint oldIndex, object oldItem, uint newIndex, object newItem)
        {
            _collection.OnChanged(action, oldIndex, oldItem, newIndex, newItem);

            //call to the user ChangedEvent
            CollectionChangedEventArgs e = new CollectionChangedEventArgs(action, oldIndex, newIndex);
            _collection.InvokeOnChanged(e);

            return HRESULT.S_OK;
        }

        #endregion

    }
}
