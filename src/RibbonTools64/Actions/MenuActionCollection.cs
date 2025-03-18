// --------------------------------------------------------------------------
// Description : CDiese Toolkit library
// Author	   : Serge Weinstock
//
//	You are free to use, distribute or modify this code
//	as long as this header is not removed or modified.
// --------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;

namespace WinForms.Actions
{
    /// <summary>
    /// A collection that stores MenuAction Actions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [Editor(typeof(MenuActionCollectionEditor), typeof(UITypeEditor))]
    public class MenuActionCollection : CollectionBase
    {
        private MenuActionList _owner;
        private MenuAction _null = new MenuAction();

        /// <summary>
        ///  Initializes a new instance of MenuActionCollection.
        /// </summary>
        public MenuActionCollection(MenuActionList owner)
        {
            Debug.Assert(owner != null);
            _owner = owner;
            _null._owner = _owner;
        }
        /// <summary>
        /// Initialises a new instance of MenuActionCollection based on another MenuActionCollection.
        /// </summary>
        /// <param name='value'>An MenuActionCollection from which the contents are copied</param>
        public MenuActionCollection(MenuActionCollection value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Initialises a new instance of MenuActionCollection containing any array of MenuActions.
        /// </summary>
        /// <param name='value'>An array of MenuActions with which to intialize the collection</param>
        public MenuActionCollection(MenuAction[] value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Returns the MenuActionList which owns this MenuActionCollection
        /// </summary>
        public MenuActionList Parent
        {
            get
            {
                return _owner;
            }
        }

        /// <summary>
        /// Returns a reference to the "null" action of this collection (needed in design mode)
        /// </summary>
        internal MenuAction Null
        {
            get
            {
                return _null;
            }
        }

        /// <summary>
        /// Represents the entry at the specified index.
        /// </summary>
        /// <param name='index'>The zero-based index of the entry to locate in the collection.</param>
        /// <returns>The entry at the specified index of the collection.</returns>
        public MenuAction this[int index]
        {
            get
            {
                return ((MenuAction)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds a MenuAction with the specified value to the MenuActionCollection.
        /// </summary>
        /// <param name='value'>The MenuAction to add.</param>
        /// <returns>The index at which the new element was inserted.</returns>
        public int Add(MenuAction value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Copies the elements of an array to the end of the MenuActionCollection.
        /// </summary>
        /// <param name='value'> An array of MenuActions containing the objects to add to the collection.</param>
        public void AddRange(MenuAction[] value)
        {
            foreach (MenuAction a in value)
            {
                this.Add(a);
            }
        }

        /// <summary>
        /// Adds the contents of another MenuActionCollection to the end of the collection.
        /// </summary>
        /// <param name='value'>An MenuActionCollection containing the objects to add to the collection.</param>
        public void AddRange(MenuActionCollection value)
        {
            foreach (MenuAction a in value)
            {
                this.Add(a);
            }
        }

        /// <summary>
        /// Returns true if this MenuActionCollection contains the specified Action.
        /// </summary>
        /// <param name='value'>The MenuAction to locate.</param>
        public bool Contains(MenuAction value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Copies the MenuActionCollection values to a one-dimensional Array instance at the  specified index.
        /// </summary>
        /// <param name='array'>The one-dimensional Array that is the destination of the values copied from MenuActionCollection .</param>
        /// <param name='index'>The index in <paramref name='array'/> where copying begins.</param>
        public void CopyTo(MenuAction[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>Returns the index of an MenuAction in the MenuActionCollection.</summary>
        /// <param name='value'>The MenuAction to locate.</param>
        /// <returns>The index of the MenuAction of <paramref name='value'/> in the MenuActionCollection, if found; otherwise, -1.</returns>
        public int IndexOf(MenuAction value)
        {
            return List.IndexOf(value);
        }

        /// <summary>Inserts a MenuAction into the MenuActionCollection at the specified index.</summary>
        /// <param name='index'>The zero-based index where <paramref name='value'/> should be inserted.</param>
        /// <param name=' value'>The MenuAction to insert.</param>
        public void Insert(int index, MenuAction value)
        {
            List.Insert(index, value);
        }

        /// <summary>Returns an enumerator that can iterate through  the MenuActionCollection.</summary>
        public new MenuActionEnumerator GetEnumerator()
        {
            return new MenuActionEnumerator(this);
        }

        /// <summary>
        /// Removes a specific MenuAction from the MenuActionCollection.
        /// </summary>
        /// <param name='value'>The MenuAction to remove from the MenuActionCollection .</param>
        public void Remove(MenuAction value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (oldValue != null) ((MenuAction)oldValue)._owner = null;
            if (newValue != null) ((MenuAction)newValue)._owner = _owner;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnInsert(int index, object value)
        {
            if (value != null) ((MenuAction)value)._owner = _owner;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnClear()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnRemove(int index, object value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValidate(object value)
        {
        }

        /// <summary>
        /// An enumerator for an MenuActionCollection
        /// </summary>
        public class MenuActionEnumerator : object, IEnumerator
        {

            private IEnumerator _baseEnumerator;
            private IEnumerable _temp;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="mappings"></param>
            public MenuActionEnumerator(MenuActionCollection mappings)
            {
                this._temp = ((IEnumerable)(mappings));
                this._baseEnumerator = _temp.GetEnumerator();
            }

            /// <summary>
            /// 
            /// </summary>
            public MenuAction Current
            {
                get
                {
                    return ((MenuAction)(_baseEnumerator.Current));
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return _baseEnumerator.Current;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return _baseEnumerator.MoveNext();
            }

            bool IEnumerator.MoveNext()
            {
                return _baseEnumerator.MoveNext();
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                _baseEnumerator.Reset();
            }

            void IEnumerator.Reset()
            {
                _baseEnumerator.Reset();
            }
        }
    }
}
