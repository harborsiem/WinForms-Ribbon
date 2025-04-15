//*****************************************************************************
//
//  File:       LabelDescriptionPropertiesProvider.cs
//
//  Contents:   Definition for label description properties provider 
//
//*****************************************************************************

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for label description properties provider interface
    /// </summary>
    public interface ILabelDescriptionPropertiesProvider
    {
        /// <summary>
        /// LabelDescription property
        /// </summary>
        string LabelDescription { get; set; }
    }

    /// <summary>
    /// Implementation of ILabelDescriptionPropertiesProvider
    /// </summary>
    public sealed unsafe class LabelDescriptionPropertiesProvider : BasePropertiesProvider, ILabelDescriptionPropertiesProvider
    {
        /// <summary>
        /// LabelDescriptionPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        /// <param name="item">ribbon control</param>
        public LabelDescriptionPropertiesProvider(RibbonStrip ribbon, uint commandId, RibbonStripItem item)
            : base(ribbon, commandId)
        {
            _item = item;
            // add supported properties
            _supportedProperties.Add(RibbonProperties.LabelDescription);
        }

        private readonly RibbonStripItem _item;
        private string _labelDescription;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.LabelDescription)
            {
                if (_labelDescription != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_labelDescription, out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region ILabelDescriptionPropertiesProvider Members

        /// <summary>
        /// Label description property
        /// </summary>
        public string LabelDescription
        {
            get
            {
                if (_labelDescription == null)
                {
                    if (_item.ResourceIds != null && _item.ResourceIds.LabelDescriptionId >= 2)
                        _labelDescription = _ribbon.LoadString(_item.ResourceIds.LabelDescriptionId);
                }
                return _labelDescription;
            }
            set
            {
                _labelDescription = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pLabelDescription = &RibbonProperties.LabelDescription)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pLabelDescription);
                }
            }
        }

        #endregion
    }
}
