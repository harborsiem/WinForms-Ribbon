//*****************************************************************************
//
//  File:       AbstractPropertySet.cs
//
//  Contents:   Helper abstract class that wraps a qat, gallery simple property set.
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
    /// Helper abstract class that wraps a gallery item or command IUISimplePropertySet.
    /// This abstract class is a constrain for UICollection of T
    /// </summary>
    public abstract class AbstractPropertySet : IUISimplePropertySet.Interface
    {
        /// <summary>
        /// Retrieves the stored value of a given property
        /// </summary>
        /// <param name="key">The Property Key of interest.</param>
        /// <param name="value">When this method returns, contains a pointer to the value for key.</param>
        /// <returns></returns>
        private protected abstract unsafe HRESULT GetValueImpl(in PROPERTYKEY key, out PROPVARIANT value);
		
        #region IUISimplePropertySet Members

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
    }
}
