namespace _18_SizeDefinition
{
    partial class DatePicker
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
            monthCalendar = new System.Windows.Forms.MonthCalendar();
            SuspendLayout();
            // 
            // monthCalendar
            // 
            monthCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            monthCalendar.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            monthCalendar.Location = new System.Drawing.Point(0, 0);
            monthCalendar.Margin = new System.Windows.Forms.Padding(0);
            monthCalendar.MaxSelectionCount = 2;
            monthCalendar.Name = "monthCalendar";
            monthCalendar.TabIndex = 1;
            // 
            // DatePicker
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(178, 162);
            Controls.Add(monthCalendar);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "DatePicker";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "DatePicker";
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar;
    }
}