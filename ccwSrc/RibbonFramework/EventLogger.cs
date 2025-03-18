using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// EventLogger event and functions for the user and
    /// Implementation of COM interface IUIEventLogger
    /// </summary>
    public sealed unsafe class EventLogger : IUIEventLogger.Interface
    {
#pragma warning disable CA1416
        private RibbonStrip _strip;
        private bool _attached;

        /// <summary>
        /// Log Event
        /// </summary>
        public event EventHandler<EventLoggerEventArgs>? LogEvent;

        internal EventLogger(RibbonStrip strip)
        {
            _strip = strip;
        }

        /// <summary>
        /// Attach to an IUIEventLogger and IUIEventingManager objects events
        /// </summary>
        public void Attach()
        {
            if (!_attached)
            {
                using ComScope<IUIEventingManager> cpEventingManager = ComScope<IUIEventingManager>.QueryFrom(_strip.Framework);
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
                using ComScope<IUIEventingManager> cpEventingManager = ComScope<IUIEventingManager>.QueryFrom(_strip.Framework);
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
            _strip.BeginInvoke((MethodInvoker)delegate
            {
                OnUIEvent(e);
            });
            //EventHandler<EventLoggerEventArgs>? handler = LogEvent;
            //if (handler != null)
            //{
            //    handler(this, e);
            //}
        }

        private void OnUIEvent(EventLoggerEventArgs e)
        {
            EventHandler<EventLoggerEventArgs>? handler = LogEvent;
            if (handler != null)
            {
                handler(this, e);
            }
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
