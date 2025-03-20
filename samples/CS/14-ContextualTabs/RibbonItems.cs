using System;

namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        public void Init()
        {
            ButtonSelect.Click += new EventHandler<EventArgs>(_buttonSelect_ExecuteEvent);
            ButtonUnselect.Click += new EventHandler<EventArgs>(_buttonUnselect_ExecuteEvent);
        }

        void _buttonSelect_ExecuteEvent(object sender, EventArgs e)
        {
            if (TabGroupTableTools.ContextAvailable != ContextAvailability.Active)
                TabGroupTableTools.ContextAvailable = ContextAvailability.Active;
        }

        void _buttonUnselect_ExecuteEvent(object sender, EventArgs e)
        {
            if (TabGroupTableTools.ContextAvailable != ContextAvailability.NotAvailable)
                TabGroupTableTools.ContextAvailable = ContextAvailability.NotAvailable;
        }

        public void Load()
        {
        }

    }
}
