using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace WinForms.Ribbon
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS8765
    /// <summary>
    /// C# implementation of the UI_HSBCOLOR
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct UI_HSBCOLOR
    : IEquatable<UI_HSBCOLOR>
    {
        /// <summary>
        /// Hue value, Range 0 .. 255
        /// </summary>
        [FieldOffset(0)]
        public readonly byte Hue;

        /// <summary>
        /// Saturation value, Range 0 .. 255
        /// </summary>
        [FieldOffset(1)]
        public readonly byte Saturation;

        /// <summary>
        /// Brightness value, Range 0 .. 255
        /// </summary>
        [FieldOffset(2)]
        public readonly byte Brightness;

        [FieldOffset(0)]
        public readonly uint Value;
        public UI_HSBCOLOR(uint value) => this.Value = value;
        public UI_HSBCOLOR(byte hue, byte saturation, byte brightness)
        { Hue = hue; Saturation = saturation; Brightness = brightness; }
        //this.Value = (uint)(hue | (saturation << 8) | (brightness << 16));
        public static implicit operator uint(UI_HSBCOLOR value) => value.Value;
        public static explicit operator UI_HSBCOLOR(uint value) => new UI_HSBCOLOR(value);
        public static bool operator ==(UI_HSBCOLOR left, UI_HSBCOLOR right) => left.Value == right.Value;
        public static bool operator !=(UI_HSBCOLOR left, UI_HSBCOLOR right) => !(left == right);

        public bool Equals(UI_HSBCOLOR other) => this.Value == other.Value;

        public override bool Equals(object obj) => obj is UI_HSBCOLOR other && this.Equals(other);

        public override int GetHashCode() => this.Value.GetHashCode();

        /// <summary>
        /// Convert RGB Color to Ribbon HSB Color format
        /// accuracy is lost if luminance is lessOrEqual 45 or greater 250
        /// if one want to convert back. In all other cases there is only
        /// a small accuracy lost by rounding.
        /// </summary>
        /// <param name="colorref">RGB Color (COLORREF)</param>
        public unsafe UI_HSBCOLOR(_COLORREF colorref)
        {
            this.Value = GetHSBValue(colorref);
        }

        /// <summary>
        /// Convert RGB Color to Ribbon HSB Color format
        /// accuracy is lost if luminance is lessOrEqual 45 or greater 250
        /// if one want to convert back. In all other cases there is only
        /// a small accuracy lost by rounding.
        /// </summary>
        /// <param name="color">RGB Color</param>
        public unsafe UI_HSBCOLOR(Color color)
        {
            COLORREF colorref = new COLORREF((uint)ColorTranslator.ToWin32(color));
            this.Value = GetHSBValue(colorref);
        }

        /// <summary>
        /// Get HSB
        /// </summary>
        /// <param name="colorRef">COLORREF value</param>
        /// <returns></returns>
        private static unsafe uint GetHSBValue(uint colorRef)
        {
            double ld;
            int brightness;
            ushort hue, luminance, saturation;
            COLORREF colorref = new(colorRef);
            PInvoke.ColorRGBToHLS(colorref, &hue, &luminance, &saturation);
            if (saturation == 0) //workaround for PInvoke function
                hue = 0;
            hue = (ushort)Math.Round((hue * 255.0) / 240.0);
            saturation = (ushort)Math.Round((saturation * 255.0) / 240.0);
            ld = luminance / 240.0;
            if (ld > 0.9821)
                brightness = 255;
            else if (ld < 0.1793)
                brightness = 0;
            else
                brightness = (int)Math.Round(257.7 + 149.9 * Math.Log(ld));
            uint value = (uint)(hue | (saturation << 8) | (brightness << 16));
            return value;
        }

        /// <summary>
        /// Convert Ribbon HSB Color format to RGB Color
        /// </summary>
        /// <returns>COLORREF</returns>
        internal COLORREF ToColorRefInternal()
        {
            ushort brightness = Brightness;
            ushort hue;
            ushort saturation;
            ushort luminance;
            double ld;
            // Convert Brightness to Luminance
            if (brightness == 0)
                ld = 0.0;
            else if (brightness == 0xff)
                ld = 1.0;
            else
                ld = Math.Exp((brightness - 257.7) / 149.9);

            // ColorHLSToRGB requires H, L and S to be in 0..240 range.
            hue = (ushort)((Hue * 240) / 255.0);
            saturation = (ushort)((Saturation * 240) / 255.0);
            luminance = (ushort)Math.Round(ld * 240);
            if (saturation == 0 && luminance > 0) //workaround for PInvoke function
                saturation = 1; 
            return PInvoke.ColorHLSToRGB(hue, luminance, saturation);
        }

        /// <summary>
        /// Convert Ribbon HSB Color format to COLORREF (_COLORREF)
        /// </summary>
        /// <returns>_COLORREF</returns>
        public _COLORREF ToColorRef()
        {
            return (_COLORREF)(uint)ToColorRefInternal();
        }

        /// <summary>
        /// Convert Ribbon HSB Color format to .NET Color
        /// </summary>
        /// <returns>Color</returns>
        public Color ToColor()
        {
            COLORREF colorref = ToColorRefInternal();
            return ColorTranslator.FromWin32((int)colorref.Value);
        }
    }
}
