namespace _08_Images
{
    partial class Form1
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
            this._ribbon = new WinForms.Ribbon.RibbonStrip();
            this.SuspendLayout();
            // 
            // _ribbon
            // 
            this._ribbon.Location = new System.Drawing.Point(0, 0);
            this._ribbon.Name = "_ribbon";
            this._ribbon.MarkupResource = "_08_Images.RibbonMarkup.ribbon";
            this._ribbon.Size = new System.Drawing.Size(501, 116);
            this._ribbon.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 428);
            this.Controls.Add(this._ribbon);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private WinForms.Ribbon.RibbonStrip _ribbon;

    }
}

