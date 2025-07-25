//*****************************************************************************
//
//  File:       LabelPropertiesProvider.cs
//
//  Contents:   Definition for label properties provider 
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
    /// Definition for label properties provider interface
    /// </summary>
    public interface ILabelPropertiesProvider
    {
        /// <summary>
        /// Label property
        /// </summary>
        string? Label { get; set; }
    }

    /// <summary>
    /// Implementation of ILabelPropertiesProvider
    /// </summary>
    public sealed class LabelPropertiesProvider : BasePropertiesProvider, ILabelPropertiesProvider
    {
        /// <summary>
        /// LabelPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        /// <param name="item">ribbon control</param>
        public LabelPropertiesProvider(RibbonStrip ribbon, uint commandId, RibbonStripItem item)
            : base(ribbon, commandId)
        {
            _item = item;
            // add supported properties
            _supportedProperties.Add(RibbonProperties.Label);
        }

        private readonly RibbonStripItem _item;
        private string? _label;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.Label)
            {
                if (_label != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_label, out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region ILabelPropertiesProvider Members

        /// <summary>
        /// Label property
        /// </summary>
        public string? Label
        {
            get
            {
                if (_label == null)
                {
                    if (_item.ResourceIds != null && _item.ResourceIds.LabelTitleId >= 2)
                        _label = _ribbon.LoadString(_item.ResourceIds.LabelTitleId);
                }
                return _label;
            }
            set
            {
                _label = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    hr = _ribbon.Framework.InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, RibbonProperties.Label);
                }
            }
        }

        #endregion
    }
}
