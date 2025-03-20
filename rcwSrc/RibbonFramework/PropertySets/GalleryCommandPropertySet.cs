//*****************************************************************************
//
//  File:       GalleryCommandPropertySet.cs
//
//  Contents:   Helper class that wraps a gallery command IUISimplePropertySet.
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
    /// Helper class that wraps a gallery command IUISimplePropertySet.
    /// </summary>
    public sealed class GalleryCommandPropertySet : AbstractPropertySet
    {
        private uint? _commandId;
        private UI_COMMANDTYPE? _commandType;
        private uint? _categoryId;

        /// <summary>
        /// Get or set the Command Id
        /// </summary>
        public uint CommandId
        {
            get
            {
                return _commandId.GetValueOrDefault();
            }
            set
            {
                _commandId = value;
            }
        }

        /// <summary>
        /// Get or set the Command Type
        /// </summary>
        public CommandType CommandType
        {
            get
            {
                return (CommandType)_commandType.GetValueOrDefault();
            }
            set
            {
                _commandType = (UI_COMMANDTYPE)value;
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
            if (key == RibbonProperties.CommandId)
            {
                if (_commandId.HasValue)
                {
                    value = (PROPVARIANT)_commandId.Value; //InitPropVariantFromUInt32
                }
                else
                {
                    value = PROPVARIANT.Empty;
                }
                return HRESULT.S_OK;
            }
            
            if (key == RibbonProperties.CommandType)
            {
                if (_commandType.HasValue)
                {
                    value = (PROPVARIANT)(uint)_commandType.Value; //InitPropVariantFromUInt32
                }
                else
                {
                    value = PROPVARIANT.Empty;
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

            Debug.WriteLine(string.Format("Class {0} does not support property: {1}.", GetType(), RibbonProperties.GetPropertyKeyName(key)));

            value = PROPVARIANT.Empty;
            return HRESULT.E_NOTIMPL;
        }

        #endregion
    }
}
