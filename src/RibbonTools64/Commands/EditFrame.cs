using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Actions;

namespace UIRibbonTools
{
    partial class EditFrame : UserControl
    {
        //const string DefaultCommandNameAtBeginning = "cmd_"; //"Command"; //@ added

        //private const string RS_REMOVE_COMMAND_HEADER = "Remove command?";
        //private static readonly string RS_REMOVE_COMMAND_MESSAGE = "There are {0:d} control(s) that reference this command." + Environment.NewLine +
        //  "If you remove this command, those controls may become unusable. " + Environment.NewLine +
        //  "Do you want to remove this command (this cannot be undone)?";

        //private TRibbonDocument _document;
        //private TRibbonCommand _command;
        //private bool _updating;
        public System.Windows.Forms.TextBox _EditName => EditName;
        public System.Windows.Forms.TextBox _EditCaption => EditCaption;
        public System.Windows.Forms.TextBox _EditDescription => EditDescription;
        public System.Windows.Forms.TextBox _EditTooltipTitle => EditTooltipTitle;
        public System.Windows.Forms.TextBox _EditTooltipDescription => EditTooltipDescription;
        public System.Windows.Forms.TextBox _EditKeytip => EditKeytip;
        public System.Windows.Forms.TextBox _EditSymbol => EditSymbol;
        public System.Windows.Forms.TextBox _EditComment => EditComment;
        public System.Windows.Forms.TextBox _EditCaptionSymbol => EditCaptionSymbol;
        public System.Windows.Forms.TextBox _EditDescriptionSymbol => EditDescriptionSymbol;
        public System.Windows.Forms.TextBox _EditTooltipTitleSymbol => EditTooltipTitleSymbol;
        public System.Windows.Forms.TextBox _EditTooltipDescriptionSymbol => EditTooltipDescriptionSymbol;
        public System.Windows.Forms.TextBox _EditKeytipSymbol => EditKeytipSymbol;
        public System.Windows.Forms.NumericUpDown _EditId => EditId;
        public System.Windows.Forms.NumericUpDown _EditCaptionId => EditCaptionId;
        public System.Windows.Forms.NumericUpDown _EditDescriptionId => EditDescriptionId;
        public System.Windows.Forms.NumericUpDown _EditTooltipTitleId => EditTooltipTitleId;
        public System.Windows.Forms.NumericUpDown _EditTooltipDescriptionId => EditTooltipDescriptionId;
        public System.Windows.Forms.NumericUpDown _EditKeytipId => EditKeytipId;

