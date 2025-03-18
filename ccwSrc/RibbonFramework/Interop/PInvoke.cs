using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Windows.Win32
{
    partial class PInvoke
    {
        //private const string CLSID_WICImagingFactory = "cacaf262-9370-4615-a13b-9f5539da4c0a";

        //// WICImagingFactory class
        //[ComImport]
        //[ClassInterface(ClassInterfaceType.None)]
        //[Guid(CLSID_WICImagingFactory)]
        //public class WICImagingFactory
        //{
        //    // implements IWICImagingFactory
        //}

    }

    namespace Graphics.Gdi
    {
        /// <inheritdoc cref="BITMAPINFOHEADER"/>
        partial struct BITMAPINFOHEADER
        {
            internal static unsafe BITMAPINFOHEADER Create()
            {
                return new BITMAPINFOHEADER() { biSize = (uint)sizeof(BITMAPINFOHEADER) };
            }
        }

        partial struct BITMAPFILEHEADER
        {
            internal static unsafe BITMAPFILEHEADER Create()
            {
                return new BITMAPFILEHEADER() { bfType = 0x4d42 };
            }
        }
    }
}
