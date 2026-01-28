using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;
using Windows.Win32.UI.Ribbon;

namespace Windows.Win32.System.Com.StructuredStorage
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Additional implementations for the CsWin32 PROPVARIANT for UIRibbon.
    /// Macros for UIRibbon
    /// </summary>
    /// <inheritdoc cref="PROPVARIANT"/>
    unsafe partial struct UIPropVariant
    {
#if UsedPInvoke
        private static void UsedFunctions()
        {
            PROPVARIANT pROPVARIANT = PROPVARIANT.Empty;
            uint puint32;
            //PInvoke.PropVariantToUInt32(&pROPVARIANT, &puint32);
            //BOOL pb;
            //PInvoke.PropVariantToBoolean(&pROPVARIANT, &pb);
            uint* prgn = null;
            uint cEle = 2;
            PInvoke.InitPropVariantFromUInt32Vector(prgn, cEle, &pROPVARIANT);
            //ColorPickerPropertiesProvider 4x

            uint* ppu;
            PInvoke.PropVariantToUInt32VectorAlloc(&pROPVARIANT, &ppu, &cEle);
            //ColorPickerPropertiesProvider 2x

            PWSTR pWSTR;
            PInvoke.PropVariantToStringAlloc(&pROPVARIANT, &pWSTR);
            //ColorPickerPropertiesProvider, FontControlProperiesprovider, FontControlEventArgs,
            //FontPropertyStore, GalleryItemEventArgs, StringValuePropertiesProvider 6x, 1x, 1x, 1x, 1x, 1x
            //UIPropVariant 2x

            PInvoke.PropVariantClear(&pROPVARIANT); //44x
            PWSTR* str1;
            PInvoke.PropVariantToStringVectorAlloc(&pROPVARIANT, &str1, &puint32); //ColorPickerPropertiesProvider 2x
            //PInvoke.PropVariantToUInt32WithDefault(&pROPVARIANT, cEle); //UICollection, GalleryItemsEventArgs 5x, 1x
            //PCWSTR szdefault = new PCWSTR();
            //PInvoke.PropVariantToStringWithDefault(&pROPVARIANT, szdefault); //UICollection
        }
#endif

        // PInvoke functions InitPropVariantxxx with Vectors (C# Arrays) or Strings make a copy in the CoTaskMem memory.
        // I have tested this feature by the input and output pointer values.
        // So one have no trouble with fixed or GCHandle pinned variable in C#.
        // That means that one can call GCHandle.Free() or end the fixed statement direct after PInvoke.InitPropVariantxxx
        // Have a look to some implementations in this file.
        // The copy in CoTaskMem will be free'd by a call PInvoke.PropVariantClear(ref PROPVARIANT)

        // (PInvoke)PropVariantClear calls CoTaskMemFree on the following types:
        //
        //     - VT_LPWSTR, VT_LPSTR, VT_CLSID (psvVal)
        //     - VT_BSTR_BLOB (bstrblobVal.pData)
        //     - VT_CF (pclipdata->pClipData, pclipdata)
        //     - VT_BLOB, VT_BLOB_OBJECT (blob.pData)
        //     - VT_STREAM, VT_STREAMED_OBJECT (pStream)
        //     - VT_VERSIONED_STREAM (pVersionedStream->pStream, pVersionedStream)
        //     - VT_STORAGE, VT_STORED_OBJECT (pStorage)
        //
        // If the VARTYPE is a VT_VECTOR, the contents are cleared as above and CoTaskMemFree is also called on
        // cabstr.pElems.
        //
        // https://learn.microsoft.com/windows/win32/api/oleauto/nf-oleauto-variantclear#remarks
        //
        //     - VT_BSTR (SysFreeString)
        //     - VT_DISPATCH / VT_UNKOWN (->Release(), if not VT_BYREF)

        internal static HRESULT UIInitPropertyFromBoolean(bool fVal, out PROPVARIANT pPropVar)
        {
            pPropVar = (PROPVARIANT)fVal; //InitPropVariantFromBoolean
            return HRESULT.S_OK;
        }

        internal static HRESULT UIInitPropertyFromBoolean(in PROPERTYKEY propertyKey, bool fVal, out PROPVARIANT pPropVar)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_BOOL;
            if (valid)
            {
                pPropVar = (PROPVARIANT)fVal; //InitPropVariantFromBoolean
                return HRESULT.S_OK;
            }
            else
            {
                pPropVar = PROPVARIANT.Empty;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static HRESULT UIInitPropertyFromUInt32(in PROPERTYKEY propertyKey, uint ulVal, out PROPVARIANT pPropVar)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_UI4;
            if (valid)
            {
                pPropVar = (PROPVARIANT)ulVal; //InitPropVariantFromUInt32
                return HRESULT.S_OK;
            }
            else
            {
                pPropVar = PROPVARIANT.Empty;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static HRESULT UIInitPropertyFromString(string psz, out PROPVARIANT pPropVar)
        {
            //check the string ???
            pPropVar = (PROPVARIANT)psz;
            //PROPVARIANT.InitPropVariantFromString(psz, out pPropVar);
            return HRESULT.S_OK;
        }

        internal static HRESULT UIInitPropertyFromString(in PROPERTYKEY propertyKey, string psz, out PROPVARIANT pPropVar)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_LPWSTR;
            if (valid)
            {
                //check the string ???
                pPropVar = (PROPVARIANT)psz;
                //PROPVARIANT.InitPropVariantFromString(psz, out pPropVar);
                return HRESULT.S_OK;
            }
            else
            {
                pPropVar = PROPVARIANT.Empty;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static unsafe HRESULT UIInitPropertyFromDecimal(in PROPERTYKEY propertyKey, Decimal decValue, out PROPVARIANT pPropVar)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_DECIMAL;
            fixed (PROPVARIANT* ppropvar = &pPropVar)
            {
                if (valid)
                {
                    ppropvar->Anonymous.decVal = new DECIMAL(decValue);
                    // Note we must set vt after writing the 16-byte decimal value (because it overwrites all 16 bytes)!
                    ppropvar->vt = VARENUM.VT_DECIMAL;
                    return HRESULT.S_OK;
                }
                else
                {
                    pPropVar = PROPVARIANT.Empty;
                    return HRESULT.E_INVALIDARG;
                }
            }
        }

        internal static unsafe HRESULT UIInitPropertyFromInterface(in PROPERTYKEY propertyKey, IUnknown* pUnk, out PROPVARIANT pPropVar)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_UNKNOWN;
            fixed (PROPVARIANT* ppropvar = &pPropVar)
            {
                if (valid)
                {
                    ppropvar->vt = VARENUM.VT_UNKNOWN;
                    ppropvar->data.punkVal = pUnk;
                    if (pUnk is not null)
                    {
                        uint count = pUnk->AddRef();
                    }
                    return HRESULT.S_OK;
                }
                else
                {
                    pPropVar = PROPVARIANT.Empty;
                    return HRESULT.E_INVALIDARG;
                }
            }
        }

        internal static HRESULT UIInitPropertyFromImage(in PROPERTYKEY propertyKey, IUIImage* pImage, out PROPVARIANT pPropVar)
        {
            return UIInitPropertyFromInterface(propertyKey, (IUnknown*)pImage, out pPropVar);
        }

        internal static unsafe HRESULT UIInitPropertyFromIUnknownArray(in PROPERTYKEY propertyKey, SAFEARRAY* psa, out PROPVARIANT pPropVar)
        {
            bool valid = (VARENUM)propertyKey.pid == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN);
            if (valid && (psa->fFeatures & ADVANCED_FEATURE_FLAGS.FADF_UNKNOWN) != 0)
            {
                HRESULT hr;
                PROPVARIANT propvar = PROPVARIANT.Empty;
                hr = PInvoke.SafeArrayCopy(psa, &propvar.Anonymous.Anonymous.Anonymous.parray);
                if (hr.Succeeded)
                {
                    propvar.vt = VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN;
                    pPropVar = propvar;
                    return hr;
                }
            }
            pPropVar = PROPVARIANT.Empty;
            return HRESULT.E_INVALIDARG;
        }

        internal static HRESULT UIPropertyToBoolean(in PROPERTYKEY propertyKey, PROPVARIANT* propvarIn, bool* pfRet)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_BOOL;
            if (valid)
            {
                *pfRet = (bool)*propvarIn; //PropVariantToBoolean
                return HRESULT.S_OK;
            }
            else
            {
                *pfRet = default;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static HRESULT UIPropertyToUInt32(in PROPERTYKEY propertyKey, PROPVARIANT* propvarIn, uint* pulVal)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_UI4;
            if (valid)
            {
                *pulVal = (uint)propvarIn; //PropVariantToUInt32
                return HRESULT.S_OK;
            }
            else
            {
                *pulVal = default;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static HRESULT UIPropertyToStringAlloc(PROPVARIANT* propvarIn, PWSTR* ppszOut)
        {
            return PInvoke.PropVariantToStringAlloc(propvarIn, ppszOut);
        }

        internal static HRESULT UIPropertyToStringAlloc(in PROPERTYKEY propertyKey, PROPVARIANT* propvarIn, PWSTR* ppszOut)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_LPWSTR;
            if (valid)
            {
                HRESULT hr;
                hr = PInvoke.PropVariantToStringAlloc(propvarIn, ppszOut);
                return hr;
            }
            else
            {
                *ppszOut = default;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static HRESULT UIPropertyToStringAlloc(in PROPERTYKEY propertyKey, PROPVARIANT* propvarIn, out string ppszOut)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_LPWSTR;
            if (valid)
            {
                PWSTR pszLocal;
                HRESULT hr;
                hr = PInvoke.PropVariantToStringAlloc(propvarIn, &pszLocal);
                ppszOut = pszLocal.ToStringAndCoTaskMemFree()!;
                return hr;
            }
            else
            {
                ppszOut = string.Empty;
                return HRESULT.E_INVALIDARG;
            }
        }

        internal static HRESULT UIPropertyToDecimal(in PROPERTYKEY propertyKey, in PROPVARIANT propvarIn, out Decimal pDecValue)
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_DECIMAL;
            if (valid && propvarIn.vt == VARENUM.VT_DECIMAL)
            {
                pDecValue = (decimal)propvarIn;
                return HRESULT.S_OK;
            }
            pDecValue = 0;
            return HRESULT.E_INVALIDARG;
        }

        internal static unsafe HRESULT UIPropertyToInterface<TInterface>(in PROPERTYKEY propertyKey, in PROPVARIANT propvarIn, TInterface** ppObj) where TInterface : unmanaged, IComIID
        {
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_UNKNOWN;
            if (valid && propvarIn.vt == VARENUM.VT_UNKNOWN)
            {
                if ((nint)propvarIn.data.punkVal != 0)
                {
                    HRESULT hr;
                    fixed (Guid* pInterfaceGuid = &TInterface.Guid)
                        hr = propvarIn.data.punkVal->QueryInterface(pInterfaceGuid, (void**)ppObj);
                    return hr;
                }
                return HRESULT.S_OK;
            }
            return HRESULT.E_INVALIDARG;
        }

        internal static unsafe HRESULT UIPropertyToInterface<TInterface>(in PROPERTYKEY propertyKey, in PROPVARIANT propvarIn, out TInterface* ppObj) where TInterface : unmanaged, IComIID
        {
            ppObj = null;
            bool valid = (VARENUM)propertyKey.pid == VARENUM.VT_UNKNOWN;
            if (valid && propvarIn.vt == VARENUM.VT_UNKNOWN)
            {
                if ((nint)propvarIn.data.punkVal != 0)
                {
                    HRESULT hr;
                    TInterface* cpInterface;
                    fixed (Guid* pInterfaceGuid = &TInterface.Guid)
                        hr = propvarIn.data.punkVal->QueryInterface(pInterfaceGuid, (void**)&cpInterface);
                    ppObj = cpInterface;
                    return hr;
                }
                return HRESULT.S_OK;
            }
            return HRESULT.E_INVALIDARG;
        }

        internal static HRESULT UIPropertyToImage(in PROPERTYKEY propertyKey, in PROPVARIANT propvarIn, IUIImage** ppImage)
        {
            HRESULT hr = UIPropertyToInterface<IUIImage>(propertyKey, propvarIn, ppImage);
            return hr;
        }

        internal static unsafe HRESULT UIPropertyToIUnknownArrayAlloc(in PROPERTYKEY propertyKey, in PROPVARIANT propvarIn, SAFEARRAY** ppsa)
        {
            bool valid = (VARENUM)propertyKey.pid == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN);
            if (valid && propvarIn.vt == (VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN))
            {
                HRESULT hr;
                hr = PInvoke.SafeArrayCopy(propvarIn.data.parray, ppsa);
                return hr;
            }
            ppsa = null;
            return HRESULT.E_INVALIDARG;
        }
    }
}
