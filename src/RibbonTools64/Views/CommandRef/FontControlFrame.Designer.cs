namespace UIRibbonTools
{
    partial class FontControlFrame
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
            this.SuspendLayout();
            // 
            // FontControlFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Name = "FontControlFrame";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _label2;
        private System.Windows.Forms.ComboBox _comboBoxFontType;
        private System.Windows.Forms.CheckBox _checkBoxStrikethrough;
        private System.Windows.Forms.CheckBox _checkBoxUnderline;
        private System.Windows.Forms.CheckBox _checkBoxHighlight;
        private System.Windows.Forms.CheckBox _checkBoxGrowShrink;

    }
}
