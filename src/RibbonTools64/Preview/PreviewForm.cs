using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.CompilerServices;
using WinForms.Ribbon;

namespace UIRibbonTools
{
    partial class PreviewForm : Form
    {
        private WinForms.Ribbon.RibbonStrip _ribbon;
        private BuildPreviewHelper _buildPreviewHelper;
        private List<RibbonTabGroup> _tabGroups = new List<RibbonTabGroup>();
        private RibbonClassBuilder _classBuilder;
        //private UI_HSBCOLOR _appButtonColorDefault = new UI_HSBCOLOR();
        //private bool _appButtonColorExist = false;

        public PreviewForm()
        {
            _buildPreviewHelper = BuildPreviewHelper.Instance;
            InitializeComponent();
            this.FormClosing += PreviewForm_FormClosing;

            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            // 
            // ribbon
            // 
            this._ribbon = new WinForms.Ribbon.RibbonStrip();
            this._ribbon.Location = new System.Drawing.Point(0, 0);
            this._ribbon.Name = "ribbon";
            this._ribbon.Size = new System.Drawing.Size(933, 132);
            this._ribbon.TabIndex = 0;
            _ribbon.ResourceIdentifier = _buildPreviewHelper.ResourceIdentifier;
            _ribbon.MarkupResource = _buildPreviewHelper.MarkupResource;
            _ribbon.RibbonHeightChanged += Ribbon_RibbonHeightChanged;
            this.Controls.Add(this._ribbon);
            _classBuilder = new RibbonClassBuilder(_ribbon);

            Load += MainForm_Load;
            Shown += MainForm_Shown;
            InitializeApplicationModes();
            InitializeContextualTabs(); //RibbonTabGroup
            InitializeContextPopups();
            InitializeComboBoxes();
            InitializeSpinners();
            InitializeColorization();
            checkedListBoxAppModes.ItemCheck += CheckListBoxAppModesClickCheck;
            checkedListBoxContextTabs.ItemCheck += CheckListBoxContextTabsClickCheck;
            listBoxContextPopups.SelectedIndexChanged += ListBoxContextPopupsClick;
            this.setColorsButton.Click += new System.EventHandler(this.SetColorsButton_Click);
            this.setDefaultColorsButton.Click += SetDefaultColorsButton_Click;
        }

        private void Ribbon_RibbonHeightChanged(object sender, EventArgs e)
        {
            Control control = tabControl;
            int height = _ribbon.Height;
            Rectangle bounds = control.Bounds;
            bounds.Height -= (height + control.Margin.Top - bounds.Y);
            bounds.Y = height + control.Margin.Top;
            control.Bounds = bounds;
        }

