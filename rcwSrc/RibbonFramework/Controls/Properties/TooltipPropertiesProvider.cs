//*****************************************************************************
//
//  File:       TooltipPropertiesProvider.cs
//
//  Contents:   Definition for tooltip properties provider 
//
//*****************************************************************************

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using System.Runtime.InteropServices;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for tooltip properties provider interface
    /// </summary>
    public interface ITooltipPropertiesProvider
    {
        /// <summary>
        /// Tooltip title property
        /// </summary>
        string TooltipTitle { get; set; }

        /// <summary>
        /// Tooltip description property
        /// </summary>
        string TooltipDescription { get; set; }
    }

    /// <summary>
    /// Implementation of ITooltipPropertiesProvider
    /// </summary>
    public sealed class TooltipPropertiesProvider : BasePropertiesProvider, ITooltipPropertiesProvider
    {
        /// <summary>
        /// TooltipPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        public TooltipPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.TooltipTitle);
            _supportedProperties.Add(RibbonProperties.TooltipDescription);
        }

        private string _tooltipTitle;
        private string _tooltipDescription;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.TooltipTitle)
            {
                if (_tooltipTitle != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_tooltipTitle, out newValue);
                    return HRESULT.S_OK;
                }
            }
            else if (key == RibbonProperties.TooltipDescription)
            {
                if (_tooltipDescription != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_tooltipDescription, out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region ITooltipPropertiesProvider Members

        /// <summary>
        /// Tooltip title property
        /// </summary>
        public unsafe string TooltipTitle
        {
            get
            {
                return _tooltipTitle;
            }
            set
            {
                _tooltipTitle = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pTooltipTitle = &RibbonProperties.TooltipTitle)
                    {
                        hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pTooltipTitle);
                    }
                }
            }
        }

        /// <summary>
        /// Tooltip description property
        /// </summary>
        public unsafe string TooltipDescription
        {
            get
            {
                return _tooltipDescription;
            }
            set
            {
                _tooltipDescription = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pTooltipDescription = &RibbonProperties.TooltipDescription)
                    {
                        hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pTooltipDescription);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// RESID in RibbonMarkup.ribbon for the TooltipTitle property.
        /// Must be set before ViewCreated event
        /// </summary>
        public ushort TooltipTitleResId { get; set; } = 0;

        /// <summary>
        /// RESID in RibbonMarkup.ribbon for the TooltipDescription property.
        /// Must be set before ViewCreated event
        /// </summary>
        public ushort TooltipDescriptionResId { get; set; } = 0;

        /// <summary>
        /// Initialize the TooltipTitle property from RibbonMarkup.ribbon
        /// </summary>
        /// <param name="title"></param>
        public void InitTooltipTitle(string title)
        {
            _tooltipTitle = title;
        }

        /// <summary>
        /// Initialize the TooltipDescription property from RibbonMarkup.ribbon
        /// </summary>
        /// <param name="description"></param>
        public void InitTooltipDescription(string description)
        {
            _tooltipDescription = description;
        }
    }
}
