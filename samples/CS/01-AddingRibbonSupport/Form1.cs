using System;
using System.Windows.Forms;
using WinForms.Ribbon;

namespace _01_AddingRibbonSupport
{
    public partial class Form1 : Form
    {
        private RibbonItems _ribbonItems;

        public Form1()
        {
            InitializeComponent();
            _ribbonItems = new RibbonItems(_ribbon);
            _ribbonItems.Init();
        }
    }
}
