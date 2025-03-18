//*****************************************************************************
//
//  File:       QatCommandPropertySet.cs
//
//  Contents:   Helper class that wraps a qat command IUISimplePropertySet.
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
    /// Helper class that wraps a qat command IUISimplePropertySet.
    /// </summary>
    public sealed class QatCommandPropertySet : AbstractPropertySet
    {
        private uint? _commandId;

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

            Debug.WriteLine(string.Format("Class {0} does not support property: {1}.", GetType(), RibbonProperties.GetPropertyKeyName(key)));

            value = PROPVARIANT.Empty;
            return HRESULT.E_NOTIMPL;
        }

        #endregion
    }
}
