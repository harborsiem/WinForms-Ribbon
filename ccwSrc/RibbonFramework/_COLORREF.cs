using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WinForms.Ribbon
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>The COLORREF value is used to specify an RGB color.</summary>
    /// <remarks>
    /// <para>When specifying an explicit [RGB](/windows/desktop/api/Wingdi/nf-wingdi-rgb) color, the **COLORREF** value has the following hexadecimal form: `0x00bbggrr` The low-order byte contains a value for the relative intensity of red; the second byte contains a value for green; and the third byte contains a value for blue. The high-order byte must be zero. The maximum value for a single byte is 0xFF. To create a **COLORREF** color value, use the [RGB](/windows/desktop/api/Wingdi/nf-wingdi-rgb) macro. To extract the individual values for the red, green, and blue components of a color value, use the [**GetRValue**](/windows/desktop/api/Wingdi/nf-wingdi-getrvalue), [GetGValue](/windows/desktop/api/Wingdi/nf-wingdi-getgvalue), and [GetBValue](/windows/desktop/api/Wingdi/nf-wingdi-getbvalue) macros, respectively.</para>
    /// <para><see href="https://learn.microsoft.com/windows/win32/gdi/colorref#">Read more on docs.microsoft.com</see>.</para>
    /// </remarks>
    /// <remarks>
    ///  <para>
    ///   Never convert native constants (such as <see cref="PInvoke.CLR_NONE"/> to <see cref="Color"/> or pass them through
    ///   any conversion in <see cref="Color"/>, <see cref="ColorTranslator"/>, etc. as they can change the value.
    ///   <see cref="COLORREF"/> is a DWORD- passing constants in native code would just pass the value as is.
    ///  </para>
    ///  <para>
    ///   <see href="https://learn.microsoft.com/windows/win32/gdi/colorref#">
    ///    Read more on https://learn.microsoft.com.
    ///   </see>
    ///  </para>
    /// </remarks>
    //_COLORREF is identical to Windows.Win32.Foundation.COLORREF
    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct _COLORREF
        : IEquatable<_COLORREF>
    {
        /// <summary>
        /// GetRValue macro
        /// </summary>
        [FieldOffset(0)]
        public readonly byte GetRValue;
        /// <summary>
        /// GetGValue macro
        /// </summary>
        [FieldOffset(1)]
        public readonly byte GetGValue;
        /// <summary>
        /// GetBValue macro
        /// </summary>
        [FieldOffset(2)]
        public readonly byte GetBValue;

        [FieldOffset(0)]
        public readonly uint Value;

        public _COLORREF(uint value) => this.Value = value;

        public static implicit operator uint(_COLORREF value) => value.Value;

        public static explicit operator _COLORREF(uint value) => new _COLORREF(value);

        public static bool operator ==(_COLORREF left, _COLORREF right) => left.Value == right.Value;

        public static bool operator !=(_COLORREF left, _COLORREF right) => !(left == right);

        public bool Equals(_COLORREF other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is _COLORREF other && this.Equals(other);

        public override int GetHashCode() => this.Value.GetHashCode();

        public override string ToString() => $"0x{this.Value:x}";

        public static implicit operator _COLORREF(Color color) => new((uint)ColorTranslator.ToWin32(color));
        public static implicit operator Color(_COLORREF color) => ColorTranslator.FromWin32((int)color.Value);
        public static implicit operator _COLORREF(int color) => new((uint)color);
    }
}
