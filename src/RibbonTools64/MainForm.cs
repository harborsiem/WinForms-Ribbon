using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Reflection;
using WinForms.Actions;
using Windows.Win32;
//using WinForms.Ribbon;
using RibbonLib;

namespace UIRibbonTools
{
    partial class MainForm : Form
    {
        private const string RS_CANNOT_LOAD_DLL = "Unable to load Ribbon Resource DLL";
        private const string RS_MODIFIED = "Modified";
        private const string RS_RIBBON_TOOLS = "Ribbon Tools";
        private const string RS_UNTITLED = "(untitled document)";
        private const string RS_CHANGED_HEADER = "Document has changed";
        private const string RS_CHANGED_MESSAGE = "The document has changed. Do you want to save the changes?";
        private const string RS_DIFFERENT_DIR_HEADER = "Directory changed";
        private static readonly string RS_DIFFERENT_DIR_MESSAGE = "You are about to save to document to a different directory." + Environment.NewLine +
            "Any images associated with this document will NOT be copied to this new directory." + Environment.NewLine +
            "If you want to keep these images, you will need to copy them to the new directory yourself." + Environment.NewLine +
            "Do you want to continue to save this document?";

        private MenuActionList _actionList;
        private MenuAction _actionPreview;
        private MenuAction _actionOpen;
        private MenuAction _actionNew;
        private MenuAction _actionGenerateCommandIDs;
        internal MenuAction _actionSaveAs;
        private MenuAction _actionSave;
        private MenuAction _actionSettings;
        private MenuAction _actionExit;
        //private MenuAction _actionNewBlank;
        private MenuAction _actionBuild;
        private MenuAction _actionTutorial;
        private MenuAction _actionWebSite;
        private MenuAction _actionDotnetWebSite;
        private MenuAction _actionMSDN;
        private MenuAction _actionSetResourceName;
        private MenuAction _actionGenerateResourceIDs;
        private MenuAction _actionConvertImage;
        private MenuAction _actionLocalizeMarkupFile;
        private MenuAction _actionDeLocalizeMarkupFile;

        private bool _initialized;
        private TRibbonDocument _document;
        //private CommandsFrame _commandsFrame;
        //private ViewsFrame  _viewsFrame;
        //private XmlSourceFrame _xmlSourceFrame;
        private bool _modified;
        //private PreviewForm  _previewForm;
        private BuildPreviewHelper _buildPreviewHelper;
        private ImageList _imageListMain;
        public ShortCutKeysHandler ShortCutKeysHandler { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            _commandsFrame.SetBoldFonts();
            _viewsFrame.SetBoldFonts();

            //@ maybe this is a bug in Core because we have to set the Font to the UserControls
            //_commandsFrame.SetFonts(this.Font);
            //_viewsFrame.SetFonts(this.Font);
            //_xmlSourceFrame.SetFonts(this.Font);

            if (components == null)
                components = new Container();
            this.Text = RS_RIBBON_TOOLS;
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            CreateMainBitmaps();

            Settings.Instance.Read(this.MinimumSize);
            this.Size = Settings.Instance.ApplicationSize;

            _buildPreviewHelper = BuildPreviewHelper.Instance;
            _buildPreviewHelper.SetActions(null, SetPreviewEnabled, Log, SetLanguages);
            toolPreviewLanguageCombo.Enabled = false;
            toolPreviewLanguageCombo.SelectedIndex = 0;
            InitEvents();
            InitMenuActions();
            toolVersion.Text = "Version: " + Application.ProductVersion;


            //    constructor Create()
            //{

            _document = new TRibbonDocument();

            //_compiler = new RibbonCompiler();
            //_compiler.OnMessage = RibbonCompilerMessage;

            //// Handle command line options
            //if ((ParamCount > 0) && File.Exists(ParamStr(1)))  // File passed at the command line?
            //    OpenFile(ParamStr(1));
            //if (FindCmdLineSwitch("BUILD"))
            //{
            //    ActionBuild.Execute();
            //    Application.ShowMainForm = false;
            //    Application.Terminate();
            //}// if /BUILD
            //else
            //{
            //    NewFile(true);
            //}//else

            //UpdateControls(); //@ added why?
            _commandsFrame.RefreshSelection(); //@ added

            ShortCutKeysHandler = new ShortCutKeysHandler(base.ProcessCmdKey);
            //toolVersion.Click += ToolVersion_Click; //Debug numericupdown
        }

