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
        private IUICollection? _cpIUICollection;
        private readonly UICollection<T> _collection;
        private int _cookie;

        internal UICollectionChangedEvent(UICollection<T> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Attach to an IUICollection object events
        /// </summary>
        /// <param name="cpIUICollection">IUICollection object</param>
        internal void Attach(IUICollection cpIUICollection)
        {
            if (_cookie != 0)
            {
                Detach();
            }

            _cpIUICollection = cpIUICollection;

            _cookie = RegisterComEvent(cpIUICollection);
        }

        /// <summary>
        /// Detach from the previous IUICollection object events
        /// </summary>
        internal void Detach()
        {
            if (_cookie != 0)
            {
                UnregisterComEvent(_cpIUICollection, _cookie);
                //_cpIUICollection = null;
                _cookie = 0;
            }
        }

        private IConnectionPoint? GetConnectionPoint(IUICollection cpIUICollection)
        {
            // get connection point container
            IConnectionPointContainer cpIConnectionPointContainer = (IConnectionPointContainer)cpIUICollection;

            // get connection point for IUICollectionChangedEvent
            Guid guid = IIDGuidIUICollectionChangedEvent;
            IConnectionPoint? cpIConnectionPoint;
            cpIConnectionPointContainer.FindConnectionPoint(ref guid, out cpIConnectionPoint);

            return cpIConnectionPoint;
        }

        private int RegisterComEvent(IUICollection cpIUICollection)
        {
            IConnectionPoint? cpIConnectionPoint = GetConnectionPoint(cpIUICollection);

            int cookie = 0;
            cpIConnectionPoint?.Advise(this, out cookie);

            return cookie;
        }

        private void UnregisterComEvent(IUICollection cpIUICollection, int cookie)
        {
            IConnectionPoint? cpIConnectionPoint = GetConnectionPoint(cpIUICollection);

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
