using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    internal sealed class ShortcutHandler : IDisposable
    {
        private IUICommandHandler _commandHandler;
        private RibbonStrip _ribbon;
        private RibbonShortcutTable? _ribbonShortcutTable;

        public ShortcutHandler(RibbonStrip ribbon, IUICommandHandler commandHandler)
        {
            _ribbon = ribbon;
            _commandHandler = commandHandler;
        }

        public void TryCreateShortcutTable(Assembly assembly)
        {
            _ribbonShortcutTable = null;

            _ribbonShortcutTable = Util.DeserializeEmbeddedResource<RibbonShortcutTable>(
                _ribbon.ShortcutTableResourceName, assembly);
            if (_ribbonShortcutTable != null)
            {
                var form = _ribbon.FindForm();
                if (form != null)
                {
                    form.KeyPreview = true;
                    form.KeyUp += new KeyEventHandler(Form_KeyUp);
                }
            }
            else
                throw new ArgumentException(string.Format("Embedded resource not found '{0}'", nameof(RibbonStrip) + "." + nameof(_ribbon.ShortcutTableResourceName)));
        }

        private unsafe void Form_KeyUp(object? sender, KeyEventArgs e)
        {
            var commandId = _ribbonShortcutTable!.HitTest(e.KeyData);
            if (commandId == 0)
                return;
            _commandHandler.Execute(commandId, UI_EXECUTIONVERB.UI_EXECUTIONVERB_EXECUTE, null, null, null);

            e.SuppressKeyPress = false;
            e.Handled = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ribbonShortcutTable != null)
                {
                    var form = _ribbon.FindForm();
                    if (form != null)
                    {
                        form.KeyUp -= new KeyEventHandler(Form_KeyUp);
                    }
                }
                _ribbonShortcutTable = null;
            }
        }
    }
}
