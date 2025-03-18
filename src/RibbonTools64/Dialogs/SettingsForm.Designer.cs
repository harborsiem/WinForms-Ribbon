namespace UIRibbonTools
{
    partial class SettingsForm
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
            pathGroupLayout = new System.Windows.Forms.TableLayoutPanel();
            ribbonCompilerLabel = new System.Windows.Forms.Label();
            ribbonCompilerText = new System.Windows.Forms.TextBox();
            compilerButton = new System.Windows.Forms.Button();
            resourceCompilerLabel = new System.Windows.Forms.Label();
            resourceCompilerText = new System.Windows.Forms.TextBox();
            resourceButton = new System.Windows.Forms.Button();
            linkerLabel = new System.Windows.Forms.Label();
            linkerText = new System.Windows.Forms.TextBox();
            linkerButton = new System.Windows.Forms.Button();
            pathGroup = new System.Windows.Forms.GroupBox();
            wrapperGroupLayout = new System.Windows.Forms.TableLayoutPanel();
            cSharpCheck = new System.Windows.Forms.CheckBox();
            vbCheck = new System.Windows.Forms.CheckBox();
            advancedWrapperClassFile = new System.Windows.Forms.CheckBox();
            wrapperGroup = new System.Windows.Forms.GroupBox();
            extrasGroupLayout = new System.Windows.Forms.TableLayoutPanel();
            autoUpdateToolsPath = new System.Windows.Forms.CheckBox();
            deleteResFile = new System.Windows.Forms.CheckBox();
            allowPngImages = new System.Windows.Forms.CheckBox();
            allowChangingResourceName = new System.Windows.Forms.CheckBox();
            ribbonFrameworkCode = new System.Windows.Forms.CheckBox();
            sizeButton = new System.Windows.Forms.Button();
            extrasGroup = new System.Windows.Forms.GroupBox();
            buttons = new System.Windows.Forms.TableLayoutPanel();
            buttonCancel = new System.Windows.Forms.Button();
            buttonOK = new System.Windows.Forms.Button();
            dialogLayout = new System.Windows.Forms.TableLayoutPanel();
            pathGroupLayout.SuspendLayout();
            pathGroup.SuspendLayout();
            wrapperGroupLayout.SuspendLayout();
            wrapperGroup.SuspendLayout();
            extrasGroupLayout.SuspendLayout();
            extrasGroup.SuspendLayout();
            buttons.SuspendLayout();
            dialogLayout.SuspendLayout();
            SuspendLayout();
            // 
            // pathGroupLayout
            // 
            pathGroupLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pathGroupLayout.AutoSize = true;
            pathGroupLayout.ColumnCount = 3;
            pathGroupLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            pathGroupLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pathGroupLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            pathGroupLayout.Controls.Add(ribbonCompilerLabel, 0, 0);
            pathGroupLayout.Controls.Add(ribbonCompilerText, 1, 0);
            pathGroupLayout.Controls.Add(compilerButton, 2, 0);
            pathGroupLayout.Controls.Add(resourceCompilerLabel, 0, 1);
            pathGroupLayout.Controls.Add(resourceCompilerText, 1, 1);
            pathGroupLayout.Controls.Add(resourceButton, 2, 1);
            pathGroupLayout.Controls.Add(linkerLabel, 0, 2);
            pathGroupLayout.Controls.Add(linkerText, 1, 2);
            pathGroupLayout.Controls.Add(linkerButton, 2, 2);
            pathGroupLayout.Location = new System.Drawing.Point(3, 22);
            pathGroupLayout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            pathGroupLayout.Name = "pathGroupLayout";
            pathGroupLayout.RowCount = 3;
            pathGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            pathGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            pathGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            pathGroupLayout.Size = new System.Drawing.Size(799, 87);
            pathGroupLayout.TabIndex = 0;
            // 
            // ribbonCompilerLabel
            // 
            ribbonCompilerLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ribbonCompilerLabel.AutoSize = true;
            ribbonCompilerLabel.Location = new System.Drawing.Point(3, 0);
            ribbonCompilerLabel.Name = "ribbonCompilerLabel";
            ribbonCompilerLabel.Size = new System.Drawing.Size(97, 29);
            ribbonCompilerLabel.TabIndex = 0;
            ribbonCompilerLabel.Text = "Ribbon Compiler";
            ribbonCompilerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ribbonCompilerText
            // 
            ribbonCompilerText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ribbonCompilerText.Location = new System.Drawing.Point(116, 3);
            ribbonCompilerText.Name = "ribbonCompilerText";
            ribbonCompilerText.ReadOnly = true;
            ribbonCompilerText.Size = new System.Drawing.Size(652, 23);
            ribbonCompilerText.TabIndex = 1;
            // 
            // compilerButton
            // 
            compilerButton.Location = new System.Drawing.Point(771, 0);
            compilerButton.Margin = new System.Windows.Forms.Padding(0);
            compilerButton.Name = "compilerButton";
            compilerButton.Size = new System.Drawing.Size(28, 28);
            compilerButton.TabIndex = 2;
            compilerButton.UseVisualStyleBackColor = true;
            // 
            // resourceCompilerLabel
            // 
            resourceCompilerLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            resourceCompilerLabel.AutoSize = true;
            resourceCompilerLabel.Location = new System.Drawing.Point(3, 29);
            resourceCompilerLabel.Name = "resourceCompilerLabel";
            resourceCompilerLabel.Size = new System.Drawing.Size(107, 29);
            resourceCompilerLabel.TabIndex = 3;
            resourceCompilerLabel.Text = "Resource Compiler";
            resourceCompilerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // resourceCompilerText
            // 
            resourceCompilerText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            resourceCompilerText.Location = new System.Drawing.Point(116, 32);
            resourceCompilerText.Name = "resourceCompilerText";
            resourceCompilerText.ReadOnly = true;
            resourceCompilerText.Size = new System.Drawing.Size(652, 23);
            resourceCompilerText.TabIndex = 4;
            // 
            // resourceButton
            // 
            resourceButton.Location = new System.Drawing.Point(771, 29);
            resourceButton.Margin = new System.Windows.Forms.Padding(0);
            resourceButton.Name = "resourceButton";
            resourceButton.Size = new System.Drawing.Size(28, 28);
            resourceButton.TabIndex = 5;
            resourceButton.UseVisualStyleBackColor = true;
            // 
            // linkerLabel
            // 
            linkerLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            linkerLabel.AutoSize = true;
            linkerLabel.Location = new System.Drawing.Point(3, 58);
            linkerLabel.Name = "linkerLabel";
            linkerLabel.Size = new System.Drawing.Size(50, 29);
            linkerLabel.TabIndex = 6;
            linkerLabel.Text = "Link.exe";
            linkerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkerText
            // 
            linkerText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            linkerText.Location = new System.Drawing.Point(116, 61);
            linkerText.Name = "linkerText";
            linkerText.ReadOnly = true;
            linkerText.Size = new System.Drawing.Size(652, 23);
            linkerText.TabIndex = 7;
            // 
            // linkerButton
            // 
            linkerButton.Location = new System.Drawing.Point(771, 58);
            linkerButton.Margin = new System.Windows.Forms.Padding(0);
            linkerButton.Name = "linkerButton";
            linkerButton.Size = new System.Drawing.Size(28, 28);
            linkerButton.TabIndex = 8;
            linkerButton.UseVisualStyleBackColor = true;
            // 
            // pathGroup
            // 
            pathGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pathGroup.AutoSize = true;
            pathGroup.Controls.Add(pathGroupLayout);
            pathGroup.Location = new System.Drawing.Point(3, 3);
            pathGroup.Name = "pathGroup";
            pathGroup.Size = new System.Drawing.Size(804, 128);
            pathGroup.TabIndex = 1;
            pathGroup.TabStop = false;
            pathGroup.Text = "Path for Tools";
            // 
            // wrapperGroupLayout
            // 
            wrapperGroupLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            wrapperGroupLayout.AutoSize = true;
            wrapperGroupLayout.ColumnCount = 2;
            wrapperGroupLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            wrapperGroupLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            wrapperGroupLayout.Controls.Add(cSharpCheck, 0, 0);
            wrapperGroupLayout.Controls.Add(vbCheck, 0, 1);
            wrapperGroupLayout.Controls.Add(advancedWrapperClassFile, 1, 0);
            wrapperGroupLayout.Location = new System.Drawing.Point(3, 22);
            wrapperGroupLayout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            wrapperGroupLayout.Name = "wrapperGroupLayout";
            wrapperGroupLayout.RowCount = 2;
            wrapperGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            wrapperGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            wrapperGroupLayout.Size = new System.Drawing.Size(799, 50);
            wrapperGroupLayout.TabIndex = 0;
            // 
            // cSharpCheck
            // 
            cSharpCheck.AutoSize = true;
            cSharpCheck.Location = new System.Drawing.Point(3, 3);
            cSharpCheck.Name = "cSharpCheck";
            cSharpCheck.Size = new System.Drawing.Size(89, 19);
            cSharpCheck.TabIndex = 0;
            cSharpCheck.Text = "C# Wrapper";
            cSharpCheck.UseVisualStyleBackColor = true;
            // 
            // vbCheck
            // 
            vbCheck.AutoSize = true;
            vbCheck.Location = new System.Drawing.Point(3, 28);
            vbCheck.Name = "vbCheck";
            vbCheck.Size = new System.Drawing.Size(135, 19);
            vbCheck.TabIndex = 1;
            vbCheck.Text = "Visual Basic Wrapper";
            vbCheck.UseVisualStyleBackColor = true;
            // 
            // advancedWrapperClassFile
            // 
            advancedWrapperClassFile.AutoSize = true;
            advancedWrapperClassFile.Location = new System.Drawing.Point(402, 3);
            advancedWrapperClassFile.Name = "advancedWrapperClassFile";
            advancedWrapperClassFile.Size = new System.Drawing.Size(327, 19);
            advancedWrapperClassFile.TabIndex = 2;
            advancedWrapperClassFile.Text = "Wrapper class name like Markup file instead RibbonItems";
            advancedWrapperClassFile.UseVisualStyleBackColor = true;
            // 
            // wrapperGroup
            // 
            wrapperGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            wrapperGroup.AutoSize = true;
            wrapperGroup.Controls.Add(wrapperGroupLayout);
            wrapperGroup.Location = new System.Drawing.Point(3, 137);
            wrapperGroup.Name = "wrapperGroup";
            wrapperGroup.Size = new System.Drawing.Size(804, 91);
            wrapperGroup.TabIndex = 2;
            wrapperGroup.TabStop = false;
            wrapperGroup.Text = "Build Code Wrapper";
            // 
            // extrasGroupLayout
            // 
            extrasGroupLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            extrasGroupLayout.AutoSize = true;
            extrasGroupLayout.ColumnCount = 1;
            extrasGroupLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            extrasGroupLayout.Controls.Add(autoUpdateToolsPath, 0, 0);
            extrasGroupLayout.Controls.Add(deleteResFile, 0, 1);
            extrasGroupLayout.Controls.Add(allowPngImages, 0, 2);
            extrasGroupLayout.Controls.Add(allowChangingResourceName, 0, 3);
            extrasGroupLayout.Controls.Add(ribbonFrameworkCode, 0, 4);
            extrasGroupLayout.Controls.Add(sizeButton, 0, 5);
            extrasGroupLayout.Location = new System.Drawing.Point(3, 22);
            extrasGroupLayout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            extrasGroupLayout.Name = "extrasGroupLayout";
            extrasGroupLayout.RowCount = 6;
            extrasGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            extrasGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            extrasGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            extrasGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            extrasGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            extrasGroupLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            extrasGroupLayout.Size = new System.Drawing.Size(799, 158);
            extrasGroupLayout.TabIndex = 0;
            // 
            // autoUpdateToolsPath
            // 
            autoUpdateToolsPath.AutoSize = true;
            autoUpdateToolsPath.Location = new System.Drawing.Point(3, 3);
            autoUpdateToolsPath.Name = "autoUpdateToolsPath";
            autoUpdateToolsPath.Size = new System.Drawing.Size(149, 19);
            autoUpdateToolsPath.TabIndex = 0;
            autoUpdateToolsPath.Text = "Auto update Tools Path";
            autoUpdateToolsPath.UseVisualStyleBackColor = true;
            // 
            // deleteResFile
            // 
            deleteResFile.AutoSize = true;
            deleteResFile.Location = new System.Drawing.Point(3, 28);
            deleteResFile.Name = "deleteResFile";
            deleteResFile.Size = new System.Drawing.Size(133, 19);
            deleteResFile.TabIndex = 1;
            deleteResFile.Text = "Delete *.rc, *.res files";
            deleteResFile.UseVisualStyleBackColor = true;
            // 
            // allowPngImages
            // 
            allowPngImages.AutoSize = true;
            allowPngImages.Location = new System.Drawing.Point(3, 53);
            allowPngImages.Name = "allowPngImages";
            allowPngImages.Size = new System.Drawing.Size(129, 19);
            allowPngImages.TabIndex = 2;
            allowPngImages.Text = "Allow *.png Images";
            allowPngImages.UseVisualStyleBackColor = true;
            // 
            // allowChangingResourceName
            // 
            allowChangingResourceName.AutoSize = true;
            allowChangingResourceName.Location = new System.Drawing.Point(3, 78);
            allowChangingResourceName.Name = "allowChangingResourceName";
            allowChangingResourceName.Size = new System.Drawing.Size(298, 19);
            allowChangingResourceName.TabIndex = 3;
            allowChangingResourceName.Text = "Allow changing ResourceName (ResourceIdentifier)";
            allowChangingResourceName.UseVisualStyleBackColor = true;
            // 
            // ribbonFrameworkCode
            // 
            ribbonFrameworkCode.AutoSize = true;
            ribbonFrameworkCode.Location = new System.Drawing.Point(3, 103);
            ribbonFrameworkCode.Name = "ribbonFrameworkCode";
            ribbonFrameworkCode.Size = new System.Drawing.Size(232, 19);
            ribbonFrameworkCode.TabIndex = 4;
            ribbonFrameworkCode.Text = "Code generation for RibbonFramework";
            ribbonFrameworkCode.UseVisualStyleBackColor = true;
            // 
            // sizeButton
            // 
            sizeButton.AutoSize = true;
            sizeButton.Location = new System.Drawing.Point(3, 128);
            sizeButton.Name = "sizeButton";
            sizeButton.Size = new System.Drawing.Size(247, 27);
            sizeButton.TabIndex = 5;
            sizeButton.Text = "Set current application size as default";
            sizeButton.UseVisualStyleBackColor = true;
            // 
            // extrasGroup
            // 
            extrasGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            extrasGroup.AutoSize = true;
            extrasGroup.Controls.Add(extrasGroupLayout);
            extrasGroup.Location = new System.Drawing.Point(3, 234);
            extrasGroup.Name = "extrasGroup";
            extrasGroup.Size = new System.Drawing.Size(804, 199);
            extrasGroup.TabIndex = 3;
            extrasGroup.TabStop = false;
            extrasGroup.Text = "Extras";
            // 
            // buttons
            // 
            buttons.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttons.AutoSize = true;
            buttons.ColumnCount = 2;
            buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            buttons.Controls.Add(buttonCancel, 1, 0);
            buttons.Controls.Add(buttonOK, 0, 0);
            buttons.Location = new System.Drawing.Point(619, 439);
            buttons.Name = "buttons";
            buttons.RowCount = 1;
            buttons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            buttons.Size = new System.Drawing.Size(188, 33);
            buttons.TabIndex = 0;
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
            // buttonOK
            // 
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Location = new System.Drawing.Point(3, 3);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(88, 27);
            buttonOK.TabIndex = 0;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            // 
            // dialogLayout
            // 
            dialogLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dialogLayout.AutoSize = true;
            dialogLayout.ColumnCount = 1;
            dialogLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            dialogLayout.Controls.Add(pathGroup, 0, 0);
            dialogLayout.Controls.Add(wrapperGroup, 0, 1);
            dialogLayout.Controls.Add(extrasGroup, 0, 2);
            dialogLayout.Controls.Add(buttons, 0, 3);
            dialogLayout.Location = new System.Drawing.Point(12, 12);
            dialogLayout.Name = "dialogLayout";
            dialogLayout.RowCount = 4;
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            dialogLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            dialogLayout.Size = new System.Drawing.Size(810, 475);
            dialogLayout.TabIndex = 0;
            // 
            // SettingsForm
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(834, 499);
            Controls.Add(dialogLayout);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(1192, 538);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(850, 538);
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            pathGroupLayout.ResumeLayout(false);
            pathGroupLayout.PerformLayout();
            pathGroup.ResumeLayout(false);
            pathGroup.PerformLayout();
            wrapperGroupLayout.ResumeLayout(false);
            wrapperGroupLayout.PerformLayout();
            wrapperGroup.ResumeLayout(false);
            wrapperGroup.PerformLayout();
            extrasGroupLayout.ResumeLayout(false);
            extrasGroupLayout.PerformLayout();
            extrasGroup.ResumeLayout(false);
            extrasGroup.PerformLayout();
            buttons.ResumeLayout(false);
            dialogLayout.ResumeLayout(false);
            dialogLayout.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel dialogLayout;
        private System.Windows.Forms.GroupBox extrasGroup;
        private System.Windows.Forms.TableLayoutPanel extrasGroupLayout;
        private System.Windows.Forms.CheckBox autoUpdateToolsPath;
        private System.Windows.Forms.CheckBox deleteResFile;
        private System.Windows.Forms.GroupBox wrapperGroup;
        private System.Windows.Forms.TableLayoutPanel wrapperGroupLayout;
        private System.Windows.Forms.CheckBox cSharpCheck;
        private System.Windows.Forms.CheckBox vbCheck;
        private System.Windows.Forms.TableLayoutPanel buttons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox pathGroup;
        private System.Windows.Forms.TableLayoutPanel pathGroupLayout;
        private System.Windows.Forms.Label ribbonCompilerLabel;
        private System.Windows.Forms.Label resourceCompilerLabel;
        private System.Windows.Forms.Label linkerLabel;
        private System.Windows.Forms.TextBox ribbonCompilerText;
        private System.Windows.Forms.TextBox resourceCompilerText;
        private System.Windows.Forms.TextBox linkerText;
        private System.Windows.Forms.CheckBox allowChangingResourceName;
        private System.Windows.Forms.CheckBox allowPngImages;
        private System.Windows.Forms.CheckBox ribbonFrameworkCode;
        private System.Windows.Forms.Button sizeButton;
        private System.Windows.Forms.Button linkerButton;
        private System.Windows.Forms.Button resourceButton;
        private System.Windows.Forms.Button compilerButton;
        private System.Windows.Forms.CheckBox advancedWrapperClassFile;
    }
}
