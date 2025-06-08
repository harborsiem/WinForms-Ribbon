//*****************************************************************************
//
//  File:       RibbonSplitButtonGallery.cs
//
//  Contents:   Helper class that wraps a ribbon split button gallery control.
//
//*****************************************************************************

using System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a ribbon split button gallery control.
    /// </summary>
    public sealed unsafe class RibbonSplitButtonGallery : RibbonStripItem,
        IBooleanValuePropertyProvider,
        IGalleryPropertiesProvider,
        IGalleryProvider,
        IGallery2PropertiesProvider,
        IEnabledPropertiesProvider, 
        IKeytipPropertiesProvider,
        ILabelPropertiesProvider,
        IImagePropertiesProvider,
        ITooltipPropertiesProvider,
        IExecuteEventsProvider,
        IPreviewEventsProvider
    {
        private static readonly EventKey s_SelectedIndexChangedKey = new EventKey();
        private BooleanValuePropertyProvider _booleanValuePropertyProvider;
        private GalleryPropertiesProvider _galleryPropertiesProvider;
        private EnabledPropertiesProvider _enabledPropertiesProvider;
        private KeytipPropertiesProvider _keytipPropertiesProvider;
        private LabelPropertiesProvider _labelPropertiesProvider;
        private ImagePropertiesProvider _imagePropertiesProvider;
        private TooltipPropertiesProvider _tooltipPropertiesProvider;
        private ExecuteEventsProvider _executeEventsProvider;
        private PreviewEventsProvider _previewEventsProvider;

        /// <summary>
        /// Initializes a new instance of the Ribbon SplitButtonGallery
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonSplitButtonGallery(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            AddPropertiesProvider(_booleanValuePropertyProvider = new BooleanValuePropertyProvider(ribbon, commandId));
            AddPropertiesProvider(_galleryPropertiesProvider = new GalleryPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_enabledPropertiesProvider = new EnabledPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_keytipPropertiesProvider = new KeytipPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_labelPropertiesProvider = new LabelPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_imagePropertiesProvider = new ImagePropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_tooltipPropertiesProvider = new TooltipPropertiesProvider(ribbon, commandId, this));

            AddEventsProvider(_executeEventsProvider = new ExecuteEventsProvider(this));
            AddEventsProvider(_previewEventsProvider = new PreviewEventsProvider(this));
        }

        #region IBooleanValuePropertyProvider Members

        /// <summary>
        /// Get or set the Checked state.
        /// </summary>
        public bool BooleanValue
        {
            get
            {
                return _booleanValuePropertyProvider.BooleanValue;
            }
            set
            {
                _booleanValuePropertyProvider.BooleanValue = value;
            }
        }

        #endregion

        #region IGalleryPropertiesProvider Members

        /// <summary>
        /// The list of categories. 
        /// Also exposed as an UICollection of CategoriesPropertySet elements
        /// </summary>
        public UICollection<CategoriesPropertySet>? GalleryCategories => _galleryPropertiesProvider.GalleryCategories;

        /// <summary>
        /// The list of SplitButtonGallery items.
        /// It is exposed as an UICollection where every element
        /// in the collection is of type: GalleryItemPropertySet
        /// </summary>
        public UICollection<GalleryItemPropertySet>? GalleryItemItemsSource => _galleryPropertiesProvider.GalleryItemItemsSource;

        /// <summary>
        /// The list of SplitButtonGallery items.
        /// It is exposed as an UICollection where every element
        /// in the collection is of type: GalleryCommandPropertySet
        /// </summary>
        public UICollection<GalleryCommandPropertySet>? GalleryCommandItemsSource => _galleryPropertiesProvider.GalleryCommand.GalleryCommandItemsSource;

        /// <summary>
        /// The list of categories. 
        /// Also exposed as an IUICollection of IUISimplePropertySet elements
        /// </summary>
        IUICollection* IGalleryProvider.Categories
        {
            get
            {
                return ((IGalleryProvider)_galleryPropertiesProvider).Categories;
            }
        }

        /// <summary>
        /// The list of SplitButtonGallery items.
        /// It is exposed as an IUICollection where every element
        /// in the collection is of type: IUISimplePropertySet
        /// </summary>
        IUICollection* IGalleryProvider.ItemsSource
        {
            get
            {
                return ((IGalleryProvider)_galleryPropertiesProvider).ItemsSource;
            }
        }

        /// <summary>
        /// Invalidate GalleryCategories or Categories if one change a value
        /// </summary>
        public void InvalidateCategories()
        {
            _galleryPropertiesProvider.InvalidateCategories();
        }

        /// <summary>
        /// Invalidate GalleryItemItemsSource or ItemsSource if one change a value
        /// </summary>
        public void InvalidateItemsSource()
        {
            _galleryPropertiesProvider.InvalidateItemsSource();
        }

        /// <summary>
        /// The index of the selected item in the SplitButtonGallery.
        /// If nothing is selected returns UI_Collection_InvalidIndex,
        /// which is a fancy way to say -1
        /// </summary>
        public int SelectedItem
        {
            get
            {
                return _galleryPropertiesProvider.SelectedItem;
            }
            set
            {
                _galleryPropertiesProvider.SelectedItem = value;
            }
        }

        /// <summary>
        /// Event provider which only fired once.
        /// In this event you can initialize the Categories
        /// Now one can work with the Categories.
        /// </summary>
        public event EventHandler<EventArgs>? CategoriesReady
        {
            add
            {
                _galleryPropertiesProvider.CategoriesReady += value;
            }
            remove
            {
                _galleryPropertiesProvider.CategoriesReady -= value;
            }
        }

        /// <summary>
        /// Event provider which only fired once.
        /// In this event you can initialize the ItemsSource
        /// Now one can work with the ItemsSource.
        /// </summary>
        public event EventHandler<EventArgs>? ItemsSourceReady
        {
            add
            {
                _galleryPropertiesProvider.ItemsSourceReady += value;
            }
            remove
            {
                _galleryPropertiesProvider.ItemsSourceReady -= value;
            }
        }

        #endregion

        #region IEnabledPropertiesProvider Members

        /// <summary>
        /// Get or set the Enabled state.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabledPropertiesProvider.Enabled;
            }
            set
            {
                _enabledPropertiesProvider.Enabled = value;
            }
        }

        #endregion

        #region IKeytipPropertiesProvider Members

        /// <summary>
        /// The keytip or key sequence that is used to access the command using the Alt key.
        /// This keytip appears when the user presses the Alt key to navigate the ribbon.
        /// The Ribbon Framework will automatically apply keytips to every command.
        /// However, if you want more control over the keytips used, you can specify them yourself.
        /// A keytip is not limited to a single character.
        /// </summary>
        public string? Keytip
        {
            get
            {
                return _keytipPropertiesProvider.Keytip;
            }
            set
            {
                _keytipPropertiesProvider.Keytip = value;
            }
        }

        #endregion

        #region ILabelPropertiesProvider Members

        /// <summary>
        /// This is the label of the command as it will appear on the ribbon or context popups.
        /// </summary>
        public string? Label
        {
            get
            {
                return _labelPropertiesProvider.Label;
            }
            set
            {
                _labelPropertiesProvider.Label = value;
            }
        }

        #endregion

        #region IImagePropertiesProvider Members

        /// <summary>
        /// Large images
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? LargeImage
        {
            get
            {
                return _imagePropertiesProvider.LargeImage;
            }
            set
            {
                _imagePropertiesProvider.LargeImage = value;
            }
        }

        /// <summary>
        /// Small images
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? SmallImage
        {
            get
            {
                return _imagePropertiesProvider.SmallImage;
            }
            set
            {
                _imagePropertiesProvider.SmallImage = value;
            }
        }

        /// <summary>
        /// Large images for use with high-contrast system settings
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? LargeHighContrastImage
        {
            get
            {
                return _imagePropertiesProvider.LargeHighContrastImage;
            }
            set
            {
                _imagePropertiesProvider.LargeHighContrastImage = value;
            }
        }

        /// <summary>
        /// Small images for use with high-contrast system settings
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? SmallHighContrastImage
        {
            get
            {
                return _imagePropertiesProvider.SmallHighContrastImage;
            }
            set
            {
                _imagePropertiesProvider.SmallHighContrastImage = value;
            }
        }

        #endregion

        #region ITooltipPropertiesProvider Members

        /// <summary>
        /// The title of the tooltip (hint) that appear when the user hovers the mouse over the command.
        /// This title is displayed in bold at the top of the tooltip.
        /// </summary>
        public string? TooltipTitle
        {
            get
            {
                return _tooltipPropertiesProvider.TooltipTitle;
            }
            set
            {
                _tooltipPropertiesProvider.TooltipTitle = value;
            }
        }

        /// <summary>
        /// The description of the tooltip as it appears below the title.
        /// </summary>
        public string? TooltipDescription
        {
            get
            {
                return _tooltipPropertiesProvider.TooltipDescription;
            }
            set
            {
                _tooltipPropertiesProvider.TooltipDescription = value;
            }
        }

        #endregion

        #region IExecuteEventsProvider Members

        /// <summary>
        /// Event provider similar to a "Selected Changed" event.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IExecuteEventsProvider.ExecuteEvent
        {
            add
            {
                _executeEventsProvider.ExecuteEvent += value;
            }
            remove
            {
                _executeEventsProvider.ExecuteEvent -= value;
            }
        }

        #endregion

        #region IPreviewEventsProvider Members

        /// <summary>
        /// Event provider for a preview.
        /// This is when the mouse enters the control.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IPreviewEventsProvider.PreviewEvent
        {
            add
            {
                _previewEventsProvider.PreviewEvent += value;
            }
            remove
            {
                _previewEventsProvider.PreviewEvent -= value;
            }
        }

        /// <summary>
        /// Event provider when the preview is cancelled.
        /// This is when the mouse leaves the control.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IPreviewEventsProvider.CancelPreviewEvent
        {
            add
            {
                _previewEventsProvider.CancelPreviewEvent += value;
            }
            remove
            {
                _previewEventsProvider.CancelPreviewEvent -= value;
            }
        }

        #endregion

        /// <summary>
        /// Event provider similar to a SelectedIndexChanged event.
        /// </summary>
        public event EventHandler<GalleryItemEventArgs>? SelectedIndexChanged
        {
            add { EventSet.Add(s_SelectedIndexChangedKey, value); }
            remove { EventSet.Remove(s_SelectedIndexChangedKey, value); }
        }

        /// <summary>
        /// Event provider for Preview.
        /// </summary>
        public event EventHandler<GalleryItemEventArgs>? Preview
        {
            add { EventSet.Add(s_PreviewKey, value); }
            remove { EventSet.Remove(s_PreviewKey, value); }
        }

        /// <summary>
        /// Event provider for CancelPreview.
        /// </summary>
        public event EventHandler<GalleryItemEventArgs>? CancelPreview
        {
            add { EventSet.Add(s_CancelPreviewKey, value); }
            remove { EventSet.Remove(s_CancelPreviewKey, value); }
        }

        internal override unsafe HRESULT OnExecute(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            GalleryItemEventArgs eventArgs = GalleryItemEventArgs.Create(*key, *currentValue, commandExecutionProperties)!;
            EventSet.Raise(s_SelectedIndexChangedKey, this, eventArgs);
            return HRESULT.S_OK;
        }

        internal override unsafe HRESULT OnPreview(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet* commandExecutionProperties, bool cancel)
        {
            GalleryItemEventArgs eventArgs = GalleryItemEventArgs.Create(*key, *currentValue, commandExecutionProperties)!;
            if (cancel)
            {
                EventSet.Raise(s_CancelPreviewKey, this, eventArgs);
            }
            else
            {
                EventSet.Raise(s_PreviewKey, this, eventArgs);
            }
            return HRESULT.S_OK;
        }

        private protected override void OnDestroyUICommand(uint commandId, UI_COMMANDTYPE typeID)
        {
            base.OnDestroyUICommand(commandId, typeID);
            if (CommandType != CommandType.Unknown)
            {
                GalleryCategories!.Destroy();
                if (CommandType == CommandType.Collection)
                    GalleryItemItemsSource!.Destroy();
                if (CommandType == CommandType.CommandCollection)
                    GalleryCommandItemsSource!.Destroy();
            }
        }
    }
}
