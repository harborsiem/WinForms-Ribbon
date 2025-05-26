using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;

namespace UIRibbonTools
{
    partial class ColorFrame
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
            frameLayout = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            upDown_RorH = new System.Windows.Forms.NumericUpDown();
            upDown_GorS = new System.Windows.Forms.NumericUpDown();
            upDown_BorB = new System.Windows.Forms.NumericUpDown();
            colorPanel = new System.Windows.Forms.Panel();
            setColorButton = new System.Windows.Forms.Button();
            hsbOrColorText = new System.Windows.Forms.TextBox();
            frameLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)upDown_RorH).BeginInit();
            ((System.ComponentModel.ISupportInitialize)upDown_GorS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)upDown_BorB).BeginInit();
            SuspendLayout();
            // 
            // frameLayout
            // 
            frameLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            frameLayout.AutoSize = true;
            frameLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            frameLayout.ColumnCount = 2;
            frameLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            frameLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            frameLayout.Controls.Add(label1, 0, 0);
            frameLayout.Controls.Add(label2, 0, 1);
            frameLayout.Controls.Add(label3, 0, 2);
            frameLayout.Controls.Add(upDown_RorH, 1, 0);
            frameLayout.Controls.Add(upDown_GorS, 1, 1);
            frameLayout.Controls.Add(upDown_BorB, 1, 2);
            frameLayout.Controls.Add(colorPanel, 0, 3);
            frameLayout.Controls.Add(setColorButton, 0, 4);
            frameLayout.Controls.Add(hsbOrColorText, 0, 5);
            frameLayout.Location = new System.Drawing.Point(0, 0);
            frameLayout.Margin = new System.Windows.Forms.Padding(0);
            frameLayout.Name = "frameLayout";
            frameLayout.RowCount = 6;
            frameLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            frameLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            frameLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            frameLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            frameLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            frameLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            frameLayout.Size = new System.Drawing.Size(140, 179);
            frameLayout.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(27, 29);
            label1.TabIndex = 0;
            label1.Text = "Red";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 29);
            label2.TabIndex = 2;
            label2.Text = "Green";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 58);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(30, 29);
            label3.TabIndex = 4;
            label3.Text = "Blue";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // upDown_RorH
            // 
            upDown_RorH.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            upDown_RorH.Location = new System.Drawing.Point(59, 3);
            upDown_RorH.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            upDown_RorH.Name = "upDown_RorH";
            upDown_RorH.Size = new System.Drawing.Size(78, 23);
            upDown_RorH.TabIndex = 1;
            // 
            // upDown_GorS
            // 
            upDown_GorS.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            upDown_GorS.Location = new System.Drawing.Point(59, 32);
            upDown_GorS.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            upDown_GorS.Name = "upDown_GorS";
            upDown_GorS.Size = new System.Drawing.Size(78, 23);
            upDown_GorS.TabIndex = 3;
            // 
            // upDown_BorB
            // 
            upDown_BorB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            upDown_BorB.Location = new System.Drawing.Point(59, 61);
            upDown_BorB.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            upDown_BorB.Name = "upDown_BorB";
            upDown_BorB.Size = new System.Drawing.Size(78, 23);
            upDown_BorB.TabIndex = 5;
            // 
            // colorPanel
            // 
            frameLayout.SetColumnSpan(colorPanel, 2);
            colorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            colorPanel.Location = new System.Drawing.Point(3, 90);
            colorPanel.Name = "colorPanel";
            colorPanel.Size = new System.Drawing.Size(134, 22);
            colorPanel.TabIndex = 6;
            // 
            // setColorButton
            // 
            setColorButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            setColorButton.AutoSize = true;
            frameLayout.SetColumnSpan(setColorButton, 2);
            setColorButton.Location = new System.Drawing.Point(3, 118);
            setColorButton.Name = "setColorButton";
            setColorButton.Size = new System.Drawing.Size(134, 29);
            setColorButton.TabIndex = 7;
            setColorButton.Text = "Set Color";
            setColorButton.UseVisualStyleBackColor = true;
            // 
            // hsbOrColorText
            // 
            hsbOrColorText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            frameLayout.SetColumnSpan(hsbOrColorText, 2);
            hsbOrColorText.Location = new System.Drawing.Point(3, 153);
            hsbOrColorText.Name = "hsbOrColorText";
            hsbOrColorText.ReadOnly = true;
            hsbOrColorText.Size = new System.Drawing.Size(134, 23);
            hsbOrColorText.TabIndex = 8;
            // 
            // ColorFrame
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(frameLayout);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "ColorFrame";
            Size = new System.Drawing.Size(140, 179);
            frameLayout.ResumeLayout(false);
            frameLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)upDown_RorH).EndInit();
            ((System.ComponentModel.ISupportInitialize)upDown_GorS).EndInit();
            ((System.ComponentModel.ISupportInitialize)upDown_BorB).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel frameLayout;
        private System.Windows.Forms.NumericUpDown upDown_RorH;
        private System.Windows.Forms.NumericUpDown upDown_BorB;
        private System.Windows.Forms.NumericUpDown upDown_GorS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel colorPanel;
        private System.Windows.Forms.Button setColorButton;
        private System.Windows.Forms.TextBox hsbOrColorText;
    }
}
