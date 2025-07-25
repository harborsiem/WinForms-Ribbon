//*****************************************************************************
//
//  File:       UICollection.cs
//
//  Contents:   Helper class that provides an implementation of the 
//              IUICollection interface.
//
//*****************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Variant;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    /// <summary>
    /// The UICollection member
    /// </summary>
    public enum CollectionType
    {
        /// <summary>
        /// The UICollection member ItemsSource of a Gallery Control with Items
        /// </summary>
        ItemsSource,
        /// <summary>
        /// The UICollection member Categories of a Gallery Control
        /// </summary>
        Categories,
        /// <summary>
        /// The UICollection member ItemsSource of a Qat Control
        /// </summary>
        QatItemsSource,
        /// <summary>
        /// The UICollection member ItemsSource of a Gallery Control with Commands
        /// </summary>
        CommandItemsSource
    }

    /// <summary>
    /// Helper class that provides an implementation of the 
    /// IUICollection interface.
    /// </summary>
    /// <typeparam name="T">An AbstractPropertySet</typeparam>
    public sealed unsafe class UICollection<T> : IUICollection.Interface, IEnumerable<T> where T : AbstractPropertySet, new()
    {
        private static readonly EventKey s_CategoriesChangedKey = new EventKey();
        private static readonly EventKey s_ItemsSourceChangedKey = new EventKey();
        private List<T> _items;
        private IUICollection* _cpIUICollection;
        private readonly RibbonStrip _ribbon;
        private readonly EventKey _changedKey;
        //private CollectionChange marker = CollectionChange.None;
        private bool _detachEvent;
        private UICollectionChangedEvent<T>? _changedEvent;
        private readonly CollectionItem _collectionItem;
        private readonly RibbonStripItem _ribbonItem;

        internal IUICollection* CpIUICollection => _cpIUICollection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpIUICollection"></param>
        /// <param name="ribbonItem"></param>
        /// <param name="collectionType"></param>
        internal unsafe UICollection(ComScope<IUICollection> cpIUICollection, RibbonStripItem ribbonItem, CollectionType collectionType)
        {
            if (cpIUICollection.IsNull)
                throw new ArgumentNullException(nameof(cpIUICollection));
            //uint refCount;
            //cpIUICollection.AsUnknown->AddRef();
            //refCount = cpIUICollection.AsUnknown->Release();
            //@ refCount = 3 for cpIUICollection, is this OK ?

            UI_COMMANDTYPE itemCommandType = (UI_COMMANDTYPE)ribbonItem.CommandType;
            if (ribbonItem == null || !(itemCommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION || itemCommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COMMANDCOLLECTION))
                throw new ArgumentException("Ribbon control is not a Collection or CommandCollection", nameof(ribbonItem));
            if (cpIUICollection.IsNull)
                throw new ArgumentException("Not a IUICollection ComObject", nameof(cpIUICollection));
            else
                _cpIUICollection = cpIUICollection;
            _ribbon = ribbonItem.Ribbon;
            if (ribbonItem is RibbonQuickAccessToolbar)
            {
                if (!(collectionType == CollectionType.QatItemsSource && typeof(T) == typeof(QatCommandPropertySet)))
                    throw new ArgumentException("RibbonQuickAccessToolbar with T or " + nameof(collectionType) + " not allowed");
            }
            else if (collectionType == CollectionType.Categories)
            {
                if (typeof(T) != typeof(CategoriesPropertySet))
                    throw new ArgumentException("T is not a valid Type: CategoriesPropertySet");
            }
            else if (!((itemCommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COMMANDCOLLECTION && typeof(T) == typeof(GalleryCommandPropertySet))
                || (itemCommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION && typeof(T) == typeof(GalleryItemPropertySet))))
            {
                throw new ArgumentException("T is not a valid Type: GalleryItemPropertySet or GalleryCommandPropertySet");
            }
            if (collectionType == CollectionType.Categories)
            {
                _changedKey = s_CategoriesChangedKey;
            }
            else
            {
                _changedKey = s_ItemsSourceChangedKey;
            }
            _ribbonItem = ribbonItem;
            _items = new List<T>();
            PropertySetEnumerator propset = new PropertySetEnumerator(this);
            foreach (T propItem in propset) //Get all existing items, only from QAT
            {
                _items.Add(propItem);
            }
            propset.Destroy();
            _collectionItem = new CollectionItem(ribbonItem, collectionType);
            _changedEvent = new UICollectionChangedEvent<T>(this);
            _changedEvent.Attach(_cpIUICollection);
        }

        /// <summary>
        ///  Disposes of the resources.
        /// </summary>
        internal void Destroy()
        {
            if (_changedEvent != null)
            {
                _changedEvent.Detach();
                _changedEvent = null;
                uint refCount = _cpIUICollection->Release();
            }
        }

        internal void OnChanged(UI_COLLECTIONCHANGE action, uint oldIndex, IUnknown* oldItem, uint newIndex, IUnknown* newItem)
        {
            if (!_detachEvent)
            {
                object? newItemObject = null;
                using ComScope<IUnknown> cpIUnknownScope = new ComScope<IUnknown>(newItem);
                if (!cpIUnknownScope.IsNull)
                {
                    newItemObject = ComHelpers.GetObjectForIUnknown(cpIUnknownScope);
                }
                if (!(newItemObject is T newGalleryItem))
                {
                    if (newItemObject != null)
                    {
                        using ComScope<IUISimplePropertySet> cpSimplePropertySet = ComScope<IUISimplePropertySet>.QueryFrom(newItem);
                        newGalleryItem = FromPropertySet(cpSimplePropertySet)!;
                    }
                    else
                    {
                        newGalleryItem = null!;
                    }
                }
                //if (!(e.OldItem is T oldGalleryItem))
                //oldGalleryItem = FromPropertySet((IUISimplePropertySet)e.OldItem);
                switch (action)
                {
                    case UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_INSERT:
                        _items.Insert((int)newIndex, newGalleryItem!);
                        break;
                    case UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_REMOVE:
                        _items.RemoveAt((int)oldIndex);
                        break;
                    case UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_REPLACE:
                        _items[(int)newIndex] = newGalleryItem!;
                        break;
                    case UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_RESET:
                        _items.Clear();
                        break;
                    default:
                        break;
                }
            }
            _detachEvent = false;
        }

        /// <summary>
        /// Interface IEnumerator of T method
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Interface IEnumerator method
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _items.Add(item);
            //marker = CollectionChange.Insert;
            _detachEvent = true;
            IUnknown* cpIUnknown = (IUnknown*)Marshal.GetIUnknownForObject(item);
            HRESULT hr = _cpIUICollection->Add(cpIUnknown);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            //marker = CollectionChange.Remove;
            _detachEvent = true;
            HRESULT hr = _cpIUICollection->RemoveAt((uint)index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            _items.Insert(index, item);
            //marker = CollectionChange.Insert;
            _detachEvent = true;
            IUnknown* cpIUnknown = (IUnknown*)Marshal.GetIUnknownForObject(item);
            _cpIUICollection->Insert((uint)index, cpIUnknown);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            //marker = CollectionChange.Reset;
            _detachEvent = true;
            _cpIUICollection->Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return _items[index]; }
            set
            {
                _items[index] = value;
                //marker = CollectionChange.Replace;
                _detachEvent = true;
                IUnknown* cpIUnknown = (IUnknown*)Marshal.GetIUnknownForObject(value);
                _cpIUICollection->Replace((uint)index, cpIUnknown);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Returns a List from uiCollection
        /// </summary>
        public ReadOnlyCollection<T> CollectionList { get { return _items.AsReadOnly(); } }

        #region IUICollection Members

        /// <summary>
        /// Retrieves the count of the collection
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        unsafe HRESULT IUICollection.Interface.GetCount(uint* count)
        {
            HRESULT hr = _cpIUICollection->GetCount(count);
            return hr;
        }

        /// <summary>
        /// Retrieves an item
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        HRESULT IUICollection.Interface.GetItem(uint index, IUnknown** item)
        {
            HRESULT hr = _cpIUICollection->GetItem(index, item);
            return hr;
        }

        /// <summary>
        /// Adds an item to the end
        /// </summary>
        /// <param name="item">Must be an object of type T</param>
        /// <returns></returns>
        HRESULT IUICollection.Interface.Add(IUnknown* item)
        {
            HRESULT hr;
            hr = _cpIUICollection->Add(item);
            return hr;
            //return HRESULT.E_INVALIDARG;
        }

        /// <summary>
        /// Inserts an item
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item">Must be an object of type T</param>
        /// <returns></returns>
        HRESULT IUICollection.Interface.Insert(uint index, IUnknown* item)
        {
            HRESULT hr;
            hr = _cpIUICollection->Insert(index, item);
            return hr;
            //return HRESULT.E_INVALIDARG;
        }

        /// <summary>
        /// Removes an item at the specified position
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        HRESULT IUICollection.Interface.RemoveAt(uint index)
        {
            HRESULT hr = _cpIUICollection->RemoveAt(index);
            return hr;
        }

        /// <summary>
        /// Replaces an item at the specified position
        /// </summary>
        /// <param name="indexReplaced"></param>
        /// <param name="itemReplaceWith">Must be an object of type T</param>
        /// <returns></returns>
        HRESULT IUICollection.Interface.Replace(uint indexReplaced, IUnknown* itemReplaceWith)
        {
            HRESULT hr;
            hr = _cpIUICollection->Replace(indexReplaced, itemReplaceWith);
            return hr;
            //return HRESULT.E_INVALIDARG;
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        /// <returns></returns>
        HRESULT IUICollection.Interface.Clear()
        {
            HRESULT hr = _cpIUICollection->Clear();
            return hr;
        }

        #endregion


        private unsafe T? FromPropertySet(IUISimplePropertySet* cpIUISimplePropertySet)
        {
            if (cpIUISimplePropertySet == null)
                return null;

            if (typeof(T) == typeof(QatCommandPropertySet))
            {
                return new QatCommandPropertySet(cpIUISimplePropertySet) as T;
            }

            //Just in case, do not know if this ever happens

            if (typeof(T) == typeof(GalleryCommandPropertySet))
            {
                return new GalleryCommandPropertySet(cpIUISimplePropertySet) as T;
            }

            //Just in case, do not know if this ever happens

            if (typeof(T) == typeof(GalleryItemPropertySet))
            {
                return new GalleryItemPropertySet(cpIUISimplePropertySet) as T;
            }

            //Just in case, do not know if this ever happens

            if (typeof(T) == typeof(CategoriesPropertySet))
            {
                return new CategoriesPropertySet(cpIUISimplePropertySet) as T;
            }
            return null;
        }

        #region IEnumUnknown

        //void IEnumUnknown.Next(uint celt, object[] rgelt, out uint pceltFetched) { 
        //    pceltFetched = 0;
        //     }

        //void IEnumUnknown.Reset() {  }

        //void IEnumUnknown.Skip(uint celt) {  }

        //void IEnumUnknown.Clone(out Windows.Win32.System.Com.IEnumUnknown ppenum) {
        //    ppenum = null;
        //     }

        #endregion

        /// <summary>
        /// Event provider: The Collection is changing.
        /// </summary>
        public event EventHandler<CollectionChangedEventArgs>? ChangedEvent
        {
            add { _ribbonItem.EventSet.Add(_changedKey, value); }
            remove { _ribbonItem.EventSet.Remove(_changedKey, value); }
        }

        internal void InvokeOnChanged(CollectionChangedEventArgs e)
        {
            _ribbon.BeginInvoke((MethodInvoker)delegate
            {
                _ribbonItem.EventSet.Raise(_changedKey, _collectionItem, e);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed unsafe class PropertySetEnumerator : IEnumerable<T>, IEnumerator<T>
        {
            private UICollection<T> _caller;
            private IEnumUnknown* _cpIEnumUnknown;
            private T? _current;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="caller">UICollection of T</param>
            public PropertySetEnumerator(UICollection<T> caller)
            {
                _caller = caller;
                ComScope<IEnumUnknown> cpEnumUnknownScope = ComScope<IEnumUnknown>.QueryFrom(caller._cpIUICollection);

                //IEnumUnknown* cpIEnumUnknown;
                //    caller._cpIUICollection->QueryInterface(IID.Get<IEnumUnknown>(), (void**)&cpIEnumUnknown);
                _cpIEnumUnknown = cpEnumUnknownScope;
                Reset();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)this;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return (IEnumerator<T>)this;
            }

            /// <summary>
            /// 
            /// </summary>
            public T Current
            {
                get
                {
                    return _current!;
                }
            }

            object? IEnumerator.Current
            {
                get
                {
                    return _current;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public unsafe bool MoveNext()
            {
                uint fetched;
                IUnknown* rgelt;
                _cpIEnumUnknown->Next(1, &rgelt, &fetched);
                if (fetched == 1)
                {
                    // GetObjectForIUnknown increments the ref count so we need to dispose.
                    using (ComScope<IUnknown> scopeRgelt = new ComScope<IUnknown>(rgelt))
                        _current = Marshal.GetObjectForIUnknown(scopeRgelt) as T;
                    if (_current == null)
                    {
                        using ComScope<IUISimplePropertySet> scopeSimplePropertySet = ComScope<IUISimplePropertySet>.QueryFrom(rgelt);
                        _current = _caller.FromPropertySet(scopeSimplePropertySet);
                        //scopeSimplePropertySet refCount = 3 here
                        //uint refCount;
                        //scopeSimplePropertySet.Value->AddRef();
                        //refCount = scopeSimplePropertySet.Value->Release();
                        //Debug.WriteLine("MoveNext refCount " + refCount);
                    }
                }
                else
                {
                    _cpIEnumUnknown->Reset();
                    _current = null;
                }
                return fetched == 0 ? false : true;
            }

            bool IEnumerator.MoveNext()
            {
                return MoveNext();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                _cpIEnumUnknown->Reset();
                _current = null;
            }

            void IEnumerator.Reset()
            {
                Reset();
            }

            void IDisposable.Dispose()
            {
                Reset();
            }

            internal void Destroy()
            {
                ComScope<IEnumUnknown> scope = new ComScope<IEnumUnknown>(_cpIEnumUnknown);
                scope.Dispose();
                //_cpIEnumUnknown = null;
            }
        }
    }
}
