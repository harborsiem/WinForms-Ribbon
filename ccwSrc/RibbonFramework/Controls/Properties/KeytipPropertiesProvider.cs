//*****************************************************************************
//
//  File:       KeytipPropertiesProvider.cs
//
//  Contents:   Definition for keytip properties provider 
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
    /// Definition for keytip properties provider interface
    /// </summary>
    public interface IKeytipPropertiesProvider
    {
        /// <summary>
        /// Keytip property
        /// </summary>
        string Keytip { get; set; }
    }

    /// <summary>
    /// Implementation of IKeytipPropertiesProvider
    /// </summary>
    public sealed unsafe class KeytipPropertiesProvider : BasePropertiesProvider, IKeytipPropertiesProvider
    {
        /// <summary>
        /// KeytipPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        /// <param name="item">ribbon control</param>
        public KeytipPropertiesProvider(RibbonStrip ribbon, uint commandId, RibbonStripItem item)
            : base(ribbon, commandId)
        {
            _item = item;
            // add supported properties
            _supportedProperties.Add(RibbonProperties.Keytip);
        }

        private readonly RibbonStripItem _item;
        private string _keytip;

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.Keytip)
            {
                if (_keytip != null)
                {
                    UIPropVariant.UIInitPropertyFromString(_keytip, out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        #region IKeytipPropertiesProvider Members

        /// <summary>
        /// Keytip property
        /// </summary>
        public string Keytip
        {
            get
            {
                if (_keytip == null)
                {
                    if (_item.ResourceIds != null && _item.ResourceIds.KeytipId >= 2)
                        _keytip = _ribbon.LoadString(_item.ResourceIds.KeytipId);
                }
                return _keytip;
            }
            set
            {
                _keytip = value;
                if (_ribbon.Framework != null)
                {
                    HRESULT hr;
                    fixed (PROPERTYKEY* pKeytip = &RibbonProperties.Keytip)
                        hr = _ribbon.Framework->InvalidateUICommand(_commandId, UI_INVALIDATIONS.UI_INVALIDATIONS_PROPERTY, pKeytip);
                }
            }
        }

        #endregion
    }
}
