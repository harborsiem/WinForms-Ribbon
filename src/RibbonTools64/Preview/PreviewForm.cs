using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private UI_HSBCOLOR _backgroundDefault;
        private UI_HSBCOLOR _highlightDefault;
        private UI_HSBCOLOR _textDefault;
        private UI_HSBCOLOR _appButtonColorDefault = new UI_HSBCOLOR();
        private UI_HSBCOLOR _backgroundCurrent;
        private UI_HSBCOLOR _highlightCurrent;
        private UI_HSBCOLOR _textCurrent;
        private UI_HSBCOLOR _appButtonColorCurrent;
        private bool _appButtonColorExist = false;
        //private string[] _colorsText = { "Red", "Green", "Blue" };
        //private string[] _hsbText = { "Hue", "Sat.", "Bright." };

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
            numericUpDownB_R.ValueChanged += BackgroundColor_ValueChanged;
            numericUpDownB_G.ValueChanged += BackgroundColor_ValueChanged;
            numericUpDownB_B.ValueChanged += BackgroundColor_ValueChanged;
            numericUpDownH_R.ValueChanged += HighlightColor_ValueChanged;
            numericUpDownH_G.ValueChanged += HighlightColor_ValueChanged;
            numericUpDownH_B.ValueChanged += HighlightColor_ValueChanged;
            numericUpDownT_R.ValueChanged += TextColor_ValueChanged;
            numericUpDownT_G.ValueChanged += TextColor_ValueChanged;
            numericUpDownT_B.ValueChanged += TextColor_ValueChanged;
            this.setColorsButton.Click += new System.EventHandler(this.SetColorsButton_Click);
            this.setDefaultColorsButton.Click += SetDefaultColorsButton_Click;
            this.textButton.Click += TextButton_Click;
            this.highlightButton.Click += HighlightButton_Click;
            this.backgroundButton.Click += BackgroundButton_Click;
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
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _backgroundDefault = _ribbon.GetBackgroundColor();
            _highlightDefault = _ribbon.GetHighlightColor();
            _textDefault = _ribbon.GetTextColor();
            //bool exist = _ribbon.SetApplicationButtonColor(_highlightDefault);
            //if (exist)
            //{
            //    _appButtonColorDefault = _ribbon.GetApplicationButtonColor();
            //}
            //_appButtonColorExist = exist;
            _backgroundCurrent = _backgroundDefault;
            _highlightCurrent = _highlightDefault;
            _textCurrent = _textDefault;
            _appButtonColorCurrent = _appButtonColorDefault;
            Color color = _backgroundCurrent.ToColor();
            numericUpDownB_R.Value = color.R;
            numericUpDownB_G.Value = color.G;
            numericUpDownB_B.Value = color.B;
            color = _highlightCurrent.ToColor();
            numericUpDownH_R.Value = color.R;
            numericUpDownH_G.Value = color.G;
            numericUpDownH_B.Value = color.B;
            color = _textCurrent.ToColor();
            numericUpDownT_R.Value = color.R;
            numericUpDownT_G.Value = color.G;
            numericUpDownT_B.Value = color.B;
            hsbBackground.Text = "HSB: 0x" + _backgroundCurrent.Value.ToString("X8");
            hsbHighlight.Text = "HSB: 0x" + _highlightCurrent.Value.ToString("X8");
            hsbText.Text = "HSB: 0x" + _textCurrent.Value.ToString("X8");
        }

        private void PreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_backgroundCurrent != _backgroundDefault)
                _ribbon.SetBackgroundColor(_backgroundDefault);
            if (_highlightCurrent != _highlightDefault)
                _ribbon.SetHighlightColor(_highlightDefault);
            if (_textCurrent != _textDefault)
                _ribbon.SetTextColor(_textDefault);
            if (_appButtonColorExist && _appButtonColorCurrent != _appButtonColorDefault)
            {
                _ribbon.SetApplicationButtonColor(_appButtonColorDefault);
            }
        }

        private void TextColor_ValueChanged(object sender, EventArgs e)
        {
            Color color = Color.FromArgb((int)numericUpDownT_R.Value, (int)numericUpDownT_G.Value, (int)numericUpDownT_B.Value);
            textColorPanel.BackColor = color;
        }

        private void HighlightColor_ValueChanged(object sender, EventArgs e)
        {
            Color color = Color.FromArgb((int)numericUpDownH_R.Value, (int)numericUpDownH_G.Value, (int)numericUpDownH_B.Value);
            highlightColorPanel.BackColor = color;
        }

        private void BackgroundColor_ValueChanged(object sender, EventArgs e)
        {
            Color color = Color.FromArgb((int)numericUpDownB_R.Value, (int)numericUpDownB_G.Value, (int)numericUpDownB_B.Value);
            backgroundColorPanel.BackColor = color;
        }

        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            SetBackgroundColor(new UI_HSBCOLOR(backgroundColorPanel.BackColor));
        }

        private void HighlightButton_Click(object sender, EventArgs e)
        {
            SetHighlightColor(new UI_HSBCOLOR(highlightColorPanel.BackColor));
        }

        private void TextButton_Click(object sender, EventArgs e)
        {
            SetTextColor(new UI_HSBCOLOR(textColorPanel.BackColor));
        }

        private void SetColorsButton_Click(object sender, EventArgs e)
        {
            SetBackgroundColor(new UI_HSBCOLOR(backgroundColorPanel.BackColor));
            SetHighlightColor(new UI_HSBCOLOR(highlightColorPanel.BackColor));
            SetTextColor(new UI_HSBCOLOR(textColorPanel.BackColor));
        }

        private void SetBackgroundColor(UI_HSBCOLOR hsbColor)
        {
            _ribbon.SetBackgroundColor(_backgroundCurrent = hsbColor);
            hsbBackground.Text = "HSB: 0x" + _backgroundCurrent.Value.ToString("X8");
        }

        private void SetHighlightColor(UI_HSBCOLOR hsbColor)
        {
            _ribbon.SetHighlightColor(_highlightCurrent = hsbColor);
            hsbHighlight.Text = "HSB: 0x" + _highlightCurrent.Value.ToString("X8");
        }

        private void SetTextColor(UI_HSBCOLOR hsbColor)
        {
            _ribbon.SetTextColor(_textCurrent = hsbColor);
            hsbText.Text = "HSB: 0x" + _textCurrent.Value.ToString("X8");
        }

        private void SetDefaultColorsButton_Click(object sender, EventArgs e)
        {
            SetBackgroundColor(_backgroundDefault);
            Color color = _backgroundDefault.ToColor();
            numericUpDownB_R.Value = color.R;
            numericUpDownB_G.Value = color.G;
            numericUpDownB_B.Value = color.B;

            SetHighlightColor(_highlightDefault);
            color = _highlightDefault.ToColor();
            numericUpDownH_R.Value = color.R;
            numericUpDownH_G.Value = color.G;
            numericUpDownH_B.Value = color.B;

            SetTextColor(_textDefault);
            color = _textDefault.ToColor();
            numericUpDownT_R.Value = color.R;
            numericUpDownT_G.Value = color.G;
            numericUpDownT_B.Value = color.B;
        }

        //private void SetRibbonColors(RibbonColors colors)
        //{
        //    numericUpDownB_R.Value = colors.BackgroundColor.R;
        //    numericUpDownB_G.Value = colors.BackgroundColor.G;
        //    numericUpDownB_B.Value = colors.BackgroundColor.B;
        //    numericUpDownH_R.Value = colors.HighlightColor.R;
        //    numericUpDownH_G.Value = colors.HighlightColor.G;
        //    numericUpDownH_B.Value = colors.HighlightColor.B;
        //    numericUpDownT_R.Value = colors.TextColor.R;
        //    numericUpDownT_G.Value = colors.TextColor.G;
        //    numericUpDownT_B.Value = colors.TextColor.B;
        //    backgroundColorPanel.BackColor = colors.BackgroundColor;
        //    highlightColorPanel.BackColor = colors.HighlightColor;
        //    textColorPanel.BackColor = colors.TextColor;
        //}
    }
}
