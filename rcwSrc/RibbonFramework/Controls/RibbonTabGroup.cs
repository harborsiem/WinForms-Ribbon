//*****************************************************************************
//
//  File:       RibbonTabGroup.cs
//
//  Contents:   Helper class that wraps a ribbon tab group control.
//
//*****************************************************************************
using Windows.Win32.UI.Ribbon;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a ribbon tab group control (ContextualTab).
    /// </summary>
    public sealed class RibbonTabGroup : RibbonStripItem,
        IContextAvailablePropertiesProvider,
        IKeytipPropertiesProvider,
        ILabelPropertiesProvider,
        ITooltipPropertiesProvider
    {
        private ContextAvailablePropertiesProvider _contextAvailablePropertiesProvider;
        private KeytipPropertiesProvider _keytipPropertiesProvider;
        private LabelPropertiesProvider _labelPropertiesProvider;
        private TooltipPropertiesProvider _tooltipPropertiesProvider;

        /// <summary>
        /// Initializes a new instance of the Ribbon TabGroup (ContextualTab).
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonTabGroup(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            AddPropertiesProvider(_contextAvailablePropertiesProvider = new ContextAvailablePropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_keytipPropertiesProvider = new KeytipPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_labelPropertiesProvider = new LabelPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_tooltipPropertiesProvider = new TooltipPropertiesProvider(ribbon, commandId, this));
        }

        #region IContextAvailablePropertiesProvider Members

        /// <summary>
        /// Get or set the status of the context.
        /// </summary>
        public ContextAvailability ContextAvailable
        {
            get
            {
                return _contextAvailablePropertiesProvider.ContextAvailable;
            }
            set
            {
                _contextAvailablePropertiesProvider.ContextAvailable = value;
            }
        }

        #endregion

        #region IKeytipPropertiesProvider Members

        /// <summary>
        /// The keytip or key sequence that is used to access the command using the Alt key.
        /// This keytip appears when the user presses the Alt key to navigate the ribbon.
        /// The Ribbon Framework will automatically apply keytips to every command.
        /// However, if you want more control over the keytips used, you can specify them yourself.
        /// A keytip is not limited to a single character.
        /// </summary>
        public string? Keytip
        {
            get
            {
                return _keytipPropertiesProvider.Keytip;
            }
            set
            {
                _keytipPropertiesProvider.Keytip = value;
            }
        }

        #endregion

        #region ILabelPropertiesProvider Members

        /// <summary>
        /// This is the label of the command as it will appear on the ribbon.
        /// </summary>
        public string? Label
        {
            get
            {
                return _labelPropertiesProvider.Label;
            }
            set
            {
                _labelPropertiesProvider.Label = value;
            }
        }

        #endregion

        #region ITooltipPropertiesProvider Members

        /// <summary>
        /// The title of the tooltip (hint) that appear when the user hovers the mouse over the command.
        /// This title is displayed in bold at the top of the tooltip.
        /// </summary>
        public string? TooltipTitle
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
        public string? TooltipDescription
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