        private void ToolVersion_Click(object sender, EventArgs e)
        {
            Control ctrl = ((SplitContainer)_commandsFrame.Controls["SplitterCommands"]).Panel2.Controls["_panel2Layout"].Controls["_propertiesPanel"].Controls["EditId"];
            NumericUpDown upDown = (NumericUpDown)ctrl;
            MessageBox.Show(upDown.Bounds.ToString() + " ; DR: " + upDown.DisplayRectangle.ToString() + " ; Margins: " + upDown.Margin.ToString() + " ; Font: " + upDown.Font.ToString());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _commandsFrame.InitSplitter();
            //_commandsFrame.InitMargins();
        }

        private void InitMenuActions()
        {
            _actionList = new MenuActionList(components);

            _actionPreview = new MenuAction(components);
            _actionOpen = new MenuAction(components);
            _actionNew = new MenuAction(components);
            _actionConvertImage = new MenuAction(components);
            _actionLocalizeMarkupFile = new MenuAction(components);
            _actionDeLocalizeMarkupFile = new MenuAction(components);
            _actionGenerateCommandIDs = new MenuAction(components);
            _actionSaveAs = new MenuAction(components);
            _actionSave = new MenuAction(components);
            _actionSettings = new MenuAction(components);
            _actionExit = new MenuAction(components);
            //_actionNewBlank = new MenuAction(components);
            _actionBuild = new MenuAction(components);
            _actionTutorial = new MenuAction(components);
            _actionWebSite = new MenuAction(components);
            _actionDotnetWebSite = new MenuAction(components);
            _actionMSDN = new MenuAction(components);
            _actionSetResourceName = new MenuAction(components);
            _actionGenerateResourceIDs = new MenuAction(components);

            //_actionList.SetAction(MenuAddCommand, ActionAddCommand);
            _actionList.Actions.AddRange(new MenuAction[] {
                _actionPreview,
                _actionOpen,
                _actionNew,
                _actionConvertImage,
                _actionLocalizeMarkupFile,
                _actionDeLocalizeMarkupFile,
                _actionGenerateCommandIDs,
                _actionSaveAs,
                _actionSave,
                _actionSettings,
                _actionExit,
                //_actionNewBlank,
                _actionBuild,
                _actionTutorial,
                _actionWebSite,
                _actionDotnetWebSite,
                _actionMSDN,
                _actionSetResourceName,
                _actionGenerateResourceIDs
            });

            _actionPreview.Execute += ActionPreviewExecute;
            _actionPreview.Enabled = false; //@ added
            _actionPreview.Hint = "Preview the ribbon (F9)"; //@ "Build & Preview the ribbon (F9)"
            _actionPreview.ImageIndex = 3;
            _actionPreview.Text = "Preview";
            _actionPreview.ShortcutKeys = Keys.F9;
            _actionList.SetAction(toolButtonPreview, _actionPreview);
            _actionList.SetAction(menuPreview, _actionPreview);

            _actionOpen.Execute += ActionOpenExecute;
            _actionOpen.Hint = "Open an existing Ribbon document (Ctrl+O)";
            _actionOpen.ImageIndex = 1;
            _actionOpen.Text = "Open";
            _actionOpen.ShortcutKeys = (Keys)(Keys.Control | Keys.O);
            _actionList.SetAction(toolButtonOpen, _actionOpen);
            _actionList.SetAction(menuOpen, _actionOpen);
            //_actionAddCommand.ShowTextOnToolBar = false;

            _actionNew.Execute += ActionNewExecute;
            _actionNew.Hint = "Create a new Ribbon document (Ctrl+N)";
            _actionNew.ImageIndex = 0;
            _actionNew.Text = "New";
            _actionNew.ShortcutKeys = (Keys)(Keys.Control | Keys.N);
            _actionList.SetAction(menuNew, _actionNew);

            //_actionConvertImage.Visible = false;
            //_nN9.Visible = false;
            _actionConvertImage.Execute += ActionImageExecute;
            _actionConvertImage.Hint = "Convert to Bitmaps with Alpha channel";
            //_actionConvertImage.ImageIndex = 0;
            _actionConvertImage.Text = "Convert Images";
            //_actionConvertImage.ShortcutKeys = (Keys)(Keys.Control | Keys.N);
            _actionList.SetAction(menuImage, _actionConvertImage);

            _actionLocalizeMarkupFile.Execute += ActionLocalizeMarkupExecute;
            _actionLocalizeMarkupFile.Hint = "Convert to localized markup file";
            _actionLocalizeMarkupFile.Text = "To localized markup file";
            _actionList.SetAction(menuLocalizeMarkup, _actionLocalizeMarkupFile);

            _actionDeLocalizeMarkupFile.Execute += ActionDeLocalizeMarkupExecute;
            _actionDeLocalizeMarkupFile.Hint = "Convert from localized markup file";
            _actionDeLocalizeMarkupFile.Text = "From localized markup file";
            _actionList.SetAction(menuDeLocalizeMarkup, _actionDeLocalizeMarkupFile);

            _actionGenerateCommandIDs.Execute += ActionGenerateCommandIDsExecute;
            _actionGenerateCommandIDs.Hint = "Generates and sets IDs for all commands in this markup.";
            _actionGenerateCommandIDs.Text = "Auto generate IDs for all commands";
            _actionList.SetAction(autoGenerateIdsForAllCommands, _actionGenerateCommandIDs);

            _actionSaveAs.Execute += ActionSaveAsExecute;
            _actionSaveAs.Enabled = false; //@ added
            _actionSaveAs.Hint = "Saves the Ribbon document under a new name (Shift+Ctrl+S)";
            _actionSaveAs.Text = "Save As";
            _actionSaveAs.ShortcutKeys = (Keys)(Keys.Shift | Keys.Control | Keys.S);
            _actionList.SetAction(menuSaveAs, _actionSaveAs);

            _actionSave.Execute += ActionSaveExecute;
            _actionSave.Enabled = false; //@ added
            _actionSave.Hint = "Saves the Ribbon document (Ctrl+S)";
            _actionSave.ImageIndex = 2;
            _actionSave.Text = "Save";
            _actionSave.ShortcutKeys = (Keys)(Keys.Control | Keys.S);
            _actionList.SetAction(toolButtonSave, _actionSave);
            _actionList.SetAction(menuSave, _actionSave);

            _actionSettings.Execute += ActionSettingsExecute;
            _actionSettings.ImageIndex = 4;
            _actionSettings.Text = "Settings";
            _actionList.SetAction(menuSettings, _actionSettings);

            _actionExit.Execute += ActionExitExecute;
            _actionExit.Hint = "Exits the " + RS_RIBBON_TOOLS;
            _actionExit.Text = "Exit";
            _actionList.SetAction(menuExit, _actionExit);

            //_actionNewBlank.Hint = "Create a new blank Ribbon Document";
            //_actionNewBlank.Text = "Empty Ribbon Document";
            ////_actionNewBlank.Shortcut = (Shortcut)16462;
            //_actionList.SetAction(menuNew, _actionNewBlank);

            _actionBuild.Execute += ActionBuildExecute;
            _actionBuild.Enabled = false; //@ added
            _actionBuild.Hint = "Build the ribbon (Ctrl+F9)";
            _actionBuild.ImageIndex = 5;
            _actionBuild.Text = "Build";
            _actionBuild.ShortcutKeys = (Keys)(Keys.Control | Keys.F9);
            _actionList.SetAction(toolButtonBuild, _actionBuild);
            _actionList.SetAction(menuBuild, _actionBuild);

            _actionTutorial.Execute += ActionTutorialExecute;
            _actionTutorial.ImageIndex = 7;
            _actionTutorial.Text = "Tutorial";
            _actionList.SetAction(menuTutorial, _actionTutorial);

            _actionWebSite.Execute += ActionWebSiteExecute;
            _actionWebSite.ImageIndex = 7;
            _actionWebSite.Text = "Ribbon Framework for Delphi website";
            _actionList.SetAction(menuWebSite, _actionWebSite);
            _actionWebSite.Visible = false;

            _actionDotnetWebSite.Execute += ActionDotnetWebSiteExecute;
            _actionDotnetWebSite.Hint = "C#, VB Ribbon Framework";
            _actionDotnetWebSite.ImageIndex = 7;
            _actionDotnetWebSite.Text = "Website for .NET Windows Ribbon";
            _actionList.SetAction(menuDotnetWebSite, _actionDotnetWebSite);

            _actionMSDN.Execute += ActionMSDNExecute;
            _actionMSDN.ImageIndex = 6;
            _actionMSDN.Text = "MSDN Windows Ribbon";
            _actionList.SetAction(menuMSDN, _actionMSDN);

            _actionSetResourceName.Visible = Settings.Instance.AllowChangingResourceName; // false; //@ not necessary in .NET Ribbon
            _actionSetResourceName.Execute += ActionSetResourceNameExecute;
            _actionSetResourceName.Hint =
                "Set a resource name for the markup. This is necessary " + Environment.NewLine +
                "if multiple markups are used in one application." + Environment.NewLine +
                "The default is APPLICATION" + Environment.NewLine +
                Environment.NewLine + "Changing of default is not necessary in .NET Ribbon";

            _actionSetResourceName.Text = "Set ribbon resource name";
            _actionList.SetAction(setresourcename, _actionSetResourceName);

            _actionGenerateResourceIDs.Execute += ActionGenerateResourceIDsExecute;
            _actionGenerateResourceIDs.Hint =
                "Generates and sets IDs for all resources in this markup." + Environment.NewLine +
                "(For applications that use multiple different ribbons," + Environment.NewLine +
                "it may be necessary to set IDs explicitly," + Environment.NewLine +
                "so that there are no conflicting resource IDs)";
            _actionGenerateResourceIDs.Text = "Auto generate IDs for all resources";
            _actionGenerateResourceIDs.ShortcutKeys = (Keys)(Keys.Control | Keys.G);
            _actionList.SetAction(autoGenerateIdsForAllResources, _actionGenerateResourceIDs);

            _actionList.ImageList = _imageListMain;
        }