        private void InitializeComboBoxes()
        {
            List<RibbonItem> selectedItems = new List<RibbonItem>();
            IList<RibbonItem> items = _buildPreviewHelper.Parser.Results.RibbonItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].RibbonClassName == "RibbonComboBox")
                {
                    selectedItems.Add(items[i]);
                }
            }
            for (int i = 0; i < selectedItems.Count; i++)
            {
                RibbonItem item = selectedItems[i];
                RibbonComboBox combo = (RibbonComboBox)_classBuilder.BuildRibbonClass(item.RibbonClassName, item.CommandName, item.CommandId);
                combo.RepresentativeString = "XXXXXX";
            }
        }

        private void InitializeSpinners()
        {
            List<RibbonItem> selectedItems = new List<RibbonItem>();
            IList<RibbonItem> items = _buildPreviewHelper.Parser.Results.RibbonItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].RibbonClassName == "RibbonSpinner")
                {
                    selectedItems.Add(items[i]);
                }
            }
            for (int i = 0; i < selectedItems.Count; i++)
            {
                RibbonItem item = selectedItems[i];
                RibbonSpinner spinner = (RibbonSpinner)_classBuilder.BuildRibbonClass(item.RibbonClassName, item.CommandName, item.CommandId);
                spinner.RepresentativeString = "XXXXX";
            }
        }

        private void CheckListBoxAppModesClickCheck(object sender, ItemCheckEventArgs e)
        {
            int i, j;
            List<byte> appModes = new List<byte>();
            for (i = 0; i < checkedListBoxAppModes.Items.Count; i++)
            {
                if (checkedListBoxAppModes.GetItemChecked(i) || (e.Index == i && e.NewValue == CheckState.Checked))
                {
                    if (!(e.Index == i && e.NewValue == CheckState.Unchecked))
                    {
                        j = int.Parse((string)checkedListBoxAppModes.Items[i]);
                        appModes.Add((byte)j);
                    }
                }
            }
            if (appModes.Count == 0)
            {
                appModes.Add(0);
                checkedListBoxAppModes.SetItemChecked(0, true);
            }
            byte[] ba = appModes.ToArray();
            _ribbon.SetModes(ba);
        }

        private void InitializeApplicationModes()
        {
            int i;
            uint allApplicationModes = _buildPreviewHelper.Parser.Results.AllApplicationModes;
            checkedListBoxAppModes.Visible = (allApplicationModes != 0);
            labelAppModes.Visible = (allApplicationModes == 0);
            if (allApplicationModes != 0)
            {
                checkedListBoxAppModes.Items.Insert(0, "0");
                for (i = 1; i < 32; i++)
                {
                    if ((allApplicationModes & (1 << i)) != 0)
                    {
                        checkedListBoxAppModes.Items.Add(i.ToString());
                    }
                }
                checkedListBoxAppModes.SetItemChecked(0, true);
            }
        }

        private void CheckListBoxContextTabsClickCheck(object sender, ItemCheckEventArgs e)
        {
            RibbonTabGroup tabGroup = _tabGroups[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                tabGroup.ContextAvailable = ContextAvailability.Available;
            }
            if (e.NewValue == CheckState.Unchecked)
            {
                tabGroup.ContextAvailable = ContextAvailability.NotAvailable;
            }
        }

        private void InitializeContextualTabs()
        {
            List<RibbonItem> selectedItems = new List<RibbonItem>();
            IList<RibbonItem> items = _buildPreviewHelper.Parser.Results.RibbonItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].RibbonClassName == "RibbonTabGroup")
                {
                    selectedItems.Add(items[i]);
                }
            }
            for (int i = 0; i < selectedItems.Count; i++)
            {
                RibbonItem item = selectedItems[i];
                checkedListBoxContextTabs.Items.Add(item);
                RibbonTabGroup tabGroup = (RibbonTabGroup)_classBuilder.BuildRibbonClass(item.RibbonClassName, item.CommandName, item.CommandId);
                _tabGroups.Add(tabGroup);
            }

            checkedListBoxContextTabs.Visible = (checkedListBoxContextTabs.Items.Count > 0);
            labelContextTabs.Visible = (checkedListBoxContextTabs.Items.Count == 0);
        }

        private void ListBoxContextPopupsClick(object sender, EventArgs e)
        {
            if (listBoxContextPopups.SelectedIndex < 0)
                return;

            RibbonItem item = (RibbonItem)(listBoxContextPopups.Items[listBoxContextPopups.SelectedIndex]);
            _ribbon.ShowContextPopup(item.CommandId, Cursor.Position.X, Cursor.Position.Y);
        }

        private void InitializeContextPopups()
        {
            List<RibbonItem> selectedItems = new List<RibbonItem>();
            IList<RibbonItem> items = _buildPreviewHelper.Parser.Results.RibbonItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsContextPopup)
                {
                    selectedItems.Add(items[i]);
                }
            }
            for (int i = 0; i < selectedItems.Count; i++)
            {
                listBoxContextPopups.Items.Add(selectedItems[i]);
            }

            listBoxContextPopups.Visible = (listBoxContextPopups.Items.Count > 0);
            labelContextPopups.Visible = (listBoxContextPopups.Items.Count == 0);
        }

        private void InitializeColorization()
        {
            radioRGB.Checked = true;
            radioRGB.CheckedChanged += RadioRGB_CheckedChanged;
            radioHSB.CheckedChanged += RadioHSB_CheckedChanged;
        }

        private void RadioRGB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRGB.Checked)
            {
                backgroundColorFrame.ColorSelection = ColorSelection.Color;
                highlightColorFrame.ColorSelection = ColorSelection.Color;
                textColorFrame.ColorSelection = ColorSelection.Color;
            }
        }

        private void RadioHSB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioHSB.Checked)
            {
                backgroundColorFrame.ColorSelection = ColorSelection.Hsb;
                highlightColorFrame.ColorSelection = ColorSelection.Hsb;
                textColorFrame.ColorSelection = ColorSelection.Hsb;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            backgroundColorFrame.Init(_ribbon.GetBackgroundColor(), WhichColor.Background, _ribbon.SetBackgroundColor);
            highlightColorFrame.Init(_ribbon.GetHighlightColor(), WhichColor.Highlight, _ribbon.SetHighlightColor);
            textColorFrame.Init(_ribbon.GetTextColor(), WhichColor.Text, _ribbon.SetTextColor);
            //bool exist = _ribbon.SetApplicationButtonColor(_highlightDefault);
            //if (exist)
            //{
            //    _appButtonColorDefault = _ribbon.GetApplicationButtonColor();
            //}
            //_appButtonColorExist = exist;
        }

        private void PreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //We can not set colors here, because the UI ribbon render wrong next time Preview with ColumnBreaks
            //_ribbon.SetBackgroundColor(backgroundColorFrame.HsbDefault);
            //_ribbon.SetHighlightColor(highlightColorFrame.HsbDefault);
            //_ribbon.SetTextColor(textColorFrame.HsbDefault);
            //if (_appButtonColorExist && _appButtonColorCurrent != _appButtonColorDefault)
            //{
            //    _ribbon.SetApplicationButtonColor(_appButtonColorDefault);
            //}
        }

        private void SetColorsButton_Click(object sender, EventArgs e)
        {
            backgroundColorFrame.SetThisColor(backgroundColorFrame.HsbSelected);
            highlightColorFrame.SetThisColor(highlightColorFrame.HsbSelected);
            textColorFrame.SetThisColor(textColorFrame.HsbSelected);
        }

        private void SetDefaultColorsButton_Click(object sender, EventArgs e)
        {
            backgroundColorFrame.SetDefaultColor();
            highlightColorFrame.SetDefaultColor();
            textColorFrame.SetDefaultColor();
        }
    }
}
