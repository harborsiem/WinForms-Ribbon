namespace UIRibbonTools
{
    partial class CommandsFrame
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandsFrame));
            toolButtonAddCommand = new System.Windows.Forms.ToolStripButton();
            toolButtonRemoveCommand = new System.Windows.Forms.ToolStripButton();
            toolButtonMoveUp = new System.Windows.Forms.ToolStripButton();
            toolButtonMoveDown = new System.Windows.Forms.ToolStripButton();
            toolButtonSearchCommand = new System.Windows.Forms.ToolStripButton();
            toolBarCommands = new System.Windows.Forms.ToolStrip();
            ListViewCommands = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            popupMenuList = new System.Windows.Forms.ContextMenuStrip(components);
            menuAddCommand = new System.Windows.Forms.ToolStripMenuItem();
            menuRemoveCommand = new System.Windows.Forms.ToolStripMenuItem();
            _nN1 = new System.Windows.Forms.ToolStripSeparator();
            menuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            menuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            LabelHeader = new System.Windows.Forms.ToolStripLabel();
            _editFrame = new EditFrame();
            _smallImagesPanel = new System.Windows.Forms.Panel();
            labelSmallImages = new System.Windows.Forms.Label();
            _largeImagesPanel = new System.Windows.Forms.Panel();
            labelLargeImages = new System.Windows.Forms.Label();
            _highContrastImagesPanel = new System.Windows.Forms.Panel();
            labelSmallHCImages = new System.Windows.Forms.Label();
            _largeHCImagesPanel = new System.Windows.Forms.Panel();
            labelLargeHCImages = new System.Windows.Forms.Label();
            imagesPanel = new System.Windows.Forms.TableLayoutPanel();
            _smallImagesFrame = new ImageListFrame();
            _largeImagesFrame = new ImageListFrame();
            _smallHCImagesFrame = new ImageListFrame();
            _largeHCImagesFrame = new ImageListFrame();
            _panel2Layout = new System.Windows.Forms.TableLayoutPanel();
            SplitterCommands = new System.Windows.Forms.SplitContainer();
            toolBarCommands.SuspendLayout();
            popupMenuList.SuspendLayout();
            toolStrip1.SuspendLayout();
            _smallImagesPanel.SuspendLayout();
            _largeImagesPanel.SuspendLayout();
            _highContrastImagesPanel.SuspendLayout();
            _largeHCImagesPanel.SuspendLayout();
            imagesPanel.SuspendLayout();
            _panel2Layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitterCommands).BeginInit();
            SplitterCommands.Panel1.SuspendLayout();
            SplitterCommands.Panel2.SuspendLayout();
            SplitterCommands.SuspendLayout();
            SuspendLayout();
            // 
            // toolButtonAddCommand
            // 
            toolButtonAddCommand.Image = (System.Drawing.Image)resources.GetObject("toolButtonAddCommand.Image");
            toolButtonAddCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButtonAddCommand.Name = "toolButtonAddCommand";
            toolButtonAddCommand.Size = new System.Drawing.Size(49, 22);
            toolButtonAddCommand.Text = "Add";
            // 
            // toolButtonRemoveCommand
            // 
            toolButtonRemoveCommand.Image = (System.Drawing.Image)resources.GetObject("toolButtonRemoveCommand.Image");
            toolButtonRemoveCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButtonRemoveCommand.Name = "toolButtonRemoveCommand";
            toolButtonRemoveCommand.Size = new System.Drawing.Size(70, 22);
            toolButtonRemoveCommand.Text = "Remove";
            // 
            // toolButtonMoveUp
            // 
            toolButtonMoveUp.Image = (System.Drawing.Image)resources.GetObject("toolButtonMoveUp.Image");
            toolButtonMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButtonMoveUp.Name = "toolButtonMoveUp";
            toolButtonMoveUp.Size = new System.Drawing.Size(42, 22);
            toolButtonMoveUp.Text = "Up";
            // 
            // toolButtonMoveDown
            // 
            toolButtonMoveDown.Image = (System.Drawing.Image)resources.GetObject("toolButtonMoveDown.Image");
            toolButtonMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButtonMoveDown.Name = "toolButtonMoveDown";
            toolButtonMoveDown.Size = new System.Drawing.Size(58, 22);
            toolButtonMoveDown.Text = "Down";
            // 
            // toolButtonSearchCommand
            // 
            toolButtonSearchCommand.Image = (System.Drawing.Image)resources.GetObject("toolButtonSearchCommand.Image");
            toolButtonSearchCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolButtonSearchCommand.Name = "toolButtonSearchCommand";
            toolButtonSearchCommand.Size = new System.Drawing.Size(62, 20);
            toolButtonSearchCommand.Text = "Search";
            // 
            // toolBarCommands
            // 
            toolBarCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolButtonAddCommand, toolButtonRemoveCommand, toolButtonMoveUp, toolButtonMoveDown, toolButtonSearchCommand });
            toolBarCommands.Location = new System.Drawing.Point(0, 0);
            toolBarCommands.Name = "toolBarCommands";
            toolBarCommands.Size = new System.Drawing.Size(293, 25);
            toolBarCommands.TabIndex = 0;
            // 
            // ListViewCommands
            // 
            ListViewCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            ListViewCommands.ContextMenuStrip = popupMenuList;
            ListViewCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            ListViewCommands.FullRowSelect = true;
            ListViewCommands.GridLines = true;
            ListViewCommands.Location = new System.Drawing.Point(0, 25);
            ListViewCommands.Margin = new System.Windows.Forms.Padding(0);
            ListViewCommands.MultiSelect = false;
            ListViewCommands.Name = "ListViewCommands";
            ListViewCommands.Size = new System.Drawing.Size(293, 481);
            ListViewCommands.TabIndex = 1;
            ListViewCommands.UseCompatibleStateImageBehavior = false;
            ListViewCommands.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 112;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Label Title";
            columnHeader2.Width = 170;
            // 
            // popupMenuList
            // 
            popupMenuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuAddCommand, menuRemoveCommand, _nN1, menuMoveUp, menuMoveDown });
            popupMenuList.Name = "popupMenuList";
            popupMenuList.Size = new System.Drawing.Size(139, 98);
            // 
            // menuAddCommand
            // 
            menuAddCommand.Name = "menuAddCommand";
            menuAddCommand.Size = new System.Drawing.Size(138, 22);
            menuAddCommand.Text = "Add";
            // 
            // menuRemoveCommand
            // 
            menuRemoveCommand.Name = "menuRemoveCommand";
            menuRemoveCommand.Size = new System.Drawing.Size(138, 22);
            menuRemoveCommand.Text = "Remove";
            // 
            // _nN1
            // 
            _nN1.Name = "_nN1";
            _nN1.Size = new System.Drawing.Size(135, 6);
            // 
            // menuMoveUp
            // 
            menuMoveUp.Name = "menuMoveUp";
            menuMoveUp.Size = new System.Drawing.Size(138, 22);
            menuMoveUp.Text = "Move Up";
            // 
            // menuMoveDown
            // 
            menuMoveDown.Name = "menuMoveDown";
            menuMoveDown.Size = new System.Drawing.Size(138, 22);
            menuMoveDown.Text = "Move Down";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { LabelHeader });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(702, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // LabelHeader
            // 
            LabelHeader.Name = "LabelHeader";
            LabelHeader.Size = new System.Drawing.Size(126, 22);
            LabelHeader.Text = "  Command Properties";
            // 
            // _editFrame
            // 
            _editFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _editFrame.Location = new System.Drawing.Point(0, 0);
            _editFrame.Margin = new System.Windows.Forms.Padding(0);
            _editFrame.Name = "_editFrame";
            _editFrame.Size = new System.Drawing.Size(702, 224);
            _editFrame.TabIndex = 0;
            // 
            // _smallImagesPanel
            // 
            _smallImagesPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _smallImagesPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            _smallImagesPanel.Controls.Add(labelSmallImages);
            _smallImagesPanel.Location = new System.Drawing.Point(0, 0);
            _smallImagesPanel.Margin = new System.Windows.Forms.Padding(0);
            _smallImagesPanel.Name = "_smallImagesPanel";
            _smallImagesPanel.Size = new System.Drawing.Size(351, 24);
            _smallImagesPanel.TabIndex = 0;
            // 
            // labelSmallImages
            // 
            labelSmallImages.Location = new System.Drawing.Point(3, 3);
            labelSmallImages.Margin = new System.Windows.Forms.Padding(3);
            labelSmallImages.Name = "labelSmallImages";
            labelSmallImages.Size = new System.Drawing.Size(166, 15);
            labelSmallImages.TabIndex = 0;
            labelSmallImages.Text = "  Small Images";
            labelSmallImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _largeImagesPanel
            // 
            _largeImagesPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _largeImagesPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            _largeImagesPanel.Controls.Add(labelLargeImages);
            _largeImagesPanel.Location = new System.Drawing.Point(351, 0);
            _largeImagesPanel.Margin = new System.Windows.Forms.Padding(0);
            _largeImagesPanel.Name = "_largeImagesPanel";
            _largeImagesPanel.Size = new System.Drawing.Size(351, 24);
            _largeImagesPanel.TabIndex = 1;
            // 
            // labelLargeImages
            // 
            labelLargeImages.Location = new System.Drawing.Point(3, 3);
            labelLargeImages.Margin = new System.Windows.Forms.Padding(3);
            labelLargeImages.Name = "labelLargeImages";
            labelLargeImages.Size = new System.Drawing.Size(238, 15);
            labelLargeImages.TabIndex = 0;
            labelLargeImages.Text = "  Large Images";
            labelLargeImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _highContrastImagesPanel
            // 
            _highContrastImagesPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _highContrastImagesPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            _highContrastImagesPanel.Controls.Add(labelSmallHCImages);
            _highContrastImagesPanel.Location = new System.Drawing.Point(0, 127);
            _highContrastImagesPanel.Margin = new System.Windows.Forms.Padding(0);
            _highContrastImagesPanel.Name = "_highContrastImagesPanel";
            _highContrastImagesPanel.Size = new System.Drawing.Size(351, 24);
            _highContrastImagesPanel.TabIndex = 4;
            // 
            // labelSmallHCImages
            // 
            labelSmallHCImages.Location = new System.Drawing.Point(3, 3);
            labelSmallHCImages.Margin = new System.Windows.Forms.Padding(3);
            labelSmallHCImages.Name = "labelSmallHCImages";
            labelSmallHCImages.Size = new System.Drawing.Size(261, 15);
            labelSmallHCImages.TabIndex = 0;
            labelSmallHCImages.Text = "  Small High-Contrast Images";
            labelSmallHCImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _largeHCImagesPanel
            // 
            _largeHCImagesPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _largeHCImagesPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            _largeHCImagesPanel.Controls.Add(labelLargeHCImages);
            _largeHCImagesPanel.Location = new System.Drawing.Point(351, 127);
            _largeHCImagesPanel.Margin = new System.Windows.Forms.Padding(0);
            _largeHCImagesPanel.Name = "_largeHCImagesPanel";
            _largeHCImagesPanel.Size = new System.Drawing.Size(351, 24);
            _largeHCImagesPanel.TabIndex = 5;
            // 
            // labelLargeHCImages
            // 
            labelLargeHCImages.Location = new System.Drawing.Point(3, 3);
            labelLargeHCImages.Margin = new System.Windows.Forms.Padding(3);
            labelLargeHCImages.Name = "labelLargeHCImages";
            labelLargeHCImages.Size = new System.Drawing.Size(321, 15);
            labelLargeHCImages.TabIndex = 0;
            labelLargeHCImages.Text = "  Large High-Contrast Images";
            labelLargeHCImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imagesPanel
            // 
            imagesPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            imagesPanel.ColumnCount = 2;
            imagesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            imagesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            imagesPanel.Controls.Add(_smallImagesPanel, 0, 0);
            imagesPanel.Controls.Add(_largeImagesPanel, 1, 0);
            imagesPanel.Controls.Add(_smallImagesFrame, 0, 1);
            imagesPanel.Controls.Add(_largeImagesFrame, 1, 1);
            imagesPanel.Controls.Add(_highContrastImagesPanel, 0, 2);
            imagesPanel.Controls.Add(_largeHCImagesPanel, 1, 2);
            imagesPanel.Controls.Add(_smallHCImagesFrame, 0, 3);
            imagesPanel.Controls.Add(_largeHCImagesFrame, 1, 3);
            imagesPanel.Location = new System.Drawing.Point(0, 227);
            imagesPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            imagesPanel.Name = "imagesPanel";
            imagesPanel.RowCount = 4;
            imagesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            imagesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            imagesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            imagesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            imagesPanel.Size = new System.Drawing.Size(702, 254);
            imagesPanel.TabIndex = 1;
            // 
            // _smallImagesFrame
            // 
            _smallImagesFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _smallImagesFrame.Location = new System.Drawing.Point(0, 24);
            _smallImagesFrame.Margin = new System.Windows.Forms.Padding(0);
            _smallImagesFrame.Name = "_smallImagesFrame";
            _smallImagesFrame.Size = new System.Drawing.Size(351, 103);
            _smallImagesFrame.TabIndex = 2;
            // 
            // _largeImagesFrame
            // 
            _largeImagesFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _largeImagesFrame.Location = new System.Drawing.Point(351, 24);
            _largeImagesFrame.Margin = new System.Windows.Forms.Padding(0);
            _largeImagesFrame.Name = "_largeImagesFrame";
            _largeImagesFrame.Size = new System.Drawing.Size(351, 103);
            _largeImagesFrame.TabIndex = 3;
            // 
            // _smallHCImagesFrame
            // 
            _smallHCImagesFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _smallHCImagesFrame.Location = new System.Drawing.Point(0, 151);
            _smallHCImagesFrame.Margin = new System.Windows.Forms.Padding(0);
            _smallHCImagesFrame.Name = "_smallHCImagesFrame";
            _smallHCImagesFrame.Size = new System.Drawing.Size(351, 103);
            _smallHCImagesFrame.TabIndex = 6;
            // 
            // _largeHCImagesFrame
            // 
            _largeHCImagesFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _largeHCImagesFrame.Location = new System.Drawing.Point(351, 151);
            _largeHCImagesFrame.Margin = new System.Windows.Forms.Padding(0);
            _largeHCImagesFrame.Name = "_largeHCImagesFrame";
            _largeHCImagesFrame.Size = new System.Drawing.Size(351, 103);
            _largeHCImagesFrame.TabIndex = 7;
            // 
            // _panel2Layout
            // 
            _panel2Layout.AutoScroll = true;
            _panel2Layout.ColumnCount = 1;
            _panel2Layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _panel2Layout.Controls.Add(_editFrame, 0, 0);
            _panel2Layout.Controls.Add(imagesPanel, 0, 1);
            _panel2Layout.Dock = System.Windows.Forms.DockStyle.Fill;
            _panel2Layout.Location = new System.Drawing.Point(0, 25);
            _panel2Layout.Margin = new System.Windows.Forms.Padding(0);
            _panel2Layout.Name = "_panel2Layout";
            _panel2Layout.RowCount = 2;
            _panel2Layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _panel2Layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _panel2Layout.Size = new System.Drawing.Size(702, 481);
            _panel2Layout.TabIndex = 1;
            // 
            // SplitterCommands
            // 
            SplitterCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitterCommands.Location = new System.Drawing.Point(0, 0);
            SplitterCommands.Name = "SplitterCommands";
            // 
            // SplitterCommands.Panel1
            // 
            SplitterCommands.Panel1.Controls.Add(ListViewCommands);
            SplitterCommands.Panel1.Controls.Add(toolBarCommands);
            SplitterCommands.Panel1MinSize = 293;
            // 
            // SplitterCommands.Panel2
            // 
            SplitterCommands.Panel2.Controls.Add(_panel2Layout);
            SplitterCommands.Panel2.Controls.Add(toolStrip1);
            SplitterCommands.Size = new System.Drawing.Size(1000, 506);
            SplitterCommands.SplitterDistance = 293;
            SplitterCommands.SplitterWidth = 5;
            SplitterCommands.TabIndex = 0;
            // 
            // CommandsFrame
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(SplitterCommands);
            Name = "CommandsFrame";
            Size = new System.Drawing.Size(1000, 506);
            toolBarCommands.ResumeLayout(false);
            toolBarCommands.PerformLayout();
            popupMenuList.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            _smallImagesPanel.ResumeLayout(false);
            _largeImagesPanel.ResumeLayout(false);
            _highContrastImagesPanel.ResumeLayout(false);
            _largeHCImagesPanel.ResumeLayout(false);
            imagesPanel.ResumeLayout(false);
            _panel2Layout.ResumeLayout(false);
            SplitterCommands.Panel1.ResumeLayout(false);
            SplitterCommands.Panel1.PerformLayout();
            SplitterCommands.Panel2.ResumeLayout(false);
            SplitterCommands.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SplitterCommands).EndInit();
            SplitterCommands.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitterCommands;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStrip toolBarCommands;
        private System.Windows.Forms.ToolStripButton toolButtonAddCommand;
        private System.Windows.Forms.ToolStripButton toolButtonRemoveCommand;
        private System.Windows.Forms.ToolStripButton toolButtonMoveUp;
        private System.Windows.Forms.ToolStripButton toolButtonMoveDown;
        private System.Windows.Forms.ToolStripButton toolButtonSearchCommand;
        private EditFrame _editFrame;
        internal System.Windows.Forms.ListView ListViewCommands;
        private System.Windows.Forms.ContextMenuStrip popupMenuList;
        private System.Windows.Forms.ToolStripMenuItem menuAddCommand;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveCommand;
        private System.Windows.Forms.ToolStripSeparator _nN1;
        private System.Windows.Forms.ToolStripMenuItem menuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem menuMoveDown;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel LabelHeader;
        internal System.Windows.Forms.TableLayoutPanel _panel2Layout;
        private ImageListFrame _smallImagesFrame;
        private ImageListFrame _largeImagesFrame;
        private System.Windows.Forms.Panel _largeHCImagesPanel;
        private System.Windows.Forms.Label labelLargeHCImages;
        private System.Windows.Forms.Panel _highContrastImagesPanel;
        private System.Windows.Forms.Label labelSmallHCImages;
        private System.Windows.Forms.Panel _largeImagesPanel;
        private System.Windows.Forms.Label labelLargeImages;
        private System.Windows.Forms.Panel _smallImagesPanel;
        private System.Windows.Forms.Label labelSmallImages;
        private ImageListFrame _largeHCImagesFrame;
        private ImageListFrame _smallHCImagesFrame;
        private System.Windows.Forms.TableLayoutPanel imagesPanel;

    }
}