        private void InitEvents()
        {
            this.ResizeBegin += MainForm_ResizeBegin;
            this.ResizeEnd += MainForm_ResizeEnd;
            Load += FormLoad;
            Shown += CMShowingChanged;
            this.Closing += FormCloseQuery;
            this.FormClosed += FormClose;
            Application.ThreadException += ApplicationEventsException;
            _timerRestoreLog.Tick += TimerRestoreLogTimer;
            tabControl.SelectedIndexChanged += PageControlChange;
            toolPreviewLanguageCombo.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            PInvoke.SuspendPainting(this.Handle);
            this.ResumeLayout(true);
            PInvoke.ResumePainting(this.Handle);
            this.Refresh();
        }

        private void CreateMainBitmaps()
        {
            _imageListMain = ImageManager.Images_Main(components);
            toolStrip.ImageList = _imageListMain;
            //mainMenuStrip.ImageList = _imageListMain;
            menuFile.DropDown.ImageList = _imageListMain;
            menuProject.DropDown.ImageList = _imageListMain;
            menuHelp.DropDown.ImageList = _imageListMain;
        }

        private void ActionBuildExecute(object sender, EventArgs e)
        {
            BuildAndPreview(false);
        }

        private void ActionExitExecute(object sender, EventArgs e)
        {
            Close();
        }

