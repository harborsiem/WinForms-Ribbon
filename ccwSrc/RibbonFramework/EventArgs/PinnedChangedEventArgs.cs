using System;
using System.Collections.Generic;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;

namespace WinForms.Ribbon
{
    //ChangedPinnedIndices
    //If Pinned changed by user first get the key RibbonProperties.RecentItems
    /// <summary>
    /// The EventArgs for RibbonRecentItems
    /// </summary>
    public sealed class PinnedChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="changedPinnedIndices"></param>
        private PinnedChangedEventArgs(IList<int> changedPinnedIndices)
        {
            ChangedPinnedIndices = changedPinnedIndices;
        }

        /// <summary>
        /// Index List to RibbonRecentItems.RecentItems of changed Pinned symbols by UI user.
        /// Usage: RecentItemsPropertySet set = RibbonRecentItems.RecentItems[ChangedPinnedIndices[i]];
        /// set.Pinned one can get the changed Pinned status
        /// </summary>
        public IList<int> ChangedPinnedIndices { get; private set; }

        /// <summary>
        /// Creates a RecentItemsEventArgs from ExecuteEventArgs of a RibbonRecentItems event
        /// </summary>
        /// <param name="sender">Parameters from event: sender = RibbonControl</param>
        /// <param name="e">Parameters from event: ExecuteEventArgs</param>
        /// <returns></returns>
        internal static unsafe PinnedChangedEventArgs? Create(object sender, ExecuteEventArgs e)
        {
            if (!(sender is RibbonRecentItems recentItems))
                throw new ArgumentException("Not a RibbonRecentItems", nameof(sender));
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            if (e.Key.HasValue && e.CurrentValue.HasValue)
            {
                return Create(recentItems, e.Key.Value, e.CurrentValue.Value, e.CommandExecutionProperties);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ribbonRecentItems"></param>
        /// <param name="key"></param>
        /// <param name="currentValue"></param>
        /// <param name="commandExecutionProperties"></param>
        /// <returns></returns>
        internal static unsafe PinnedChangedEventArgs Create(RibbonRecentItems ribbonRecentItems, in PROPERTYKEY key, in PROPVARIANT currentValue, IUISimplePropertySet* commandExecutionProperties)
        {
            IList<int>? changedPinnedIndices = new List<int>();
            if (key == RibbonProperties.RecentItems)
            {
                if (currentValue.vt == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN))
                {
                    // go over recent items
                    SAFEARRAY* psa;
                    int lBound;
                    int uBound;
                    UIPropVariant.UIPropertyToIUnknownArrayAlloc(RibbonProperties.RecentItems, currentValue, &psa);
                    uint dim = PInvoke.SafeArrayGetDim(psa);
                    PInvoke.SafeArrayGetLBound(psa, 1U, &lBound);
                    PInvoke.SafeArrayGetUBound(psa, 1U, &uBound);
                    //checks for dim = 1, lBound = 0 ?
                    for (int i = 0; i < (uBound + 1); i++)
                    {
                        IUnknown* unk;
                        PInvoke.SafeArrayGetElement(psa, &i, &unk);
                        if (unk is null)
                            break;
                        using ComScope<IUISimplePropertySet> cpIUISimplePropertySet = ComScope<IUISimplePropertySet>.QueryFrom(unk);
//                        unk->AddRef();
//                        uint refCount = unk->Release();
//#if DEBUG
//                        //refCount = 5 here
//                        Debug.WriteLine("IUnknown refCount : " + refCount);
//#endif

                        if (!cpIUISimplePropertySet.IsNull)
                        {
                            int index = GetChangedPinned(ribbonRecentItems, cpIUISimplePropertySet, i);
                            if (index >= 0)
                                changedPinnedIndices.Add(index);
                            //cpIUISimplePropertySet.Value->AddRef();
//                            refCount = cpIUISimplePropertySet.Value->Release();
//#if DEBUG
//                            //refCount = 5 here
//                            Debug.WriteLine("IUISimplePropertySet refCount : " + refCount);
//#endif
                        }
                    }
                    HRESULT hr = PInvoke.SafeArrayDestroy(psa);
                }
            }
            PinnedChangedEventArgs e = new PinnedChangedEventArgs(changedPinnedIndices);
            return e;
        }

        /// <summary>
        /// returns an index to RecentItems if Pinned changed or -1.
        /// </summary>
        /// <param name="ribbonRecentItems"></param>
        /// <param name="commandExecutionProperties"></param>
        /// <param name="index">RibbonRecentItems.RecentItems index</param>
        /// <returns></returns>
        private static unsafe int GetChangedPinned(RibbonRecentItems ribbonRecentItems, IUISimplePropertySet* commandExecutionProperties, int index)
        {
            if (index >= ribbonRecentItems.RecentItems.Count)
                throw new ArgumentException("Internal Error", nameof(index));
            RecentItemsPropertySet propSet = ribbonRecentItems.RecentItems[index];
            bool oldPinned = propSet.Pinned;

            // Get only the Pinned property, because the string values cannot be changed by user

            // get item pinned value
            PROPVARIANT propvar;
            // If Pinning is not set then output is null and HRESULT is not S_OK
            HRESULT hr;
            fixed (PROPERTYKEY* pKeyPinned = &RibbonProperties.Pinned)
                hr = commandExecutionProperties->GetValue(pKeyPinned, &propvar);
            if (hr == HRESULT.S_OK)
            {
                bool result = (bool)propvar; //PropVariantToBoolean
                if (oldPinned != result)
                {
                    propSet.Pinned = result;
                    return index;
                }
            }
            return -1;
        }
    }
}
