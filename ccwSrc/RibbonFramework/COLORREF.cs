using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Windows.Win32.Foundation
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="COLORREF"/>
    partial struct COLORREF
    {
        public byte GetRValue => (byte)Value;

        public byte GetGValue => (byte)(Value >> 8);

        public byte GetBValue => (byte)(Value >> 16);

        public COLORREF(Color color)
        {
            if (color == Color.Transparent) //CLR_NONE
                this.Value = uint.MaxValue;
            else
                this.Value = (uint)ColorTranslator.ToWin32(color);
        }

        public Color ToColor()
        {
            if (this.Value == uint.MaxValue)
                return Color.Transparent;
            return ColorTranslator.FromWin32((int)Value);
        }
    }
}
