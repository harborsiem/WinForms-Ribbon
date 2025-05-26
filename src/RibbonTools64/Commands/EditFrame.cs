using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace UIRibbonTools
{
    partial class EditFrame : UserControl
    {
        private CommandsFrame _frame;
        private TRibbonCommand _command;

        public EditFrame()
        {
            InitializeComponent();
            if (components == null)
                components = new Container();
            Load += EditFrame_Load;
        }

        private void EditFrame_Load(object sender, EventArgs e)
        {
            if (DeviceDpi != 96) //Workaround for wrong Margins of NumericUpDown
            {
                Size thisSize = this.Size;
                Padding upDownMargin = EditId.Margin;
                Padding margin = EditName.Margin;
                int diffHeight = (upDownMargin.Top + upDownMargin.Bottom - margin.Top - margin.Bottom) * 5;
                int diffWidth = (upDownMargin.Left + upDownMargin.Right - margin.Left - margin.Right) * 5;
                _propertiesPanel.SuspendLayout();
                this.SuspendLayout();
                EditId.Margin = margin;
                EditCaptionId.Margin = margin;
                EditDescriptionId.Margin = margin;
                EditTooltipTitleId.Margin = margin;
                EditTooltipDescriptionId.Margin = margin;
                EditKeytipId.Margin = margin;
                _propertiesPanel.ResumeLayout(false);
                _propertiesPanel.PerformLayout();
                int endPositionY = EditComment.Location.Y + EditComment.Size.Height;
                this.Size = new Size(thisSize.Width - diffWidth, endPositionY + margin.Bottom);
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        //    public void InitMargins()
        //    {
        //        EditName.Margin = new Padding(3);
        //        EditId.Margin = new Padding(3);
        //        EditSymbol.Margin = new Padding(3);
        //        EditCaption.Margin = new Padding(3);
        //        EditCaptionId.Margin = new Padding(3);
        //        EditCaptionSymbol.Margin = new Padding(3);
        //        EditDescription.Margin = new Padding(3);
        //        EditDescriptionId.Margin = new Padding(3);
        //        EditDescriptionSymbol.Margin = new Padding(3);
        //        EditTooltipTitle.Margin = new Padding(3);
        //        EditTooltipTitleId.Margin = new Padding(3);
        //        EditTooltipTitleSymbol.Margin = new Padding(3);
        //        EditTooltipDescription.Margin = new Padding(3);
        //        EditTooltipDescriptionId.Margin = new Padding(3);
        //        EditTooltipDescriptionSymbol.Margin = new Padding(3);
        //        EditKeytip.Margin = new Padding(3);
        //        EditKeytipId.Margin = new Padding(3);
        //        EditKeytipSymbol.Margin = new Padding(3);
        //        EditComment.Margin = new Padding(3);
        //    }

        public void SetBoldFonts()
        {
            labelProperty.Font = new Font(labelProperty.Font, FontStyle.Bold);
            labelValue.Font = new Font(labelValue.Font, FontStyle.Bold);
            labelID.Font = new Font(labelID.Font, FontStyle.Bold);
            labelSymbol.Font = new Font(labelSymbol.Font, FontStyle.Bold);
        }

        internal void InitAddon()
        {
            InitEvents();
            InitToolTips();
        }

        internal void ShowSelection(CommandsFrame frame, TRibbonCommand command)
        {
            _frame = frame;
            _command = command;
            if (command != null)
            {
                frame.EnableControls(true);
                EditName.Text = command.Name;
                EditSymbol.Text = command.Symbol;
                EditId.Value = command.Id;
                EditComment.Text = command.Comment;

                EditCaption.Text = ContentToTextbox(command.LabelTitle.Content);
                EditCaptionId.Value = command.LabelTitle.Id;
                EditCaptionSymbol.Text = command.LabelTitle.Symbol;

                EditDescription.Text = ContentToTextbox(command.LabelDescription.Content);
                EditDescriptionId.Value = command.LabelDescription.Id;
                EditDescriptionSymbol.Text = command.LabelDescription.Symbol;

                EditTooltipTitle.Text = ContentToTextbox(command.TooltipTitle.Content);
                EditTooltipTitleId.Value = command.TooltipTitle.Id;
                EditTooltipTitleSymbol.Text = command.TooltipTitle.Symbol;

                EditTooltipDescription.Text = ContentToTextbox(command.TooltipDescription.Content);
                EditTooltipDescriptionId.Value = command.TooltipDescription.Id;
                EditTooltipDescriptionSymbol.Text = command.TooltipDescription.Symbol;

                EditKeytip.Text = command.Keytip.Content;
                EditKeytipId.Value = command.Keytip.Id;
                EditKeytipSymbol.Text = command.Keytip.Symbol;
            }
            else
            {
                frame.EnableControls(false);
                EditName.Text = string.Empty;
                EditSymbol.Text = string.Empty;
                EditId.Value = 0;
                EditComment.Text = string.Empty;

                EditCaption.Text = string.Empty;
                EditCaptionId.Value = 0;
                EditCaptionSymbol.Text = string.Empty;

                EditDescription.Text = string.Empty;
                EditDescriptionId.Value = 0;
                EditDescriptionSymbol.Text = string.Empty;

                EditTooltipTitle.Text = string.Empty;
                EditTooltipTitleId.Value = 0;
                EditTooltipTitleSymbol.Text = string.Empty;

                EditTooltipDescription.Text = string.Empty;
                EditTooltipDescriptionId.Value = 0;
                EditTooltipDescriptionSymbol.Text = string.Empty;

                EditKeytip.Text = string.Empty;
                EditKeytipId.Value = 0;
                EditKeytipSymbol.Text = string.Empty;
            }
        }

        internal void EditNameSelect()
        {
            EditName.Select();
        }

        internal void EditIdText(string text)
        {
            EditId.Text = text;
        }

        private void InitEvents()
        {
            EditName.TextChanged += EditNameChange;
            EditName.KeyPress += EditNameKeyPress;
            EditId.TextChanged += EditIdChange;
            EditId.ValueChanged += UpDownChanging;
            EditSymbol.TextChanged += EditSymbolChange;
            EditSymbol.KeyPress += EditNameKeyPress;

            EditCaption.TextChanged += EditCaptionChange;
            EditCaptionId.TextChanged += EditCaptionIdChange;
            EditCaptionId.ValueChanged += UpDownChanging;
            EditCaptionSymbol.TextChanged += EditCaptionSymbolChange;

            EditDescription.TextChanged += EditDescriptionChange;
            EditDescriptionId.TextChanged += EditDescriptionIdChange;
            EditDescriptionId.ValueChanged += UpDownChanging;
            EditDescriptionSymbol.TextChanged += EditDescriptionSymbolChange;

            EditTooltipTitle.TextChanged += EditTooltipTitleChange;
            EditTooltipTitleId.TextChanged += EditTooltipTitleIdChange;
            EditTooltipTitleId.ValueChanged += UpDownChanging;
            EditTooltipTitleSymbol.TextChanged += EditTooltipTitleSymbolChange;

            EditTooltipDescription.TextChanged += EditTooltipDescriptionChange;
            EditTooltipDescriptionId.TextChanged += EditTooltipDescriptionIdChange;
            EditTooltipDescriptionId.ValueChanged += UpDownChanging;
            EditTooltipDescriptionSymbol.TextChanged += EditTooltipDescriptionSymbolChange;

            EditKeytip.TextChanged += EditKeytipChange;
            EditKeytipId.TextChanged += EditKeytipIdChange;
            EditKeytipId.ValueChanged += UpDownChanging;
            EditKeytipSymbol.TextChanged += EditKeyTipSymbolChange;

            EditComment.TextChanged += EditCommentChange;
        }

        private void InitToolTips()
        {
            ToolTip commandsTip = new ToolTip(components);
            commandsTip.SetToolTip(EditName,
                "The command name is used to connect commands with controls." + Environment.NewLine +
                "Unless you specify a Symbol name, this name is also used" + Environment.NewLine +
                "as the name of the constant for this command.");
            commandsTip.IsBalloon = false;
            //commandsTip.Popup += CommandsTip_Popup;
            commandsTip.SetToolTip(EditId,
                "A unique numeric identifier for the command (the value of" + Environment.NewLine +
                "the Symbol constant). Use 0 for auto-generated identifiers.");
            commandsTip.SetToolTip(EditSymbol,
                "This is the name of the constant that will be generated to access" + Environment.NewLine +
                "this command. If not specified, the command Name is used.");
            commandsTip.SetToolTip(EditCaption, "The caption/label title for the command.");
            commandsTip.SetToolTip(EditCaptionId,
                "Numeric resource string identifier for the caption." + Environment.NewLine +
                "Use 0 for auto-generated identifiers.");
            commandsTip.SetToolTip(EditCaptionSymbol,
                "Constant name for the resource identifier." + Environment.NewLine +
                "If not specified, it is automatically generated.");
            commandsTip.SetToolTip(EditDescription,
                "The label description for the command." + Environment.NewLine +
                "Is used when the command is displayed in the application menu.");
            commandsTip.SetToolTip(EditDescriptionId,
                "Numeric resource string identifier for the description." + Environment.NewLine +
                "Use 0 for auto-generated identifiers.");
            commandsTip.SetToolTip(EditDescriptionSymbol,
                "Constant name for the resource identifier." + Environment.NewLine +
                "If not specified, it is automatically generated.");
            commandsTip.SetToolTip(EditTooltipTitle,
                "The tooltip title for the command." + Environment.NewLine +
                "(This is the bold caption of the tooltip)");
            commandsTip.SetToolTip(EditTooltipTitleId,
                "Numeric resource string identifier for the tooltip title." + Environment.NewLine +
                "Use 0 for auto-generated identifiers.");
            commandsTip.SetToolTip(EditTooltipTitleSymbol,
                "Constant name for the resource identifier." + Environment.NewLine +
                "If not specified, it is automatically generated.");
            commandsTip.SetToolTip(EditTooltipDescription,
                "The tooltip description for the command." + Environment.NewLine +
                "(Is displayed below the tooltip title in the tooltip popup)");
            commandsTip.SetToolTip(EditTooltipDescriptionId,
                "Numeric resource string identifier for the tooltip description." + Environment.NewLine +
                "Use 0 for auto-generated identifiers.");
            commandsTip.SetToolTip(EditTooltipDescriptionSymbol,
                "Constant name for the resource identifier." + Environment.NewLine +
                "If not specified, it is automatically generated.");
            commandsTip.SetToolTip(EditKeytip,
                "The keytip for the command. This is key sequence that is shown" + Environment.NewLine +
                "when the user pressed the Alt key to access ribbon controls.");
            commandsTip.SetToolTip(EditKeytipId,
                "Numeric resource string identifier for the keytip." + Environment.NewLine +
                "Use 0 for auto-generated identifiers.");
            commandsTip.SetToolTip(EditKeytipSymbol,
                "Constant name for the resource identifier." + Environment.NewLine +
                "If not specified, it is automatically generated.");
            commandsTip.SetToolTip(EditComment,
                "This text is placed as a comment in the *.h file" + Environment.NewLine +
                "containing the constant for this command.");
        }

        private void EditCaptionChange(object sender, EventArgs e)
        {
            //if (_command != null && (_command.LabelTitle.Content != EditCaption.Text))
            //{
            //    _command.LabelTitle.Content = EditCaption.Text;
            //    ListViewCommands.SelectedItems[0].SubItems[1].Text = EditCaption.Text;
            //    Modified();
            //}
            if (_command != null)
            {
                string textboxContent = TextboxToContent(EditCaption.Text);
                if (_command.LabelTitle.Content != textboxContent)
                {
                    _command.LabelTitle.Content = textboxContent;
                    _frame.ListViewCommands.SelectedItems[0].SubItems[1].Text = EditCaption.Text;
                    Modified();
                }
            }
        }

        private void EditCaptionIdChange(object sender, EventArgs e)
        {
            if (_command != null && (EditCaptionId.Value != 1) && (_command.LabelTitle.Id != EditCaptionId.Value))
            {
                _command.LabelTitle.Id = (int)EditCaptionId.Value;
                Modified();
            }
        }

        private void EditCaptionSymbolChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.LabelTitle.Symbol != EditCaptionSymbol.Text))
            {
                _command.LabelTitle.Symbol = EditCaptionSymbol.Text;
                Modified();
            }
        }

        private void EditCommentChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.Comment != EditComment.Text))
            {
                _command.Comment = EditComment.Text;
                Modified();
            }
        }

        private void EditDescriptionChange(object sender, EventArgs e)
        {
            if (_command != null)
            {
                string textboxContent = TextboxToContent(EditDescription.Text);
                if (_command.LabelDescription.Content != textboxContent)
                {
                    _command.LabelDescription.Content = textboxContent;
                    Modified();
                }
            }
        }

        private void EditDescriptionIdChange(object sender, EventArgs e)
        {
            if (_command != null && (EditDescriptionId.Value != 1) && (_command.LabelDescription.Id != EditDescriptionId.Value))
            {
                _command.LabelDescription.Id = (int)EditDescriptionId.Value;
                Modified();
            }
        }

        private void EditDescriptionSymbolChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.LabelDescription.Symbol != EditDescriptionSymbol.Text))
            {
                _command.LabelDescription.Symbol = EditDescriptionSymbol.Text;
                Modified();
            }
        }

        private void EditIdChange(object sender, EventArgs e)
        {
            if (_command != null && (EditId.Value != 1) && (_command.Id != EditId.Value))
            {
                _command.Id = (int)EditId.Value;
                Modified();
            }
        }

        private void EditKeytipChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.Keytip.Content != EditKeytip.Text))
            {
                _command.Keytip.Content = EditKeytip.Text;
                Modified();
            }
        }

        private void EditKeytipIdChange(object sender, EventArgs e)
        {
            if (_command != null && (EditKeytipId.Value != 1) && (_command.Keytip.Id != EditKeytipId.Value))
            {
                _command.Keytip.Id = (int)EditKeytipId.Value;
                Modified();
            }
        }

        private void EditKeyTipSymbolChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.Keytip.Symbol != EditKeytipSymbol.Text))
            {
                _command.Keytip.Symbol = EditKeytipSymbol.Text;
                Modified();
            }
        }

        private void EditNameChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.Name != EditName.Text))
            {
                _command.Name = EditName.Text;
                _frame.ListViewCommands.SelectedItems[0].Text = EditName.Text;
                Modified();
            }
        }

        private void EditNameKeyPress(object sender, KeyPressEventArgs e)
        {
            bool allowed = false;
            // Only allow valid Name/Symbol characters
            TextBox edit = sender as TextBox;
            switch (e.KeyChar)
            {
                case (char)3: // Ctrl-C
                case (char)0x16: // Ctrl-V
                case (char)0x18: // Ctrl-X
                case (char)8: // backspace
                case '_':
                    allowed = true;
                    break;
            }
            if (!allowed)
            {
                allowed = char.IsLetter(e.KeyChar);
            }
            if (allowed)
            {
                e.Handled = false;
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                if (edit.SelectionStart == 0)
                    e.KeyChar = (char)0;
            }
            else
                e.KeyChar = (char)0;
            if (e.KeyChar == (char)0)
                e.Handled = true;
        }

        private void EditSymbolChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.Symbol != EditSymbol.Text))
            {
                _command.Symbol = EditSymbol.Text;
                Modified();
            }
        }

        private void EditTooltipDescriptionChange(object sender, EventArgs e)
        {
            if (_command != null)
            {
                string textboxContent = TextboxToContent(EditTooltipDescription.Text);
                if (_command.TooltipDescription.Content != textboxContent)
                {
                    _command.TooltipDescription.Content = textboxContent;
                    Modified();
                }
            }
        }

        private void EditTooltipDescriptionIdChange(object sender, EventArgs e)
        {
            if (_command != null && (EditTooltipDescriptionId.Value != 1) && (_command.TooltipDescription.Id != EditTooltipDescriptionId.Value))
            {
                _command.TooltipDescription.Id = (int)EditTooltipDescriptionId.Value;
                Modified();
            }
        }

        private void EditTooltipDescriptionSymbolChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.TooltipDescription.Symbol != EditTooltipDescriptionSymbol.Text))
            {
                _command.TooltipDescription.Symbol = EditTooltipDescriptionSymbol.Text;
                Modified();
            }
        }

        private void EditTooltipTitleChange(object sender, EventArgs e)
        {
            if (_command != null)
            {
                string textboxContent = TextboxToContent(EditTooltipTitle.Text);
                if (_command.TooltipTitle.Content != textboxContent)
                {
                    _command.TooltipTitle.Content = textboxContent;
                    Modified();
                }
            }
        }

        private void EditTooltipTitleIdChange(object sender, EventArgs e)
        {
            if (_command != null && (EditTooltipTitleId.Value != 1) && (_command.TooltipTitle.Id != EditTooltipTitleId.Value))
            {
                _command.TooltipTitle.Id = (int)EditTooltipTitleId.Value;
                Modified();
            }
        }

        private void EditTooltipTitleSymbolChange(object sender, EventArgs e)
        {
            if (_command != null && (_command.TooltipTitle.Symbol != EditTooltipTitleSymbol.Text))
            {
                _command.TooltipTitle.Symbol = EditTooltipTitleSymbol.Text;
                Modified();
            }
        }

        private void Modified()
        {
            _frame.Modified();
        }

        private void UpDownChanging(object sender, EventArgs e)
        {
            NumericUpDown upDown = sender as NumericUpDown;
            // Skip value 1
            bool allowChange = upDown.Value != 1;
            if (!allowChange)
            {
                if (upDown.Text == "0" || upDown.Text == "1")
                    upDown.Value = 2;
                else
                    upDown.Value = 0;
            }
        }

        private static string ContentToTextbox(string content)
        {
            if (content == null)
                return null;
            return content.Replace(((char)0xA).ToString(), @"\n");
        }

        private static string TextboxToContent(string textboxText)
        {
            if (string.IsNullOrEmpty(textboxText))
                return null;
            return textboxText.Replace(@"\n", ((char)0xA).ToString());
        }
    }
}
