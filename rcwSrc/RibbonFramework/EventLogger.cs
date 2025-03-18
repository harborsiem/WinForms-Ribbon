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
        private RibbonStrip _strip;
        private IUIEventingManager _cpEventingManager;
        private bool attached;

        /// <summary>
        /// Log Event
        /// </summary>
        public event EventHandler<EventLoggerEventArgs>? LogEvent;

        internal EventLogger(RibbonStrip strip, IUIEventingManager cpEventingManager)
        {
            _strip = strip;
            _cpEventingManager = cpEventingManager;
        }

        /// <summary>
        /// Attach to an IUIEventLogger and IUIEventingManager objects events
        /// </summary>
        public void Attach()
        {
            if (!attached)
            {
                _cpEventingManager.SetEventLogger(this);
                attached = true;
            }
        }

        /// <summary>
        /// Detach the log events
        /// </summary>
        public void Detach()
        {
            _cpEventingManager.SetEventLogger(null);
            attached = false;
        }

        /// <summary>
        /// Don't call it from user code
        /// </summary>
        /// <param name="pEventParams"></param>
        unsafe void IUIEventLogger.OnUIEvent(UI_EVENTPARAMS* pEventParams)
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
            if (attached)
                Detach();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _cpEventingManager = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
