//*****************************************************************************
//
//  File:       GalleryPropertiesProvider.cs
//
//  Contents:   Definition for gallery properties provider 
//
//*****************************************************************************

using System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for gallery properties provider interface
    /// </summary>
    public interface IGalleryPropertiesProvider
    {
        /// <summary>
        /// Categories property, Managed version
        /// </summary>
        UICollection<CategoriesPropertySet>? GalleryCategories { get; }

        /// <summary>
        /// Items source property
        /// </summary>
        UICollection<GalleryItemPropertySet>? GalleryItemItemsSource { get; }

        /// <summary>
        /// Invalidate GalleryCategories or Categories if one change a value
        /// </summary>
        void InvalidateCategories();

        /// <summary>
        /// Invalidate GalleryItemItemsSource or ItemsSource if one change a value
        /// </summary>
        void InvalidateItemsSource();

        /// <summary>
        /// Selected item property
        /// </summary>
        int SelectedItem { get; set; }

        /// <summary>
        /// Called when the Categories property is ready to be initialized
        /// </summary>
        event EventHandler<EventArgs>? CategoriesReady;

        /// <summary>
        /// Called when the ItemsSource property is ready to be initialized
        /// </summary>
        event EventHandler<EventArgs>? ItemsSourceReady;
    }

    /// <summary>
    /// Definition for gallery properties provider interface
    /// </summary>
    internal interface IGalleryProvider
    {
        /// <summary>
        /// Categories property
        /// </summary>
        unsafe IUICollection* Categories { get; }

        /// <summary>
        /// Items source property
        /// </summary>
        unsafe IUICollection* ItemsSource { get; }
    }

    /// <summary>
    /// Implementation of IGalleryPropertiesProvider
    /// </summary>
    public sealed class GalleryPropertiesProvider : BasePropertiesProvider, IGalleryPropertiesProvider,
        IGalleryProvider
    {
        private static readonly EventKey s_CategoriesReadyKey = new EventKey();
        private static readonly EventKey s_ItemsSourceReadyKey = new EventKey();
        private RibbonStripItem _ribbonItem;
        private bool categoriesReadyFired;
        private bool itemsSourceReadyFired;
        private readonly EventSet _eventSet;
        internal GalleryCommandProperties GalleryCommand { get; private set; }

        /// <summary>
        /// GalleryPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        /// <param name="ribbonItem">ribbon control that instantiate the provider</param>
        public GalleryPropertiesProvider(RibbonStrip ribbon, uint commandId, RibbonStripItem ribbonItem)
            : base(ribbon, commandId)
        {
            _ribbonItem = ribbonItem;
            _eventSet = ribbonItem.EventSet;

            // add supported properties
            _supportedProperties.Add(RibbonProperties.Categories);
            _supportedProperties.Add(RibbonProperties.ItemsSource);
            _supportedProperties.Add(RibbonProperties.SelectedItem);
            GalleryCommand = new GalleryCommandProperties();
        }

        private uint? _selectedItem;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            fixed (PROPVARIANT* newValueLocal = &newValue) { }
            if (key == RibbonProperties.Categories)
            {
                if (!categoriesReadyFired)
                {
                    categoriesReadyFired = true;
                    if (currentValue is not null)
                    {
                        IUICollection* cpCollection;
                        UIPropVariant.UIPropertyToInterface<IUICollection>(RibbonProperties.Categories, *currentValue, out cpCollection);

                        //refCount = 3 here. (native Framework + PROPVARIANT currentValue + UIPropertyToInterface cpCollection)
                        //refCount from (native Framework + PROPVARIANT) have to be released by Framework

                        ComScope<IUICollection> pCollection = new ComScope<IUICollection>(cpCollection);
                        //(*currentValue).Clear(); //PropVariantClear ??? => no
                        GalleryCategories = new UICollection<CategoriesPropertySet>(pCollection, _ribbonItem, CollectionType.Categories);
                    }
                    //if (CategoriesReady != null)
                    {
                        //Invoke ?
                        try
                        {
                            _eventSet.Raise(s_CategoriesReadyKey, _ribbonItem, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            return _ribbonItem.EventExceptionHandler(ex);
                        }
                    }
                }
            }
            else if (key == RibbonProperties.ItemsSource)
            {
                if (!itemsSourceReadyFired)
                {
                    itemsSourceReadyFired = true;
                    if (currentValue is not null)
                    {
                        IUICollection* cpCollection;
                        UIPropVariant.UIPropertyToInterface<IUICollection>(RibbonProperties.ItemsSource, *currentValue, out cpCollection);

                        //refCount = 3 here. (native Framework + PROPVARIANT currentValue + UIPropertyToInterface cpCollection)
                        //refCount from (native Framework + PROPVARIANT) have to be released by Framework
                        //cpCollection is released in UICollection.Destroy from OnDestroyUICommand

                        ComScope<IUICollection> pCollection = new ComScope<IUICollection>(cpCollection);
                        //(*currentValue).Clear(); //PropVariantClear ??? => no
                        UI_COMMANDTYPE itemCommandType = (UI_COMMANDTYPE)_ribbonItem.CommandType;
                        if (itemCommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION)
                            GalleryItemItemsSource = new UICollection<GalleryItemPropertySet>(pCollection, _ribbonItem, CollectionType.ItemsSource);
                        else if (itemCommandType == UI_COMMANDTYPE.UI_COMMANDTYPE_COMMANDCOLLECTION)
                            GalleryCommand.GalleryCommandItemsSource = new UICollection<GalleryCommandPropertySet>(pCollection, _ribbonItem, CollectionType.CommandItemsSource);
                    }
                    //if (ItemsSourceReady != null)
                    {
                        //Invoke ?
                        try
                        {
                            _eventSet.Raise(s_ItemsSourceReadyKey, _ribbonItem, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            return _ribbonItem.EventExceptionHandler(ex);
                        }
                    }
                }
            }
            else if (key == RibbonProperties.SelectedItem)
            {
                if (_selectedItem.HasValue)
                {
                    newValue = (PROPVARIANT)_selectedItem.Value; //InitPropVariantFromUInt32
                }
            }

            return HRESULT.S_OK;
        }

        #region IGalleryPropertiesProvider Members

        /// <summary>
        /// Categories property
        /// </summary>
        public UICollection<CategoriesPropertySet>? GalleryCategories { get; private set; }

        /// <summary>
        /// Items source property for Item
        /// </summary>
        public UICollection<GalleryItemPropertySet>? GalleryItemItemsSource { get; private set; }

        /// <summary>
        /// Categories property
        /// </summary>
        unsafe IUICollection* IGalleryProvider.Categories
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    fixed (PROPERTYKEY* pKeyCategories = &RibbonProperties.Categories)
                        hr = _ribbon.Framework->GetUICommandProperty(_commandId, pKeyCategories, &propvar);
                    if (hr.Succeeded)
                    {
                        IUICollection* result;
                        UIPropVariant.UIPropertyToInterface<IUICollection>(RibbonProperties.Categories, propvar, out result);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Items source property
        /// </summary>
        unsafe IUICollection* IGalleryProvider.ItemsSource
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    fixed (PROPERTYKEY* pKeyItemsSource = &RibbonProperties.ItemsSource)
                        hr = _ribbon.Framework->GetUICommandProperty(_commandId, pKeyItemsSource, &propvar);
                    if (hr.Succeeded)
                    {
                        IUICollection* result;
                        UIPropVariant.UIPropertyToInterface<IUICollection>(RibbonProperties.ItemsSource, propvar, out result);
                        propvar.Clear(); //PropVariantClear
                        return result;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Invalidate GalleryCategories or Categories if one change a value
        /// </summary>
        public unsafe void InvalidateCategories()
        {
            if (_ribbon.Framework != null)
            {
                fixed (PROPERTYKEY* pKeyCategories = &RibbonProperties.Categories)
                    _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pKeyCategories);
            }
        }

        /// <summary>
        /// Invalidate GalleryItemItemsSource or ItemsSource if one change a value
        /// </summary>
        public unsafe void InvalidateItemsSource()
        {
            if (_ribbon.Framework != null)
            {
                fixed (PROPERTYKEY* pKeyItemsSource = &RibbonProperties.ItemsSource)
                    _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pKeyItemsSource);
            }
        }

        /// <summary>
        /// Selected item property
        /// </summary>
        public unsafe int SelectedItem
        {
            get
            {
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar;
                    fixed (PROPERTYKEY* pKeySelectedItem = &RibbonProperties.SelectedItem)
                        hr = _ribbon.Framework->GetUICommandProperty(_commandId, pKeySelectedItem, &propvar);
                    if (hr.Succeeded)
                    {
                        uint result = (uint)propvar; //PropVariantToUInt32
                        return (int)result;
                    }
                }

                return unchecked((int)_selectedItem.GetValueOrDefault(PInvoke.UI_COLLECTION_INVALIDINDEX));
            }
            set
            {
                _selectedItem = (uint)value;

                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    PROPVARIANT propvar = (PROPVARIANT)(uint)value; //InitPropVariantFromUInt32
                    fixed (PROPERTYKEY* pKeySelectedItem = &RibbonProperties.SelectedItem)
                        hr = _ribbon.Framework->SetUICommandProperty(_commandId, pKeySelectedItem, &propvar);
                }
            }
        }

        /// <summary>
        /// Called when the Categories property is ready to be initialized
        /// </summary>
        public event EventHandler<EventArgs>? CategoriesReady
        {
            add { _eventSet.Add(s_CategoriesReadyKey, value); }
            remove { _eventSet.Remove(s_CategoriesReadyKey, value); }
        }

        /// <summary>
        /// Called when the ItemsSource property is ready to be initialized
        /// </summary>
        public event EventHandler<EventArgs>? ItemsSourceReady
        {
            add { _eventSet.Add(s_ItemsSourceReadyKey, value); }
            remove { _eventSet.Remove(s_ItemsSourceReadyKey, value); }
        }

        #endregion
    }
}
