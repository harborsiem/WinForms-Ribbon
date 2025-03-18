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
        string Label { get; set; }
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
        public LabelPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.Label);
        }

        private string _label;

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
        public string Label
        {
            get
            {
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

        /// <summary>
        /// RESID in RibbonMarkup.ribbon for the Label property.
        /// Must be set before ViewCreated event
        /// </summary>
        public ushort LabelResId { get; set; } = 0;

        /// <summary>
        /// Initialize the Label property from RibbonMarkup.ribbon
        /// </summary>
        /// <param name="label"></param>
        public void InitLabel(string label)
        {
            _label = label;
        }
    }
}
