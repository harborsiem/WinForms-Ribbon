using System;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {
            ApplicationMenu.TooltipTitle = "Menu";
            ApplicationMenu.TooltipDescription = "Application main menu";

            ButtonNew.Click += new EventHandler<EventArgs>(_buttonNew_Click);
        }

        void _buttonNew_Click(object sender, EventArgs e)
        {
            MessageBox.Show("New button pressed");
        }

        public void Load()
        {
        }

    }
}
