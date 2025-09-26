//#define Unused
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;
using static Windows.Win32.System.Variant.VARENUM;
using FILETIME = Windows.Win32.Foundation.FILETIME;

namespace Windows.Win32.System.Com.StructuredStorage
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Additional implementations for the CsWin32 PROPVARIANT.
    /// Most code is taken from Microsoft VARIANT implementation.
    /// </summary>
    /// <inheritdoc cref="PROPVARIANT"/>
    unsafe partial struct PROPVARIANT : IDisposable
    {
        // PInvoke functions InitPropVariantxxx with Vectors (C# Arrays) or Strings do a copy to the CoTaskMem memory.
        // I have tested this feature by the input and output pointer values.
        // So one have no trouble with fixed or GCHandle pinned variable in C# after the call InitPropVariantxxx.
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

        public static PROPVARIANT Empty { get; } //PropVariantInit

        public static PROPVARIANT True { get; } = CreateBoolVariant(value: true);

        public static PROPVARIANT False { get; } = CreateBoolVariant(value: false);

        private static PROPVARIANT CreateBoolVariant(bool value)
        {
            PROPVARIANT variant = new() { vt = VT_BOOL };
            variant.data.boolVal = value ? VARIANT_BOOL.VARIANT_TRUE : VARIANT_BOOL.VARIANT_FALSE;
            return variant;
        }

        public bool IsEmpty => vt == VT_EMPTY && data.uhVal == 0;

        public VARENUM Type => vt & VT_TYPEMASK;

        public bool Byref => vt.HasFlag(VT_BYREF);

        [UnscopedRef]
        public ref VARENUM vt => ref Anonymous.Anonymous.vt;

        [UnscopedRef]
        public ref _Anonymous_e__Union._Anonymous_e__Struct_unmanaged._Anonymous_e__Union_unmanaged data => ref Anonymous.Anonymous.Anonymous;

        public void Clear()
        {
            // PropVariantClear is essentially a superset of VariantClear it calls CoTaskMemFree on the following types:
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
            //     - VT_DISPATCH / VT_UNKNOWN (->Release(), if not VT_BYREF)

            fixed (PROPVARIANT* t = &this)
            {
                PInvoke.PropVariantClear(ref *t);
            }

            Anonymous.Anonymous.vt = VT_EMPTY;
            Anonymous.Anonymous.Anonymous = default;
        }

        public void Dispose() => Clear();

        public object? ToObject()
        {
            if (vt == VT_DECIMAL)
            {
                return (decimal)Anonymous.decVal;
            }

            fixed (PROPVARIANT* thisVariant = &this)
            {
                void* data = &thisVariant->Anonymous.Anonymous.Anonymous;
                if (Byref)
                {
                    data = *(void**)data;

                    // CLR allows VT_EMPTY/NULL | VT_BYREF to have no data.
                    // In other cases, the variant is invalid.
                    if (data is null && !(Type == VT_EMPTY || Type == VT_NULL))
                    {
                        throw new ArgumentException("Invalid Variant");
                    }
                }

                // Note that the following check also covers VT_ILLEGAL.
                if ((vt & ~(VT_BYREF | VT_ARRAY | VT_VECTOR)) >= (VARENUM)0x80)
                {
                    throw new InvalidOleVariantTypeException();
                }

                if ((vt & VT_VECTOR) != 0)
                {
                    return ToVector(thisVariant->data.ca, vt);
                }

                if ((vt & VT_ARRAY) != 0)
                {
                    return ToArray(*(SAFEARRAY**)data, vt);
                }

                return ToObject(Type, Byref, data);
            }
        }

        private static object? ToObject(VARENUM type, bool byRef, void* data)
        {
            switch (type)
            {
                case VT_EMPTY:
                    if (byRef)
                    {
                        // CLR returns VT_EMPTY | VT_BYREF data as nuint.
                        return IntPtr.Size == 8 ? (ulong)data : (object)(uint)data;
                    }

                    return null;
                case VT_NULL:
                    return Convert.DBNull;
                case VT_I1:
                    return *((sbyte*)data);
                case VT_UI1:
                    return *((byte*)data);
                case VT_I2:
                    return *((short*)data);
                case VT_UI2:
                    return *((ushort*)data);
                case VT_I4:
                case VT_INT:
                case VT_ERROR:
                case VT_HRESULT:
                    return *((int*)data);
                case VT_UI4:
                case VT_UINT:
                    return *((uint*)data);
                case VT_I8:
                    return *((long*)data);
                case VT_UI8:
                    return *((ulong*)data);
                case VT_R4:
                    return *((float*)data);
                case VT_R8:
                    return *((double*)data);
                case VT_CY:
                    long cyVal = *((long*)data);
                    return decimal.FromOACurrency(cyVal);
                case VT_DATE:
                    double date = *((double*)data);
                    return DateTime.FromOADate(date);
                case VT_BSTR:
                case VT_LPWSTR:
                    return Marshal.PtrToStringUni(*(IntPtr*)data);
                case VT_LPSTR:
                    return Marshal.PtrToStringAnsi(*(IntPtr*)data);
                case VT_DISPATCH:
                case VT_UNKNOWN:
                    IUnknown* pInterface = *(IUnknown**)data;
                    if (pInterface is null)
                    {
                        return null;
                    }

                    return Marshal.GetObjectForIUnknown((nint)pInterface);
                case VT_DECIMAL:
                    return (decimal)*((DECIMAL*)data); //@@@ ??? wrong address for decimal!, but decimal is handled before
                case VT_BOOL:
                    return (*(VARIANT_BOOL*)data) != VARIANT_BOOL.VARIANT_FALSE;
                case VT_VARIANT:
                    // We only support VT_VARIANT | VT_BYREF.
                    if (!byRef)
                    {
                        break;
                    }

                    // BYREF VARIANTS are not allowed to be nested.
                    PROPVARIANT* pVariant = (PROPVARIANT*)data;
                    if (pVariant->Byref)
                    {
                        throw new InvalidOleVariantTypeException();
                    }

                    return pVariant->ToObject();
                case VT_CLSID:
                    // We only support VT_CLSID.
                    // This is the type of InitPropVariantFromCLSID.
                    if (byRef)
                    {
                        break;
                    }

                    return **((Guid**)data);
                case VT_FILETIME:
                    // We only support VT_FILETIME.
                    // This is the type of InitPropVariantFromFILETIME.
                    if (byRef)
                    {
                        break;
                    }

                    return (*(FILETIME*)data).ToDateTime();
                case VT_VOID:
                    return null;
                    //IStream* (VT_STREAM), IStorage* (VT_STORAGE), VERSIONEDSTREAM* (VT_STREAMED_OBJECT ?)
            }

            throw new ArgumentException(string.Format(SR.COM2UnhandledVT, type));
        }

        private static Array? ToArray(SAFEARRAY* psa, VARENUM vt)
        {
            if (psa is null)
            {
                return null;
            }

            VARENUM arrayType = vt & ~VT_ARRAY;

            if (arrayType == VT_RECORD)
            {
                // Exit early so we don't have to consider this in the helper methods.
                throw new ArgumentException(string.Format(SR.COM2UnhandledVT, arrayType));
            }

            Array array = CreateArrayFromSafeArray(psa, arrayType);

            GCHandle pin = default;

            try
            {
                pin = GCHandle.Alloc(array, GCHandleType.Pinned);
            }
            catch (ArgumentException)
            {
            }

            HRESULT hr = PInvoke.SafeArrayLock(psa); //@@@ PInvokeCore
            Debug.Assert(hr == HRESULT.S_OK);

            try
            {
                if (array.Rank == 1)
                {
                    switch (arrayType)
                    {
                        case VT_I1:
                            new Span<sbyte>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<sbyte>(array));
                            break;
                        case VT_UI1:
                            new Span<byte>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<byte>(array));
                            break;
                        case VT_I2:
                            new Span<short>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<short>(array));
                            break;
                        case VT_UI2:
                            new Span<ushort>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<ushort>(array));
                            break;
                        case VT_I4:
                        case VT_INT:
                            new Span<int>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<int>(array));
                            break;
                        case VT_UI4:
                        case VT_UINT:
                        case VT_ERROR: // Not explicitly mentioned in the docs but trivial to implement.
                            new Span<uint>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<uint>(array));
                            break;
                        case VT_I8:
                            new Span<long>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<long>(array));
                            break;
                        case VT_UI8:
                            new Span<ulong>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<ulong>(array));
                            break;
                        case VT_R4:
                            new Span<float>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<float>(array));
                            break;
                        case VT_R8:
                            new Span<double>(psa->pvData, array.Length)
                                .CopyTo(GetSpan<double>(array));
                            break;
                        case VT_BOOL:
                            {
                                Span<VARIANT_BOOL> data = new(psa->pvData, array.Length);
                                var result = GetSpan<bool>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = data[i] != VARIANT_BOOL.VARIANT_FALSE;
                                }

                                break;
                            }

                        case VT_DECIMAL:
                            {
                                Span<DECIMAL> data = new(psa->pvData, array.Length);
                                var result = GetSpan<decimal>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = (decimal)data[i];
                                }

                                break;
                            }

                        case VT_CY:
                            {
                                Span<long> data = new(psa->pvData, array.Length);
                                var result = GetSpan<decimal>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = decimal.FromOACurrency(data[i]);
                                }

                                break;
                            }

                        case VT_DATE:
                            {
                                Span<double> data = new(psa->pvData, array.Length);
                                var result = GetSpan<DateTime>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = DateTime.FromOADate(data[i]);
                                }

                                break;
                            }

                        case VT_BSTR:
                            {
                                Span<IntPtr> data = new(psa->pvData, array.Length);
                                var result = GetSpan<string?>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = Marshal.PtrToStringUni(data[i]);
                                }

                                break;
                            }

                        case VT_DISPATCH:
                        case VT_UNKNOWN:
                            {
                                Span<IntPtr> data = new(psa->pvData, array.Length);
                                var result = GetSpan<object?>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = data[i] == IntPtr.Zero ? null : Marshal.GetObjectForIUnknown((nint)(IUnknown*)data[i]);
                                }

                                break;
                            }

                        case VT_VARIANT:
                            {
                                Span<PROPVARIANT> data = new(psa->pvData, array.Length);
                                var result = GetSpan<object?>(array);
                                for (int i = 0; i < data.Length; i++)
                                {
                                    result[i] = data[i].ToObject();
                                }

                                break;
                            }

                        default:
                            throw new ArgumentException(string.Format(SR.COM2UnhandledVT, vt));
                    }
                }
                else if (array.Length != 0)
                {
                    // CLR arrays are laid out in row-major order.
                    // See CLI 8.9.1: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf
                    // However, SAFEARRAYs are laid out in column-major order.
                    // See https://docs.microsoft.com/previous-versions/windows/desktop/automat/array-manipulation-functions
                    // Therefore, we need to transpose data.
                    TransposeArray(psa, array, arrayType);
                }
            }
            finally
            {
                if (pin.IsAllocated)
                {
                    pin.Free();
                }

                hr = PInvoke.SafeArrayUnlock(psa); //@@@ PInvokeCore
                Debug.Assert(hr == HRESULT.S_OK);
            }

            return array;
        }

        private static void TransposeArray(SAFEARRAY* psa, Array array, VARENUM arrayType)
        {
            if (array.Rank <= 32)
            {
                StackTransposeArray(psa, array, arrayType);
            }
            else
            {
                Debug.Fail("The CLR should not support arrays with more than 32 dimensions.");
                HeapTransposeArray(psa, array, arrayType);
            }

            static void StackTransposeArray(SAFEARRAY* psa, Array array, VARENUM arrayType)
            {
                Span<int> indices = stackalloc int[array.Rank];
                Span<int> lower = stackalloc int[array.Rank];
                Span<int> upper = stackalloc int[array.Rank];
                InternalTransposeArray(psa, array, arrayType, indices, lower, upper);
            }

            static void HeapTransposeArray(SAFEARRAY* psa, Array array, VARENUM arrayType)
            {
                int[] indices = new int[array.Rank];
                int[] lower = new int[array.Rank];
                int[] upper = new int[array.Rank];
                InternalTransposeArray(psa, array, arrayType, indices, lower, upper);
            }

            static void InternalTransposeArray(SAFEARRAY* psa, Array array, VARENUM arrayType, Span<int> indices, Span<int> lower, Span<int> upper)
            {
                int lastIndex = array.Rank - 1;
                int i;
                for (i = 0; i < array.Rank; i++)
                {
                    indices[i] = lower[i] = array.GetLowerBound(i);
                    upper[i] = array.GetUpperBound(i);
                }

                // Loop through all the indices.
                while (true)
                {
BeginMainLoop:

                    SetArrayValue(psa, array, indices, lower, arrayType);

                    for (i = lastIndex; i > 0;)
                    {
                        if (++indices[i] <= upper[i])
                        {
                            goto BeginMainLoop;
                        }

                        indices[i] = lower[i];
                        --i;
                    }

                    // Special case for the first index, it must be enumerated only once
                    if (++indices[0] > upper[0])
                    {
                        break;
                    }
                }
            }
        }

        private static void SetArrayValue(SAFEARRAY* psa, Array array, Span<int> indices, Span<int> lowerBounds, VARENUM arrayType)
        {
            static void SetValue<T>(Array array, T value, Span<int> indices, Span<int> lowerBounds)
            {
                // CLR arrays are laid out in row-major order.
                // See CLI 8.9.1: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf
                var span = GetSpan<T>(array);
                int offset = 0;
                int multiplier = 1;
                for (int i = array.Rank; i >= 1; i--)
                {
                    int diff = indices[i - 1] - lowerBounds[i - 1];
                    offset += diff * multiplier;
                    multiplier *= array.GetLength(i - 1);
                }

                span[offset] = value;
            }

            switch (arrayType)
            {
                case VT_I1:
                    SetValue(array, psa->GetValue<sbyte>(indices), indices, lowerBounds);
                    break;
                case VT_UI1:
                    SetValue(array, psa->GetValue<byte>(indices), indices, lowerBounds);
                    break;
                case VT_I2:
                    SetValue(array, psa->GetValue<short>(indices), indices, lowerBounds);
                    break;
                case VT_UI2:
                    SetValue(array, psa->GetValue<ushort>(indices), indices, lowerBounds);
                    break;
                case VT_I4:
                case VT_INT:
                    SetValue(array, psa->GetValue<int>(indices), indices, lowerBounds);
                    break;
                case VT_UI4:
                case VT_UINT:
                case VT_ERROR: // Not explicitly mentioned in the docs but trivial to implement.
                    SetValue(array, psa->GetValue<uint>(indices), indices, lowerBounds);
                    break;
                case VT_I8:
                    SetValue(array, psa->GetValue<long>(indices), indices, lowerBounds);
                    break;
                case VT_UI8:
                    SetValue(array, psa->GetValue<ulong>(indices), indices, lowerBounds);
                    break;
                case VT_R4:
                    SetValue(array, psa->GetValue<float>(indices), indices, lowerBounds);
                    break;
                case VT_R8:
                    SetValue(array, psa->GetValue<double>(indices), indices, lowerBounds);
                    break;
                case VT_BOOL:
                    {
                        VARIANT_BOOL data = psa->GetValue<VARIANT_BOOL>(indices);
                        SetValue(array, data != VARIANT_BOOL.VARIANT_FALSE, indices, lowerBounds);
                        break;
                    }

                case VT_DECIMAL:
                    {
                        DECIMAL data = psa->GetValue<DECIMAL>(indices);
                        SetValue(array, (decimal)data, indices, lowerBounds);
                        break;
                    }

                case VT_CY:
                    {
                        long data = psa->GetValue<long>(indices);
                        SetValue(array, decimal.FromOACurrency(data), indices, lowerBounds);
                        break;
                    }

                case VT_DATE:
                    {
                        double data = psa->GetValue<double>(indices);
                        SetValue(array, DateTime.FromOADate(data), indices, lowerBounds);
                        break;
                    }

                case VT_BSTR:
                    {
                        IntPtr data = psa->GetValue<IntPtr>(indices);
                        SetValue(array, Marshal.PtrToStringUni(data), indices, lowerBounds);
                        break;
                    }

                case VT_DISPATCH:
                case VT_UNKNOWN:
                    {
                        IntPtr data = psa->GetValue<IntPtr>(indices);
                        if (data == IntPtr.Zero)
                        {
                            SetValue<object?>(array, null, indices, lowerBounds);
                        }
                        else
                        {
                            SetValue(array, Marshal.GetObjectForIUnknown((nint)(IUnknown*)data), indices, lowerBounds);
                        }

                        break;
                    }

                case VT_VARIANT:
                    {
                        PROPVARIANT data = psa->GetValue<PROPVARIANT>(indices);
                        SetValue(array, data.ToObject(), indices, lowerBounds);
                        break;
                    }

                default:
                    throw new ArgumentException(string.Format(SR.COM2UnhandledVT, arrayType));
            }
        }

        private static Array CreateArrayFromSafeArray(SAFEARRAY* psa, VARENUM vt)
        {
            Type elementType;
            if (vt == VT_EMPTY)
            {
                throw new InvalidOleVariantTypeException();
            }

            VARENUM arrayVarType = psa->VarType;
            if (arrayVarType == VT_EMPTY)
            {
                if (psa->cbElements != GetElementSizeForVarType(vt))
                {
                    throw new SafeArrayTypeMismatchException();
                }
            }

            // Allow limited conversion between arrays of different but related types.
            else if (arrayVarType != vt
                && !(vt == VT_INT && arrayVarType == VT_I4)
                && !(vt == VT_UINT && arrayVarType == VT_UI4)
                && !(vt == VT_I4 && arrayVarType == VT_INT)
                && !(vt == VT_UI4 && arrayVarType == VT_UINT)
                && !(vt == VT_UNKNOWN && arrayVarType == VT_DISPATCH)
                && !(arrayVarType == VT_RECORD))
            {
                // To match CLR behavior.
                throw new SafeArrayTypeMismatchException();
            }

            elementType = vt switch
            {
                VT_I1 => typeof(sbyte),
                VT_UI1 => typeof(byte),
                VT_I2 => typeof(short),
                VT_UI2 => typeof(ushort),
                VT_I4 or VT_INT => typeof(int),
                VT_I8 => typeof(long),
                VT_UI8 => typeof(ulong),
                VT_UI4 or VT_UINT or VT_ERROR => typeof(uint),
                VT_R4 => typeof(float),
                VT_R8 => typeof(double),
                VT_BOOL => typeof(bool),
                VT_DECIMAL or VT_CY => typeof(decimal),
                VT_DATE => typeof(DateTime),
                VT_BSTR => typeof(string),
                VT_DISPATCH or VT_UNKNOWN or VT_VARIANT => typeof(object),
                _ => throw new ArgumentException(string.Format(SR.COM2UnhandledVT, vt)),
            };

            if (psa->cDims == 1 && psa->GetBounds().lLbound == 0)
            {
                // SZArray.
                return Array.CreateInstance(elementType, (int)psa->GetBounds().cElements);
            }

            int[] lengths = new int[psa->cDims];
            int[] bounds = new int[psa->cDims];
            int counter = 0;

            // Copy the lower bounds and count of elements for the dimensions. These need to copied in reverse order.
            for (int i = psa->cDims - 1; i >= 0; i--)
            {
                lengths[counter] = (int)psa->GetBounds(i).cElements;
                bounds[counter] = psa->GetBounds(i).lLbound;
                counter++;
            }

            return Array.CreateInstance(elementType, lengths, bounds);
        }

        private static uint GetElementSizeForVarType(VARENUM vt)
        {
            switch (vt)
            {
                case VT_EMPTY:
                case VT_NULL:
                case VT_VOID:
                    return 0;
                case VT_I1:
                case VT_UI1:
                    return 1;
                case VT_I2:
                case VT_UI2:
                case VT_BOOL:
                    return 2;
                case VT_I4:
                case VT_UI4:
                case VT_INT:
                case VT_UINT:
                case VT_R4:
                case VT_HRESULT:
                case VT_ERROR:
                    return 4;
                case VT_I8:
                case VT_UI8:
                case VT_CY:
                case VT_R8:
                case VT_DATE:
                    return 8;
                case VT_DECIMAL:
                    return (uint)sizeof(DECIMAL);
                case VT_VARIANT:
                    return (uint)sizeof(PROPVARIANT);
                case VT_BSTR:
                case VT_LPSTR:
                case VT_LPWSTR:
                case VT_UNKNOWN:
                case VT_DISPATCH:
                case VT_USERDEFINED:
                case VT_CARRAY:
                case VT_SAFEARRAY:
                case VT_PTR:
                    return (uint)IntPtr.Size;
                default:
                    if ((vt & VT_ARRAY) != 0)
                    {
                        return (uint)sizeof(SAFEARRAY*);
                    }

                    return 0;
            }
        }

        private static object ToVector(in CA ca, VARENUM vectorType)
        {
            VARENUM vt = vectorType & ~VT_VECTOR;
            switch (vt)
            {
                case VT_I1:
                    return new Span<sbyte>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_UI1:
                    return new Span<byte>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_I2:
                    return new Span<short>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_UI2:
                    return new Span<ushort>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_BOOL:
                    {
                        Span<VARIANT_BOOL> data = new(ca.pElems, (int)ca.cElems);
                        bool[] result = new bool[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = data[i] != VARIANT_BOOL.VARIANT_FALSE;
                        }

                        return result;
                    }

                case VT_I4:
                case VT_INT: // Not explicitly mentioned in the docs but trivial to implement.
                    return new Span<int>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_UI4:
                case VT_ERROR:
                case VT_UINT: // Not explicitly mentioned in the docs but trivial to implement.
                    return new Span<uint>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_I8:
                    return new Span<long>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_UI8:
                    return new Span<ulong>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_R4:
                    return new Span<float>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_R8:
                    return new Span<double>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_CY:
                    {
                        Span<long> data = new(ca.pElems, (int)ca.cElems);
                        decimal[] result = new decimal[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = decimal.FromOACurrency(data[i]);
                        }

                        return result;
                    }

                case VT_DATE:
                    {
                        Span<double> data = new(ca.pElems, (int)ca.cElems);
                        var result = new DateTime[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = DateTime.FromOADate(data[i]);
                        }

                        return result;
                    }

                case VT_FILETIME:
                    {
                        var data = new Span<FILETIME>(ca.pElems, (int)ca.cElems);
                        var result = new DateTime[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = data[i].ToDateTime();
                        }

                        return result;
                    }

                case VT_CLSID:
                    return new Span<Guid>(ca.pElems, (int)ca.cElems).ToArray();
                case VT_BSTR:
                case VT_LPWSTR:
                    {
                        Span<IntPtr> data = new(ca.pElems, (int)ca.cElems);
                        string?[] result = new string?[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = Marshal.PtrToStringUni(data[i]);
                        }

                        return result;
                    }

                case VT_LPSTR:
                    {
                        Span<IntPtr> data = new(ca.pElems, (int)ca.cElems);
                        string?[] result = new string?[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = Marshal.PtrToStringAnsi(data[i]);
                        }

                        return result;
                    }

                case VT_VARIANT:
                    {
                        Span<PROPVARIANT> data = new(ca.pElems, (int)ca.cElems);
                        object?[] result = new object?[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            result[i] = data[i].ToObject();
                        }

                        return result;
                    }

                case VT_CF: // Not implemented.
                case VT_BSTR_BLOB: // System use only.
                default: // Documentation does not specify any other types that are supported.
                    throw new ArgumentException(string.Format(SR.COM2UnhandledVT, vt));
            }
        }

