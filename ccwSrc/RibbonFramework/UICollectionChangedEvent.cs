//*****************************************************************************
//
//  File:       UICollectionChangedEvent.cs
//
//  Contents:   Helper class that exposes an OnChanged event for a given 
//              IUICollector instance.
//
//*****************************************************************************

using System;
using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that exposes an OnChanged event for a given 
    /// IUICollection instance.
    /// </summary>
    public sealed unsafe class UICollectionChangedEvent<T> : IUICollectionChangedEvent.Interface where T : AbstractPropertySet, new()
    {
        private static readonly EventKey ChangedEventKey = new EventKey();
        private IUICollection* _cpIUICollection;
        private readonly CollectionItem _sender;
        private readonly UICollection<T> _collection;
        private uint _cookie;
        private readonly EventSet _eventSet = new EventSet();

        internal UICollectionChangedEvent(CollectionItem sender, UICollection<T> collection)
        {
            _sender = sender;
            _collection = collection;
        }

        /// <summary>
        /// Attach to an IUICollection object events
        /// </summary>
        /// <param name="cpIUICollection">IUICollection object</param>
        internal void Attach(IUICollection* cpIUICollection)
        {
            //uint refCount;
            //cpIUICollection->AddRef();
            //refCount = cpIUICollection->Release();
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
                //uint refCount = _cpIUICollection->Release();
                _cookie = 0;
            }
        }

        /// <summary>
        /// The Changed event
        /// </summary>
        public event EventHandler<CollectionChangedEventArgs>? ChangedEvent
        {
            add { _eventSet.Add(ChangedEventKey, value); }
            remove { _eventSet.Remove(ChangedEventKey, value); }
        }

        private ComScope<IConnectionPoint> GetConnectionPoint(IUICollection* cpIUICollection)
        {
            // get connection point container
            using ComScope<IConnectionPointContainer> connectionPointContainerScope = ComScope<IConnectionPointContainer>.QueryFrom(cpIUICollection);

            // get connection point for IUICollectionChangedEvent
            ComScope<IConnectionPoint> cpIConnectionPoint = new ComScope<IConnectionPoint>(null);
            connectionPointContainerScope.Value->FindConnectionPoint(IID.Get<IUICollectionChangedEvent>(), cpIConnectionPoint);

            return cpIConnectionPoint;
        }

        private uint RegisterComEvent(IUICollection* cpIUICollection)
        {
            using var connectionPointScope = GetConnectionPoint(cpIUICollection);

            uint cookie = 0;
            using ComScope<IUnknown> punkSink = ComHelpers.GetComScope<IUnknown>(this);
            connectionPointScope.Value->Advise(punkSink, &cookie);
            return cookie;
        }

        private void UnregisterComEvent(IUICollection* cpIUICollection, uint cookie)
        {
            using var cpConnectionPointScope = GetConnectionPoint(cpIUICollection);
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

            CollectionChangedEventArgs e = new CollectionChangedEventArgs(action, oldIndex, newIndex);
            _collection._ribbonStrip.BeginInvoke((MethodInvoker)delegate
            {
                OnChanged(e);
            });

            //_eventSet.Raise(ChangedEventKey, _sender, e);

            return HRESULT.S_OK;
        }

        #endregion

        private void OnChanged(CollectionChangedEventArgs e)
        {
            _eventSet.Raise(ChangedEventKey, _sender, e);
        }
    }
}
