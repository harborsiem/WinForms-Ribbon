namespace UIRibbonTools
{
    partial class ImageEditForm
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
            if (disposing)
            {
                Destroy();
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEditForm));
            PaintBox = new System.Windows.Forms.PictureBox();
            topRightLayout = new System.Windows.Forms.TableLayoutPanel();
            Label2 = new System.Windows.Forms.Label();
            EditImageFile = new System.Windows.Forms.TextBox();
            Label3 = new System.Windows.Forms.Label();
            ComboBoxMinDpi = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            EditResourceId = new System.Windows.Forms.NumericUpDown();
            label7 = new System.Windows.Forms.Label();
            EditSymbol = new System.Windows.Forms.TextBox();
            RightButton = new System.Windows.Forms.Button();
            Label1 = new System.Windows.Forms.Label();
            MemoHelp = new System.Windows.Forms.TextBox();
            buttonsPanel = new System.Windows.Forms.TableLayoutPanel();
            buttonOk = new System.Windows.Forms.Button();
            buttonCancel = new System.Windows.Forms.Button();
            dialogLayout = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)PaintBox).BeginInit();
            topRightLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)EditResourceId).BeginInit();
            buttonsPanel.SuspendLayout();
            dialogLayout.SuspendLayout();
            SuspendLayout();
            // 
            // PaintBox
            // 
            PaintBox.BackColor = System.Drawing.Color.Transparent;
            PaintBox.Location = new System.Drawing.Point(0, 0);
            PaintBox.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            PaintBox.Name = "PaintBox";
            PaintBox.Size = new System.Drawing.Size(64, 64);
            PaintBox.TabIndex = 2;
            PaintBox.TabStop = false;
            // 
            // topRightLayout
            // 
            topRightLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            topRightLayout.AutoSize = true;
            topRightLayout.ColumnCount = 3;
            topRightLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            topRightLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            topRightLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            topRightLayout.Controls.Add(Label2, 0, 0);
            topRightLayout.Controls.Add(EditImageFile, 1, 0);
            topRightLayout.Controls.Add(Label3, 0, 1);
            topRightLayout.Controls.Add(ComboBoxMinDpi, 1, 1);
            topRightLayout.Controls.Add(label6, 0, 2);
            topRightLayout.Controls.Add(EditResourceId, 1, 2);
            topRightLayout.Controls.Add(label7, 0, 3);
            topRightLayout.Controls.Add(EditSymbol, 1, 3);
            topRightLayout.Controls.Add(RightButton, 2, 0);
            topRightLayout.Location = new System.Drawing.Point(67, 0);
            topRightLayout.Margin = new System.Windows.Forms.Padding(0);
            topRightLayout.Name = "topRightLayout";
            topRightLayout.RowCount = 4;
            topRightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            topRightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            topRightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            topRightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            topRightLayout.Size = new System.Drawing.Size(420, 116);
            topRightLayout.TabIndex = 1;
            // 
            // Label2
            // 
            Label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            Label2.AutoSize = true;
            Label2.Location = new System.Drawing.Point(3, 0);
            Label2.Name = "Label2";
            Label2.Size = new System.Drawing.Size(61, 29);
            Label2.TabIndex = 0;
            Label2.Text = "Image File";
            Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditImageFile
            // 
            EditImageFile.Location = new System.Drawing.Point(159, 3);
            EditImageFile.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            EditImageFile.Name = "EditImageFile";
            EditImageFile.ReadOnly = true;
            EditImageFile.Size = new System.Drawing.Size(233, 23);
            EditImageFile.TabIndex = 1;
            // 
            // Label3
            // 
            Label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            Label3.AutoSize = true;
            Label3.Location = new System.Drawing.Point(3, 29);
            Label3.Name = "Label3";
            Label3.Size = new System.Drawing.Size(150, 29);
            Label3.TabIndex = 3;
            Label3.Text = "Minimum target resolution";
            Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBoxMinDpi
            // 
            ComboBoxMinDpi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxMinDpi.FormattingEnabled = true;
            ComboBoxMinDpi.Items.AddRange(new object[] { "auto", "96 dpi", "120 dpi", "144 dpi", "192 dpi" });
            ComboBoxMinDpi.Location = new System.Drawing.Point(159, 32);
            ComboBoxMinDpi.Name = "ComboBoxMinDpi";
            ComboBoxMinDpi.Size = new System.Drawing.Size(140, 23);
            ComboBoxMinDpi.TabIndex = 4;
            // 
            // label6
            // 
            label6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(3, 58);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(18, 29);
            label6.TabIndex = 5;
            label6.Text = "ID";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditResourceId
            // 
            EditResourceId.Location = new System.Drawing.Point(159, 61);
            EditResourceId.Maximum = new decimal(new int[] { 59999, 0, 0, 0 });
            EditResourceId.Name = "EditResourceId";
            EditResourceId.Size = new System.Drawing.Size(140, 23);
            EditResourceId.TabIndex = 6;
            // 
            // label7
            // 
            label7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(3, 87);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(47, 29);
            label7.TabIndex = 7;
            label7.Text = "Symbol";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditSymbol
            // 
            EditSymbol.Location = new System.Drawing.Point(159, 90);
            EditSymbol.Name = "EditSymbol";
            EditSymbol.Size = new System.Drawing.Size(139, 23);
            EditSymbol.TabIndex = 8;
            // 
            // RightButton
            // 
            RightButton.Location = new System.Drawing.Point(392, 3);
            RightButton.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            RightButton.Name = "RightButton";
            RightButton.Size = new System.Drawing.Size(23, 23);
            RightButton.TabIndex = 2;
            RightButton.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new System.Drawing.Point(0, 116);
            Label1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            Label1.Name = "Label1";
            Label1.Size = new System.Drawing.Size(62, 15);
            Label1.TabIndex = 2;
            Label1.Text = "Quick Tips";
            // 
            // MemoHelp
            // 
            MemoHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MemoHelp.BackColor = System.Drawing.SystemColors.Info;
            dialogLayout.SetColumnSpan(MemoHelp, 2);
            MemoHelp.Location = new System.Drawing.Point(0, 134);
            MemoHelp.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            MemoHelp.MaximumSize = new System.Drawing.Size(583, 234);
            MemoHelp.Multiline = true;
            MemoHelp.Name = "MemoHelp";
            MemoHelp.ReadOnly = true;
            MemoHelp.Size = new System.Drawing.Size(487, 234);
            MemoHelp.TabIndex = 3;
            MemoHelp.Text = resources.GetString("MemoHelp.Text");
            MemoHelp.WordWrap = false;
            // 
            // buttonsPanel
            // 
            buttonsPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonsPanel.AutoSize = true;
            buttonsPanel.ColumnCount = 2;
            buttonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            buttonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            buttonsPanel.Controls.Add(buttonOk, 0, 0);
            buttonsPanel.Controls.Add(buttonCancel, 1, 0);
            buttonsPanel.Location = new System.Drawing.Point(299, 371);
            buttonsPanel.Margin = new System.Windows.Forms.Padding(0);
            buttonsPanel.Name = "buttonsPanel";
            buttonsPanel.RowCount = 1;
            buttonsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            buttonsPanel.Size = new System.Drawing.Size(188, 33);
            buttonsPanel.TabIndex = 0;
            // 
            // buttonOk
            // 
            buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOk.Location = new System.Drawing.Point(3, 3);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(88, 27);
            buttonOk.TabIndex = 0;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(97, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(88, 27);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // dialogLayout
            // 
            dialogLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dialogLayout.AutoSize = true;
            dialogLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            dialogLayout.ColumnCount = 2;
            dialogLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            dialogLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            dialogLayout.Controls.Add(PaintBox, 0, 0);
            dialogLayout.Controls.Add(topRightLayout, 1, 0);
            dialogLayout.Controls.Add(Label1, 0, 1);
            dialogLayout.Controls.Add(MemoHelp, 0, 2);
            dialogLayout.Controls.Add(buttonsPanel, 1, 3);
            dialogLayout.Location = new System.Drawing.Point(9, 9);
            dialogLayout.Margin = new System.Windows.Forms.Padding(0);
            dialogLayout.Name = "dialogLayout";
            dialogLayout.RowCount = 4;
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.Size = new System.Drawing.Size(487, 404);
            dialogLayout.TabIndex = 0;
            // 
            // ImageEditForm
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(505, 422);
            Controls.Add(dialogLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImageEditForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit Image";
            ((System.ComponentModel.ISupportInitialize)PaintBox).EndInit();
            topRightLayout.ResumeLayout(false);
            topRightLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)EditResourceId).EndInit();
            buttonsPanel.ResumeLayout(false);
            dialogLayout.ResumeLayout(false);
            dialogLayout.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox MemoHelp;
        private System.Windows.Forms.PictureBox PaintBox;
        private System.Windows.Forms.TableLayoutPanel topRightLayout;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox EditImageFile;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.ComboBox ComboBoxMinDpi;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown EditResourceId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox EditSymbol;
        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.TableLayoutPanel buttonsPanel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel dialogLayout;
    }
}
