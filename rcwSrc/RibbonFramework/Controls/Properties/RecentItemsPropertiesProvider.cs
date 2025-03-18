//*****************************************************************************
//
//  File:       RecentItemsPropertiesProvider.cs
//
//  Contents:   Definition for recent items properties provider 
//
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Shell.PropertiesSystem;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;
using Windows.Win32.System.Com.StructuredStorage;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for recent items properties provider interface
    /// </summary>
    public interface IRecentItemsPropertiesProvider
    {
        /// <summary>
        /// Recent items property
        /// </summary>
        IList<RecentItemsPropertySet> RecentItems { get; }

        /// <summary>
        /// Maximal count of RecentItems
        /// </summary>
        int MaxCount { get; }
    }

    /// <summary>
    /// Implementation of IRecentItemsPropertiesProvider
    /// </summary>
    public sealed class RecentItemsPropertiesProvider : BasePropertiesProvider, IRecentItemsPropertiesProvider
    {
        /// <summary>
        /// RecentItemsPropertiesProvider ctor
        /// </summary>
        /// <param name="ribbon">parent ribbon</param>
        /// <param name="commandId">ribbon control command id</param>
        public RecentItemsPropertiesProvider(RibbonStrip ribbon, uint commandId)
            : base(ribbon, commandId)
        {
            // add supported properties
            _supportedProperties.Add(RibbonProperties.RecentItems);
            MaxCount = -1;
        }

        private IList<RecentItemsPropertySet> _recentItems = new List<RecentItemsPropertySet>();

        /// <summary>
        /// Handles IUICommandHandler.UpdateProperty function for the supported properties
        /// </summary>
        /// <param name="key">The Property Key to update</param>
        /// <param name="currentValue">A pointer to the current value for key. This parameter can be null</param>
        /// <param name="newValue">When this method returns, contains a pointer to the new value for key</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise</returns>
        private protected override unsafe HRESULT UpdatePropertyImpl(in PROPERTYKEY key, PROPVARIANT* currentValue, out PROPVARIANT newValue)
        {
            if (key == RibbonProperties.RecentItems)
            {
                if (currentValue is not null)
                {
                    if (MaxCount == -1)
                    {
                        PROPVARIANT currentValueLocal = *currentValue;
                        if (currentValueLocal.vt == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN))
                        {
                            MaxCount = GetSingleDimArrayCount(currentValueLocal);
                        }
                    }
                }
                if (_recentItems != null)
                {
                    NewValueHelper(out newValue);
                    return HRESULT.S_OK;
                }
            }
            fixed (PROPVARIANT* newValueLocal = &newValue) { }

            return HRESULT.S_OK;
        }

        private static unsafe int GetSingleDimArrayCount(in PROPVARIANT propVarIn)
        {
            if (propVarIn.vt == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN))
            {
                SAFEARRAY* psa = propVarIn.Anonymous.Anonymous.Anonymous.parray;
                if (PInvoke.SafeArrayGetDim(psa) == 1)
                {
                    int lBound;
                    int uBound;
                    PInvoke.SafeArrayGetLBound(psa, 1, &lBound);
                    PInvoke.SafeArrayGetUBound(psa, 1, &uBound);
                    int count = uBound - lBound + 1;
                    return count;
                }
            }
            return 0;
        }

        //Maybe we have to resize the list of RecentItems to MaxCount
        //RecentItemsPropertySet[] array = _recentItems.ToArray();
        //if (array.Length > MaxCount)
        //    Array.Resize(ref array, MaxCount);
        private unsafe HRESULT NewValueHelper(out PROPVARIANT newValue)
        {
            HRESULT hr;
            SAFEARRAYBOUND bounds = new()
            {
                cElements = (uint)_recentItems.Count,
                lLbound = 0
            };

            SAFEARRAY* psa = PInvoke.SafeArrayCreate(VARENUM.VT_UNKNOWN, 1, &bounds);
            if (psa != null)
            {
                uint current = 0;
                for (int i = 0; i < _recentItems.Count; i++)
                {
                    IntPtr pUnk = IntPtr.Zero;
                    if (_recentItems[i] != null)
                    {
                        pUnk = Marshal.GetIUnknownForObject(_recentItems[i]);
                        hr = PInvoke.SafeArrayPutElement(psa, &i, (void*)pUnk);
                        int c = Marshal.Release(pUnk); //pUnk->Release();
                        //#if DEBUG
                        //  Debug.WriteLine("Put IUnknown count: " + c);
                        //#endif
                        if (hr != HRESULT.S_OK)
                            break;
                        else
                            current++;
                    }
                    else
                    {
                        break;
                    }
                }
                // We will only populate items up to before the first failed item, and discard the rest.
                SAFEARRAYBOUND sab = new SAFEARRAYBOUND()
                {
                    cElements = current,
                    lLbound = 0
                };
                PInvoke.SafeArrayRedim(psa, &sab);
                hr = UIPropVariant.UIInitPropertyFromIUnknownArray(RibbonProperties.RecentItems, psa, out newValue);
                PInvoke.SafeArrayDestroy(psa);
                return hr;
            }
            newValue = PROPVARIANT.Empty;
            return HRESULT.S_OK;
        }

        #region IRecentItemsPropertiesProvider Members

        /// <summary>
        /// Recent items property
        /// </summary>
        public IList<RecentItemsPropertySet> RecentItems
        {
            get
            {
                return _recentItems;
            }
            private set
            {
                _recentItems = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxCount
        {
            get;
            private set;
        }

        #endregion
    }
}
