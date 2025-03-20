using System;
using System.Drawing;
using System.Windows.Forms;
using WinForms.Ribbon;
using System.Diagnostics;

namespace _12_FontControl
{
    public partial class Form1 : Form
    {
        private RibbonItems _ribbonItems;
        public RichTextBox RichTextBox1 => richTextBox1;

        public Form1()
        {
            InitializeComponent();
            _ribbonItems = new RibbonItems(_ribbon);
            _ribbonItems.Init(this);
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            _ribbonItems.richTextBox1_SelectionChanged(sender, e);
        }
    }
}
