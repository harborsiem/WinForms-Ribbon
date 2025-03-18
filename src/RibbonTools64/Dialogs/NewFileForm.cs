using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace UIRibbonTools
{
    partial class NewFileForm : Form
    {
        const string RS_SELECT_DIR_CAPTION = "Select or create directory for Ribbon Document";

        public NewFileForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //this.ClientSize = new Size(dialogLayout.Width + 18, dialogLayout.Height + 18);
            this.MinimumSize = this.Size;
            this.MaximumSize = new Size(Int32.MaxValue, this.Size.Height);
            if (components == null)
                components = new Container();
            directoryButton.ImageList = ImageManager.ImageList_NewFile(components);
            directoryButton.ImageIndex = 0;
            directoryButton.MouseEnter += Button1_MouseEnter;
            directoryButton.MouseLeave += Button1_MouseLeave;
            //EditDirectory.Text = Directory.GetCurrentDirectory();
            directoryButton.Click += EditDirectoryRightButtonClick;
            EditFilename.TextChanged += EditFilenameChange;
            EditDirectory.TextChanged += EditDirectoryChange;
        }

        private void Button1_MouseLeave(object sender, EventArgs e)
        {
            directoryButton.ImageIndex = 0;
        }

        private void Button1_MouseEnter(object sender, EventArgs e)
        {
            directoryButton.ImageIndex = 1;
        }

        public static bool NewFileDialog(out RibbonTemplate template, out string fileName)
        {
            NewFileForm dialog;
            bool result;
            template = RibbonTemplate.None;
            fileName = string.Empty;
            dialog = new NewFileForm();
            try
            {
                result = (dialog.ShowDialog() == DialogResult.OK);
                if (result)
                {
                    int itemIndex = 0;
                    if (dialog.wordPadRadioButton.Checked)
                        itemIndex = 1;
                    template = (RibbonTemplate)(itemIndex);
                    fileName = Path.Combine(dialog.EditDirectory.Text, dialog.EditFilename.Text);
                }
            }
            finally
            {
                dialog.Close();
            }
            return result;
        }

        private void EditDirectoryChange(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void EditDirectoryRightButtonClick(object sender, EventArgs e)
        {
            string directory;

            directory = EditDirectory.Text;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = RS_SELECT_DIR_CAPTION;
            dialog.UseDescriptionForTitle = true;
            //dialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
                dialog.InitialDirectory = directory;
            else
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //dialog.ClientGuid = new Guid("0AC0837C-BBF8-452A-850D-79D08E667CA7"); //My PC
            }
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                directory = dialog.SelectedPath;
                EditDirectory.Text = directory;
                UpdateControls();
            }
        }

        private void EditFilenameChange(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            ButtonOk.Enabled = Directory.Exists(EditDirectory.Text) && (!string.IsNullOrEmpty(EditFilename.Text));
        }
    }
}
