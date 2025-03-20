using System;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {
            ButtonDropB.Click += new EventHandler<EventArgs>(_buttonDropB_ExecuteEvent);
        }

        void _buttonDropB_ExecuteEvent(object sender, EventArgs e)
        {
            MessageBox.Show("drop B button pressed");
        }

        public void Load()
        {
        }

    }
}