        private void ActionGenerateCommandIDsExecute(object sender, EventArgs e)
        {
            TRibbonCommand command;
            for (int i = 0; i < _commandsFrame.ListViewCommands.Items.Count; i++)
            {
                command = (TRibbonCommand)_commandsFrame.ListViewCommands.Items[i].Tag;
                if (command.Id == 0)
                    // Try to mimic the auto ID generation of the ribbon compiler.
                    command.Id = _commandsFrame.FindSmallestUnusedID(i + 2);
            }

            _commandsFrame.RefreshSelection();
        }

        private void ActionGenerateResourceIDsExecute(object sender, EventArgs e)
        {
            int autoID;

            void SetID(TRibbonString rs)
            {
                if (!string.IsNullOrEmpty(rs.Content))
                {
                    rs.Id = autoID;
                    autoID++;
                }
            }

            void SetImageID(TRibbonList<TRibbonImage> rl)
            {
                for (int i1 = 0; i1 < rl.Count; i1++)
                {
                    rl[i1].Id = autoID;
                    autoID++;
                }
            }

            TRibbonCommand command;
            int i, maxID;
            string s;

            //
            // First work out the maximum no of command ids that will be required
            maxID = _commandsFrame.ListViewCommands.Items.Count; //@ bugfix
            for (i = 0; i < _commandsFrame.ListViewCommands.Items.Count; i++)
            {
                command = (TRibbonCommand)_commandsFrame.ListViewCommands.Items[i].Tag;
                {
                    if (!string.IsNullOrEmpty(command.LabelTitle.Content))
                        maxID++;
                    if (!string.IsNullOrEmpty(command.LabelDescription.Content))
                        maxID++;
                    if (!string.IsNullOrEmpty(command.TooltipTitle.Content))
                        maxID++;
                    if (!string.IsNullOrEmpty(command.TooltipDescription.Content))
                        maxID++;
                    if (!string.IsNullOrEmpty(command.Keytip.Content))
                        maxID++;
                    maxID += command.SmallImages.Count + command.LargeImages.Count
                        + command.SmallHighContrastImages.Count + command.LargeHighContrastImages.Count;
                }
            }

            if (InputQuery.Show(this, "ID Number", "Enter the starting ID number between 2 & " + (59999 - maxID).ToString(), out s) == DialogResult.OK)
            {
                if (!int.TryParse(s, out autoID))
                {
                    throw new ArgumentException("Invalid integer value");
                }
            }
            else
                return;

            if ((autoID < 2) || (autoID + maxID > 59999))
            {
                throw new ArgumentException(autoID.ToString() + "is an invalid starting ID. "
                + "Must be a number between 2 && < " + (59999 - maxID).ToString(), nameof(autoID));
            }

            for (i = 0; i < _commandsFrame.ListViewCommands.Items.Count; i++)
            {
                command = (TRibbonCommand)_commandsFrame.ListViewCommands.Items[i].Tag;

                {
                    SetID(command.LabelTitle);
                    SetID(command.LabelDescription);
                    SetID(command.TooltipTitle);
                    SetID(command.TooltipDescription);
                    SetID(command.Keytip);

                    SetImageID(command.SmallImages);
                    SetImageID(command.LargeImages);
                    SetImageID(command.SmallHighContrastImages);
                    SetImageID(command.LargeHighContrastImages);
                }
            }
            _commandsFrame.RefreshSelection();
        }

