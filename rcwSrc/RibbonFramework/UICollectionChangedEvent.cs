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
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.Foundation;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that exposes an OnChanged event for a given 
    /// IUICollection instance.
    /// </summary>
    public sealed class UICollectionChangedEvent<T> : IUICollectionChangedEvent where T : AbstractPropertySet, new()
    {
        private static readonly EventKey ChangedEventKey = new EventKey();
        private static readonly Guid IIDGuidIUICollectionChangedEvent = typeof(IUICollectionChangedEvent).GUID;
        private IUICollection? _cpIUICollection;
        private readonly CollectionItem _sender;
        private readonly UICollection<T> _collection;
        private int _cookie;
        private readonly EventSet _eventSet = new EventSet();

        /// <summary>
        /// 
        /// </summary>
        internal UICollectionChangedEvent(CollectionItem sender, UICollection<T> collection)
        {
            _sender = sender;
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

        /// <summary>
        /// The Changed event
        /// </summary>
        public event EventHandler<CollectionChangedEventArgs>? ChangedEvent
        {
            add { _eventSet.Add(ChangedEventKey, value); }
            remove { _eventSet.Remove(ChangedEventKey, value); }
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

            CollectionChangedEventArgs e = new CollectionChangedEventArgs(action, oldIndex, newIndex);
            _collection._ribbon.BeginInvoke((MethodInvoker)delegate
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