#if NET462_OR_GREATER
        //This is a dummy, because functions with GetSpan are not used by RibbonFramework
        private static Span<T> GetSpan<T>(Array array)
        //=> throw new NotSupportedException();
        => CreateSpan<T>(array);
        private static Span<T> CreateSpan<T>(Array array)
        {
            return new Span<T>(Marshal.UnsafeAddrOfPinnedArrayElement(array, 0).ToPointer(), array.Length);
        }
#else
        private static Span<T> GetSpan<T>(Array array)
            => MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(Marshal.UnsafeAddrOfPinnedArrayElement(array, 0).ToPointer()), array.Length);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator bool(PROPVARIANT value)
            => value.vt == VT_BOOL ? value.data.boolVal != VARIANT_BOOL.VARIANT_FALSE : ThrowInvalidCast<bool>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(bool value)
            => value ? True : False;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(PROPVARIANT value)
            => value.vt == VT_I2 ? value.data.iVal : ThrowInvalidCast<short>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(short value)
            => new()
            {
                vt = VT_I2,
                data = new() { iVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(PROPVARIANT value)
            => value.vt == VT_UI2 ? value.data.uiVal : ThrowInvalidCast<ushort>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(ushort value)
            => new()
            {
                vt = VT_UI2,
                data = new() { uiVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(PROPVARIANT value)
            => value.vt is VT_I4 or VT_INT ? value.data.intVal : ThrowInvalidCast<int>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(int value)
            => new()
            {
                // Legacy marshalling uses VT_I4, not VT_INT
                vt = VT_I4,
                data = new() { intVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(PROPVARIANT value)
            => value.vt is VT_UI4 or VT_UINT ? value.data.uintVal : ThrowInvalidCast<uint>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(uint value)
            => new()
            {
                // Legacy marshalling uses VT_UI4, not VT_UINT
                vt = VT_UI4,
                data = new() { uintVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(PROPVARIANT value)
            => value.vt is VT_I8 ? value.data.hVal : ThrowInvalidCast<long>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(long value)
            => new()
            {
                vt = VT_I8,
                data = new() { hVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(PROPVARIANT value)
            => value.vt is VT_UI8 ? value.data.uhVal : ThrowInvalidCast<ulong>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(ulong value)
            => new()
            {
                vt = VT_UI8,
                data = new() { uhVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator BSTR(PROPVARIANT value)
            => value.vt == VT_BSTR ? value.data.bstrVal : ThrowInvalidCast<BSTR>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PCWSTR(PROPVARIANT value)
            => value.vt == VT_LPWSTR ? value.data.pwszVal : ThrowInvalidCast<PCWSTR>();

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static explicit operator PROPVARIANT(string value)
        //    => new()
        //    {
        //        // Runtime marshalling converts strings to BSTR variants
        //        vt = VT_BSTR,
        //        data = new() { bstrVal = new(value) }
        //    };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(string value)
            => new()
            {
                // Runtime marshalling converts strings to PWSTR variants
                vt = VT_LPWSTR,
                data = new() { pwszVal = new((char*)Marshal.StringToCoTaskMemUni(value)) }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(BSTR value)
            => new()
            {
                vt = VT_BSTR,
                data = new() { bstrVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(PCWSTR value)
            => new()
            {
                vt = VT_LPWSTR,
                data = new() { pwszVal = new PWSTR(value.Value) }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator CY(PROPVARIANT value)
            => value.vt == VT_CY ? value.data.cyVal : ThrowInvalidCast<CY>();

        public static explicit operator decimal(PROPVARIANT value) => value.vt switch
        {
            VT_DECIMAL => (decimal)value.Anonymous.decVal,
            VT_CY => decimal.FromOACurrency(value.data.cyVal.int64),
            _ => ThrowInvalidCast<decimal>(),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(decimal value)
            => new()
            {
                Anonymous = new() { decVal = new DECIMAL(value) },
                // Note we must set vt after writing the 16-byte decimal value (because it overwrites all 16 bytes)!
                vt = VARENUM.VT_DECIMAL
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(IUnknown* value)
            => new()
            {
                vt = VT_UNKNOWN,
                data = new() { punkVal = value }
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator IUnknown*(PROPVARIANT value)
            => value.vt == VT_UNKNOWN ? value.data.punkVal : throw new InvalidCastException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(PROPVARIANT value)
            => value.vt == VT_R8 ? value.data.dblVal : ThrowInvalidCast<double>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PROPVARIANT(double value)
            => new()
            {
                vt = VT_R8,
                data = new() { dblVal = value }
            };

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T ThrowInvalidCast<T>() => throw new InvalidCastException();

        /// <summary>
        ///  Converts the given object to <see cref="PROPVARIANT"/>.
        /// </summary>
        public static PROPVARIANT FromObject(object? value)
        {
            if (value is null)
            {
                return Empty;
            }

            if (value is string stringValue)
            {
                return (PROPVARIANT)stringValue;
            }
            else if (value is bool boolValue)
            {
                return (PROPVARIANT)boolValue;
            }
            else if (value is short shortValue)
            {
                return (PROPVARIANT)shortValue;
            }
            else if (value is int intValue)
            {
                return (PROPVARIANT)intValue;
            }
            else if (value is uint uintValue)
            {
                return (PROPVARIANT)uintValue;
            }
            else if (value is double doubleValue)
            {
                return (PROPVARIANT)doubleValue;
            }

            // Need to fill out to match Marshal behavior so we can remove the call.
            // https://github.com/dotnet/winforms/issues/8596

            PROPVARIANT variant = default;
            Marshal.GetNativeVariantForObject(value, (nint)(void*)&variant);
            return variant;
        }

        partial struct _Anonymous_e__Union
        {
            partial struct _Anonymous_e__Struct_unmanaged
            {
                partial struct _Anonymous_e__Union_unmanaged
                {
                    [FieldOffset(0)]
                    internal CA ca;
                }
            }
        }


        /// <summary>
        /// Converts a string array to a PROPVARIANT
        /// with pinned strings, so the GC can't move the strings in memory till PInvoke function is called
        /// used in ColorPickerPropertiesProvider
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="propVar"></param>
        /// <returns></returns>
        internal static unsafe HRESULT InitPropVariantFromStringVector(string[] strings, out PROPVARIANT propVar)
        {
            if (strings == null || strings.Length == 0)
                throw new ArgumentNullException(nameof(strings));
            HRESULT hr = HRESULT.E_FAIL;
            PCWSTR[] array = new PCWSTR[strings.Length];
            GCHandle[] pins = new GCHandle[strings.Length];
            try
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (strings[i] != null)
                        pins[i] = GCHandle.Alloc(strings[i], GCHandleType.Pinned);
                    else
                        pins[i] = GCHandle.Alloc(string.Empty, GCHandleType.Pinned);
                    array[i] = (char*)pins[i].AddrOfPinnedObject();
                }
                fixed (PCWSTR* parray = array)
                    hr = PInvoke.InitPropVariantFromStringVector(parray, (uint)strings.Length, out propVar);
            }
            finally
            {
                for (int i = 0; i < pins.Length; i++)
                {
                    if (pins[i].IsAllocated)
                        pins[i].Free();
                }
            }
            return hr;
        }

#if Unused
        /// <summary>
        /// Clone the PROPVARIANT
        /// </summary>
        /// <returns></returns>
        public unsafe PROPVARIANT Clone()
        {
            PROPVARIANT result;
            PInvoke.PropVariantCopy(out result, in this);
            return result;
        }

        /// <summary>
        /// Set a string value
        /// </summary>
        /// <param name="value">The new value to set.</param>
        /// <param name="propvar">The PROPVARIANT.</param>
        public static unsafe void InitPropVariantFromString(string value, out PROPVARIANT propvar)
        {
            //Strings require special consideration, because they cannot be empty as well
            if (String.IsNullOrEmpty(value) || String.IsNullOrEmpty((value).Trim()))
                throw new ArgumentException("String argument cannot be null or empty.");
            fixed (PROPVARIANT* ppropvar = &propvar)
            {
                ppropvar->vt = VARENUM.VT_LPWSTR;
                ppropvar->Anonymous.Anonymous.Anonymous.pwszVal = (PWSTR)(char*)Marshal.StringToCoTaskMemUni(value);
            }
        }

        /// <summary>
        /// Get a C# value from a PROPVARIANT
        /// </summary>
        public unsafe object? Value
        {
            get
            {
                switch (vt)
                {
                    case VARENUM.VT_BOOL:
                        return (bool)Anonymous.Anonymous.Anonymous.boolVal; // != 0;
                    case VARENUM.VT_I1:
                        return (sbyte)Anonymous.Anonymous.Anonymous.cVal.Value;
                    case VARENUM.VT_I2:
                        return Anonymous.Anonymous.Anonymous.iVal;
                    case VARENUM.VT_INT:
                    case VARENUM.VT_I4:
                        return Anonymous.Anonymous.Anonymous.intVal;
                    case VARENUM.VT_I8:
                        return Anonymous.Anonymous.Anonymous.hVal;
                    case VARENUM.VT_UI1:
                        return Anonymous.Anonymous.Anonymous.bVal;
                    case VARENUM.VT_UI2:
                        return Anonymous.Anonymous.Anonymous.uiVal;
                    case VARENUM.VT_UINT:
                    case VARENUM.VT_UI4:
                        return Anonymous.Anonymous.Anonymous.uintVal;
                    case VARENUM.VT_UI8:
                        return Anonymous.Anonymous.Anonymous.uhVal;
                    case VARENUM.VT_R4:
                        return Anonymous.Anonymous.Anonymous.fltVal;
                    case VARENUM.VT_R8:
                        return Anonymous.Anonymous.Anonymous.dblVal;
                    case VARENUM.VT_ERROR:
                        return Anonymous.Anonymous.Anonymous.scode;
                    case VARENUM.VT_CY:
                        return decimal.FromOACurrency(Anonymous.Anonymous.Anonymous.cyVal.int64);
                    case VARENUM.VT_DATE:
                        return DateTime.FromOADate(Anonymous.Anonymous.Anonymous.date);
                    //case VARENUM.VT_FILETIME:
                    //    return FileTimeToDateTime(Anonymous.Anonymous.Anonymous.filetime);
                    case VARENUM.VT_UNKNOWN:
                        return Marshal.GetObjectForIUnknown((IntPtr)Anonymous.Anonymous.Anonymous.punkVal);
                    case VARENUM.VT_DISPATCH:
                        return Marshal.GetObjectForIUnknown((IntPtr)Anonymous.Anonymous.Anonymous.pdispVal);
                    case VARENUM.VT_ARRAY | VARENUM.VT_UNKNOWN:
                        return GetIUnknownSafeArray(Anonymous.Anonymous.Anonymous.parray);
                    case VARENUM.VT_DECIMAL:
                        DECIMAL decVal = Anonymous.decVal;
                        decVal.wReserved = 0;
                        return (decimal)decVal;
                    case VARENUM.VT_BSTR:
                        return Anonymous.Anonymous.Anonymous.bstrVal.ToString();
                    //return Marshal.PtrToStringBSTR((IntPtr)Anonymous.Anonymous.Anonymous.bstrVal);
                    //case VARENUM.VT_BLOB:
                    //    return GetBlobData();
                    case VARENUM.VT_LPSTR:
                        return Anonymous.Anonymous.Anonymous.pszVal.ToString();
                    //return Marshal.PtrToStringAnsi((IntPtr)Anonymous.Anonymous.Anonymous.pszVal);
                    case VARENUM.VT_LPWSTR:
                        return Anonymous.Anonymous.Anonymous.pwszVal.ToString();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_I2:
                    //    return GetInt16Vector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_UI2:
                    //    return GetUInt16Vector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_I4:
                    //    return GetInt32Vector();
                    case VARENUM.VT_VECTOR | VARENUM.VT_UI4:
                        return GetUInt32Vector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_I8:
                    //    return GetInt64Vector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_UI8:
                    //    return GetUInt64Vector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_R8:
                    //    return GetDoubleVector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_BOOL:
                    //    return GetBooleanVector();
                    //case VARENUM.VT_VECTOR | VARENUM.VT_FILETIME:
                    //    return GetDateTimeVector();
                    case VARENUM.VT_VECTOR | VARENUM.VT_LPWSTR:
                        return GetStringVector();
                    case VARENUM.VT_EMPTY:
                    case VARENUM.VT_NULL: return null;
                    default:
                        throw new NotSupportedException("The type of this variable is not supported ('" + vt.ToString() + "')");
                }
            }
        }

        /// <summary>
        /// Creates a PropVariant from an object
        /// Only objects for Ribbon are supported
        /// </summary>
        /// <param name="value">The object containing the data.</param>
        /// <returns>An initialized PropVariant</returns>
        public static PROPVARIANT FromObjectOld(object value)
        {
            if (value == null)
                return PROPVARIANT.Empty;

            PROPVARIANT propvar = PROPVARIANT.Empty;

            if (value is String strValue)
            {
                //Strings require special consideration, because they cannot be empty as well

                if (String.IsNullOrWhiteSpace(strValue))
                    throw new ArgumentException("String argument cannot be null or empty.");
                InitPropVariantFromString(strValue, out propvar);
                return propvar;
            }
            else if (value is bool boolValue)
            {
                return (PROPVARIANT)boolValue; //InitPropVariantFromBoolean
            }
            else if (value is short shortValue)
            {
                return (PROPVARIANT)shortValue; //InitPropVariantFromInt16
            }
            else if (value is ushort ushortValue)
            {
                return (PROPVARIANT)ushortValue; //InitPropVariantFromUInt16
            }
            else if (value is int intValue)
            {
                return (PROPVARIANT)intValue; //InitPropVariantFromInt32
            }
            else if (value is uint uintValue)
            {
                return (PROPVARIANT)uintValue; //InitPropVariantFromUInt32
            }
            else if (value is long longValue)
            {
                return (PROPVARIANT)longValue; //InitPropVariantFromInt64
            }
            else if (value is ulong ulongValue)
            {
                return (PROPVARIANT)ulongValue; //InitPropVariantFromUInt64
            }
            else if (value is double doubleValue)
            {
                return (PROPVARIANT)doubleValue; //InitPropVariantFromDouble
            }
            else if (value is decimal decimalValue)
            {
                return (PROPVARIANT)decimalValue;
            }
            //else if (value is DateTime dateTimeValue)
            //{
            //    InitPropVariantFromDateTime(dateTimeValue, out propVar);
            //    return propVar;
            //}
            else if (value is string[] stringVectorValue)
            {
                InitPropVariantFromStringVector(stringVectorValue, out propvar);
                return propvar;
            }
            else if (value is short[] shortVectorValue)
            {
                fixed (short* shortVectorLocal = shortVectorValue)
                    PInvoke.InitPropVariantFromInt16Vector(shortVectorLocal, (uint)shortVectorValue.Length, out propvar);
                return propvar;
            }
            else if (value is ushort[] ushortVectorValue)
            {
                fixed (ushort* ushortVectorLocal = ushortVectorValue)
                    PInvoke.InitPropVariantFromUInt16Vector(ushortVectorLocal, (uint)ushortVectorValue.Length, out propvar);
                return propvar;
            }
            else if (value is int[] intVectorValue)
            {
                fixed (int* intVectorLocal = intVectorValue)
                    PInvoke.InitPropVariantFromInt32Vector(intVectorLocal, (uint)intVectorValue.Length, out propvar);
                return propvar;
            }
            else if (value is uint[] uintVectorValue)
            {
                fixed (uint* uintVectorLocal = uintVectorValue)
                    PInvoke.InitPropVariantFromUInt32Vector(uintVectorLocal, (uint)uintVectorValue.Length, out propvar);
                return propvar;
            }
            else if (value is long[] longVectorValue)
            {
                fixed (long* longVectorLocal = longVectorValue)
                    PInvoke.InitPropVariantFromInt64Vector(longVectorLocal, (uint)longVectorValue.Length, out propvar);
                return propvar;
            }
            else if (value is ulong[] ulongVectorValue)
            {
                fixed (ulong* ulongVectorLocal = ulongVectorValue)
                    PInvoke.InitPropVariantFromUInt64Vector(ulongVectorLocal, (uint)ulongVectorValue.Length, out propvar);
                return propvar;
            }
            //else if (value is DateTime[] dateTimeVectorValue)
            //{
            //    FILETIME[] filetimes = new FILETIME[dateTimeVectorValue.Length];
            //    for (int i = 0; i < filetimes.Length; i++)
            //    {
            //        long filetime = dateTimeVectorValue[i].ToFileTime();
            //        filetimes[i] = new FILETIME() { dwHighDateTime = (int)(filetime >> 16), dwLowDateTime = (int)(filetime & 0xffff) };
            //    }
            //    fixed (FILETIME* filetimesLocal = filetimes)
            //        PInvoke.InitPropVariantFromFileTimeVector(filetimesLocal, (uint)dateTimeVectorValue.Length, out propVar);
            //    return propVar;
            //}
            else if (value is bool[] boolVectorValue)
            {
                BOOL[] boolVector = new BOOL[boolVectorValue.Length];
                for (int i = 0; i < boolVectorValue.Length; i++)
                {
                    boolVector[i] = boolVectorValue[i];
                }
                fixed (BOOL* boolVectorLocal = boolVector)
                    PInvoke.InitPropVariantFromBooleanVector(boolVectorLocal, (uint)boolVectorValue.Length, out propvar);
                return propvar;
            }
            else
            {
                throw new ArgumentException("This Value type is not supported.");
            }
        }

        private unsafe UInt32[] GetUInt32Vector()
        {
            uint count = PInvoke.PropVariantGetElementCount(in this);
            uint[] array = new uint[count];
            if (count == 0)
                return array;

            uint value;
            for (uint i = 0; i < count; i++)
            {
                PInvoke.PropVariantGetUInt32Elem(in this, i, &value);
                array[i] = value;
            }
            return array;
        }

        private unsafe string[] GetStringVector()
        {
            //fixed (PROPVARIANT* pThis = &this) ;
            uint count = PInvoke.PropVariantGetElementCount(in this);
            string[] array = new string[count];
            if (count == 0)
                return array;

            PWSTR value;
            for (uint i = 0; i < count; i++)
            {
                PInvoke.PropVariantGetStringElem(in this, i, &value);
                array[i] = value.ToString();
                PInvoke.CoTaskMemFree(value);
            }
            return array;
        }


        /// <summary>
        /// Marshals an unmanaged SafeArray to a managed Array object.
        /// </summary>
        internal static unsafe Array GetIUnknownSafeArray(SAFEARRAY* psa)
        {
            uint cDims = PInvoke.SafeArrayGetDim(psa);
            if (cDims != 1)
                throw new ArgumentException("Multi-dimensional SafeArrays not supported.");

            int lBound;
            PInvoke.SafeArrayGetLBound(psa, 1U, &lBound);
            int uBound;
            PInvoke.SafeArrayGetUBound(psa, 1U, &uBound);

            int n = uBound - lBound + 1; // uBound is inclusive

            object[] array = new object[n];

            IntPtr value = new IntPtr();
            for (int i = lBound; i <= uBound; i++)
            {
                PInvoke.SafeArrayGetElement(psa, &i, &value);
                array[i] = (value == IntPtr.Zero) ? null! : Marshal.GetObjectForIUnknown(value);
            }
            return array;
        }
#endif
    }
}
