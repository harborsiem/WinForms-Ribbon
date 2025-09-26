using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    /// <summary>
    /// EventLogger event and functions for the user and
    /// Implementation of COM interface IUIEventLogger
    /// </summary>
    public sealed unsafe class EventLogger : IUIEventLogger.Interface
    {
#pragma warning disable CA1416
        private static EventKey s_LogEventKey = new EventKey();
        private RibbonStrip _ribbon;
        private bool _attached;

        /// <summary>
        /// Log Event
        /// </summary>
        public event EventHandler<EventLoggerEventArgs>? LogEvent
        {
            add { _ribbon.EventSet.Add(s_LogEventKey, value); }
            remove { _ribbon.EventSet.Remove(s_LogEventKey, value); }
        }

        internal EventLogger(RibbonStrip ribbon)
        {
            _ribbon = ribbon;
        }

        /// <summary>
        /// Attach to an IUIEventLogger and IUIEventingManager objects events
        /// </summary>
        public void Attach()
        {
            if (!_attached)
            {
                using ComScope<IUIEventingManager> cpEventingManager = ComScope<IUIEventingManager>.QueryFrom(_ribbon.Framework);
                using ComScope<IUIEventLogger> cpEventLogger = ComHelpers.GetComScope<IUIEventLogger>(this);
                cpEventingManager.Value->SetEventLogger(cpEventLogger);
                _attached = true;
            }
        }

        /// <summary>
        /// Detach the log events
        /// </summary>
        public void Detach()
        {
            if (_attached)
            {
                using ComScope<IUIEventingManager> cpEventingManager = ComScope<IUIEventingManager>.QueryFrom(_ribbon.Framework);
                cpEventingManager.Value->SetEventLogger(null);
                _attached = false;
            }
        }

        /// <summary>
        /// Don't call it from user code
        /// </summary>
        /// <param name="pEventParams"></param>
        unsafe void IUIEventLogger.Interface.OnUIEvent(UI_EVENTPARAMS* pEventParams)
        {
            EventLoggerEventArgs e = new EventLoggerEventArgs(in *pEventParams);
            _ribbon.BeginInvoke((MethodInvoker)delegate
            {
                OnUIEvent(e);
            });
        }

        private void OnUIEvent(EventLoggerEventArgs e)
        {
            _ribbon.EventSet.Raise(s_LogEventKey, this, e);
        }

        /// <summary>
        ///  Disposes of the resources.
        /// </summary>
        internal void Destroy()
        {
            Detach();
        }
    }
}
