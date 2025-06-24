//*****************************************************************************
//
//  File:       RecentItemsPropertySet.cs
//
//  Contents:   Helper class that wraps a recent items simple property set.
//
//*****************************************************************************

using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a recent items simple property set.
    /// </summary>
    public sealed unsafe class RecentItemsPropertySet : IUISimplePropertySet.Interface
    {
        private string _label;
        private string _labelDescription;
        private bool? _pinned;

        /// <summary>
        /// ctor
        /// </summary>
        public RecentItemsPropertySet() { }

        //internal unsafe RecentItemsPropertySet(IUISimplePropertySet* cpIUISimplePropertySet)
        //{

        //}

        /// <summary>
        /// This is the label as it will appear on the ribbon.
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
            }
        }

        /// <summary>
        /// A longer description. This description is used right side of the application menu
        /// </summary>
        public string LabelDescription
        {
            get
            {
                return _labelDescription;
            }
            set
            {
                _labelDescription = value;
            }
        }

        /// <summary>
        /// The pinned status
        /// </summary>
        public bool Pinned
        {
            get
            {
                return _pinned.GetValueOrDefault();
            }
            set
            {
                _pinned = value;
            }
        }

        /// <summary>
        /// Gets or sets the object that contains to this PropertySet
        /// Additional object for the user
        /// </summary>
        public object? Tag { get; set; }

        #region IUISimplePropertySet Members

        /// <summary>
        /// Retrieves the stored value of a given property
        /// </summary>
        /// <param name="key">The Property Key of interest.</param>
        /// <param name="value">When this method returns, contains a pointer to the value for key.</param>
        /// <returns></returns>
        private unsafe HRESULT GetValueImpl(in PROPERTYKEY key, out PROPVARIANT value)
        {
            if (key == RibbonProperties.Label)
            {
                if ((_label == null) || (_label.Trim() == string.Empty))
                {
                    value = PROPVARIANT.Empty;
                }
                else
                {
                    UIPropVariant.UIInitPropertyFromString(_label, out value);
                }
                return HRESULT.S_OK;
            }

            if (key == RibbonProperties.LabelDescription)
            {
                if ((_labelDescription == null) || (_labelDescription.Trim() == string.Empty))
                {
                    value = PROPVARIANT.Empty;
                }
                else
                {
                    UIPropVariant.UIInitPropertyFromString(_labelDescription, out value);
                }
                return HRESULT.S_OK;
            }

            if (key == RibbonProperties.Pinned)
            {
                if (_pinned.HasValue)
                {
                    value = (PROPVARIANT)_pinned.Value; //UIInitPropertyFromBoolean
                }
                else
                {
                    value = PROPVARIANT.Empty;
                }
                return HRESULT.S_OK;
            }

            Debug.WriteLine(string.Format("Class {0} does not support property: {1}.", GetType().ToString(), RibbonProperties.GetPropertyKeyName(key)));

            value = PROPVARIANT.Empty;
            return HRESULT.E_NOTIMPL;
        }

        /// <summary>
        /// Retrieves the stored value of a given property
        /// </summary>
        /// <param name="key">The Property Key of interest.</param>
        /// <param name="value">When this method returns, contains a pointer to the value for key.</param>
        /// <returns></returns>
        unsafe HRESULT IUISimplePropertySet.Interface.GetValue(PROPERTYKEY* key, PROPVARIANT* value)
        {
            return GetValueImpl(*key, out *value);
        }

        #endregion

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public RecentItemsPropertySet Clone()
        {
            RecentItemsPropertySet result = new RecentItemsPropertySet()
            {
                Label = this.Label,
                LabelDescription = this.LabelDescription,
                Pinned = this.Pinned,
                Tag = this.Tag
            };
            return result;
        }
    }
}
