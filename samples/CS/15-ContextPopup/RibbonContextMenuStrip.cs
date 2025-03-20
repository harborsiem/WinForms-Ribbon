using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using WinForms.Ribbon;

namespace WinForms.Ribbon
{
    public class RibbonContextMenuStrip : ContextMenuStrip
    {
        private uint _contextPopupID;
        private RibbonStrip _ribbon;

        public RibbonContextMenuStrip(RibbonStrip ribbon, uint contextPopupID)
            : base()
        {
            _contextPopupID = contextPopupID;
            _ribbon = ribbon;
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            _ribbon.ShowContextPopup(_contextPopupID, Cursor.Position.X, Cursor.Position.Y);
            e.Cancel = true;
        }
    }
}
