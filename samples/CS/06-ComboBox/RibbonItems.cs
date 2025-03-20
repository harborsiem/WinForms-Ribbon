using System;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {

        public void Init()
        {
            ButtonDropA.Click += new EventHandler<EventArgs>(_buttonDropA_ExecuteEvent);
            ButtonDropB.Click += new EventHandler<EventArgs>(_buttonDropB_ExecuteEvent);
            ButtonDropC.Click += new EventHandler<EventArgs>(_buttonDropC_ExecuteEvent);
            ButtonDropD.Click += new EventHandler<EventArgs>(_buttonDropD_ExecuteEvent);
            ButtonDropE.Click += new EventHandler<EventArgs>(_buttonDropE_ExecuteEvent);
            ButtonDropF.Click += new EventHandler<EventArgs>(_buttonDropF_ExecuteEvent);

            InitComboBoxes();
        }

        void _buttonDropA_ExecuteEvent(object sender, EventArgs e)
        {
            // get selected item index from combo box 1
            int selectedItemIndex = ComboBox1.SelectedItem;

            if (selectedItemIndex == -1)
            {
                MessageBox.Show("No item is selected in simple combo");
            }
            else
            {
                string label = ComboBox1.GalleryItemItemsSource[(int)selectedItemIndex].Label;
                MessageBox.Show("Selected item in simple combo is: " + label);
            }
        }

        void _buttonDropB_ExecuteEvent(object sender, EventArgs e)
        {
            // get string value from combo box 2
            string stringValue = ComboBox2.StringValue;
            MessageBox.Show("String value in advanced combo is: " + stringValue);
        }

        void _buttonDropC_ExecuteEvent(object sender, EventArgs e)
        {
            foreach (GalleryItemPropertySet propSet in ComboBox1.GalleryItemItemsSource)
            {
                string label = propSet.Label;
                MessageBox.Show("Label = " + label);
            }
        }

        void _buttonDropD_ExecuteEvent(object sender, EventArgs e)
        {
            ComboBox1.GalleryItemItemsSource.ChangedEvent += new EventHandler<CollectionChangedEventArgs>(_uiCollectionChangedEvent_ChangedEvent);
        }

        void _buttonDropE_ExecuteEvent(object sender, EventArgs e)
        {
            UICollection<GalleryItemPropertySet> itemsSource1 = ComboBox1.GalleryItemItemsSource;
            int count = itemsSource1.Count;
            count++;
            itemsSource1.Add(new GalleryItemPropertySet() { Label = "Label " + count.ToString(), CategoryId = -1 });
        }

        void _buttonDropF_ExecuteEvent(object sender, EventArgs e)
        {
            ComboBox1.GalleryItemItemsSource.ChangedEvent -= new EventHandler<CollectionChangedEventArgs>(_uiCollectionChangedEvent_ChangedEvent);
        }

        void _uiCollectionChangedEvent_ChangedEvent(object sender, CollectionChangedEventArgs e)
        {
            MessageBox.Show("Got ChangedEvent. Action = " + e.Action.ToString());
        }

        private void InitComboBoxes()
        {
            ComboBox1.RepresentativeString = "Label 1";
            ComboBox2.RepresentativeString = "XXXXXXXXXXX";
            ComboBox3.RepresentativeString = "XXXXXXXXXXX";

            ComboBox1.Label = "Simple Combo";
            ComboBox2.Label = "Advanced Combo";
            ComboBox3.Label = "Another Combo";

            ComboBox1.ItemsSourceReady += new EventHandler<EventArgs>(_comboBox1_ItemsSourceReady);

            ComboBox2.CategoriesReady += new EventHandler<EventArgs>(_comboBox2_CategoriesReady);
            ComboBox2.ItemsSourceReady += new EventHandler<EventArgs>(_comboBox2_ItemsSourceReady);

            ComboBox3.ItemsSourceReady += new EventHandler<EventArgs>(_comboBox3_ItemsSourceReady);
        }

        void _comboBox1_ItemsSourceReady(object sender, EventArgs e)
        {
            // set combobox1 items
            UICollection<GalleryItemPropertySet> itemsSource1 = ComboBox1.GalleryItemItemsSource;
            itemsSource1.Clear();
            itemsSource1.Add(new GalleryItemPropertySet() { Label = "Label 1", CategoryId = -1 });
            itemsSource1.Add(new GalleryItemPropertySet() { Label = "Label 2", CategoryId = -1 });
            itemsSource1.Add(new GalleryItemPropertySet() { Label = "Label 3", CategoryId = -1 });

        }

        void _comboBox2_CategoriesReady(object sender, EventArgs e)
        {
            // set _comboBox2 categories
            UICollection<GalleryItemPropertySet> categories2 = ComboBox2.GalleryCategories;
            categories2.Clear();
            categories2.Add(new GalleryItemPropertySet() { Label = "Category 1", CategoryId = 1 });
            categories2.Add(new GalleryItemPropertySet() { Label = "Category 2", CategoryId = 2 });
        }

        void _comboBox2_ItemsSourceReady(object sender, EventArgs e)
        {
            // set _comboBox2 items
            UICollection<GalleryItemPropertySet> itemsSource2 = ComboBox2.GalleryItemItemsSource;
            itemsSource2.Clear();
            itemsSource2.Add(new GalleryItemPropertySet() { Label = "Label 1", CategoryId = 1 });
            itemsSource2.Add(new GalleryItemPropertySet() { Label = "Label 2", CategoryId = 1 });
            itemsSource2.Add(new GalleryItemPropertySet() { Label = "Label 3", CategoryId = 2 });
        }

        void _comboBox3_ItemsSourceReady(object sender, EventArgs e)
        {
            // set combobox3 items
            UICollection<GalleryItemPropertySet> itemsSource3 = ComboBox3.GalleryItemItemsSource;
            itemsSource3.Clear();
            itemsSource3.Add(new GalleryItemPropertySet() { Label = "Label 1", CategoryId = -1 });
            itemsSource3.Add(new GalleryItemPropertySet() { Label = "Label 2", CategoryId = -1 });
            itemsSource3.Add(new GalleryItemPropertySet() { Label = "Label 3", CategoryId = -1 });
        }

        public void Load()
        {
        }

    }
}
