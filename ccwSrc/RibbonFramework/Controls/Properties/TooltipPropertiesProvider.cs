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
        string? TooltipTitle { get; set; }

        /// <summary>
        /// Tooltip description property
        /// </summary>
        string? TooltipDescription { get; set; }
    }

    /// <summary>
    /// Implementation of ITooltipPropertiesProvider
    /// </summary>
    public sealed unsafe class TooltipPropertiesProvider : BasePropertiesProvider, ITooltipPropertiesProvider
    {
        /// <summary>
        /// TooltipPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">Parent RibbonStrip</param>
        /// <param name="commandId">ribbon control command id</param>
        /// <param name="ribbonItem">ribbon control</param>
        public TooltipPropertiesProvider(RibbonStrip ribbon, uint commandId, RibbonStripItem ribbonItem)
            : base(ribbon, commandId)
        {
            _ribbonItem = ribbonItem;
            // add supported properties
            _supportedProperties.Add(RibbonProperties.TooltipTitle);
            _supportedProperties.Add(RibbonProperties.TooltipDescription);
        }

        private readonly RibbonStripItem _ribbonItem;
        private string? _tooltipTitle;
        private string? _tooltipDescription;

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
        public unsafe string? TooltipTitle
        {
            get
            {
                if (_tooltipTitle == null)
                {
                    if (_ribbonItem.ResourceIds != null && _ribbonItem.ResourceIds.TooltipTitleId >= 2)
                        _tooltipTitle = _ribbon.LoadString(_ribbonItem.ResourceIds.TooltipTitleId);
                }
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
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pTooltipTitle);
                    }
                }
            }
        }

        /// <summary>
        /// Tooltip description property
        /// </summary>
        public unsafe string? TooltipDescription
        {
            get
            {
                if (_tooltipDescription == null)
                {
                    if (_ribbonItem.ResourceIds != null && _ribbonItem.ResourceIds.TooltipDescriptionId >= 2)
                        _tooltipDescription = _ribbon.LoadString(_ribbonItem.ResourceIds.TooltipDescriptionId);
                }
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
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pTooltipDescription);
                    }
                }
            }
        }

        #endregion
    }
}
