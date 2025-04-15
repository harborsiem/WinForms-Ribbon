//*****************************************************************************
//
//  File:       RibbonApplicationMenu.cs
//
//  Contents:   Helper class that wraps a ribbon application menu control.
//
//*****************************************************************************


namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a ribbon application menu control.
    /// </summary>
    public sealed class RibbonApplicationMenu : RibbonStripItem,
        ITooltipPropertiesProvider
    {
        private TooltipPropertiesProvider _tooltipPropertiesProvider;

        /// <summary>
        /// Constructor for the Ribbon ApplicationMenu
        /// </summary>
        /// <param name="ribbon">Parent Ribbon control</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonApplicationMenu(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            AddPropertiesProvider(_tooltipPropertiesProvider = new TooltipPropertiesProvider(ribbon, commandId, this));
        }

        #region ITooltipPropertiesProvider Members

        /// <summary>
        /// The title of the tooltip (hint) that appear when the user hovers the mouse over the command.
        /// This title is displayed in bold at the top of the tooltip.
        /// </summary>
        public string TooltipTitle
        {
            get
            {
                return _tooltipPropertiesProvider.TooltipTitle;
            }
            set
            {
                _tooltipPropertiesProvider.TooltipTitle = value;
            }
        }

        /// <summary>
        /// The description of the tooltip as it appears below the title.
        /// </summary>
        public string TooltipDescription
        {
            get
            {
                return _tooltipPropertiesProvider.TooltipDescription;
            }
            set
            {
                _tooltipPropertiesProvider.TooltipDescription = value;
            }
        }

        #endregion

    }
}
