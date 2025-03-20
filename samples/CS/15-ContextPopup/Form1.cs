using System;
using System.Windows.Forms;

using WinForms.Ribbon;

namespace _15_ContextPopup
{
    public partial class Form1 : Form
    {
        private RibbonItems _ribbonItems;
        public Panel Panel1 => panel1;
        public Panel Panel2 => panel2;

        public Form1()
        {
            InitializeComponent();
            _ribbonItems = new RibbonItems(_ribbon);
            _ribbonItems.Init(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ribbonItems.Load();
        }
    }
}
