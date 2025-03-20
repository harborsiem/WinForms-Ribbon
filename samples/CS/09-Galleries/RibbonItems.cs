using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using _09_Galleries;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        private RibbonButton[] _buttons;
        private Form1 _form;

        public void Init(Form1 form)
        {
            _form = form;
            DropDownGallery.ItemsSourceReady += new EventHandler<EventArgs>(_dropDownGallery_ItemsSourceReady);
            SplitButtonGallery.CategoriesReady += new EventHandler<EventArgs>(_splitButtonGallery_CategoriesReady);
            SplitButtonGallery.ItemsSourceReady += new EventHandler<EventArgs>(_splitButtonGallery_ItemsSourceReady);
            InRibbonGallery.ItemsSourceReady += new EventHandler<EventArgs>(_inRibbonGallery_ItemsSourceReady);

            DropDownGallery.SelectedIndexChanged += new EventHandler<GalleryItemEventArgs>(_dropDownGallery_ExecuteEvent);
            DropDownGallery.Preview += new EventHandler<GalleryItemEventArgs>(_dropDownGallery_OnPreview);
            DropDownGallery.CancelPreview += new EventHandler<GalleryItemEventArgs>(_dropDownGallery_OnCancelPreview);
        }

        unsafe void _dropDownGallery_ItemsSourceReady(object sender, EventArgs e)
        {
            // set label
            DropDownGallery.Label = "Size";

            // set _dropDownGallery items
            UICollection<GalleryItemPropertySet> itemsSource = DropDownGallery.GalleryItemItemsSource;
            itemsSource.Clear();
            foreach (Image image in _form.ImageListLines.Images)
            {
                itemsSource.Add(new GalleryItemPropertySet()
                {
                    ItemImage = new UIImage(Ribbon, (Bitmap)image).UIImageHandle //.ConvertToUIImage((Bitmap)image)
                }); 
            }
        }

        void _splitButtonGallery_CategoriesReady(object sender, EventArgs e)
        {
            // set _splitButtonGallery categories
            UICollection<GalleryItemPropertySet> categories = SplitButtonGallery.GalleryCategories;
            categories.Clear();
            categories.Add(new GalleryItemPropertySet() { Label = "Category 1", CategoryId = 1 });
        }

        unsafe void _splitButtonGallery_ItemsSourceReady(object sender, EventArgs e)
        {
            // set label
            SplitButtonGallery.Label = "Brushes";

            // prepare helper classes for commands
            _buttons = new RibbonButton[_form.ImageListBrushes.Images.Count];
            uint i;
            for (i = 0; i < _buttons.Length; ++i)
            {
                _buttons[i] = new RibbonButton(Ribbon, 2000 + i)
                {
                    Label = "Label " + i.ToString(),
                    LargeImage = new UIImage(Ribbon, (Bitmap)_form.ImageListBrushes.Images[(int)i]).UIImageHandle //.ConvertToUIImage((Bitmap)_form.ImageListBrushes.Images[(int)i])
                };
            }

            // set _splitButtonGallery items
            UICollection<GalleryCommandPropertySet> itemsSource = SplitButtonGallery.GalleryCommandItemsSource;
            itemsSource.Clear();
            i = 0;
            foreach (Image image in _form.ImageListBrushes.Images)
            {
                itemsSource.Add(new GalleryCommandPropertySet()
                {
                    CommandId = 2000 + i++,
                    CommandType = CommandType.Action,
                    CategoryId = 1
                });
            }
        }

        unsafe void _inRibbonGallery_ItemsSourceReady(object sender, EventArgs e)
        {
            // set _inRibbonGallery items
            UICollection<GalleryItemPropertySet> itemsSource = InRibbonGallery.GalleryItemItemsSource;
            itemsSource.Clear();
            foreach (Image image in _form.ImageListShapes.Images)
            {
                itemsSource.Add(new GalleryItemPropertySet()
                {
                    ItemImage = new UIImage(Ribbon, (Bitmap)image).UIImageHandle //.ConvertToUIImage((Bitmap)image)
                });
            }
        }

        void _dropDownGallery_OnCancelPreview(object sender, GalleryItemEventArgs e)
        {
            SelectedItem<GalleryItemPropertySet> item = e.SelectedItem;
            Debug.WriteLine("DropDownGallery::OnCancelPreview");
        }

        void _dropDownGallery_OnPreview(object sender, GalleryItemEventArgs e)
        {
            SelectedItem<GalleryItemPropertySet> item = e.SelectedItem;
            Debug.WriteLine("DropDownGallery::OnPreview");
        }

        void _dropDownGallery_ExecuteEvent(object sender, GalleryItemEventArgs e)
        {
            SelectedItem<GalleryItemPropertySet> item = e.SelectedItem;
            Debug.WriteLine("DropDownGallery::Selected = " + item.SelectedItemIndex);
            Debug.WriteLine("DropDownGallery::ExecuteEvent");
        }

        public void Load()
        {
        }

    }
}
