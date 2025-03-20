using System;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {
            ButtonSwitchToAdvanced.Click += new EventHandler<EventArgs>(_buttonSwitchToAdvanced_ExecuteEvent);
            ButtonSwitchToSimple.Click += new EventHandler<EventArgs>(_buttonSwitchToSimple_ExecuteEvent);
        }

        void _buttonSwitchToAdvanced_ExecuteEvent(object sender, EventArgs e)
        {
            Ribbon.SetModes(1);
        }

        void _buttonSwitchToSimple_ExecuteEvent(object sender, EventArgs e)
        {
            Ribbon.SetModes(0);
        }

        public void Load()
        {
        }

    }
}
