using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    // Public Windows Ribbon enums implemented by the framework

    /// <summary>
    /// Specifies values that identify the availability
    /// of a contextual tab.
    /// </summary>
    public enum ContextAvailability
    {
        /// <summary>
        /// A contextual tab is not available for the selected object.
        /// </summary>
        NotAvailable = UI_CONTEXTAVAILABILITY.UI_CONTEXTAVAILABILITY_NOTAVAILABLE,
        /// <summary>
        /// A contextual tab is available for the selected object.
        /// The tab is not the active tab.
        /// </summary>
        Available = UI_CONTEXTAVAILABILITY.UI_CONTEXTAVAILABILITY_AVAILABLE,
        /// <summary>
        /// A contextual tab is available for the selected object.
        /// The tab is the active tab.
        /// </summary>
        Active = UI_CONTEXTAVAILABILITY.UI_CONTEXTAVAILABILITY_ACTIVE,
    }

    /// <summary>
    /// Specifies values that identify the font property state
    /// of a FontControl, such as Strikethrough.
    /// </summary>
    public enum FontProperties
    {
        /// <summary>
        /// The property is not available.
        /// </summary>
        NotAvailable = UI_FONTPROPERTIES.UI_FONTPROPERTIES_NOTAVAILABLE,
        /// <summary>
        /// The property is not set.
        /// </summary>
        NotSet = UI_FONTPROPERTIES.UI_FONTPROPERTIES_NOTSET,
        /// <summary>
        /// The property is set.
        /// </summary>
        Set = UI_FONTPROPERTIES.UI_FONTPROPERTIES_SET,
    }

    /// <summary>
    /// Specifies values that identify the
    /// vertical-alignment state of a FontControl.
    /// </summary>
    public enum FontVerticalPosition
    {
        /// <summary>
        /// Vertical positioning is not enabled.
        /// </summary>
        NotAvailable = UI_FONTVERTICALPOSITION.UI_FONTVERTICALPOSITION_NOTAVAILABLE,
        /// <summary>
        /// Vertical positioning is enabled but not toggled.
        /// </summary>
        NotSet = UI_FONTVERTICALPOSITION.UI_FONTVERTICALPOSITION_NOTSET,
        /// <summary>
        /// Vertical positioning is enabled and toggled for superscript.
        /// </summary>
        SuperScript = UI_FONTVERTICALPOSITION.UI_FONTVERTICALPOSITION_SUPERSCRIPT,
        /// <summary>
        /// Vertical positioning is enabled and toggled for subscript.
        /// </summary>
        SubScript = UI_FONTVERTICALPOSITION.UI_FONTVERTICALPOSITION_SUBSCRIPT,
    }

    /// <summary>
    /// Specifies values that identify the
    /// underline state of a FontControl.
    /// </summary>
    public enum FontUnderline
    {
        /// <summary>
        /// Underlining is not enabled.
        /// </summary>
        NotAvailable = UI_FONTUNDERLINE.UI_FONTUNDERLINE_NOTAVAILABLE,
        /// <summary>
        /// Underlining is off.
        /// </summary>
        NotSet = UI_FONTUNDERLINE.UI_FONTUNDERLINE_NOTSET,
        /// <summary>
        /// Underlining is on.
        /// </summary>
        Set = UI_FONTUNDERLINE.UI_FONTUNDERLINE_SET,
    }

    /// <summary>
    /// Specifies values that identify whether
    /// the font size of a highlighted text run
    /// should be incremented or decremented.
    /// </summary>
    public enum FontDeltaSize
    {
        /// <summary>
        /// Increment the font size.
        /// </summary>
        Grow = UI_FONTDELTASIZE.UI_FONTDELTASIZE_GROW,
        /// <summary>
        /// Decrement the font size.
        /// </summary>
        Shrink = UI_FONTDELTASIZE.UI_FONTDELTASIZE_SHRINK,
    }

    /// <summary>
    /// Specifies values that identify the dock state
    /// of the Quick Access Toolbar (QAT).
    /// </summary>
    public enum ControlDock
    {
        /// <summary>
        /// The QAT is docked in the nonclient area of the Ribbon host application.
        /// </summary>
        Top = UI_CONTROLDOCK.UI_CONTROLDOCK_TOP,
        /// <summary>
        /// The QAT is docked as a visually integral band below the Ribbon,
        /// </summary>
        Bottom = UI_CONTROLDOCK.UI_CONTROLDOCK_BOTTOM,
    }


    // Types for the color picker

    /// <summary>
    /// Specifies the values that identify how a color swatch
    /// in a DropDownColorPicker or a FontControl color picker
    /// (Text color or Text highlight) is filled.
    /// </summary>
    public enum SwatchColorType
    {
        /// <summary>
        /// The swatch is transparent.
        /// </summary>
        NoColor = UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_NOCOLOR,
        /// <summary>
        /// The swatch is filled with a solid RGB color
        /// bound to GetSysColor(COLOR_WINDOWTEXT).
        /// </summary>
        Automatic = UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_AUTOMATIC,
        /// <summary>
        /// The swatch is filled with a solid RGB color.
        /// </summary>
        RGB = UI_SWATCHCOLORTYPE.UI_SWATCHCOLORTYPE_RGB,
    }

    /// <summary>
    /// Specifies whether a swatch has normal or monochrome mode.
    /// </summary>
    public enum SwatchColorMode
    {
        /// <summary>
        /// The swatch is normal mode.
        /// </summary>
        Normal = UI_SWATCHCOLORMODE.UI_SWATCHCOLORMODE_NORMAL,
        /// <summary>
        /// The swatch is monochrome. The swatch's RGB color
        /// value will be interpreted as a 1 bit-per-pixel pattern.
        /// </summary>
        Monochrome = UI_SWATCHCOLORMODE.UI_SWATCHCOLORMODE_MONOCHROME,
    }

    /// <summary>
    /// Specifies values that identify the types
    /// of changes that can be made to a collection.
    /// </summary>
    public enum CollectionChange
    {
        /// <summary>
        /// Insert an item into the collection.
        /// </summary>
        Insert = UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_INSERT,
        /// <summary>
        /// Delete an item from the collection.
        /// </summary>
        Remove = UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_REMOVE,
        /// <summary>
        /// Replace an item in the collection.
        /// </summary>
        Replace = UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_REPLACE,
        /// <summary>
        /// Delete all items from the collection.
        /// </summary>
        Reset = UI_COLLECTIONCHANGE.UI_COLLECTIONCHANGE_RESET,
    }

    /// <summary>
    /// Specifies values that identify the type
    /// of Command associated with a Ribbon control.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// The type of command is not known.
        /// </summary>
        Unknown = UI_COMMANDTYPE.UI_COMMANDTYPE_UNKNOWN,
        /// <summary>
        /// Group
        /// </summary>
        Group = UI_COMMANDTYPE.UI_COMMANDTYPE_GROUP,
        /// <summary>
        /// Action (Button, HelpButton)
        /// </summary>
        Action = UI_COMMANDTYPE.UI_COMMANDTYPE_ACTION,
        /// <summary>
        /// Anchor (ApplicationMenu, DropDownButton,
        /// SplitButton, Tab)
        /// </summary>
        Anchor = UI_COMMANDTYPE.UI_COMMANDTYPE_ANCHOR,
        /// <summary>
        /// Context (TabGroup)
        /// </summary>
        Context = UI_COMMANDTYPE.UI_COMMANDTYPE_CONTEXT,
        /// <summary>
        /// Collection (ComboBox, DropDownGallery,
        /// InRibbonGallery, SplitButtonGallery)
        /// </summary>
        Collection = UI_COMMANDTYPE.UI_COMMANDTYPE_COLLECTION,
        /// <summary>
        /// Command collection (DropDownGallery, InRibbonGallery,
        /// QuickAccessToolbar, SplitButtonGallery)
        /// </summary>
        CommandCollection = UI_COMMANDTYPE.UI_COMMANDTYPE_COMMANDCOLLECTION,
        /// <summary>
        /// Decimal (Spinner)
        /// </summary>
        Decimal = UI_COMMANDTYPE.UI_COMMANDTYPE_DECIMAL,
        /// <summary>
        /// Boolean (ToggleButton, CheckBox)
        /// </summary>
        Boolean = UI_COMMANDTYPE.UI_COMMANDTYPE_BOOLEAN,
        /// <summary>
        /// Font (FontControl)
        /// </summary>
        Font = UI_COMMANDTYPE.UI_COMMANDTYPE_FONT,
        /// <summary>
        /// RecentItems
        /// </summary>
        RecentItems = UI_COMMANDTYPE.UI_COMMANDTYPE_RECENTITEMS,
        /// <summary>
        /// ColorAnchor (DropDownColorPicker)
        /// </summary>
        ColorAnchor = UI_COMMANDTYPE.UI_COMMANDTYPE_COLORANCHOR,
        /// <summary>
        /// ColorCollection.
        /// This Command type is not supported by any framework controls.
        /// </summary>
        ColorCollection = UI_COMMANDTYPE.UI_COMMANDTYPE_COLORCOLLECTION,
    }

    //following types and interfaces are in UIRibbon since Windows 8
    /// <summary>
    /// Identifies the types of events associated with a Ribbon.
    /// UI_EVENTTYPE enum
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// The ApplicationMenu opened
        /// </summary>
        ApplicationMenuOpened = UI_EVENTTYPE.UI_EVENTTYPE_ApplicationMenuOpened,
        /// <summary>
        /// The Ribbon minimized
        /// </summary>
        RibbonMinimized = UI_EVENTTYPE.UI_EVENTTYPE_RibbonMinimized,
        /// <summary>
        /// The Ribbon expanded
        /// </summary>
        RibbonExpanded = UI_EVENTTYPE.UI_EVENTTYPE_RibbonExpanded,
        /// <summary>
        /// The application mode changed
        /// </summary>
        ApplicationModeSwitched = UI_EVENTTYPE.UI_EVENTTYPE_ApplicationModeSwitched,
        /// <summary>
        /// A Tab activated
        /// </summary>
        TabActivated = UI_EVENTTYPE.UI_EVENTTYPE_TabActivated,
        /// <summary>
        /// A menu opened
        /// </summary>
        MenuOpened = UI_EVENTTYPE.UI_EVENTTYPE_MenuOpened,
        /// <summary>
        /// A Command executed
        /// </summary>
        CommandExecuted = UI_EVENTTYPE.UI_EVENTTYPE_CommandExecuted,
        /// <summary>
        /// A Command tooltip displayed.
        /// </summary>
        TooltipShown = UI_EVENTTYPE.UI_EVENTTYPE_TooltipShown
    }

    /// <summary>
    /// Identifies the locations where events associated
    /// with a Ribbon control can originate.
    /// UI_EVENTLOCATION enum
    /// </summary>
    public enum EventLocation
    {
        /// <summary>
        /// The Ribbon
        /// </summary>
        Ribbon = UI_EVENTLOCATION.UI_EVENTLOCATION_Ribbon,
        /// <summary>
        /// The QuickAccessToolbar
        /// </summary>
        QAT = UI_EVENTLOCATION.UI_EVENTLOCATION_QAT,
        /// <summary>
        /// The ApplicationMenu
        /// </summary>
        ApplicationMenu = UI_EVENTLOCATION.UI_EVENTLOCATION_ApplicationMenu,
        /// <summary>
        /// The ContextPopup
        /// </summary>
        ContextPopup = UI_EVENTLOCATION.UI_EVENTLOCATION_ContextPopup
    }

    ///// <summary>
    ///// Contains information about a Command associated with a event.
    ///// Marshalling of strings can only be done in the wrapper class of interface IUIEventLogger
    ///// UI_EVENTPARAMS_COMMAND struct
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public struct EventParametersCommand
    //{
    //    /// <summary>
    //    /// The command Id
    //    /// </summary>
    //    public uint CommandID;
    //    /// <summary>
    //    /// The command name (not Marshaled)
    //    /// </summary>
    //    public IntPtr CommandName; //PCWStr
    //    /// <summary>
    //    /// The parent command Id
    //    /// </summary>
    //    public uint ParentCommandID;
    //    /// <summary>
    //    /// The parent command name (not Marshaled)
    //    /// </summary>
    //    public IntPtr ParentCommandName; //PCWStr
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public uint SelectionIndex;
    //    /// <summary>
    //    /// The event location
    //    /// </summary>
    //    public EventLocation Location;
    //}

    /// <summary>
    /// Used for property "UseDarkMode". Values of this enum determine whether or not the ribbon should support Windows' "Dark Mode".
    /// protected virtual bool DarkModeSupported
    /// protected virtual bool SetDarkModeCore(DarkMode darkModeSetting) => true;
    /// protected virtual bool IsDarkModeEnabled
    /// public DarkMode DarkMode
    /// </summary>
    public enum DarkMode
    {
        /// <summary>
        ///  Dark mode in the current context is not supported.
        /// </summary>
        NotSupported = 0,

        /// <summary>
        /// The setting for the current dark mode context is inherited from the parent context.
        /// </summary>
        Inherits = 1,

        /// <summary>
        /// Dark mode for the current context is enabled.
        /// </summary>
        Enabled = 2,

        /// <summary>
        /// Dark mode the current context is disabled.
        /// </summary>
        Disabled = 3,
    }
}
