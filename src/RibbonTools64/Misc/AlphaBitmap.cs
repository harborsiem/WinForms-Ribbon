using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace UIRibbonTools
{
    public unsafe sealed class AlphaBitmap
    {
        private AlphaBitmap() { }

        private static unsafe Bitmap TryConvertToAlphaBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format32bppRgb && bitmap.RawFormat.Guid == ImageFormat.Bmp.Guid)
            {
                BitmapData bmpData = null;
                BitmapData alphaData = null;
                Bitmap alpha = null;
                try
                {
                    bmpData = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    if (BitmapHasAlpha(bmpData))
                    {
                        //alpha = new Bitmap(bitmap.Width, bitmap.Height, bmpData.Stride, PixelFormat.Format32bppArgb, bmpData.Scan0);
                        alpha = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                        alphaData = alpha.LockBits(new Rectangle(new Point(), alpha.Size), ImageLockMode.WriteOnly, alpha.PixelFormat);
                        CopyBitmapData(bmpData, alphaData);
                        return alpha;
                    }
                }
                finally
                {
                    if (bmpData != null)
                        bitmap.UnlockBits(bmpData);
                    if (alpha != null)
                    {
                        alpha.UnlockBits(alphaData);
                        bitmap.Dispose();
                    }
                }
            }
            return bitmap;
        }

        //From Microsoft System.Drawing.Icon.cs
        private static unsafe bool BitmapHasAlpha(BitmapData bmpData)
        {
            bool hasAlpha = false;
            for (int i = 0; i < bmpData.Height; i++)
            {
                for (int j = 3; j < Math.Abs(bmpData.Stride); j += 4)
                {
                    // Stride here is fine since we know we're doing this on the whole image.
                    unsafe
                    {
                        byte* candidate = unchecked(((byte*)bmpData.Scan0.ToPointer()) + (i * bmpData.Stride) + j);
                        if (*candidate != 0)
                        {
                            hasAlpha = true;
                            return hasAlpha;
                        }
                    }
                }
            }

            return false;
        }

        //private unsafe void CopyBitmapData2(BitmapData sourceData, BitmapData targetData)
        //{
        //    byte* srcPtr = (byte*)sourceData.Scan0;
        //    byte* destPtr = (byte*)targetData.Scan0;

        //    //int num = 0;
        //    //int num2 = 0;
        //    int height = Math.Min(sourceData.Height, targetData.Height);
        //    long bytesToCopyEachIter = Math.Abs(targetData.Stride);

        //    for (int i = 0; i < height; i++)
        //    {
        //        //IntPtr ptr;
        //        //IntPtr ptr2;
        //        //if (IntPtr.Size == 4)
        //        //{
        //        //    ptr = new IntPtr(sourceData.Scan0.ToInt32() + num);
        //        //    ptr2 = new IntPtr(targetData.Scan0.ToInt32() + num2);
        //        //}
        //        //else
        //        //{
        //        //    ptr = new IntPtr(sourceData.Scan0.ToInt64() + num);
        //        //    ptr2 = new IntPtr(targetData.Scan0.ToInt64() + num2);
        //        //}
        //        //PInvoke.CopyMemory(new HandleRef(this, ptr2), new HandleRef(this, ptr), Math.Abs(targetData.Stride));
        //        PInvoke.RtlMoveMemory(destPtr, srcPtr, (nuint)bytesToCopyEachIter);
        //        srcPtr += sourceData.Stride;
        //        destPtr += targetData.Stride;
        //    }
        //}

        private static unsafe void CopyBitmapData(BitmapData sourceData, BitmapData targetData)
        {
            byte* srcPtr = (byte*)sourceData.Scan0;
            byte* destPtr = (byte*)targetData.Scan0;

            Debug.Assert(sourceData.Height == targetData.Height, "Unexpected height. How did this happen?");
            int height = Math.Min(sourceData.Height, targetData.Height);
            long bytesToCopyEachIter = Math.Abs(targetData.Stride);

            for (int i = 0; i < height; i++)
            {
                Buffer.MemoryCopy(srcPtr, destPtr, bytesToCopyEachIter, bytesToCopyEachIter);
                srcPtr += sourceData.Stride;
                destPtr += targetData.Stride;
            }

            //GC.KeepAlive(null); // finalizer mustn't deallocate data blobs while this method is running
        }

        public static Bitmap TryCreateAlphaBitmap(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            Bitmap bmp = new Bitmap(stream);
            return TryConvertToAlphaBitmap(bmp);
        }

        /// <summary>
        /// Load a Bitmap (with transparency) from file (*.bmp or *.png)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        public static Bitmap TryCreateAlphaBitmap(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName))
                throw new ArgumentException("File does not exist", nameof(fileName));
            Bitmap bmp = new Bitmap(fileName);
            return TryConvertToAlphaBitmap(bmp);
        }

        /// <summary>
        /// Load a Bitmap (with transparency) from file (*.bmp or *.png) via file content or Bitmap ctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="highContrast"></param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        public static Bitmap TryAlphaBitmapFromFile(string fileName, bool highContrast = false)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName))
                throw new ArgumentException("File does not exist", nameof(fileName));
            Bitmap bitmap = new Bitmap(fileName);
            if (!highContrast)
            {
                bitmap = TryConvertToAlphaBitmap(bitmap);
            }
            if (!highContrast)
                if (!(bitmap.PixelFormat == PixelFormat.Format32bppArgb || bitmap.PixelFormat == PixelFormat.Format32bppPArgb))
                    bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
                else
                {
                    if ((int)bitmap.HorizontalResolution != 96)
                    {
                        bitmap.SetResolution(96.0f, 96.0f); //only png bitmaps can have other resolution
                    }
                }
            return bitmap;
        }

        /// <summary>
        /// Get the managed ARGB Bitmap if possible (32 bit per pixel)
        /// </summary>
        /// <param name="hBitmap">Handle to a Bitmap</param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        public static unsafe Bitmap FromHbitmap(IntPtr hBitmap)
        {
            return FromHbitmap(new HBITMAP(hBitmap));
        }

        /// <summary>
        /// Get the managed ARGB Bitmap if possible (32 bit per pixel)
        /// </summary>
        /// <param name="hBitmap">Handle to a Bitmap</param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        internal static unsafe Bitmap FromHbitmap(HBITMAP hBitmap)
        {
            if (hBitmap == HBITMAP.Null)
                throw new ArgumentNullException(nameof(hBitmap));
            // Create the BITMAP structure and get info from our nativeHBitmap
            BITMAP bitmapStruct = new BITMAP();
            int bitmapSize = sizeof(BITMAP);
            int size = PInvoke.GetObject((HGDIOBJ)hBitmap, bitmapSize, &bitmapStruct);
            Bitmap managedBitmap;
            if (Has32BitAlpha(in bitmapStruct))
            {
                // Create the managed bitmap using the pointer to the pixel data of the native HBitmap
                managedBitmap = new Bitmap(
                    bitmapStruct.bmWidth, bitmapStruct.bmHeight, bitmapStruct.bmWidthBytes, PixelFormat.Format32bppArgb, (nint)bitmapStruct.bmBits);
                if (bitmapStruct.bmHeight > 0)
                    managedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            else
            {
                managedBitmap = Bitmap.FromHbitmap(hBitmap);
            }
            PInvoke.DeleteObject((HGDIOBJ)hBitmap);
            return managedBitmap;
        }

        private static unsafe bool Has32BitAlpha(in BITMAP bitmapStruct)
        {
            if (bitmapStruct.bmBitsPixel == 32)
            {
                for (int i = 0; i < bitmapStruct.bmHeight; i++)
                {
                    for (int j = 3; j < Math.Abs(bitmapStruct.bmWidthBytes); j += 4)
                    {
                        // Stride here is fine since we know we're doing this on the whole image.
                        unsafe
                        {
                            byte* candidate = unchecked(((byte*)bitmapStruct.bmBits) + (i * bitmapStruct.bmWidthBytes) + j);
                            if (*candidate != 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Load a Bitmap (with transparency) from file (*.bmp or *.png) via Windows API
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        public static Bitmap ImageFromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename))
                throw new ArgumentException("File does not exist", nameof(filename));
            byte[] bytes = new byte[2];
            FileStream stream = File.OpenRead(filename);
            int count;
            if (stream.Length > 54) //minimum length of a bitmap file
                count = stream.Read(bytes, 0, 2);
            stream.Close();
            if (bytes[0] == 0x42 && bytes[1] == 0x4d) //"BM"
            {
                HANDLE handle = PInvoke.LoadImage(HINSTANCE.Null, filename, GDI_IMAGE_TYPE.IMAGE_BITMAP, 0, 0,
                    IMAGE_FLAGS.LR_LOADFROMFILE | IMAGE_FLAGS.LR_CREATEDIBSECTION | IMAGE_FLAGS.LR_SHARED);
                if (handle != HANDLE.Null)
                    return FromHbitmap(new HBITMAP((void*)handle));
                //A V5 Bitmap is not supported by LoadImage, so we try with a Bitmap Ctor
            }
            try
            {
                Bitmap bitmap = new Bitmap(filename);
                if ((int)bitmap.HorizontalResolution != 96)
                {
                    bitmap.SetResolution(96.0f, 96.0f); //only png bitmaps can have other resolution
                }
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Load a Bitmap (with transparency) from a native resource dll via Windows API
        /// The resourceHandle should be Ribbon.MarkupHandle, id is an Image id from RibbonMarkup.h
        /// </summary>
        /// <param name="resourceHandle">LoadLibrary handle (Ribbon.MarkupHandle)</param>
        /// <param name="id">id of the image (from headerfile RibbonMarkup.h)</param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        public static Bitmap ImageFromResource(IntPtr resourceHandle, uint id)
        {
            return ImageFromResource(new HMODULE(resourceHandle), id);
        }

        /// <summary>
        /// Load a Bitmap (with transparency) from a native resource dll via Windows API
        /// The resourceHandle should be Ribbon.MarkupHandle, id is an Image id from RibbonMarkup.h
        /// </summary>
        /// <param name="resourceHandle">LoadLibrary handle (Ribbon.MarkupHandle)</param>
        /// <param name="id">id of the image (from headerfile RibbonMarkup.h)</param>
        /// <returns>The Bitmap with fully transparency if available</returns>
        internal static Bitmap ImageFromResource(HMODULE resourceHandle, uint id)
        {
            if (resourceHandle == IntPtr.Zero)
                throw new ArgumentNullException(nameof(resourceHandle));
            HANDLE bmpHandle = PInvoke.LoadImage(resourceHandle, (PCWSTR)(char*)id, GDI_IMAGE_TYPE.IMAGE_BITMAP, 0, 0, IMAGE_FLAGS.LR_CREATEDIBSECTION | IMAGE_FLAGS.LR_SHARED);
            if (bmpHandle != HANDLE.Null)
            {
                return FromHbitmap(new HBITMAP((void*)bmpHandle));
            }
            //Lookup for the Bitmap resource in the resource folder IMAGE
            //Maybe it is a Bitmap V5 or a PNG Bitmap
            HRSRC hResource = default;
            fixed (char* imageLocal = "IMAGE")
            {
                hResource = PInvoke.FindResource(resourceHandle, (PCWSTR)(char*)id, imageLocal);
            }
            if (hResource == IntPtr.Zero)
                return null;
            uint imageSize = PInvoke.SizeofResource(resourceHandle, hResource);
            if (imageSize == 0)
                return null;
            HGLOBAL res = PInvoke.LoadResource(resourceHandle, hResource);
            void* pResourceData = PInvoke.LockResource(res);
            if (pResourceData is null)
                return null;
            byte[] imageData = new byte[imageSize];
            Marshal.Copy((IntPtr)pResourceData, imageData, 0, (int)imageSize);
            MemoryStream stream = new MemoryStream(imageData);
            return new Bitmap(stream);
        }

        /// <summary>
        /// Set a RGB Color value if the alpha is 0, best guess is Color.LightGray.ToArgb() & 0xffffff
        /// </summary>
        /// <param name="bitmap">The Bitmap</param>
        /// <param name="transparentRGB">The RGB Color if alpha == 0</param>
        /// <returns>The converted Bitmap</returns>
        public static Bitmap SetTransparentRGB(Bitmap bitmap, int transparentRGB)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            int x, y;
            IntPtr p;
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                for (y = 0; y < bitmap.Height; y++)
                {
                    p = data.Scan0 + y * 4 * bitmap.Width;
                    for (x = 0; x < bitmap.Width; x++)
                    {
                        uint value = (uint)Marshal.ReadInt32(p);
                        if ((value & 0xff000000) == 0)
                            Marshal.WriteInt32(p, transparentRGB);
                        p = p + 4;
                    }
                }

                bitmap.UnlockBits(data);
            }
            return bitmap;
        }

        //test with BitmapV5
        public static Bitmap TryARGB(Stream stream)
        {
            Bitmap result = null;
            Bitmap tmp = new Bitmap(stream);
            if (tmp.PixelFormat == PixelFormat.Format32bppRgb && tmp.RawFormat.Guid == ImageFormat.Bmp.Guid)
            {
                BITMAPFILEHEADER bitmapFileHeader = BITMAPFILEHEADER.Create();

            }
            else
                return tmp;
            return result;
        }

        public static Bitmap TryARGB(string filename)
        {
            Bitmap result = null;
            FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            result = TryARGB(fs);
            fs.Close();
            return result;
        }

#if Native
        class NativeMethods
        {
            public enum BitmapCompressionMode : uint
            {
                BI_RGB = 0,
                BI_RLE8 = 1,
                BI_RLE4 = 2,
                BI_BITFIELDS = 3,
                BI_JPEG = 4,
                BI_PNG = 5
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct BITMAPINFOHEADER
            {
                public uint biSize;
                public int biWidth;
                public int biHeight;
                public ushort biPlanes;
                public ushort biBitCount;
                public BitmapCompressionMode biCompression;
                public uint biSizeImage;
                public int biXPelsPerMeter;
                public int biYPelsPerMeter;
                public uint biClrUsed;
                public uint biClrImportant;
                public static BITMAPINFOHEADER Create() => new BITMAPINFOHEADER
                {
                    biSize = (uint)Marshal.SizeOf<BITMAPINFOHEADER>()
                };
            }

            [DllImport("gdi32", CharSet = CharSet.Auto, EntryPoint = "GetObject")]
            public static extern int GetObjectBitmap(IntPtr hObject, int nCount, ref BITMAP lpObject);

            /// <summary>
            /// The BITMAP structure defines the type, width, height, color format, and bit values of a bitmap.
            /// </summary>
            [Serializable]
            [StructLayout(LayoutKind.Sequential)]
            public struct BITMAP
            {
                /// <summary>
                /// The bitmap type. This member must be zero.
                /// </summary>
                public int bmType;

                /// <summary>
                /// The width, in pixels, of the bitmap. The width must be greater than zero.
                /// </summary>
                public int bmWidth;

                /// <summary>
                /// The height, in pixels, of the bitmap. The height must be greater than zero.
                /// </summary>
                public int bmHeight;

                /// <summary>
                /// The number of bytes in each scan line. This value must be divisible by 2, because the system assumes that the bit 
                /// values of a bitmap form an array that is word aligned.
                /// </summary>
                public int bmWidthBytes;

                /// <summary>
                /// The count of color planes.
                /// </summary>
                public short bmPlanes;

                /// <summary>
                /// The number of bits required to indicate the color of a pixel.
                /// </summary>
                public short bmBitsPixel;

                /// <summary>
                /// A pointer to the location of the bit values for the bitmap. The bmBits member must be a pointer to an array of 
                /// character (1-byte) values.
                /// </summary>
                public IntPtr bmBits;
            }

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint type,
                int cxDesired, int cyDesired, uint fuLoad);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadImage(IntPtr hinst, IntPtr lpszName, uint type,
                int cxDesired, int cyDesired, uint fuLoad);

            public enum ImageType
            {
                IMAGE_BITMAP = 0,
                IMAGE_ICON = 1,
                IMAGE_CURSOR = 2
            }

            [Flags]
            public enum ImageLoad
            {
                LR_CREATEDIBSECTION = 0x00002000,
                //When the uType parameter specifies IMAGE_BITMAP, causes the function to return a DIB section bitmap rather than a compatible bitmap. This flag is useful for loading a bitmap without mapping it to the colors of the display device. 
                LR_DEFAULTCOLOR = 0x00000000,
                //The default flag; it does nothing. All it means is "not LR_MONOCHROME". 
                LR_DEFAULTSIZE = 0x00000040,
                //Uses the width or height specified by the system metric values for cursors or icons, if the cxDesired or cyDesired values are set to zero. If this flag is not specified and cxDesired and cyDesired are set to zero, the function uses the actual resource size. If the resource contains multiple images, the function uses the size of the first image. 
                LR_LOADFROMFILE = 0x00000010,
                //Loads the stand-alone image from the file specified by lpszName (icon, cursor, or bitmap file). 
                LR_LOADMAP3DCOLORS = 0x00001000,
                //Searches the color table for the image and replaces the following shades of gray with the corresponding 3-D color.
                //Dk Gray, RGB(128,128,128) with COLOR_3DSHADOW
                //Gray, RGB(192,192,192) with COLOR_3DFACE
                //Lt Gray, RGB(223, 223, 223) with COLOR_3DLIGHT
                //Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.
                LR_LOADTRANSPARENT = 0x00000020,
                //Retrieves the color value of the first pixel in the image and replaces the corresponding entry in the color table with the default window color (COLOR_WINDOW). All pixels in the image that use that entry become the default window color. This value applies only to images that have corresponding color tables.
                //Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.
                //If fuLoad includes both the LR_LOADTRANSPARENT and LR_LOADMAP3DCOLORS values, LR_LOADTRANSPARENT takes precedence.However, the color table entry is replaced with COLOR_3DFACE rather than COLOR_WINDOW.
                LR_MONOCHROME = 0x00000001,
                //Loads the image in black and white.
                LR_SHARED = 0x00008000,
                //Shares the image handle if the image is loaded multiple times.If LR_SHARED is not set, a second call to LoadImage for the same resource will load the image again and return a different handle.
                //When you use this flag, the system will destroy the resource when it is no longer needed.
                //Do not use LR_SHARED for images that have non-standard sizes, that may change after loading, or that are loaded from a file.
                //When loading a system icon or cursor, you must use LR_SHARED or the function will fail to load the resource.
                //This function finds the first image in the cache with the requested resource name, regardless of the size requested.
                LR_VGACOLOR = 0x00000080
            }

            [DllImport("kernel32.dll")]
            public static extern IntPtr FindResource(IntPtr hModule, string lpName, IntPtr lpType);

            [DllImport("kernel32.dll")]
            public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

            [DllImport("kernel32.dll")]
            public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, string lpType);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

            [DllImport("kernel32.dll")]
            public static extern IntPtr LockResource(IntPtr hResData);
        }
#endif //Native
    }
}
