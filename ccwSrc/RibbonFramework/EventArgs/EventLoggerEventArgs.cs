using System;
using System.Collections.Generic;
using System.Text;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    /// <summary>
    /// The EventArgs of EventLogger
    /// </summary>
    public sealed class EventLoggerEventArgs : EventArgs
    {
        /// <summary>
        /// Identifies the types of events associated with a Ribbon.
        /// </summary>
        public EventType EventType { get; private set; }
        /// <summary>
        /// The application modes. Only used when a EventType ApplicationModeSwitched has been fired.
        /// In all other cases it is set to 0.
        /// </summary>
        public Int32 Modes { get; private set; }
        /// <summary>
        /// The ID of the Command directly related to the event, which is specified in the markup resource file.
        /// </summary>
        public uint CommandID { get; private set; }
        /// <summary>
        /// The Command name that is associated with CommandId.
        /// </summary>
        public String? CommandName { get; private set; }
        /// <summary>
        /// The ID for the parent of the Command, which is specified in the markup resource file.
        /// </summary>
        public uint ParentCommandID { get; private set; }
        /// <summary>
        /// The Command name of the parent that is associated with CommandId.
        /// </summary>
        public String? ParentCommandName { get; private set; }
        /// <summary>
        /// SelectionIndex is used only when a EventType CommandExecuted has been fired in response to the user selecting an item within a ComboBox or item gallery.
        /// In those cases, SelectionIndex contains the index of the selected item. In all other cases, it is set to 0.
        /// </summary>
        public int SelectionIndex { get; private set; }
        /// <summary>
        /// Identifies the locations where events associated with a Ribbon control can originate.
        /// </summary>
        public EventLocation Location { get; private set; }

        internal EventLoggerEventArgs(in UI_EVENTPARAMS pEventParams)
        {
            EventType = (EventType)pEventParams.EventType;
            switch (pEventParams.EventType)
            {
                case UI_EVENTTYPE.UI_EVENTTYPE_ApplicationModeSwitched:
                    Modes = (pEventParams.Anonymous.Modes);
                    break;
                case UI_EVENTTYPE.UI_EVENTTYPE_CommandExecuted:
                case UI_EVENTTYPE.UI_EVENTTYPE_TooltipShown:
                case UI_EVENTTYPE.UI_EVENTTYPE_TabActivated:
                case UI_EVENTTYPE.UI_EVENTTYPE_MenuOpened:
                    CopyAndMarshal(in pEventParams);
                    break;
                case UI_EVENTTYPE.UI_EVENTTYPE_ApplicationMenuOpened:
                case UI_EVENTTYPE.UI_EVENTTYPE_RibbonExpanded:
                case UI_EVENTTYPE.UI_EVENTTYPE_RibbonMinimized:
                default:
                    break;
            }
        }

        private void CopyAndMarshal(in UI_EVENTPARAMS pEventParams)
        {
            CommandID = pEventParams.Anonymous.Params.CommandID;
            CommandName = pEventParams.Anonymous.Params.CommandName.ToString(); //PCWStr
            ParentCommandID = pEventParams.Anonymous.Params.ParentCommandID;
            ParentCommandName = pEventParams.Anonymous.Params.ParentCommandName.ToString(); //PCWStr
            SelectionIndex = (int)pEventParams.Anonymous.Params.SelectionIndex;
            Location = (EventLocation)pEventParams.Anonymous.Params.Location;
        }
    }
}
