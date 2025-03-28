//*****************************************************************************
//
//  File:       GalleryItemPropertySet.cs
//
//  Contents:   Helper class that wraps a gallery item IUISimplePropertySet.
//
//*****************************************************************************

using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class that wraps a gallery item IUISimplePropertySet.
    /// Should we have a IDisposable for IUIImage Release ???
    /// </summary>
    public sealed unsafe class GalleryItemPropertySet : AbstractPropertySet
    {
        private string? _label;
        private uint? _categoryId;
        private UIImage? _itemImage;

        /// <summary>
        /// Get or set the label
        /// </summary>
        public string? Label
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
        /// Get or set the Category Id
        /// </summary>
        public int CategoryId
        {
            get
            {
                return (int)_categoryId.GetValueOrDefault(PInvoke.UI_COLLECTION_INVALIDINDEX);
            }
            set
            {
                _categoryId = (uint)value;
            }
        }

        /// <summary>
        /// Get or set the Item Image
        /// </summary>
        public UIImage? ItemImage
        {
            get
            {
                return _itemImage;
            }
            set
            {
                _itemImage = value;
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
        private protected override unsafe HRESULT GetValueImpl(in PROPERTYKEY key, out PROPVARIANT value)
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

            if (key == RibbonProperties.CategoryId)
            {
                if (_categoryId.HasValue)
                {
                    value = (PROPVARIANT)_categoryId.Value; //InitPropVariantFromUInt32
                }
                else
                {
                    value = PROPVARIANT.Empty;
                }
                return HRESULT.S_OK;
            }

            if (key == RibbonProperties.ItemImage)
            {
                if (_itemImage != null)
                {
                    UIPropVariant.UIInitPropertyFromImage(RibbonProperties.ItemImage, _itemImage.UIImageHandle, out value);
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

        #endregion
    }
}
