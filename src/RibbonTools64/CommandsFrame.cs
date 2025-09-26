#define editFrame
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
    partial class CommandsFrame : UserControl
    {
        const string DefaultCommandNameAtBeginning = "cmd_"; //"Command"; //@ added

        private const string RS_REMOVE_COMMAND_HEADER = "Remove command?";
        private static readonly string RS_REMOVE_COMMAND_MESSAGE = "There are {0:d} control(s) that reference this command." + Environment.NewLine +
          "If you remove this command, those controls may become unusable. " + Environment.NewLine +
          "Do you want to remove this command (this cannot be undone)?";

        private TRibbonDocument _document;
        private TRibbonCommand _command;
        private bool _updating;
        //private ImageListFrame _smallImagesFrame;
        //private ImageListFrame _largeImagesFrame;
        //private ImageListFrame _smallHCImagesFrame;
        //private ImageListFrame _largeHCImagesFrame;
        private int _newCommandIndex;
        private MenuActionList _actionList;
        private MenuAction _actionAddCommand;
        private MenuAction _actionRemoveCommand;
        private MenuAction _actionMoveUp;
        private MenuAction _actionMoveDown;
        private MenuAction _actionSearchCommand;
        private ImageList _imageListToolbars;
        private ListViewColumnSorter _listViewColumnSorter;
        private Timer _listViewTimer; //@ added

        public CommandsFrame()
        {
            InitializeComponent();
            if (components == null)
                components = new Container();

            _listViewColumnSorter = new ListViewColumnSorter();
            _listViewTimer = new Timer(); //@ added, because we don't want disabled the right site after each selection change
            _listViewTimer.Interval = 100;

            bool runtime = (LicenseManager.UsageMode == LicenseUsageMode.Runtime);
            if (runtime)
                InitAddon();
        }

        //@ Why we have to do it ? This Width are set by Designercode already
        public void InitSplitter()
        {
            int toolWidth = 12 + toolButtonAddCommand.Width + toolButtonRemoveCommand.Width + toolButtonMoveUp.Width + toolButtonMoveDown.Width + toolButtonSearchCommand.Width;
            SplitterCommands.SplitterDistance = toolWidth;
            SplitterCommands.Panel1MinSize = toolWidth;
        }

        public void SetBoldFonts()
        {
            _editFrame.SetBoldFonts();
            labelSmallImages.Font = new Font(labelSmallImages.Font, FontStyle.Bold);
            labelLargeImages.Font = new Font(labelLargeImages.Font, FontStyle.Bold);
            labelSmallHCImages.Font = new Font(labelSmallHCImages.Font, FontStyle.Bold);
            labelLargeHCImages.Font = new Font(labelLargeHCImages.Font, FontStyle.Bold);
            LabelHeader.Font = new Font(LabelHeader.Font, FontStyle.Bold);
        }

        public void SetFonts(Font font)
        {
            this.Font = font;
            _smallImagesFrame.Font = font;
            _largeImagesFrame.Font = font;
            _smallHCImagesFrame.Font = font;
            _largeHCImagesFrame.Font = font;
        }

        private void InitMenuActions()
        {
            _actionList = new MenuActionList(components);

            _actionAddCommand = new MenuAction(components);
            _actionRemoveCommand = new MenuAction(components);
            _actionMoveUp = new MenuAction(components);
            _actionMoveDown = new MenuAction(components);
            _actionSearchCommand = new MenuAction(components);

            _actionList.Actions.AddRange(new MenuAction[] {
                _actionAddCommand,
                _actionRemoveCommand,
                _actionMoveUp,
                _actionMoveDown,
                _actionSearchCommand
            });

            _actionAddCommand.Execute += ActionAddCommandExecute;
            _actionAddCommand.Update += ActionAddCommandUpdate;
            _actionAddCommand.Hint = "Adds a new command";
            _actionAddCommand.ImageIndex = 0;
            _actionAddCommand.ShortcutKeys = Keys.Shift | Keys.Control | Keys.Insert;
            _actionAddCommand.Text = "Add";
            _actionList.SetAction(toolButtonAddCommand, _actionAddCommand);
            _actionList.SetAction(menuAddCommand, _actionAddCommand);
            //_actionAddCommand.ShowTextOnToolBar = false;

            _actionRemoveCommand.Execute += ActionRemoveCommandExecute;
            _actionRemoveCommand.Update += ActionUpdate;
            _actionRemoveCommand.Hint = "Removes the selected command";
            _actionRemoveCommand.ImageIndex = 1;
            _actionRemoveCommand.ShortcutKeys = Keys.Control | Keys.Delete;
            _actionRemoveCommand.Text = "Remove";
            _actionList.SetAction(toolButtonRemoveCommand, _actionRemoveCommand);
            _actionList.SetAction(menuRemoveCommand, _actionRemoveCommand);

            _actionMoveUp.Execute += ActionMoveUpExecute;
            _actionMoveUp.Update += ActionUpdateUp;
            _actionMoveUp.Hint = "Moves the selected command up in the list";
            _actionMoveUp.ImageIndex = 2;
            _actionMoveUp.ShortcutKeys = Keys.Control | Keys.Up;
            _actionMoveUp.Text = "Up";
            _actionList.SetAction(toolButtonMoveUp, _actionMoveUp);
            _actionList.SetAction(menuMoveUp, _actionMoveUp);

            _actionMoveDown.Execute += ActionMoveDownExecute;
            _actionMoveDown.Update += ActionUpdateDown;
            _actionMoveDown.Hint = "Moves the selected command down in the list";
            _actionMoveDown.ImageIndex = 3;
            _actionMoveDown.ShortcutKeys = Keys.Control | Keys.Down;
            _actionMoveDown.Text = "Down";
            _actionList.SetAction(toolButtonMoveDown, _actionMoveDown);
            _actionList.SetAction(menuMoveDown, _actionMoveDown);

            _actionSearchCommand.Execute += ActionSearchCommandExecute;
            _actionSearchCommand.Update += ActionUpdate;
            _actionSearchCommand.Hint = string.Empty;
            _actionSearchCommand.ImageIndex = 4;
            _actionSearchCommand.ShortcutKeys = Keys.Control | Keys.F;
            _actionSearchCommand.Text = "Search";
            _actionList.SetAction(toolButtonSearchCommand, _actionSearchCommand);

            _actionList.ImageList = _imageListToolbars;
        }

        private void InitAddon()
        {
            _imageListToolbars = ImageManager.ImageListToolbars_Commands(components);
            toolBarCommands.ImageList = _imageListToolbars;
            popupMenuList.ImageList = _imageListToolbars;

            InitMenuActions();
            InitEvents();
            _editFrame.InitAddon();
        }

        private void InitEvents()
        {
            ListViewCommands.ColumnClick += ListViewCommandsColumnClick;
            ListViewCommands.ItemSelectionChanged += ListViewCommandsSelectItem;
            _listViewTimer.Tick += ListViewTimer_Tick;
        }


        private void CommandsTip_Popup(object sender, PopupEventArgs e)
        {
            ToolTip tip = sender as ToolTip;
            string text;
            if (tip != null)
            {
                text = tip.GetToolTip(e.AssociatedControl);
            }
        }

        private void ActionAddCommandExecute(object sender, EventArgs e)
        {
            TRibbonCommand command;

            _newCommandIndex++;
            command = _document.Application.AddCommand(DefaultCommandNameAtBeginning + (_newCommandIndex.ToString())); //@ changed
            ListViewItem item = AddCommand(command);
            ListViewCommands.Items[item.Index].Selected = true;
            ListViewCommands.Items[item.Index].Focused = true;
            ListViewCommands.Items[item.Index].EnsureVisible();
            _editFrame.EditNameSelect();
            BtnGenerateIDClick(sender, EventArgs.Empty);
            Modified();
        }

        private void ActionUpdateUp(object sender, EventArgs e) //@ added, bugfix
        {
            MenuAction action = sender as MenuAction;
            if (action != null)
            {
                action.Enabled = (ListViewCommands.SelectedItems.Count > 0 && (ListViewCommands.SelectedItems[0]) != null
                    && ListViewCommands.SelectedItems[0].Index != 0);
            }
        }

        private void ActionUpdateDown(object sender, EventArgs e) //@ added, bugfix
        {
            MenuAction action = sender as MenuAction;
            if (action != null)
            {
                action.Enabled = (ListViewCommands.SelectedItems.Count > 0 && (ListViewCommands.SelectedItems[0]) != null
                    && ListViewCommands.SelectedItems[0].Index != ListViewCommands.Items.Count - 1);
            }
        }

        private void ActionUpdate(object sender, EventArgs e)
        {
            (sender as MenuAction).Enabled = (ListViewCommands.SelectedItems.Count > 0 && (ListViewCommands.SelectedItems[0]) != null);
        }

        private void ActionAddCommandUpdate(object sender, EventArgs e)
        {
            (sender as MenuAction).Enabled = (_document != null);
        }

        private void ActionRemoveCommandExecute(object sender, EventArgs e)
        {
            if (_command != null && ((_command.ReferenceCount == 0) ||
#if !MessageBox
              (TaskDialog.ShowDialog(this, new TaskDialogPage()
              {
                  Text = string.Format(RS_REMOVE_COMMAND_MESSAGE, _command.ReferenceCount),
                  Heading = RS_REMOVE_COMMAND_HEADER,
                  Caption = "Confirm",
                  Buttons =
                  {
                      TaskDialogButton.Yes,
                      TaskDialogButton.No
                  },
                  Icon = TaskDialogIcon.None,
                  DefaultButton = TaskDialogButton.Yes
              }) == TaskDialogButton.Yes)))
#else
              (MessageBox.Show(string.Format(RS_REMOVE_COMMAND_MESSAGE, _command.ReferenceCount), RS_REMOVE_COMMAND_HEADER,
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)))
#endif
            {
                if (ListViewCommands.SelectedItems.Count > 0)
                {
                    _document.Application.RemoveCommand(_command);
                    ListViewCommands.SelectedItems[0].Remove();
                    ShowSelection();
                    Modified();
                }
            }
        }

        private void ActionMoveDownExecute(object sender, EventArgs e)
        {
            MoveCommand(1);
        }

        private void ActionMoveUpExecute(object sender, EventArgs e)
        {
            MoveCommand(-1);
        }

        private void ActionSearchCommandExecute(object sender, EventArgs e)
        {
            CommandSearchForm commandSearchForm;

            commandSearchForm = new CommandSearchForm(this, ListViewCommands);
            try
            {
                if (commandSearchForm.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    if (commandSearchForm.ListViewCommands.SelectedItems.Count == 0 || commandSearchForm.ListViewCommands.SelectedItems[0] == null)
                        return;
                    foreach (ListViewItem item in this.ListViewCommands.Items)
                    {
                        if (item.Tag == commandSearchForm.ListViewCommands.SelectedItems[0].Tag)
                            this.ListViewCommands.Items[item.Index].Selected = true;
                        this.ListViewCommands.SelectedItems[0].EnsureVisible();
                    }
                }
            }
            finally
            {
                commandSearchForm.Close();
            }
        }

        public void ActivateFrame()
        {
            Program.ApplicationForm.ShortCutKeysHandler.Add(_actionList);
            //@ changed
            //_actionRemoveCommand.Shortcut = Shortcut.CtrlDel;
            //_actionMoveUp.Shortcut = Shortcut.AltUpArrow;
            //_actionMoveDown.Shortcut = Shortcut.AltDownArrow;
        }

        private ListViewItem AddCommand(TRibbonCommand command)
        {
            ListViewItem result = ListViewCommands.Items.Add(new ListViewItem(command.Name));
            //result.Text = command.Name;
            result.SubItems.Add(command.LabelTitle.Content);
            result.Tag = command;
            return result;
        }

        public void ClearDocument()
        {
            ListViewCommands.Items.Clear();
        }

        public int FindSmallestUnusedID(int minId = 2)
        {
            Dictionary<int, TRibbonCommand> iDs;
            int i;
            const int MaxValidID = 59999;

            iDs = new Dictionary<int, TRibbonCommand>();

            try
            {
                // Gather all IDs that are already taken in a dictionary
                foreach (TRibbonCommand command in _document.Application.Commands)
                    if (command.Id > 0)
                        iDs.Add(command.Id, command);

                // Iterate all allowed IDs, starting with the smallest. Return the first one that hasn't been used yet
                for (i = minId; i < MaxValidID; i++)
                    if (!iDs.ContainsKey(i))
                        return (i);

                throw new ArgumentOutOfRangeException("No valid, unused ID could be found within the range between " + minId.ToString() + " && " + MaxValidID.ToString());
            }
            finally
            {
                //iDs.Free;
            }
        }

        private void BtnGenerateIDClick(object sender, EventArgs e)
        {
            int highID;
            int minID;

            highID = 1;
            foreach (TRibbonCommand command in _document.Application.Commands)
                if (command.Id > highID)
                    highID = command.Id;


            minID = 0;

            if (ListViewCommands.SelectedItems.Count > 0 && ListViewCommands.SelectedItems[0] != null)
                minID = ListViewCommands.SelectedItems[0].Index;

            minID = minID + 2;

            // By using at least the index of the item, we mimic the behavior of the ribbon compiler's ID auto generation as closely as possible.
            string idText = FindSmallestUnusedID(minID).ToString();
            _editFrame.EditIdText(idText);
        }

        public void DeactivateFrame()
        {
            Program.ApplicationForm.ShortCutKeysHandler.Remove(_actionList);
            //@ changed
            //_actionRemoveCommand.Shortcut = 0;
            //_actionMoveUp.Shortcut = 0;
            //_actionMoveDown.Shortcut = 0;
        }


        public void EnableControls(bool enable)
        {
            for (int i = 0; i < _panel2Layout.Controls.Count; i++)
                _panel2Layout.Controls[i].Enabled = enable;

            toolButtonRemoveCommand.Enabled = enable;
        }

        private void ListViewCommandsColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _listViewColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_listViewColumnSorter.Order == SortOrder.Ascending)
                {
                    _listViewColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _listViewColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _listViewColumnSorter.SortColumn = e.Column;
                _listViewColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.ListViewCommands.Sort();


            List<TRibbonCommand> commands;

            commands = new List<TRibbonCommand>();
            try
            {
                foreach (ListViewItem item in ((ListView)sender).Items)
                    commands.Add((TRibbonCommand)item.Tag);

                _document.Application.Commands.Assign(commands);
            }
            finally
            {
                //commands.Free;
            }
        }

        //ListViewCommandsCompare: Implementation in class ListViewColumnSorter

        private void ListViewTimer_Tick(object sender, EventArgs e) //@ added
        {
            ShowSelection();
            _listViewTimer.Stop();
        }

        private void ListViewCommandsSelectItem(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) //@ some code added
            {
                _listViewTimer.Start();
                return;
            }
            _listViewTimer.Stop();
            ShowSelection();
        }

        internal void Modified()
        {
            if (!_updating)
                Program.ApplicationForm.Modified();
        }

        private void MoveCommand(int direction)
        {
            ListViewItem item, newItem;
            TRibbonCommand command;
            ListViewCommands.Sorting = SortOrder.None;

            item = ListViewCommands.SelectedItems[0];
            if ((item == null) || (item.Tag == null))
                return;
            command = (TRibbonCommand)item.Tag;

            if (_document.Application.Reorder(command, direction))
            {
                int itemIndex = item.Index;
                ListViewCommands.SelectedItems[0].Remove();
                if (direction < 0)
                    newItem = ListViewCommands.Items.Insert(itemIndex - 1, item);
                else
                    newItem = ListViewCommands.Items.Insert(itemIndex + 1, item);
                newItem = (item);
                //item.Free;
                ListViewCommands.Items[newItem.Index].Selected = true;
                newItem.Focused = true;
                newItem.EnsureVisible();
                Modified();
            }
        }

        private void PanelCommandPropertiesResize(object sender, EventArgs e)
        {
            //@ different implementation, function not needed
            //PanelImages.Height = (PanelCommandProperties.Height - PanelProps.Height - LabelHeader.Height) / 2;
        }

        private void PanelHighContrastImagesResize(object sender, EventArgs e)
        {
            //@ different implementation, function not needed
            //PanelSmallHCImages.Width = PanelHighContrastImages.Width / 2;
        }

        private void PanelImagesResize(object sender, EventArgs e)
        {
            //@ different implementation, function not needed
            //PanelSmallImages.Width = PanelImages.Width / 2;
        }

        public void ShowDocument(TRibbonDocument document)
        {
            int index;
            _document = document;
            _newCommandIndex = 0;
            ListViewCommands.ListViewItemSorter = null;
            ListViewCommands.Sorting = SortOrder.None; //@ added
            _listViewColumnSorter.Order = SortOrder.None;
            ListViewCommands.Items.Clear();
            ListViewCommands.BeginUpdate();
            try
            {
                foreach (TRibbonCommand command in _document.Application.Commands)
                {
                    AddCommand(command);
                    int commandNameMinLength = DefaultCommandNameAtBeginning.Length;
                    if (command.Name.Length >= commandNameMinLength && command.Name.Substring(0, commandNameMinLength).Equals(DefaultCommandNameAtBeginning))
                    //@ changed
                    //if (SameText(string.Copy(Command.Name, 1, 7), "Command"))
                    {
                        if (!int.TryParse(command.Name.Substring(commandNameMinLength), out index))
                            index = -1;
                        if (index > _newCommandIndex)
                            _newCommandIndex = index;
                    }
                }
                if (ListViewCommands.Items.Count > 0)
                {
                    ListViewCommands.Items[0].Selected = true;
                }
                else
                    ShowSelection();
            }
            finally
            {
                ListViewCommands.EndUpdate();
                ListViewCommands.ListViewItemSorter = _listViewColumnSorter;
            }
        }

        public void RefreshSelection()
        {
            ShowSelection();
        }

        private void ShowSelection()
        {
            ListViewItem item = null;
            if (ListViewCommands.SelectedItems.Count > 0)
                item = ListViewCommands.SelectedItems[0];
            if ((item != null))
            {
                _command = (TRibbonCommand)item.Tag;
                toolButtonMoveUp.Enabled = (item.Index > 0);
                toolButtonMoveDown.Enabled = (item.Index < (ListViewCommands.Items.Count - 1));
            }
            else
            {
                _command = null;
                toolButtonMoveUp.Enabled = false;
                toolButtonMoveDown.Enabled = false;
            }

            _updating = true;
            try
            {
                _editFrame.ShowSelection(this, _command);
                _smallImagesFrame.ShowImages(_command, ImageFlags.None);
                _largeImagesFrame.ShowImages(_command, ImageFlags.Large);
                _smallHCImagesFrame.ShowImages(_command, ImageFlags.HighContrast);
                _largeHCImagesFrame.ShowImages(_command, ImageFlags.Large | ImageFlags.HighContrast);
            }
            finally
            {
                _updating = false;
            }
        }
    }
}