        private void ActionMSDNExecute(object sender, EventArgs e)
        {
            OpenWebsite("https://learn.microsoft.com/en-us/windows/win32/windowsribbon/-uiplat-windowsribbon-entry");
        }

        private void ActionNewExecute(object sender, EventArgs e)
        {
            NewFile(false);
        }

        private void ActionImageExecute(object sender, EventArgs e)
        {
            ConvertImageForm dialog = new ConvertImageForm();
            dialog.ShowDialog(this);
        }

        private void ActionLocalizeMarkupExecute(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MarkupLocalizer localizer = new MarkupLocalizer();
                string directoryName = Path.GetDirectoryName(dialog.FileName);
                directoryName = Path.Combine(directoryName, "Converted");
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                string saveFileName = Path.Combine(directoryName, Path.GetFileNameWithoutExtension(dialog.FileName) + Path.GetExtension(dialog.FileName));
                localizer.Localize(dialog.FileName, saveFileName);
            }
        }

        private void ActionDeLocalizeMarkupExecute(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MarkupLocalizer localizer = new MarkupLocalizer();
                string directoryName = Path.GetDirectoryName(dialog.FileName);
                directoryName = Path.Combine(directoryName, "Converted");
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                string saveFileName = Path.Combine(directoryName, Path.GetFileNameWithoutExtension(dialog.FileName) + Path.GetExtension(dialog.FileName));
                localizer.DeLocalize(dialog.FileName, saveFileName);
            }
        }

        private void ActionOpenExecute(object sender, EventArgs e)
        {
            if (!CheckSave())
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ReadOnlyChecked = false;
            dialog.CheckFileExists = true;
            dialog.DefaultExt = "xml";
            dialog.Filter = "Ribbon XML Files| *.xml";
            dialog.Title = "Open Ribbon File";
            if (dialog.ShowDialog() == DialogResult.OK)
                OpenFile(dialog.FileName);
        }

        private void ActionPreviewExecute(object sender, EventArgs e)
        {
            BuildAndPreview(true);
        }

        private void ActionSaveAsExecute(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.DefaultExt = "xml";
            dialog.Filter = "Ribbon XML Files| *.xml";
            dialog.Title = "Save Ribbon File";

            string originalDirectory, newDirectory;
            originalDirectory = Path.GetDirectoryName(_document.Filename);
            dialog.FileName = Path.GetFileName(_document.Filename);
            dialog.InitialDirectory = originalDirectory;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                newDirectory = Path.GetDirectoryName(dialog.FileName);
                if ((!string.IsNullOrEmpty(originalDirectory)) && (!string.Equals(originalDirectory, newDirectory, StringComparison.OrdinalIgnoreCase)))
                {
                    if (MessageBox.Show(RS_DIFFERENT_DIR_MESSAGE, RS_DIFFERENT_DIR_HEADER, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                        return;
                }
                _document.SaveToFile(dialog.FileName);
                UpdateCaption();
                UpdateControls();
                ClearModified();
                ShowFilename(dialog.FileName);
                _buildPreviewHelper.SetRibbonXmlFile(dialog.FileName);
            }
        }

        private void ActionSaveExecute(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_document.Filename))
                ActionSaveAsExecute(sender, e);
            else
            {
                _document.SaveToFile(_document.Filename);
                ClearModified();
            }
        }

        private void ActionSetResourceNameExecute(object sender, EventArgs e)
        {
            string userInput;
            userInput = InputBox.Show(this, "Enter resource name", "Please enter a resource name that is used for this ribbon markup", _document.Application.ResourceName);
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                if (userInput != _document.Application.ResourceName)
                {
                    Modified();
                    _document.Application.ResourceName = userInput;
                }
            }
        }

        private void ActionSettingsExecute(object sender, EventArgs e)
        {
            ShowSettingsDialog();
        }

        private void ActionTutorialExecute(object sender, EventArgs e)
        {
            if (Settings.Instance.RibbonFramework)
                OpenWebsite("https://github.com/harborsiem/WinForms-Ribbon/wiki");
            else
                OpenWebsite("https://github.com/harborsiem/WindowsRibbon/wiki");
            //OpenWebsite("https://www.bilsen.com/windowsribbon/tutorial.shtml");
        }

        private void ActionWebSiteExecute(object sender, EventArgs e)
        {
            OpenWebsite("https://www.bilsen.com/windowsribbon/index.shtml");
        }

        private void ActionDotnetWebSiteExecute(object sender, EventArgs e)
        {
            if (Settings.Instance.RibbonFramework)
                OpenWebsite("https://github.com/harborsiem/WinForms-Ribbon");
            else
                OpenWebsite("https://github.com/harborsiem/WindowsRibbon");
        }

        private void ApplicationEventsException(object sender, ThreadExceptionEventArgs e)
        {
            memoMessages.BackColor = Color.Red;
            Log(MessageKind.Error, e.Exception.Message);
            _timerRestoreLog.Enabled = true;
        }

        //@ => we have Tooltips
        //private void ApplicationEventsHint(object sender, EventArgs e)

        private void BuildAndPreview(bool preview)
        {
            ClearLog();
            if (_modified)
                ActionSaveExecute(this, EventArgs.Empty);
            string resourceIdentifier = Settings.Instance.AllowChangingResourceName ? _document.Application.ResourceName : TRibbonObject.ApplicationDefaultName;
            _buildPreviewHelper.ResourceIdentifier = resourceIdentifier;
            if (preview)
            {
                _buildPreviewHelper.ShowPreviewDialog(this);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    _buildPreviewHelper.BuildRibbonFile(resourceIdentifier);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
        //private void BuildAndPreview(bool preview)
        //{
        //    Handle DllInstance;
        //    RibbonCompileResult result;
        //    ClearLog();
        //    if (_modified)
        //        ActionSaveExecute(this, EventArgs.Empty);
        //    FreeAndNil(_previewForm);
        //    // Create DLL only if a preview is requested
        //    result = _compiler.Compile(_document, _document.Application.ResourceName, preview);

        //    if (result == RibbonCompileResult.Ok)
        //    {
        //        if (preview)
        //        {
        //            DllInstance = LoadLibraryEx(PChar(_compiler.OutputDllPath), 0, LOAD_LIBRARY_AS_DATAFILE);
        //            if (DllInstance == 0)
        //            {
        //                Log(MessageKind.Error, RS_CANNOT_LOAD_DLL);
        //                return;
        //            }

        //            try
        //            {
        //                _previewForm = new PreviewForm(DllInstance, _document, _document.Application.ResourceName);
        //                _previewForm.ShowDialog();
        //            }
        //            catch (Exception)
        //            {
        //                FreeLibrary(DllInstance);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        memoMessages.ForeColor = Color.Red;
        //        //    memoMessages.Update();
        //        _timerRestoreLog.Enabled = true;
        //        //    if (result == RibbonCompileResult.RibbonCompilerError) 
        //        //    {
        //        //      _xmlSourceFrame.ActivateFrame();
        //        //      tabControl.SelectedTab = tabPageXmlSource;
        //        //    }
        //    }
        //}

        private bool CheckSave()
        {
            bool result = true;
            if (_modified)
            {
                switch (MessageBox.Show(RS_CHANGED_MESSAGE, RS_CHANGED_HEADER, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            if (_actionSave.Enabled)
                                ActionSaveExecute(this, EventArgs.Empty);
                            else
                                ActionSaveAsExecute(this, EventArgs.Empty);
                            break;
                        }

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        result = false;
                        break;
                }
            }
            return result;
        }

        private void ClearDocument()
        {
            _commandsFrame.ClearDocument();
            _viewsFrame.ClearDocument();
            _xmlSourceFrame.ClearDocument();
            _document.Clear();
        }

        private void ClearLog()
        {
            memoMessages.Clear();
        }

        private void ClearModified()
        {
            _modified = false;
            statusModified.Text = string.Empty;
        }

        private void CMShowingChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                _initialized = true;
                if (!Settings.Instance.ToolsAvailable())
                    if (MessageBox.Show(Settings.RS_TOOLS_MESSAGE + Environment.NewLine + Settings.RS_TOOLS_SETUP, Settings.RS_TOOLS_HEADER, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)

                        ShowSettingsDialog();
            }
        }

        private void Destroy()
        {
            _document.Dispose();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            memoMessages.SelectionLength = 0;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && !string.IsNullOrEmpty(args[1]) && File.Exists(args[1]))
            {
                OpenFile(Utils.GetExactFilenameWithPath(args[1]));
            }
        }

        private void FormClose(object sender, FormClosedEventArgs e)
        {
            Settings.Instance.Write();
            UIImage.Destroy();
            Application.Exit();
        }

        private void FormCloseQuery(object sender, CancelEventArgs e)
        {
            if (!CheckSave())
                e.Cancel = true;
        }

        static readonly string[] MSG_TYPES = { string.Empty, "WARNING: ", "ERROR: ", string.Empty };

        private void Log(MessageKind msgType,
            string msg)
        {
            List<string> lines = new List<string>(memoMessages.Lines);
            lines.Add(MSG_TYPES[(int)msgType] + msg);
            memoMessages.Lines = lines.ToArray();
        }

        public void Modified()
        {
            if (!_modified)
            {
                _modified = true;
                statusModified.Text = RS_MODIFIED;
                _buildPreviewHelper.HasValidParser = false;
            }
        }

        private void ShowFilename(string value)
        {
            statusHints.Text = value;
        }

        private void NewFile(bool emptyFile)
        {
            RibbonTemplate template;
            string fileName, filePath;
            Stream zipStream;

            if (!CheckSave())
                return;

            if (emptyFile)
            {
                template = RibbonTemplate.None;
                fileName = string.Empty;
            }
            else if (!NewFileForm.NewFileDialog(out template, out fileName))
                return;

            ClearDocument();
            if (template == RibbonTemplate.None)
            {
                if (!string.IsNullOrEmpty(fileName))
                    _document.SaveToFile(fileName);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    filePath = Path.GetDirectoryName(fileName);
                    zipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UIRibbonTools.Wordpad.zip");
                    string tmpFile = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                    FileStream fs = File.Create(tmpFile);
                    zipStream.CopyTo(fs);
                    fs.Close();
                    try
                    {
                        string resFolder = Path.Combine(filePath, "Res");
                        if (Directory.Exists(resFolder))
                        {
                            Directory.Delete(resFolder, true);
                            File.Delete(Path.Combine(filePath, "RibbonMarkup.xml"));
                        }
                        ZipFile.ExtractToDirectory(tmpFile, filePath);
                    }
                    finally
                    {
                        zipStream.Close();
                        File.Delete(tmpFile);
                    }
                    File.Move(Path.Combine(filePath, "RibbonMarkup.xml"), fileName);
                    _document.LoadFromFile(fileName);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

            tabControl.SelectedTab = tabPageCommands;
            ActiveControl = _commandsFrame.ListViewCommands;
            ShowDocument();
            UpdateCaption();
            UpdateControls();
            ClearModified();
            ShowFilename(fileName);
            _buildPreviewHelper.SetRibbonXmlFile(fileName);
        }

        private void OpenFile(string fileName)
        {
            ClearDocument();
            _document.LoadFromFile(fileName);
            tabControl.SelectedTab = tabPageCommands;
            ActiveControl = _commandsFrame.ListViewCommands;
            ShowDocument();
            UpdateCaption();
            UpdateControls();
            ClearModified();
            ShowFilename(fileName);
            _buildPreviewHelper.SetRibbonXmlFile(fileName);
        }

        private void OpenWebsite(string url)
        {
            ProcessStartInfo psi = new ProcessStartInfo() { UseShellExecute = true, FileName = url };
            Process.Start(psi);
        }

        private void PageControlChange(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageViews)
            {
                _commandsFrame.DeactivateFrame();
                _xmlSourceFrame.DeactivateFrame();
                _viewsFrame.ActivateFrame();
            }
            else if (tabControl.SelectedTab == tabPageCommands)
            {
                _viewsFrame.DeactivateFrame();
                _xmlSourceFrame.DeactivateFrame();
                _commandsFrame.ActivateFrame();
            }
            else if (tabControl.SelectedTab == tabPageXmlSource)
            {
                if (_modified)
                    ActionSaveExecute(sender, e);
                _viewsFrame.DeactivateFrame();
                _commandsFrame.DeactivateFrame();
                _xmlSourceFrame.ActivateFrame();
            }
        }

        //@ todo
        //private void RibbonCompilerMessage(RibbonCompiler compiler,
        //    MessageKind msgType, string msg)
        //{
        //    if (msgType == MessageKind.Pipe)
        //        memoMessages.Text = memoMessages.Text + msg;
        //    else
        //        Log(msgType, msg);
        //}

        private void ShowDocument()
        {
            _xmlSourceFrame.DeactivateFrame();
            _viewsFrame.DeactivateFrame();
            _commandsFrame.ActivateFrame();
            _commandsFrame.ShowDocument(_document);
            _viewsFrame.ShowDocument(_document);
            _xmlSourceFrame.ShowDocument(_document);
        }

        private void ShowSettingsDialog()
        {
            SettingsForm dialog;

            dialog = new SettingsForm();
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _actionSetResourceName.Visible = Settings.Instance.AllowChangingResourceName;
                    if (!Settings.Instance.AllowChangingResourceName)
                    {
                        if (_document.Application.ResourceName != TRibbonObject.ApplicationDefaultName)
                        {
                            Modified();
                            _document.Application.ResourceName = TRibbonObject.ApplicationDefaultName;
                        }
                    }
                }
            }
            finally
            {
                //dialog.Close();
            }
        }

        private void TimerRestoreLogTimer(object sender, EventArgs e)
        {
            _timerRestoreLog.Enabled = false;
            memoMessages.BackColor = SystemColors.Window;
        }

        private void UpdateCaption()
        {
            if (string.IsNullOrEmpty(_document.Filename))
                this.Text = RS_UNTITLED + " - " + RS_RIBBON_TOOLS;
            else
                this.Text = Path.GetFileName(_document.Filename) + " - " + RS_RIBBON_TOOLS;
        }

        private void UpdateControls()
        {
            bool enabled = File.Exists(_document.Filename);
            _actionPreview.Enabled = enabled;
            _actionBuild.Enabled = enabled;
            _actionSave.Enabled = enabled; //@ added
            _actionSaveAs.Enabled = enabled; //@ added
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //@ added
        {
            return ShortCutKeysHandler.ProcessCmdKey(ref msg, keyData);
        }

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox combo = sender as ToolStripComboBox;
            if (combo != null)
            {
                _buildPreviewHelper.UICulture = (string)combo.SelectedItem;
            }
        }

        private void SetLanguages(IList<string> languages)
        {
            toolPreviewLanguageCombo.SelectedIndexChanged -= LanguageComboBox_SelectedIndexChanged;
            toolPreviewLanguageCombo.BeginUpdate();
            toolPreviewLanguageCombo.Items.Clear();
            toolPreviewLanguageCombo.Enabled = true;
            toolPreviewLanguageCombo.Items.Add(BuildPreviewHelper.Neutral);
            if (languages != null)
            {
                for (int i = 0; i < languages.Count; i++)
                {
                    toolPreviewLanguageCombo.Items.Add(languages[i]);
                }
            }
            toolPreviewLanguageCombo.EndUpdate();
            toolPreviewLanguageCombo.SelectedIndex = 0;
            toolPreviewLanguageCombo.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
        }

        private void SetPreviewEnabled(bool enabled)
        {
            _actionPreview.Enabled = enabled;
        }
    }
}
