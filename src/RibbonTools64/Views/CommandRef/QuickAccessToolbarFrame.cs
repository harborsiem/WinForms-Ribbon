using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Controls;

namespace UIRibbonTools
{
    [DesignTimeVisible(false)]
    partial class QuickAccessToolbarFrame : CommandRefObjectFrame
    {
        private static Image sample = ImageManager.QuickAccessToolBarSample();

        private Label Label2 { get => _label2; }
        private ComboBox ComboBoxCustomizeCommand { get => _comboBoxCustomizeCommand; }
        private COMBOBOXINFO _commandInfo;
        private ToolTip _commandTip;

        private TRibbonQuickAccessToolbar _quickAccessToolbar;

        public QuickAccessToolbarFrame()
        {
            bool designtime = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designtime)
                InitializeComponent();
        }

        protected override void InitComponentStep1()
        {
            if (components == null)
                components = new Container();
            _commandTip = new ToolTip(components);
            this._label2 = new System.Windows.Forms.Label();
            this._comboBoxCustomizeCommand = new System.Windows.Forms.ComboBox();
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
            // _label2
            // 
            this._label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this._label2.AutoSize = true;
            this._label2.Location = new System.Drawing.Point(3, 27);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(105, 27);
            this._label2.TabIndex = 0;
            this._label2.Text = "Customize Command";
            this._label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _comboBoxCustomizeCommand
            // 
            this._comboBoxCustomizeCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutPanel.SetColumnSpan(this._comboBoxCustomizeCommand, 3);
            this._comboBoxCustomizeCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxCustomizeCommand.Location = new System.Drawing.Point(123, 30);
            this._comboBoxCustomizeCommand.MaxDropDownItems = 20;
            this._comboBoxCustomizeCommand.Name = "_comboBoxCustomizeCommand";
            this._comboBoxCustomizeCommand.Size = new System.Drawing.Size(250, 21);
            this._comboBoxCustomizeCommand.TabIndex = 2;

            LabelHeader.Text = "  Quick Access Toolbar Properties";
            LabelHeader.ImageIndex = 19;
        }

        protected override void InitComponentStep3()
        {
            this.LayoutPanel.Controls.Add(this._label2, 0, 1);
            this.LayoutPanel.Controls.Add(this._comboBoxCustomizeCommand, 1, 1);

            base.InitComponentStep3();
        }

        protected override void InitResume()
        {

            base.InitResume();
        }

        protected override void InitEvents()
        {
            base.InitEvents();
            ComboBoxCustomizeCommand.SelectedIndexChanged += ComboBoxCustomizeCommandChange;
            ComboBoxCustomizeCommand.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxCustomizeCommand.DrawItem += ComboBoxCommand_DrawItem;
            ComboBoxCustomizeCommand.DropDownClosed += ComboBoxCommand_DropDownClosed;
            ComboBoxCustomizeCommand.HandleCreated += ComboBoxCommand_HandleCreated;
        }

        protected override void InitTooltips(IContainer components)
        {
            base.InitTooltips(components);
            ViewsTip.SetToolTip(ComboBoxCustomizeCommand, "The command to use to customize the quick access toolbar");
        }

        public override void ActivateFrame()
        {
            ViewsFrame frameViews;
            TRibbonCommand cmd;
            base.ActivateFrame();
            frameViews = Owner as ViewsFrame;
            if (ComboBoxCustomizeCommand.SelectedIndex >= 0)
            {
                cmd = RibbonCommandItem.Selected(ComboBoxCustomizeCommand);
                // ComboBoxCustomizeCommand.Items[ComboBoxCustomizeCommand.SelectedIndex] as TRibbonCommand;
            }
            else
                cmd = null;
            ComboBoxCustomizeCommand.Items.Clear();
            ComboBoxCustomizeCommand.Items.AddRange(frameViews.Commands.ToArray());
            if (cmd == null)
                ComboBoxCustomizeCommand.SelectedIndex = 0;
            else
            {
                ComboBoxCustomizeCommand.SelectedIndex = RibbonCommandItem.IndexOf(ComboBoxCustomizeCommand, cmd);
                //ComboBoxCustomizeCommand.Items.IndexOf(cmd);
                if (ComboBoxCustomizeCommand.SelectedIndex < 0)
                    ComboBoxCustomizeCommand.SelectedIndex = 0;
            }
        }

        private void ComboBoxCustomizeCommandChange(
            object sender, EventArgs e)
        {
            TRibbonCommand newRef;
            if (_quickAccessToolbar == null) //@ added
                return;

            if (ComboBoxCustomizeCommand.SelectedIndex < 0)
                newRef = null;
            else
            {
                newRef = RibbonCommandItem.Selected(ComboBoxCustomizeCommand);
                // ComboBoxCustomizeCommand.Items[ComboBoxCustomizeCommand.SelectedIndex] as TRibbonCommand;
            }
            if (newRef != _quickAccessToolbar.CustomizeCommandRef)
            {
                _quickAccessToolbar.CustomizeCommandRef = newRef;
                Modified();
            }
        }

        protected override void Initialize(TRibbonObject subject)
        {
            base.Initialize(subject);
            _quickAccessToolbar = subject as TRibbonQuickAccessToolbar;
            if (_quickAccessToolbar.CustomizeCommandRef == null)
                ComboBoxCustomizeCommand.SelectedIndex = 0;
            else
            {
                ComboBoxCustomizeCommand.SelectedIndex = RibbonCommandItem.IndexOf(ComboBoxCustomizeCommand, _quickAccessToolbar.CustomizeCommandRef);
                // ComboBoxCustomizeCommand.Items.IndexOf(_quickAccessToolbar.CustomizeCommandRef);
            }
        }

        protected override Image SetImageSample()
        {
            return sample;
        }

        private void ComboBoxCommand_HandleCreated(object sender, EventArgs e)
        {
            _commandInfo = COMBOBOXINFO.Create();
            PInvoke.GetComboBoxInfo((HWND)_comboBoxCustomizeCommand.Handle, ref _commandInfo);
        }

        private void ComboBoxCommand_DropDownClosed(object sender, EventArgs e)
        {
            _commandTip.Hide(_comboBoxCustomizeCommand);
        }

        private void ComboBoxCommand_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            string text = _comboBoxCustomizeCommand.GetItemText(_comboBoxCustomizeCommand.Items[e.Index]);
            RibbonCommandItem item = (RibbonCommandItem)_comboBoxCustomizeCommand.Items[e.Index];
            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && (e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)
            {
                RECT comboRect;
                RECT listRect;
                PInvoke.GetWindowRect(_commandInfo.hwndCombo, out comboRect);
                PInvoke.GetWindowRect(_commandInfo.hwndList, out listRect);
                int plusY = listRect.top - comboRect.top;
                _commandTip.Hide(_comboBoxCustomizeCommand);
                _commandTip.Show(item.Description, _comboBoxCustomizeCommand, e.Bounds.Right, e.Bounds.Y + plusY, 2000);
            }
            e.DrawFocusRectangle();
        }
    }
}
