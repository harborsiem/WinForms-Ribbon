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
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.System.Variant;

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
        /// ctor
        /// </summary>
        public GalleryCommandPropertySet() { }

        internal unsafe GalleryCommandPropertySet(ComScope<IUISimplePropertySet> cpIUISimplePropertySet)
        {
            PROPVARIANT propvar = PROPVARIANT.Empty;
            HRESULT hr;
            fixed (PROPERTYKEY* pKeyCommandId = &RibbonProperties.CommandId)
                hr = cpIUISimplePropertySet.Value->GetValue(pKeyCommandId, &propvar);
            uint commandId = 0;
            if (propvar.vt == VARENUM.VT_UI4)
                commandId = (uint)propvar;
            //commandId = PInvoke.PropVariantToUInt32WithDefault(&propvar, 0);

            propvar = PROPVARIANT.Empty;
            fixed (PROPERTYKEY* pKeyCategoryId = &RibbonProperties.CategoryId)
                hr = cpIUISimplePropertySet.Value->GetValue(pKeyCategoryId, &propvar);
            uint categoryId = PInvoke.UI_COLLECTION_INVALIDINDEX;
            if (propvar.vt == VARENUM.VT_UI4)
                categoryId = (uint)propvar;
            //categoryId = PInvoke.PropVariantToUInt32WithDefault(&propvar, PInvoke.UI_COLLECTION_INVALIDINDEX);

            propvar = PROPVARIANT.Empty;
            fixed (PROPERTYKEY* pKeyCommandType = &RibbonProperties.CommandType)
                hr = cpIUISimplePropertySet.Value->GetValue(pKeyCommandType, &propvar);
            UI_COMMANDTYPE commandType = UI_COMMANDTYPE.UI_COMMANDTYPE_UNKNOWN;
            if (propvar.vt == VARENUM.VT_UI4)
                commandType = (UI_COMMANDTYPE)(uint)propvar;
            //commandType = (UI_COMMANDTYPE)PInvoke.PropVariantToUInt32WithDefault(&propvar, 0);
            CommandId = commandId;
            CommandType = (CommandType)commandType;
            CategoryId = (int)categoryId;
        }

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
