using System;
using System.Windows.Forms;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {
            Button.Click += new EventHandler<EventArgs>(_button_ExecuteEvent);
        }

        void _button_ExecuteEvent(object sender, EventArgs e)
        {
            MessageBox.Show("RibbonCheckBox check status is: " + CheckBox.BooleanValue.ToString());
        }

        public void Load()
        {
        }

    }
}
