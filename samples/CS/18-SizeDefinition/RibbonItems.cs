using System;
using System.Drawing;
using System.Windows.Forms;
using _18_SizeDefinition;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public RibbonItems(RibbonStrip ribbon, bool withNames) : this(ribbon)
        {
            if (!withNames)
                return;
            TabHome.Name = nameof(TabHome);
            GroupParagraph.Name = nameof(GroupParagraph);
            DecreaseIndent.Name = nameof(DecreaseIndent);
            IncreaseIndent.Name = nameof(IncreaseIndent);
            StartList.Name = nameof(StartList);
            LineSpacing.Name = nameof(LineSpacing);
            AlignLeft.Name = nameof(AlignLeft);
            AlignCenter.Name = nameof(AlignCenter);
            AlignRight.Name = nameof(AlignRight);
            Justify.Name = nameof(Justify);
            Paragraph.Name = nameof(Paragraph);
            TabSpecialLayouts.Name = nameof(TabSpecialLayouts);
            Group1.Name = nameof(Group1);
            Combo1.Name = nameof(Combo1);
            Button1.Name = nameof(Button1);
            Combo2.Name = nameof(Combo2);
            Button2.Name = nameof(Button2);
            Hidden1.Name = nameof(Hidden1);
            Group2.Name = nameof(Group2);
            Hidden2.Name = nameof(Hidden2);
            Group3.Name = nameof(Group3);
            ButtonLabel.Name = nameof(ButtonLabel);
            Group4.Name = nameof(Group4);
            Group5.Name = nameof(Group5);
            ButtonDate.Name = nameof(ButtonDate);
        }

        public void Init()
        {
            Hidden1.Enabled = false;
            Hidden2.Enabled = false;
            ButtonLabel.Enabled = false;
            Combo1.RepresentativeString = "XXXXXX";
            Combo2.RepresentativeString = "XXXXXX";
            Combo1.ItemsSourceReady += Combo1_ItemsSourceReady;
            Combo1.SelectedIndexChanged += Combo1_SelectedIndexChanged;
            Combo2.ItemsSourceReady += Combo2_ItemsSourceReady;
            Combo2.SelectedIndexChanged += Combo2_SelectedIndexChanged;
            //TabHome.Label = "Home";
            ButtonDate.Click += ButtonDateTime_ExecuteEvent;
            ButtonDate.Label = DateTime.Now.ToShortDateString();
        }

        private void Combo2_SelectedIndexChanged(object sender, GalleryItemEventArgs e)
        {
            string value = Combo2.StringValue;
            SelectedItem<GalleryItemPropertySet> selected = e.SelectedItem;
            int index = selected.SelectedItemIndex;
            GalleryItemPropertySet propertySet = selected.PropertySet;
            int indexFromCombo = Combo2.SelectedItem;
        }

        private void Combo1_SelectedIndexChanged(object sender, GalleryItemEventArgs e)
        {
            string value = Combo1.StringValue;
            SelectedItem<GalleryItemPropertySet> selected = e.SelectedItem;
            int index = selected.SelectedItemIndex;
            GalleryItemPropertySet propertySet = selected.PropertySet;
            int indexFromCombo = Combo1.SelectedItem;
        }

        private void Combo2_ItemsSourceReady(object sender, EventArgs e)
        {
            UICollection<GalleryItemPropertySet> collection = Combo2.GalleryItemItemsSource;
            collection.Add(new GalleryItemPropertySet() { Label = "1", CategoryId = -1 });
            collection.Add(new GalleryItemPropertySet() { Label = "2", CategoryId = -1 });
            collection.Add(new GalleryItemPropertySet() { Label = "3", CategoryId = -1 });
        }

        private void Combo1_ItemsSourceReady(object sender, EventArgs e)
        {
            UICollection<GalleryItemPropertySet> collection = Combo1.GalleryItemItemsSource;
            collection.Add(new GalleryItemPropertySet() { Label = "1", CategoryId = -1 });
            collection.Add(new GalleryItemPropertySet() { Label = "2", CategoryId = -1 });
            collection.Add(new GalleryItemPropertySet() { Label = "3", CategoryId = -1 });
        }

        public void Load()
        {
        }

        private void ButtonDateTime_ExecuteEvent(object sender, EventArgs e)
        {
            DatePicker dialog = new DatePicker();
            int x = System.Windows.Forms.Cursor.Position.X;
            int y = System.Windows.Forms.Cursor.Position.Y;
            dialog.Location = new Point(x, y + 10);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ButtonDate.Label = dialog.Label;
            }
        }

    }
}
