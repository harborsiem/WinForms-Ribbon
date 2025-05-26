namespace UIRibbonTools
{
    partial class PreviewForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabSheetAppModes = new System.Windows.Forms.TabPage();
            checkedListBoxAppModes = new System.Windows.Forms.CheckedListBox();
            labelAppModes = new System.Windows.Forms.Label();
            tabSheetContextTabs = new System.Windows.Forms.TabPage();
            checkedListBoxContextTabs = new System.Windows.Forms.CheckedListBox();
            labelContextTabs = new System.Windows.Forms.Label();
            tabSheetContextPopups = new System.Windows.Forms.TabPage();
            listBoxContextPopups = new System.Windows.Forms.ListBox();
            labelContextPopups = new System.Windows.Forms.Label();
            tabSheetColorize = new System.Windows.Forms.TabPage();
            radioHSB = new System.Windows.Forms.RadioButton();
            radioRGB = new System.Windows.Forms.RadioButton();
            colorizeLayout = new System.Windows.Forms.TableLayoutPanel();
            backgroundGroup = new System.Windows.Forms.GroupBox();
            backgroundColorFrame = new ColorFrame();
            highlightGroup = new System.Windows.Forms.GroupBox();
            highlightColorFrame = new ColorFrame();
            textColorGroup = new System.Windows.Forms.GroupBox();
            textColorFrame = new ColorFrame();
            setColorsButton = new System.Windows.Forms.Button();
            setDefaultColorsButton = new System.Windows.Forms.Button();
            tabControl = new System.Windows.Forms.TabControl();
            tabSheetAppModes.SuspendLayout();
            tabSheetContextTabs.SuspendLayout();
            tabSheetContextPopups.SuspendLayout();
            tabSheetColorize.SuspendLayout();
            colorizeLayout.SuspendLayout();
            backgroundGroup.SuspendLayout();
            highlightGroup.SuspendLayout();
            textColorGroup.SuspendLayout();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // tabSheetAppModes
            // 
            tabSheetAppModes.Controls.Add(checkedListBoxAppModes);
            tabSheetAppModes.Controls.Add(labelAppModes);
            tabSheetAppModes.Location = new System.Drawing.Point(4, 24);
            tabSheetAppModes.Name = "tabSheetAppModes";
            tabSheetAppModes.Padding = new System.Windows.Forms.Padding(3);
            tabSheetAppModes.Size = new System.Drawing.Size(925, 325);
            tabSheetAppModes.TabIndex = 0;
            tabSheetAppModes.Text = "Application Modes";
            tabSheetAppModes.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxAppModes
            // 
            checkedListBoxAppModes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            checkedListBoxAppModes.CheckOnClick = true;
            checkedListBoxAppModes.ColumnWidth = 200;
            checkedListBoxAppModes.FormattingEnabled = true;
            checkedListBoxAppModes.Location = new System.Drawing.Point(9, 22);
            checkedListBoxAppModes.MultiColumn = true;
            checkedListBoxAppModes.Name = "checkedListBoxAppModes";
            checkedListBoxAppModes.Size = new System.Drawing.Size(905, 292);
            checkedListBoxAppModes.TabIndex = 1;
            // 
            // labelAppModes
            // 
            labelAppModes.AutoSize = true;
            labelAppModes.Location = new System.Drawing.Point(10, 3);
            labelAppModes.Name = "labelAppModes";
            labelAppModes.Size = new System.Drawing.Size(225, 15);
            labelAppModes.TabIndex = 0;
            labelAppModes.Text = "* There are no application modes defined";
            // 
            // tabSheetContextTabs
            // 
            tabSheetContextTabs.Controls.Add(checkedListBoxContextTabs);
            tabSheetContextTabs.Controls.Add(labelContextTabs);
            tabSheetContextTabs.Location = new System.Drawing.Point(4, 24);
            tabSheetContextTabs.Name = "tabSheetContextTabs";
            tabSheetContextTabs.Padding = new System.Windows.Forms.Padding(3);
            tabSheetContextTabs.Size = new System.Drawing.Size(925, 325);
            tabSheetContextTabs.TabIndex = 1;
            tabSheetContextTabs.Text = "Contextual Tabs";
            tabSheetContextTabs.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxContextTabs
            // 
            checkedListBoxContextTabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            checkedListBoxContextTabs.CheckOnClick = true;
            checkedListBoxContextTabs.ColumnWidth = 500;
            checkedListBoxContextTabs.FormattingEnabled = true;
            checkedListBoxContextTabs.Location = new System.Drawing.Point(9, 22);
            checkedListBoxContextTabs.MultiColumn = true;
            checkedListBoxContextTabs.Name = "checkedListBoxContextTabs";
            checkedListBoxContextTabs.Size = new System.Drawing.Size(905, 292);
            checkedListBoxContextTabs.TabIndex = 1;
            // 
            // labelContextTabs
            // 
            labelContextTabs.AutoSize = true;
            labelContextTabs.Location = new System.Drawing.Point(10, 3);
            labelContextTabs.Name = "labelContextTabs";
            labelContextTabs.Size = new System.Drawing.Size(207, 15);
            labelContextTabs.TabIndex = 0;
            labelContextTabs.Text = "* There are no contextual tabs defined";
            // 
            // tabSheetContextPopups
            // 
            tabSheetContextPopups.Controls.Add(listBoxContextPopups);
            tabSheetContextPopups.Controls.Add(labelContextPopups);
            tabSheetContextPopups.Location = new System.Drawing.Point(4, 24);
            tabSheetContextPopups.Name = "tabSheetContextPopups";
            tabSheetContextPopups.Size = new System.Drawing.Size(925, 325);
            tabSheetContextPopups.TabIndex = 2;
            tabSheetContextPopups.Text = "Context Popups";
            tabSheetContextPopups.UseVisualStyleBackColor = true;
            // 
            // listBoxContextPopups
            // 
            listBoxContextPopups.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listBoxContextPopups.FormattingEnabled = true;
            listBoxContextPopups.ItemHeight = 15;
            listBoxContextPopups.Location = new System.Drawing.Point(9, 22);
            listBoxContextPopups.Name = "listBoxContextPopups";
            listBoxContextPopups.Size = new System.Drawing.Size(905, 289);
            listBoxContextPopups.TabIndex = 1;
            // 
            // labelContextPopups
            // 
            labelContextPopups.AutoSize = true;
            labelContextPopups.Location = new System.Drawing.Point(10, 3);
            labelContextPopups.Name = "labelContextPopups";
            labelContextPopups.Size = new System.Drawing.Size(209, 15);
            labelContextPopups.TabIndex = 0;
            labelContextPopups.Text = "* There are no context popups defined";
            // 
            // tabSheetColorize
            // 
            tabSheetColorize.Controls.Add(radioHSB);
            tabSheetColorize.Controls.Add(radioRGB);
            tabSheetColorize.Controls.Add(colorizeLayout);
            tabSheetColorize.Controls.Add(setColorsButton);
            tabSheetColorize.Controls.Add(setDefaultColorsButton);
            tabSheetColorize.Location = new System.Drawing.Point(4, 24);
            tabSheetColorize.Name = "tabSheetColorize";
            tabSheetColorize.Size = new System.Drawing.Size(925, 325);
            tabSheetColorize.TabIndex = 3;
            tabSheetColorize.Text = "Colorize";
            tabSheetColorize.UseVisualStyleBackColor = true;
            // 
            // radioHSB
            // 
            radioHSB.AutoSize = true;
            radioHSB.Location = new System.Drawing.Point(315, 240);
            radioHSB.Name = "radioHSB";
            radioHSB.Size = new System.Drawing.Size(97, 19);
            radioHSB.TabIndex = 4;
            radioHSB.TabStop = true;
            radioHSB.Text = "Select by HSB";
            radioHSB.UseVisualStyleBackColor = true;
            // 
            // radioRGB
            // 
            radioRGB.AutoSize = true;
            radioRGB.Location = new System.Drawing.Point(196, 240);
            radioRGB.Name = "radioRGB";
            radioRGB.Size = new System.Drawing.Size(97, 19);
            radioRGB.TabIndex = 3;
            radioRGB.TabStop = true;
            radioRGB.Text = "Select by RGB";
            radioRGB.UseVisualStyleBackColor = true;
            // 
            // colorizeLayout
            // 
            colorizeLayout.AutoSize = true;
            colorizeLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            colorizeLayout.ColumnCount = 3;
            colorizeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            colorizeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            colorizeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            colorizeLayout.Controls.Add(backgroundGroup, 0, 0);
            colorizeLayout.Controls.Add(highlightGroup, 1, 0);
            colorizeLayout.Controls.Add(textColorGroup, 2, 0);
            colorizeLayout.Location = new System.Drawing.Point(8, 5);
            colorizeLayout.Name = "colorizeLayout";
            colorizeLayout.RowCount = 1;
            colorizeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            colorizeLayout.Size = new System.Drawing.Size(456, 225);
            colorizeLayout.TabIndex = 0;
            // 
            // backgroundGroup
            // 
            backgroundGroup.Controls.Add(backgroundColorFrame);
            backgroundGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            backgroundGroup.Location = new System.Drawing.Point(3, 3);
            backgroundGroup.Name = "backgroundGroup";
            backgroundGroup.Size = new System.Drawing.Size(146, 219);
            backgroundGroup.TabIndex = 0;
            backgroundGroup.TabStop = false;
            backgroundGroup.Text = "BackgroundColor";
            // 
            // backgroundColorFrame
            // 
            backgroundColorFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            backgroundColorFrame.AutoSize = true;
            backgroundColorFrame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            backgroundColorFrame.Location = new System.Drawing.Point(3, 22);
            backgroundColorFrame.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            backgroundColorFrame.Name = "backgroundColorFrame";
            backgroundColorFrame.Size = new System.Drawing.Size(140, 179);
            backgroundColorFrame.TabIndex = 0;
            // 
            // highlightGroup
            // 
            highlightGroup.Controls.Add(highlightColorFrame);
            highlightGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            highlightGroup.Location = new System.Drawing.Point(155, 3);
            highlightGroup.Name = "highlightGroup";
            highlightGroup.Size = new System.Drawing.Size(146, 219);
            highlightGroup.TabIndex = 1;
            highlightGroup.TabStop = false;
            highlightGroup.Text = "HighlightColor";
            // 
            // highlightColorFrame
            // 
            highlightColorFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            highlightColorFrame.AutoSize = true;
            highlightColorFrame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            highlightColorFrame.Location = new System.Drawing.Point(3, 22);
            highlightColorFrame.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            highlightColorFrame.Name = "highlightColorFrame";
            highlightColorFrame.Size = new System.Drawing.Size(140, 179);
            highlightColorFrame.TabIndex = 0;
            // 
            // textColorGroup
            // 
            textColorGroup.Controls.Add(textColorFrame);
            textColorGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            textColorGroup.Location = new System.Drawing.Point(307, 3);
            textColorGroup.Name = "textColorGroup";
            textColorGroup.Size = new System.Drawing.Size(146, 219);
            textColorGroup.TabIndex = 2;
            textColorGroup.TabStop = false;
            textColorGroup.Text = "TextColor";
            // 
            // textColorFrame
            // 
            textColorFrame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textColorFrame.AutoSize = true;
            textColorFrame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            textColorFrame.Location = new System.Drawing.Point(3, 22);
            textColorFrame.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textColorFrame.Name = "textColorFrame";
            textColorFrame.Size = new System.Drawing.Size(140, 179);
            textColorFrame.TabIndex = 0;
            // 
            // setColorsButton
            // 
            setColorsButton.Location = new System.Drawing.Point(8, 236);
            setColorsButton.Name = "setColorsButton";
            setColorsButton.Size = new System.Drawing.Size(88, 27);
            setColorsButton.TabIndex = 1;
            setColorsButton.Text = "Set Colors";
            setColorsButton.UseVisualStyleBackColor = true;
            // 
            // setDefaultColorsButton
            // 
            setDefaultColorsButton.Location = new System.Drawing.Point(102, 236);
            setDefaultColorsButton.Name = "setDefaultColorsButton";
            setDefaultColorsButton.Size = new System.Drawing.Size(88, 27);
            setDefaultColorsButton.TabIndex = 2;
            setDefaultColorsButton.Text = "Set Defaults";
            setDefaultColorsButton.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Controls.Add(tabSheetAppModes);
            tabControl.Controls.Add(tabSheetContextTabs);
            tabControl.Controls.Add(tabSheetContextPopups);
            tabControl.Controls.Add(tabSheetColorize);
            tabControl.Location = new System.Drawing.Point(0, 170);
            tabControl.Margin = new System.Windows.Forms.Padding(0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(933, 353);
            tabControl.TabIndex = 1;
            // 
            // PreviewForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 533);
            Controls.Add(tabControl);
            Name = "PreviewForm";
            ShowInTaskbar = false;
            Text = "Ribbon Preview";
            tabSheetAppModes.ResumeLayout(false);
            tabSheetAppModes.PerformLayout();
            tabSheetContextTabs.ResumeLayout(false);
            tabSheetContextTabs.PerformLayout();
            tabSheetContextPopups.ResumeLayout(false);
            tabSheetContextPopups.PerformLayout();
            tabSheetColorize.ResumeLayout(false);
            tabSheetColorize.PerformLayout();
            colorizeLayout.ResumeLayout(false);
            backgroundGroup.ResumeLayout(false);
            backgroundGroup.PerformLayout();
            highlightGroup.ResumeLayout(false);
            highlightGroup.PerformLayout();
            textColorGroup.ResumeLayout(false);
            textColorGroup.PerformLayout();
            tabControl.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSheetAppModes;
        private System.Windows.Forms.CheckedListBox checkedListBoxAppModes;
        private System.Windows.Forms.Label labelAppModes;
        private System.Windows.Forms.TabPage tabSheetContextTabs;
        private System.Windows.Forms.TabPage tabSheetContextPopups;
        private System.Windows.Forms.TabPage tabSheetColorize;
        private System.Windows.Forms.CheckedListBox checkedListBoxContextTabs;
        private System.Windows.Forms.Label labelContextTabs;
        private System.Windows.Forms.Label labelContextPopups;
        private System.Windows.Forms.ListBox listBoxContextPopups;
        private System.Windows.Forms.TableLayoutPanel colorizeLayout;
        private System.Windows.Forms.GroupBox backgroundGroup;
        private System.Windows.Forms.GroupBox highlightGroup;
        private System.Windows.Forms.GroupBox textColorGroup;
        private System.Windows.Forms.Button setColorsButton;
        private System.Windows.Forms.Button setDefaultColorsButton;
        private ColorFrame backgroundColorFrame;
        private ColorFrame highlightColorFrame;
        private ColorFrame textColorFrame;
        private System.Windows.Forms.RadioButton radioHSB;
        private System.Windows.Forms.RadioButton radioRGB;
    }
}

