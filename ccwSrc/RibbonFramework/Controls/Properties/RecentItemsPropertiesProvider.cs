#define UseArrayScope
//*****************************************************************************
//
//  File:       RecentItemsPropertiesProvider.cs
//
//  Contents:   Definition for recent items properties provider 
//
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
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
        /// <param name="ribbon">Parent RibbonStrip</param>
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
                        if (currentValue->vt == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN))
                        {
                            MaxCount = GetSingleDimArrayCount(currentValue);
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

        private static unsafe int GetSingleDimArrayCount(PROPVARIANT* propVarIn)
        {
#if UseArrayScope
            ComSafeArrayScope<IUISimplePropertySet> scope = new(propVarIn->data.parray);
            return scope.Length;
#else
            return (int)propVarIn->data.parray->GetBounds().cElements;
#endif
        }

        //Maybe we have to resize the list of RecentItems to MaxCount
        private unsafe HRESULT NewValueHelper(out PROPVARIANT newValue)
        {
            int count = 0;
            for (int i = 0; i < _recentItems.Count; i++)
            {
                if (_recentItems[i] == null)
                    break;
                count++;
            }
#if UseArrayScope
            using var scope = new ComSafeArrayScope<IUISimplePropertySet>((uint)count);
            for (int i = 0; i < count; i++)
            {
                using (var item = new ComScope<IUISimplePropertySet>((IUISimplePropertySet*)Marshal.GetIUnknownForObject(_recentItems[i])))
                {
                    scope[i] = item;
                }
            }
            UIPropVariant.UIInitPropertyFromIUnknownArray(RibbonProperties.RecentItems, scope, out newValue);
            return HRESULT.S_OK;
#else
            HRESULT hr;
            SAFEARRAYBOUND bounds = new()
            {
                cElements = (uint)count,
                lLbound = 0
            };

            SAFEARRAY* psa = PInvoke.SafeArrayCreate(VARENUM.VT_UNKNOWN, 1, &bounds);
            if (psa != null)
            {
                for (int i = 0; i < count; i++)
                {
                    IUnknown* pUnk = null;
                    pUnk = (IUnknown*)Marshal.GetIUnknownForObject(_recentItems[i]);
                    hr = PInvoke.SafeArrayPutElement(psa, &i, pUnk);
                    uint refCount = pUnk->Release();
//#if DEBUG
//                        //refCount = 1 here
//                        Debug.WriteLine("Put IUnknown refCount: " + refCount);
//#endif
                    if (hr != HRESULT.S_OK)
                        break;
                }
                //fixed (PROPVARIANT* newValueLocal = &newValue)
                hr = UIPropVariant.UIInitPropertyFromIUnknownArray(RibbonProperties.RecentItems, psa, out newValue);
                PInvoke.SafeArrayDestroy(psa);
                return hr;
            }
            newValue = PROPVARIANT.Empty;
            return HRESULT.S_OK;
#endif
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
