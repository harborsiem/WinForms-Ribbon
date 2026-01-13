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
//using System.Runtime.InteropServices.ComTypes;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that exposes an OnChanged event for a given 
    /// IUICollection instance.
    /// </summary>
    public sealed unsafe class UICollectionChangedEvent<T> : IUICollectionChangedEvent.Interface where T : AbstractPropertySet, new()
    {
        private readonly UICollection<T> _collection;
        private uint _cookie;

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

        private ComScope<IConnectionPoint> GetConnectionPoint()
        {
            // get connection point container from IUICollection
            using var connectionPointContainerScope = _collection.CpIUICollection.GetInterface<IConnectionPointContainer>();

            // get connection point for IUICollectionChangedEvent
            ComScope<IConnectionPoint> cpIConnectionPoint = new ComScope<IConnectionPoint>(null);
            connectionPointContainerScope.Value->FindConnectionPoint(IID.Get<IUICollectionChangedEvent>(), cpIConnectionPoint);

            return cpIConnectionPoint;
        }

        private uint RegisterComEvent()
        {
            using var connectionPointScope = GetConnectionPoint();

            uint cookie = 0;
            using ComScope<IUnknown> punkSink = ComHelpers.GetComScope<IUnknown>(this);
            connectionPointScope.Value->Advise(punkSink, &cookie);
            return cookie;
        }

        private void UnregisterComEvent(uint cookie)
        {
            using var cpConnectionPointScope = GetConnectionPoint();
            cpConnectionPointScope.Value->Unadvise(cookie);
        }

        #region IUICollectionChangedEvent Members

        HRESULT IUICollectionChangedEvent.Interface.OnChanged(UI_COLLECTIONCHANGE action, uint oldIndex, IUnknown* oldItem, uint newIndex, IUnknown* newItem)
        {
            //IUISimplePropertySet* cpSimpleOldItem;
            //oldItem->QueryInterface(IID.Get<IUISimplePropertySet>(), (void**)&cpSimpleOldItem);
            //IUISimplePropertySet* cpSimpleNewItem;
            //oldItem->QueryInterface(IID.Get<IUISimplePropertySet>(), (void**)&cpSimpleNewItem);
            //uint refCount;
            //refCount = 2 here
            //if (newItem != null)
            //{
            //    newItem->AddRef();
            //    refCount = newItem->Release();
            //}

            _collection.OnChanged(action, oldIndex, oldItem, newIndex, newItem);

            //call to the user ChangedEvent
            CollectionChangedEventArgs e = new CollectionChangedEventArgs(action, oldIndex, newIndex);
            _collection.InvokeOnChanged(e);

            return HRESULT.S_OK;
        }

        #endregion

    }
}
