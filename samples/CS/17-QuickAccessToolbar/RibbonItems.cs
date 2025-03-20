using System;
using System.IO;
using System.Windows.Forms;


namespace WinForms.Ribbon
{
    partial class RibbonItems
    {
        private Stream _stream;

        public void Init()
        {
            ButtonNew.Click += new EventHandler<EventArgs>(_buttonNew_ExecuteEvent);
            ButtonSave.Click += new EventHandler<EventArgs>(_buttonSave_ExecuteEvent);
            ButtonOpen.Click += new EventHandler<EventArgs>(_buttonOpen_ExecuteEvent);

            // register to the QAT customize button
            QAT.Click += new EventHandler<EventArgs>(_ribbonQuickAccessToolbar_ExecuteEvent);
        }

        void _buttonNew_ExecuteEvent(object sender, EventArgs e)
        {
            // changing QAT commands list 
            UICollection<QatCommandPropertySet> itemsSource = QAT.QatItemsSource;
            itemsSource.Clear();
            itemsSource.Add(new QatCommandPropertySet() { CommandId = (uint)Cmd.cmdButtonNew });
            itemsSource.Add(new QatCommandPropertySet() { CommandId = (uint)Cmd.cmdButtonOpen });
            itemsSource.Add(new QatCommandPropertySet() { CommandId = (uint)Cmd.cmdButtonSave });
        }

        void _buttonSave_ExecuteEvent(object sender, EventArgs e)
        {
            // save ribbon QAT settings 
            _stream = new MemoryStream();
            Ribbon.SaveSettingsToStream(_stream);
        }

        void _buttonOpen_ExecuteEvent(object sender, EventArgs e)
        {
            if (_stream == null)
            {
                return;
            }

            // load ribbon QAT settings 
            _stream.Position = 0;
            Ribbon.LoadSettingsFromStream(_stream);
        }

        void _ribbonQuickAccessToolbar_ExecuteEvent(object sender, EventArgs e)
        {
            MessageBox.Show("Open customize commands dialog..");
        }

        public void Load()
        {
        }

    }
}
