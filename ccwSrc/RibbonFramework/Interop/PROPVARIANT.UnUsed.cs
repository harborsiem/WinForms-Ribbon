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

#if Unused
        /// <summary>
        /// Clone the PROPVARIANT
        /// </summary>
        /// <returns></returns>
        public unsafe PROPVARIANT Clone()
        {
            PROPVARIANT result;
            fixed (PROPVARIANT* pThis = &this)
                PInvoke.PropVariantCopy(&result, pThis); //@@@ PInvokeCore
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
                //return HRESULT.S_OK;
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
                return (PROPVARIANT)strValue; //InitPropVariantFromString(strValue, out propvar);
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
                InitPropVariantFromStringVector(stringVectorValue, out propvar); //@@@ PInvokeCore
                return propvar;
            }
            else if (value is short[] shortVectorValue)
            {
                fixed (short* shortVectorLocal = shortVectorValue)
                    PInvoke.InitPropVariantFromInt16Vector(shortVectorLocal, (uint)shortVectorValue.Length, &propvar); //@@@ PInvokeCore
                return propvar;
            }
            else if (value is ushort[] ushortVectorValue)
            {
                fixed (ushort* ushortVectorLocal = ushortVectorValue)
                    PInvoke.InitPropVariantFromUInt16Vector(ushortVectorLocal, (uint)ushortVectorValue.Length, &propvar); //@@@ PInvokeCore
                return propvar;
            }
            else if (value is int[] intVectorValue)
            {
                fixed (int* intVectorLocal = intVectorValue)
                    PInvoke.InitPropVariantFromInt32Vector(intVectorLocal, (uint)intVectorValue.Length, &propvar); //@@@ PInvokeCore
                return propvar;
            }
            else if (value is uint[] uintVectorValue)
            {
                fixed (uint* uintVectorLocal = uintVectorValue)
                    PInvoke.InitPropVariantFromUInt32Vector(uintVectorLocal, (uint)uintVectorValue.Length, &propvar); //@@@ PInvokeCore
                return propvar;
            }
            else if (value is long[] longVectorValue)
            {
                fixed (long* longVectorLocal = longVectorValue)
                    PInvoke.InitPropVariantFromInt64Vector(longVectorLocal, (uint)longVectorValue.Length, &propvar); //@@@ PInvokeCore
                return propvar;
            }
            else if (value is ulong[] ulongVectorValue)
            {
                fixed (ulong* ulongVectorLocal = ulongVectorValue)
                    PInvoke.InitPropVariantFromUInt64Vector(ulongVectorLocal, (uint)ulongVectorValue.Length, &propvar); //@@@ PInvokeCore
                return propvar;
            }
            //else if (value is DateTime[] dateTimeVectorValue)
            //{
            //    PInvoke.InitPropVariantFromDateTimeVector(dateTimeVectorValue, out propVar);
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
                    PInvoke.InitPropVariantFromBooleanVector(boolVectorLocal, (uint)boolVectorValue.Length, &propvar);
                return propvar;
            }
            else
            {
                throw new ArgumentException("This Value type is not supported.");
            }
        }

        private unsafe UInt32[] GetUInt32Vector()
        {
            uint count;
            fixed (PROPVARIANT* pThis = &this)
            {
                count = PInvoke.PropVariantGetElementCount(pThis); //@@@ PInvokeCore
                uint[] array = new uint[count];
                if (count == 0)
                    return array;

                uint value;
                for (uint i = 0; i < count; i++)
                {
                    PInvoke.PropVariantGetUInt32Elem(pThis, i, &value); //@@@ PInvokeCore
                    array[i] = value;
                }
                return array;
            }
        }

        private unsafe string[] GetStringVector()
        {
            uint count;
            fixed (PROPVARIANT* pThis = &this)
            {
                count = PInvoke.PropVariantGetElementCount(pThis); //@@@ PInvokeCore
                string[] array = new string[count];
                if (count == 0)
                    return array;

                PWSTR value;
                for (uint i = 0; i < count; i++)
                {
                    PInvoke.PropVariantGetStringElem(pThis, i, &value); //@@@ PInvokeCore
                    array[i] = value.ToString();
                    PInvoke.CoTaskMemFree(value); //@@@ PInvokeCore
                }
                return array;
            }
        }

        /// <summary>
        /// Marshals an unmanaged SafeArray to a managed Array object.
        /// </summary>
        private static unsafe Array GetIUnknownSafeArray(SAFEARRAY* psa)
        {
            uint cDims = PInvoke.SafeArrayGetDim(psa); //@@@ PInvokeCore
            if (cDims != 1)
                throw new ArgumentException("Multi-dimensional SafeArrays not supported.");

            int lBound;
            PInvoke.SafeArrayGetLBound(psa, 1U, &lBound); //@@@ PInvokeCore
            int uBound;
            PInvoke.SafeArrayGetUBound(psa, 1U, &uBound); //@@@ PInvokeCore

            int n = uBound - lBound + 1; // uBound is inclusive

            IUnknown*[] array = new IUnknown*[n];

            void* value = null;
            for (int i = lBound; i <= uBound; i++)
            {
                PInvoke.SafeArrayGetElement(psa, &i, &value); //@@@ PInvokeCore
                array[i] = (value == null) ? null : (IUnknown*)value;
            }
            return array;
        }
#endif
    }
}
