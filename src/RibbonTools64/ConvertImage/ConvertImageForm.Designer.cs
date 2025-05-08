namespace UIRibbonTools
{
    partial class ConvertImageForm
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
            inSelectorLayout = new System.Windows.Forms.TableLayoutPanel();
            inSelectorLabel = new System.Windows.Forms.Label();
            inSelectorCombo = new System.Windows.Forms.ComboBox();
            inIgnoreAlphaCheck = new System.Windows.Forms.CheckBox();
            inFilesLabel = new System.Windows.Forms.Label();
            inSelectButton = new System.Windows.Forms.Button();
            inPathLabel = new System.Windows.Forms.Label();
            inPathTextBox = new System.Windows.Forms.TextBox();
            inSelectorGroup = new System.Windows.Forms.GroupBox();
            outSelectorLayout = new System.Windows.Forms.TableLayoutPanel();
            outSelectorLabel = new System.Windows.Forms.Label();
            outCombo = new System.Windows.Forms.ComboBox();
            outIgnoreAlphaCheck = new System.Windows.Forms.CheckBox();
            outFolderLabel = new System.Windows.Forms.Label();
            outSelectButton = new System.Windows.Forms.Button();
            outPathLabel = new System.Windows.Forms.Label();
            outPathTextBox = new System.Windows.Forms.TextBox();
            outSelectorGroup = new System.Windows.Forms.GroupBox();
            convertButton = new System.Windows.Forms.Button();
            dialogLayout = new System.Windows.Forms.TableLayoutPanel();
            inSelectorLayout.SuspendLayout();
            inSelectorGroup.SuspendLayout();
            outSelectorLayout.SuspendLayout();
            outSelectorGroup.SuspendLayout();
            dialogLayout.SuspendLayout();
            SuspendLayout();
            // 
            // inSelectorLayout
            // 
            inSelectorLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            inSelectorLayout.AutoSize = true;
            inSelectorLayout.ColumnCount = 2;
            inSelectorLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            inSelectorLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            inSelectorLayout.Controls.Add(inSelectorLabel, 0, 0);
            inSelectorLayout.Controls.Add(inSelectorCombo, 1, 0);
            inSelectorLayout.Controls.Add(inIgnoreAlphaCheck, 1, 1);
            inSelectorLayout.Controls.Add(inFilesLabel, 0, 2);
            inSelectorLayout.Controls.Add(inSelectButton, 1, 2);
            inSelectorLayout.Controls.Add(inPathLabel, 0, 3);
            inSelectorLayout.Controls.Add(inPathTextBox, 1, 3);
            inSelectorLayout.Location = new System.Drawing.Point(3, 18);
            inSelectorLayout.Margin = new System.Windows.Forms.Padding(0);
            inSelectorLayout.Name = "inSelectorLayout";
            inSelectorLayout.RowCount = 4;
            inSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            inSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            inSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            inSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            inSelectorLayout.Size = new System.Drawing.Size(349, 116);
            inSelectorLayout.TabIndex = 0;
            // 
            // inSelectorLabel
            // 
            inSelectorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            inSelectorLabel.AutoSize = true;
            inSelectorLabel.Location = new System.Drawing.Point(3, 0);
            inSelectorLabel.Name = "inSelectorLabel";
            inSelectorLabel.Size = new System.Drawing.Size(40, 29);
            inSelectorLabel.TabIndex = 0;
            inSelectorLabel.Text = "Inputs";
            inSelectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inSelectorCombo
            // 
            inSelectorCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            inSelectorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            inSelectorCombo.FormattingEnabled = true;
            inSelectorCombo.Items.AddRange(new object[] { "Show Bitmap Infos (*.bmp, *.png, *.ico)", "Bitmap (*.bmp, *.png, ...)", "Icon", "Standard-Icons.com Gif", "VS-ImageLib XAML", "Svg files (*.svg)" });
            inSelectorCombo.Location = new System.Drawing.Point(49, 3);
            inSelectorCombo.Name = "inSelectorCombo";
            inSelectorCombo.Size = new System.Drawing.Size(297, 23);
            inSelectorCombo.TabIndex = 1;
            // 
            // inIgnoreAlphaCheck
            // 
            inIgnoreAlphaCheck.AutoSize = true;
            inIgnoreAlphaCheck.Location = new System.Drawing.Point(49, 32);
            inIgnoreAlphaCheck.Name = "inIgnoreAlphaCheck";
            inIgnoreAlphaCheck.Size = new System.Drawing.Size(141, 19);
            inIgnoreAlphaCheck.TabIndex = 3;
            inIgnoreAlphaCheck.Text = "Ignore Alpha Channel";
            inIgnoreAlphaCheck.UseVisualStyleBackColor = true;
            // 
            // inFilesLabel
            // 
            inFilesLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            inFilesLabel.AutoSize = true;
            inFilesLabel.Location = new System.Drawing.Point(3, 54);
            inFilesLabel.Name = "inFilesLabel";
            inFilesLabel.Size = new System.Drawing.Size(30, 33);
            inFilesLabel.TabIndex = 2;
            inFilesLabel.Text = "Files";
            inFilesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inSelectButton
            // 
            inSelectButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            inSelectButton.Location = new System.Drawing.Point(49, 57);
            inSelectButton.Name = "inSelectButton";
            inSelectButton.Size = new System.Drawing.Size(297, 27);
            inSelectButton.TabIndex = 4;
            inSelectButton.Text = "Select";
            inSelectButton.UseVisualStyleBackColor = true;
            // 
            // inPathLabel
            // 
            inPathLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            inPathLabel.AutoSize = true;
            inPathLabel.Location = new System.Drawing.Point(3, 87);
            inPathLabel.Name = "inPathLabel";
            inPathLabel.Size = new System.Drawing.Size(31, 29);
            inPathLabel.TabIndex = 7;
            inPathLabel.Text = "Path";
            inPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inPathTextBox
            // 
            inPathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            inPathTextBox.Location = new System.Drawing.Point(49, 90);
            inPathTextBox.Name = "inPathTextBox";
            inPathTextBox.ReadOnly = true;
            inPathTextBox.Size = new System.Drawing.Size(297, 23);
            inPathTextBox.TabIndex = 8;
            // 
            // inSelectorGroup
            // 
            inSelectorGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            inSelectorGroup.AutoSize = true;
            inSelectorGroup.Controls.Add(inSelectorLayout);
            inSelectorGroup.Location = new System.Drawing.Point(3, 3);
            inSelectorGroup.Name = "inSelectorGroup";
            inSelectorGroup.Size = new System.Drawing.Size(355, 153);
            inSelectorGroup.TabIndex = 0;
            inSelectorGroup.TabStop = false;
            inSelectorGroup.Text = "Input Selector";
            // 
            // outSelectorLayout
            // 
            outSelectorLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            outSelectorLayout.AutoSize = true;
            outSelectorLayout.ColumnCount = 2;
            outSelectorLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            outSelectorLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            outSelectorLayout.Controls.Add(outSelectorLabel, 0, 0);
            outSelectorLayout.Controls.Add(outCombo, 1, 0);
            outSelectorLayout.Controls.Add(outIgnoreAlphaCheck, 1, 1);
            outSelectorLayout.Controls.Add(outFolderLabel, 0, 2);
            outSelectorLayout.Controls.Add(outSelectButton, 1, 2);
            outSelectorLayout.Controls.Add(outPathLabel, 0, 3);
            outSelectorLayout.Controls.Add(outPathTextBox, 1, 3);
            outSelectorLayout.Location = new System.Drawing.Point(3, 18);
            outSelectorLayout.Margin = new System.Windows.Forms.Padding(0);
            outSelectorLayout.Name = "outSelectorLayout";
            outSelectorLayout.RowCount = 4;
            outSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            outSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            outSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            outSelectorLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            outSelectorLayout.Size = new System.Drawing.Size(350, 116);
            outSelectorLayout.TabIndex = 0;
            // 
            // outSelectorLabel
            // 
            outSelectorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            outSelectorLabel.AutoSize = true;
            outSelectorLabel.Location = new System.Drawing.Point(3, 0);
            outSelectorLabel.Name = "outSelectorLabel";
            outSelectorLabel.Size = new System.Drawing.Size(50, 29);
            outSelectorLabel.TabIndex = 0;
            outSelectorLabel.Text = "Outputs";
            outSelectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // outCombo
            // 
            outCombo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            outCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            outCombo.FormattingEnabled = true;
            outCombo.Items.AddRange(new object[] { "Bitmap V5", "Bitmap", "Png" });
            outCombo.Location = new System.Drawing.Point(59, 3);
            outCombo.Name = "outCombo";
            outCombo.Size = new System.Drawing.Size(288, 23);
            outCombo.TabIndex = 1;
            // 
            // outIgnoreAlphaCheck
            // 
            outIgnoreAlphaCheck.AutoSize = true;
            outIgnoreAlphaCheck.Location = new System.Drawing.Point(59, 32);
            outIgnoreAlphaCheck.Name = "outIgnoreAlphaCheck";
            outIgnoreAlphaCheck.Size = new System.Drawing.Size(141, 19);
            outIgnoreAlphaCheck.TabIndex = 3;
            outIgnoreAlphaCheck.Text = "Ignore Alpha Channel";
            outIgnoreAlphaCheck.UseVisualStyleBackColor = true;
            // 
            // outFolderLabel
            // 
            outFolderLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            outFolderLabel.AutoSize = true;
            outFolderLabel.Location = new System.Drawing.Point(3, 54);
            outFolderLabel.Name = "outFolderLabel";
            outFolderLabel.Size = new System.Drawing.Size(40, 33);
            outFolderLabel.TabIndex = 2;
            outFolderLabel.Text = "Folder";
            outFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // outSelectButton
            // 
            outSelectButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            outSelectButton.Location = new System.Drawing.Point(59, 57);
            outSelectButton.Name = "outSelectButton";
            outSelectButton.Size = new System.Drawing.Size(288, 27);
            outSelectButton.TabIndex = 5;
            outSelectButton.Text = "Select";
            outSelectButton.UseVisualStyleBackColor = true;
            // 
            // outPathLabel
            // 
            outPathLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            outPathLabel.AutoSize = true;
            outPathLabel.Location = new System.Drawing.Point(3, 87);
            outPathLabel.Name = "outPathLabel";
            outPathLabel.Size = new System.Drawing.Size(31, 29);
            outPathLabel.TabIndex = 6;
            outPathLabel.Text = "Path";
            outPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // outPathTextBox
            // 
            outPathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            outPathTextBox.Location = new System.Drawing.Point(59, 90);
            outPathTextBox.Name = "outPathTextBox";
            outPathTextBox.ReadOnly = true;
            outPathTextBox.Size = new System.Drawing.Size(288, 23);
            outPathTextBox.TabIndex = 7;
            // 
            // outSelectorGroup
            // 
            outSelectorGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            outSelectorGroup.AutoSize = true;
            outSelectorGroup.Controls.Add(outSelectorLayout);
            outSelectorGroup.Location = new System.Drawing.Point(364, 3);
            outSelectorGroup.Name = "outSelectorGroup";
            outSelectorGroup.Size = new System.Drawing.Size(355, 153);
            outSelectorGroup.TabIndex = 1;
            outSelectorGroup.TabStop = false;
            outSelectorGroup.Text = "Output Selector";
            // 
            // convertButton
            // 
            convertButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            convertButton.Location = new System.Drawing.Point(631, 162);
            convertButton.Name = "convertButton";
            convertButton.Size = new System.Drawing.Size(88, 27);
            convertButton.TabIndex = 2;
            convertButton.Text = "Convert";
            convertButton.UseVisualStyleBackColor = true;
            // 
            // dialogLayout
            // 
            dialogLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dialogLayout.AutoSize = true;
            dialogLayout.ColumnCount = 2;
            dialogLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            dialogLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            dialogLayout.Controls.Add(inSelectorGroup, 0, 0);
            dialogLayout.Controls.Add(outSelectorGroup, 1, 0);
            dialogLayout.Controls.Add(convertButton, 1, 1);
            dialogLayout.Location = new System.Drawing.Point(9, 9);
            dialogLayout.Margin = new System.Windows.Forms.Padding(0);
            dialogLayout.Name = "dialogLayout";
            dialogLayout.RowCount = 2;
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.Size = new System.Drawing.Size(722, 192);
            dialogLayout.TabIndex = 2;
            // 
            // ConvertImageForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(740, 210);
            Controls.Add(dialogLayout);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(2387, 249);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(756, 249);
            Name = "ConvertImageForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Image Converter";
            inSelectorLayout.ResumeLayout(false);
            inSelectorLayout.PerformLayout();
            inSelectorGroup.ResumeLayout(false);
            inSelectorGroup.PerformLayout();
            outSelectorLayout.ResumeLayout(false);
            outSelectorLayout.PerformLayout();
            outSelectorGroup.ResumeLayout(false);
            outSelectorGroup.PerformLayout();
            dialogLayout.ResumeLayout(false);
            dialogLayout.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox inSelectorGroup;
        private System.Windows.Forms.TableLayoutPanel inSelectorLayout;
        private System.Windows.Forms.Label inSelectorLabel;
        private System.Windows.Forms.ComboBox inSelectorCombo;
        private System.Windows.Forms.Label inFilesLabel;
        private System.Windows.Forms.CheckBox inIgnoreAlphaCheck;
        private System.Windows.Forms.GroupBox outSelectorGroup;
        private System.Windows.Forms.TableLayoutPanel outSelectorLayout;
        private System.Windows.Forms.Label outSelectorLabel;
        private System.Windows.Forms.ComboBox outCombo;
        private System.Windows.Forms.Label outFolderLabel;
        private System.Windows.Forms.CheckBox outIgnoreAlphaCheck;
        private System.Windows.Forms.TableLayoutPanel dialogLayout;
        private System.Windows.Forms.Button inSelectButton;
        private System.Windows.Forms.Button outSelectButton;
        private System.Windows.Forms.TextBox inPathTextBox;
        private System.Windows.Forms.Label inPathLabel;
        private System.Windows.Forms.Label outPathLabel;
        private System.Windows.Forms.TextBox outPathTextBox;
        private System.Windows.Forms.Button convertButton;
    }
}