        public EditFrame()
        {
            InitializeComponent();
            if (components == null)
                components = new Container();

            bool runtime = (LicenseManager.UsageMode == LicenseUsageMode.Runtime);
            //if (runtime)
            //    InitAddon();
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

    //    public void SetBoldFonts()
    //    {
    //        labelProperty.Font = new Font(labelProperty.Font, FontStyle.Bold);
    //        labelValue.Font = new Font(labelValue.Font, FontStyle.Bold);
    //        labelID.Font = new Font(labelID.Font, FontStyle.Bold);
    //        labelSymbol.Font = new Font(labelSymbol.Font, FontStyle.Bold);
    //    }


    //    private void InitAddon()
    //    {
    //        InitEvents();
    //        InitToolTips();
    //    }

    //    private void InitEvents()
    //    {
    //        EditName.TextChanged += EditNameChange;
    //        EditName.KeyPress += EditNameKeyPress;
    //        EditId.TextChanged += EditIdChange;
    //        EditId.ValueChanged += UpDownChanging;
    //        EditSymbol.TextChanged += EditSymbolChange;
    //        EditSymbol.KeyPress += EditNameKeyPress;

    //        EditCaption.TextChanged += EditCaptionChange;
    //        EditCaptionId.TextChanged += EditCaptionIdChange;
    //        EditCaptionId.ValueChanged += UpDownChanging;
    //        EditCaptionSymbol.TextChanged += EditCaptionSymbolChange;

    //        EditDescription.TextChanged += EditDescriptionChange;
    //        EditDescriptionId.TextChanged += EditDescriptionIdChange;
    //        EditDescriptionId.ValueChanged += UpDownChanging;
    //        EditDescriptionSymbol.TextChanged += EditDescriptionSymbolChange;

    //        EditTooltipTitle.TextChanged += EditTooltipTitleChange;
    //        EditTooltipTitleId.TextChanged += EditTooltipTitleIdChange;
    //        EditTooltipTitleId.ValueChanged += UpDownChanging;
    //        EditTooltipTitleSymbol.TextChanged += EditTooltipTitleSymbolChange;

    //        EditTooltipDescription.TextChanged += EditTooltipDescriptionChange;
    //        EditTooltipDescriptionId.TextChanged += EditTooltipDescriptionIdChange;
    //        EditTooltipDescriptionId.ValueChanged += UpDownChanging;
    //        EditTooltipDescriptionSymbol.TextChanged += EditTooltipDescriptionSymbolChange;

    //        EditKeytip.TextChanged += EditKeytipChange;
    //        EditKeytipId.TextChanged += EditKeytipIdChange;
    //        EditKeytipId.ValueChanged += UpDownChanging;
    //        EditKeytipSymbol.TextChanged += EditKeyTipSymbolChange;

    //        EditComment.TextChanged += EditCommentChange;
    //    }

    //    private void InitToolTips()
    //    {
    //        ToolTip commandsTip = new ToolTip(components);
    //        commandsTip.SetToolTip(EditName,
    //            "The command name is used to connect commands with controls." + Environment.NewLine +
    //            "Unless you specify a Symbol name, this name is also used" + Environment.NewLine +
    //            "as the name of the constant for this command.");
    //        commandsTip.IsBalloon = false;
    //        //commandsTip.Popup += CommandsTip_Popup;
    //        commandsTip.SetToolTip(EditId,
    //            "A unique numeric identifier for the command (the value of" + Environment.NewLine +
    //            "the Symbol constant). Use 0 for auto-generated identifiers.");
    //        commandsTip.SetToolTip(EditSymbol,
    //            "This is the name of the constant that will be generated to access" + Environment.NewLine +
    //            "this command. If not specified, the command Name is used.");
    //        commandsTip.SetToolTip(EditCaption, "The caption/label title for the command.");
    //        commandsTip.SetToolTip(EditCaptionId,
    //            "Numeric resource string identifier for the caption." + Environment.NewLine +
    //            "Use 0 for auto-generated identifiers.");
    //        commandsTip.SetToolTip(EditCaptionSymbol,
    //            "Constant name for the resource identifier." + Environment.NewLine +
    //            "If not specified, it is automatically generated.");
    //        commandsTip.SetToolTip(EditDescription,
    //            "The label description for the command." + Environment.NewLine +
    //            "Is used when the command is displayed in the application menu.");
    //        commandsTip.SetToolTip(EditDescriptionId,
    //            "Numeric resource string identifier for the description." + Environment.NewLine +
    //            "Use 0 for auto-generated identifiers.");
    //        commandsTip.SetToolTip(EditDescriptionSymbol,
    //            "Constant name for the resource identifier." + Environment.NewLine +
    //            "If not specified, it is automatically generated.");
    //        commandsTip.SetToolTip(EditTooltipTitle,
    //            "The tooltip title for the command." + Environment.NewLine +
    //            "(This is the bold caption of the tooltip)");
    //        commandsTip.SetToolTip(EditTooltipTitleId,
    //            "Numeric resource string identifier for the tooltip title." + Environment.NewLine +
    //            "Use 0 for auto-generated identifiers.");
    //        commandsTip.SetToolTip(EditTooltipTitleSymbol,
    //            "Constant name for the resource identifier." + Environment.NewLine +
    //            "If not specified, it is automatically generated.");
    //        commandsTip.SetToolTip(EditTooltipDescription,
    //            "The tooltip description for the command." + Environment.NewLine +
    //            "(Is displayed below the tooltip title in the tooltip popup)");
    //        commandsTip.SetToolTip(EditTooltipDescriptionId,
    //            "Numeric resource string identifier for the tooltip description." + Environment.NewLine +
    //            "Use 0 for auto-generated identifiers.");
    //        commandsTip.SetToolTip(EditTooltipDescriptionSymbol,
    //            "Constant name for the resource identifier." + Environment.NewLine +
    //            "If not specified, it is automatically generated.");
    //        commandsTip.SetToolTip(EditKeytip,
    //            "The keytip for the command. This is key sequence that is shown" + Environment.NewLine +
    //            "when the user pressed the Alt key to access ribbon controls.");
    //        commandsTip.SetToolTip(EditKeytipId,
    //            "Numeric resource string identifier for the keytip." + Environment.NewLine +
    //            "Use 0 for auto-generated identifiers.");
    //        commandsTip.SetToolTip(EditKeytipSymbol,
    //            "Constant name for the resource identifier." + Environment.NewLine +
    //            "If not specified, it is automatically generated.");
    //        commandsTip.SetToolTip(EditComment,
    //            "This text is placed as a comment in the *.h file" + Environment.NewLine +
    //            "containing the constant for this command.");
    //    }

    //    public int FindSmallestUnusedID(int minId = 2)
    //    {
    //        Dictionary<int, TRibbonCommand> iDs;
    //        int i;
    //        const int MaxValidID = 59999;

    //        iDs = new Dictionary<int, TRibbonCommand>();

    //        try
    //        {
    //            // Gather all IDs that are already taken in a dictionary
    //            foreach (TRibbonCommand command in _document.Application.Commands)
    //                if (command.Id > 0)
    //                    iDs.Add(command.Id, command);

    //            // Iterate all allowed IDs, starting with the smallest. Return the first one that hasn't been used yet
    //            for (i = minId; i < MaxValidID; i++)
    //                if (!iDs.ContainsKey(i))
    //                    return (i);

    //            throw new ArgumentOutOfRangeException("No valid, unused ID could be found within the range between " + minId.ToString() + " && " + MaxValidID.ToString());
    //        }
    //        finally
    //        {
    //            //iDs.Free;
    //        }
    //    }

    //    private void EditCaptionChange(object sender, EventArgs e)
    //    {
    //        //if (_command != null && (_command.LabelTitle.Content != EditCaption.Text))
    //        //{
    //        //    _command.LabelTitle.Content = EditCaption.Text;
    //        //    ListViewCommands.SelectedItems[0].SubItems[1].Text = EditCaption.Text;
    //        //    Modified();
    //        //}
    //        if (_command != null)
    //        {
    //            string textboxContent = TextboxToContent(EditCaption.Text);
    //            if (_command.LabelTitle.Content != textboxContent)
    //            {
    //                _command.LabelTitle.Content = textboxContent;
    //                ListViewCommands.SelectedItems[0].SubItems[1].Text = EditCaption.Text;
    //                Modified();
    //            }
    //        }
    //    }

    //    private void EditCaptionIdChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (EditCaptionId.Value != 1) && (_command.LabelTitle.Id != EditCaptionId.Value))
    //        {
    //            _command.LabelTitle.Id = (int)EditCaptionId.Value;
    //            Modified();
    //        }
    //    }

    //    private void EditCaptionSymbolChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.LabelTitle.Symbol != EditCaptionSymbol.Text))
    //        {
    //            _command.LabelTitle.Symbol = EditCaptionSymbol.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditCommentChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.Comment != EditComment.Text))
    //        {
    //            _command.Comment = EditComment.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditDescriptionChange(object sender, EventArgs e)
    //    {
    //        if (_command != null)
    //        {
    //            string textboxContent = TextboxToContent(EditDescription.Text);
    //            if (_command.LabelDescription.Content != textboxContent)
    //            {
    //                _command.LabelDescription.Content = textboxContent;
    //                Modified();
    //            }
    //        }
    //    }

    //    private void EditDescriptionIdChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (EditDescriptionId.Value != 1) && (_command.LabelDescription.Id != EditDescriptionId.Value))
    //        {
    //            _command.LabelDescription.Id = (int)EditDescriptionId.Value;
    //            Modified();
    //        }
    //    }

    //    private void EditDescriptionSymbolChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.LabelDescription.Symbol != EditDescriptionSymbol.Text))
    //        {
    //            _command.LabelDescription.Symbol = EditDescriptionSymbol.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditIdChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (EditId.Value != 1) && (_command.Id != EditId.Value))
    //        {
    //            _command.Id = (int)EditId.Value;
    //            Modified();
    //        }
    //    }

    //    private void EditKeytipChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.Keytip.Content != EditKeytip.Text))
    //        {
    //            _command.Keytip.Content = EditKeytip.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditKeytipIdChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (EditKeytipId.Value != 1) && (_command.Keytip.Id != EditKeytipId.Value))
    //        {
    //            _command.Keytip.Id = (int)EditKeytipId.Value;
    //            Modified();
    //        }
    //    }

    //    private void EditKeyTipSymbolChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.Keytip.Symbol != EditKeytipSymbol.Text))
    //        {
    //            _command.Keytip.Symbol = EditKeytipSymbol.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditNameChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.Name != EditName.Text))
    //        {
    //            _command.Name = EditName.Text;
    //            ListViewCommands.SelectedItems[0].Text = EditName.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditNameKeyPress(object sender, KeyPressEventArgs e)
    //    {
    //        bool allowed = false;
    //        // Only allow valid Name/Symbol characters
    //        TextBox edit = sender as TextBox;
    //        switch (e.KeyChar)
    //        {
    //            case (char)3: // Ctrl-C
    //            case (char)0x16: // Ctrl-V
    //            case (char)0x18: // Ctrl-X
    //            case (char)8: // backspace
    //            case '_':
    //                allowed = true;
    //                break;
    //        }
    //        if (!allowed)
    //        {
    //            allowed = char.IsLetter(e.KeyChar);
    //        }
    //        if (allowed)
    //        {
    //            e.Handled = false;
    //            return;
    //        }
    //        if (char.IsDigit(e.KeyChar))
    //        {
    //            if (edit.SelectionStart == 0)
    //                e.KeyChar = (char)0;
    //        }
    //        else
    //            e.KeyChar = (char)0;
    //        if (e.KeyChar == (char)0)
    //            e.Handled = true;
    //    }

    //    private void EditSymbolChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.Symbol != EditSymbol.Text))
    //        {
    //            _command.Symbol = EditSymbol.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditTooltipDescriptionChange(object sender, EventArgs e)
    //    {
    //        if (_command != null)
    //        {
    //            string textboxContent = TextboxToContent(EditTooltipDescription.Text);
    //            if (_command.TooltipDescription.Content != textboxContent)
    //            {
    //                _command.TooltipDescription.Content = textboxContent;
    //                Modified();
    //            }
    //        }
    //    }

    //    private void EditTooltipDescriptionIdChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (EditTooltipDescriptionId.Value != 1) && (_command.TooltipDescription.Id != EditTooltipDescriptionId.Value))
    //        {
    //            _command.TooltipDescription.Id = (int)EditTooltipDescriptionId.Value;
    //            Modified();
    //        }
    //    }

    //    private void EditTooltipDescriptionSymbolChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.TooltipDescription.Symbol != EditTooltipDescriptionSymbol.Text))
    //        {
    //            _command.TooltipDescription.Symbol = EditTooltipDescriptionSymbol.Text;
    //            Modified();
    //        }
    //    }

    //    private void EditTooltipTitleChange(object sender, EventArgs e)
    //    {
    //        if (_command != null)
    //        {
    //            string textboxContent = TextboxToContent(EditTooltipTitle.Text);
    //            if (_command.TooltipTitle.Content != textboxContent)
    //            {
    //                _command.TooltipTitle.Content = textboxContent;
    //                Modified();
    //            }
    //        }
    //    }

    //    private void EditTooltipTitleIdChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (EditTooltipTitleId.Value != 1) && (_command.TooltipTitle.Id != EditTooltipTitleId.Value))
    //        {
    //            _command.TooltipTitle.Id = (int)EditTooltipTitleId.Value;
    //            Modified();
    //        }
    //    }

    //    private void EditTooltipTitleSymbolChange(object sender, EventArgs e)
    //    {
    //        if (_command != null && (_command.TooltipTitle.Symbol != EditTooltipTitleSymbol.Text))
    //        {
    //            _command.TooltipTitle.Symbol = EditTooltipTitleSymbol.Text;
    //            Modified();
    //        }
    //    }

    //    private void EnableControls(bool enable)
    //    {
    //        for (int i = 0; i < _panel2Layout.Controls.Count; i++)
    //            _panel2Layout.Controls[i].Enabled = enable;

    //        toolButtonRemoveCommand.Enabled = enable;
    //    }


    //    private void Modified()
    //    {
    //        if (!_updating)
    //            ((MainForm)FindForm()).Modified();
    //    }


    //    private void UpDownChanging(object sender, EventArgs e)
    //    {
    //        NumericUpDown upDown = sender as NumericUpDown;
    //        // Skip value 1
    //        bool allowChange = upDown.Value != 1;
    //        if (!allowChange)
    //        {
    //            if (upDown.Text == "0" || upDown.Text == "1")
    //                upDown.Value = 2;
    //            else
    //                upDown.Value = 0;
    //        }
    //    }

    //    private static string ContentToTextbox(string content)
    //    {
    //        if (content == null)
    //            return null;
    //        return content.Replace(((char)0xA).ToString(), @"\n");
    //    }

    //    private static string TextboxToContent(string textboxText)
    //    {
    //        if (string.IsNullOrEmpty(textboxText))
    //            return null;
    //        return textboxText.Replace(@"\n", ((char)0xA).ToString());
    //    }
    }
}
