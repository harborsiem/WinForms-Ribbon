//*****************************************************************************
//
//  File:       RibbonSpinner.cs
//
//  Contents:   Helper class that wraps a ribbon spinner control.
//
//*****************************************************************************

using System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a ribbon spinner control.
    /// </summary>
    public sealed class RibbonSpinner : RibbonStripItem,
        ISpinnerPropertiesProvider,
        IRepresentativeStringPropertiesProvider,
        IEnabledPropertiesProvider,
        IKeytipPropertiesProvider,
        ILabelPropertiesProvider,
        IImagePropertiesProvider,
        ITooltipPropertiesProvider,
        IExecuteEventsProvider
    {
        private static readonly EventKey s_ValueChangedKey = new EventKey();
        private SpinnerPropertiesProvider _spinnerPropertiesProvider;
        private RepresentativeStringPropertiesProvider _representativeStringPropertiesProvider;
        private EnabledPropertiesProvider _enabledPropertiesProvider;
        private KeytipPropertiesProvider _keytipPropertiesProvider;
        private LabelPropertiesProvider _labelPropertiesProvider;
        private ImagePropertiesProvider _imagePropertiesProvider;
        private TooltipPropertiesProvider _tooltipPropertiesProvider;
        private ExecuteEventsProvider _executeEventsProvider;

        /// <summary>
        /// Initializes a new instance of the Ribbon Spinner
        /// </summary>
        /// <param name="ribbon">Parent Ribbon control</param>
        /// <param name="commandId">Command id attached to this control</param>
        public RibbonSpinner(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            AddPropertiesProvider(_spinnerPropertiesProvider = new SpinnerPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_representativeStringPropertiesProvider = new RepresentativeStringPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_enabledPropertiesProvider = new EnabledPropertiesProvider(ribbon, commandId));
            AddPropertiesProvider(_keytipPropertiesProvider = new KeytipPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_labelPropertiesProvider = new LabelPropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_imagePropertiesProvider = new ImagePropertiesProvider(ribbon, commandId, this));
            AddPropertiesProvider(_tooltipPropertiesProvider = new TooltipPropertiesProvider(ribbon, commandId, this));

            AddEventsProvider(_executeEventsProvider = new ExecuteEventsProvider(this));
        }

        #region ISpinnerPropertiesProvider Members

        /// <summary>
        /// The actual decimal value of the spinner.
        /// </summary>
        public decimal DecimalValue
        {
            get
            {
                return _spinnerPropertiesProvider.DecimalValue;
            }
            set
            {
                _spinnerPropertiesProvider.DecimalValue = value;
            }
        }

        /// <summary>
        /// The size of the step when pressing on increment / decrement buttons.
        /// </summary>
        public decimal Increment
        {
            get
            {
                return _spinnerPropertiesProvider.Increment;
            }
            set
            {
                _spinnerPropertiesProvider.Increment = value;
            }
        }

        /// <summary>
        /// Maximum value that can be set using the spinner control.
        /// </summary>
        public decimal MaxValue
        {
            get
            {
                return _spinnerPropertiesProvider.MaxValue;
            }
            set
            {
                _spinnerPropertiesProvider.MaxValue = value;
            }
        }

        /// <summary>
        /// Minimum value that can be set using the spinner control.
        /// </summary>
        public decimal MinValue
        {
            get
            {
                return _spinnerPropertiesProvider.MinValue;
            }
            set
            {
                _spinnerPropertiesProvider.MinValue = value;
            }
        }

        /// <summary>
        /// The number of digits to show after the point.
        /// </summary>
        public uint DecimalPlaces
        {
            get
            {
                return _spinnerPropertiesProvider.DecimalPlaces;
            }
            set
            {
                _spinnerPropertiesProvider.DecimalPlaces = value;
            }
        }

        /// <summary>
        /// The units of the value.
        /// </summary>
        public string FormatString
        {
            get
            {
                return _spinnerPropertiesProvider.FormatString;
            }
            set
            {
                _spinnerPropertiesProvider.FormatString = value;
            }
        }

        #endregion

        #region IRepresentativeStringPropertiesProvider Members

        /// <summary>
        /// A string that represents the common value for the Spinner.
        /// This is used to calculate the width of the Spinner,
        /// so you should set here the longest string you forecast.
        /// Note that it doesn't have to be an actual value,
        /// it can be also: "XXXXXXXX".
        ///
        /// Note: Set it before Ribbon is initialized.
        /// </summary>
        public string RepresentativeString
        {
            get
            {
                return _representativeStringPropertiesProvider.RepresentativeString;
            }
            set
            {
                _representativeStringPropertiesProvider.RepresentativeString = value;
            }
        }

        #endregion

        #region IEnabledPropertiesProvider Members

        /// <summary>
        /// Get or set the Enabled state.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabledPropertiesProvider.Enabled;
            }
            set
            {
                _enabledPropertiesProvider.Enabled = value;
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
        /// This is the label of the command as it will appear on the ribbon or context popups.
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

        #region IImagePropertiesProvider Members

        /// <summary>
        /// Large images
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? LargeImage
        {
            get
            {
                return _imagePropertiesProvider.LargeImage;
            }
            set
            {
                _imagePropertiesProvider.LargeImage = value;
            }
        }

        /// <summary>
        /// Small images
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? SmallImage
        {
            get
            {
                return _imagePropertiesProvider.SmallImage;
            }
            set
            {
                _imagePropertiesProvider.SmallImage = value;
            }
        }

        /// <summary>
        /// Large images for use with high-contrast system settings
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? LargeHighContrastImage
        {
            get
            {
                return _imagePropertiesProvider.LargeHighContrastImage;
            }
            set
            {
                _imagePropertiesProvider.LargeHighContrastImage = value;
            }
        }

        /// <summary>
        /// Small images for use with high-contrast system settings
        /// For setting the Image, use UIImage class.
        /// </summary>
        public UIImage? SmallHighContrastImage
        {
            get
            {
                return _imagePropertiesProvider.SmallHighContrastImage;
            }
            set
            {
                _imagePropertiesProvider.SmallHighContrastImage = value;
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

        #region IExecuteEventsProvider Members

        /// <summary>
        /// Event provider similar to a "Value Changed" event.
        /// </summary>
        event EventHandler<ExecuteEventArgs>? IExecuteEventsProvider.ExecuteEvent
        {
            add
            {
                _executeEventsProvider.ExecuteEvent += value;
            }
            remove
            {
                _executeEventsProvider.ExecuteEvent -= value;
            }
        }

        #endregion

        /// <summary>
        /// Event provider similar to a "Value Changed" event.
        /// </summary>
        public event EventHandler<EventArgs>? ValueChanged
        {
            add { EventSet.Add(s_ValueChangedKey, value); }
            remove { EventSet.Remove(s_ValueChangedKey, value); }
        }

        internal override unsafe HRESULT OnExecute(PROPERTYKEY* key, PROPVARIANT* currentValue, IUISimplePropertySet? commandExecutionProperties)
        {
            //if (*key == RibbonProperties.DecimalValue)
            //{
            //    decimal val = currentValue.ToDecimal();
            //    _spinnerPropertiesProvider.DecimalValue = val;
            //}
            EventSet.Raise(s_ValueChangedKey, this, EventArgs.Empty);
            return HRESULT.S_OK;
        }
    }
}
