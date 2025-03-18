using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIRibbonTools
{
    [DesignTimeVisible(false)]
    partial class AppMenuGroupFrame : CommandRefObjectFrame
    {
        private static Image sample = ImageManager.MenuGroupSample();

        private System.Windows.Forms.Label _labelCategory;
        private System.Windows.Forms.ComboBox _comboBoxCategory;

        private Label LabelCategory { get => _labelCategory; }
        private ComboBox ComboBoxCategory { get => _comboBoxCategory; }

        public TRibbonAppMenuGroup _menuGroup;

        public AppMenuGroupFrame()
        {
            bool designtime = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designtime)
                InitializeComponent();
        }

        protected override void InitComponentStep1()
        {
            if (components == null)
                components = new Container();
            this._labelCategory = new System.Windows.Forms.Label();
            this._comboBoxCategory = new System.Windows.Forms.ComboBox();

            base.InitComponentStep1();
        }

        protected override void InitSuspend()
        {

            base.InitSuspend();
        }

        protected override void InitComponentStep2()
        {
            base.InitComponentStep2();

            // 
            // _labelCategory
            // 
            this._labelCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this._labelCategory.AutoSize = true;
            this._labelCategory.Location = new System.Drawing.Point(3, 27);
            this._labelCategory.Name = "_labelCategory";
            this._labelCategory.Size = new System.Drawing.Size(77, 27);
            this._labelCategory.TabIndex = 0;
            this._labelCategory.Text = "Category Class";
            this._labelCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _comboBoxCategory
            // 
            this._comboBoxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutPanel.SetColumnSpan(this._comboBoxCategory, 3);
            this._comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxCategory.Items.AddRange(new object[] {
            "Standard Items",
            "Major Items"});
            this._comboBoxCategory.Location = new System.Drawing.Point(123, 30);
            this._comboBoxCategory.MaxDropDownItems = 20;
            this._comboBoxCategory.Name = "_comboBoxCategory";
            this._comboBoxCategory.Size = new System.Drawing.Size(250, 21);
            this._comboBoxCategory.TabIndex = 2;

            LabelHeader.Text = "  Application Menu Group Properties";
            LabelHeader.ImageIndex = 24;
        }

        protected override void InitComponentStep3()
        {
            this.LayoutPanel.Controls.Add(this._labelCategory, 0, 1);
            this.LayoutPanel.Controls.Add(this._comboBoxCategory, 1, 1);

            base.InitComponentStep3();
        }

        protected override void InitResume()
        {

            base.InitResume();
        }

        protected override void InitEvents()
        {
            base.InitEvents();
            ComboBoxCategory.SelectedIndexChanged += ComboBoxCategoryChange;
        }

        protected override void InitTooltips(IContainer components)
        {
            base.InitTooltips(components);
            ViewsTip.SetToolTip(ComboBoxCategory,
                "Whether this group contains Major Items (large buttons, default)," + Environment.NewLine +
                "or Standard Items (small buttons)");
        }

        private void ComboBoxCategoryChange(object sender, EventArgs e)
        {
            if (IsInInitialize) return;
            if (ComboBoxCategory.SelectedIndex != (int)(_menuGroup.CategoryClass))
            {
                _menuGroup.CategoryClass = (RibbonMenuCategoryClass)(ComboBoxCategory.SelectedIndex);
                Modified();
            }
        }

        protected override void Initialize(TRibbonObject subject)
        {
            base.Initialize(subject);
            _menuGroup = subject as TRibbonAppMenuGroup;
            ComboBoxCategory.SelectedIndex = (int)(_menuGroup.CategoryClass);
        }

        protected override Image SetImageSample()
        {
            return sample;
        }
    }
}
