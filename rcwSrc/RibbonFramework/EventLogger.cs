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
    [ComVisible(true)]
    [Guid("0406D872-D9E9-4D6D-A053-B12ECFC22C35")]
    public sealed class EventLogger : IUIEventLogger
    {
#pragma warning disable CA1416
        private static EventKey s_LogEventKey = new EventKey();
        private EventSet _eventSet; //taken from RibbonStrip
        private RibbonStrip _ribbon;
        private bool _attached;

        /// <summary>
        /// Log Event
        /// </summary>
        public event EventHandler<EventLoggerEventArgs>? LogEvent
        {
            add { _eventSet.Add(s_LogEventKey, value); }
            remove { _eventSet.Remove(s_LogEventKey, value); }
        }

        internal EventLogger(RibbonStrip ribbon)
        {
            _ribbon = ribbon;
            _eventSet = ribbon.EventSet;
        }

        /// <summary>
        /// Attach to an IUIEventLogger and IUIEventingManager objects events
        /// </summary>
        public void Attach()
        {
            if (!_attached)
            {
                IUIEventingManager cpEventingManager = (IUIEventingManager)_ribbon.Framework!;
                cpEventingManager.SetEventLogger(this);
                cpEventingManager = null!;
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
                IUIEventingManager cpEventingManager = (IUIEventingManager)_ribbon.Framework!;
                cpEventingManager.SetEventLogger(null);
                cpEventingManager = null!;
                _attached = false;
            }
        }

        /// <summary>
        /// Don't call it from user code
        /// </summary>
        /// <param name="pEventParams"></param>
        unsafe void IUIEventLogger.OnUIEvent(UI_EVENTPARAMS* pEventParams)
        {
            EventLoggerEventArgs e = new EventLoggerEventArgs(in *pEventParams);
            _ribbon.BeginInvoke((MethodInvoker)delegate
            {
                OnUIEvent(e);
            });
        }

        private void OnUIEvent(EventLoggerEventArgs e)
        {
            _eventSet.Raise(s_LogEventKey, this, e);
            //EventHandler<EventLoggerEventArgs>? handler = LogEvent;
            //if (handler != null)
            //{
            //    handler(this, e);
            //}
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
