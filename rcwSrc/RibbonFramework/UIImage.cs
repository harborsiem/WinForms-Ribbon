using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Controls;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Helper class for IUIImage interface.
    /// This class supports Bitmaps with Alpha channel if available (PixelFormat.Format32bppArgb)
    /// Class need some work
    /// </summary>
    public sealed class UIImage : IDisposable
    {
        private RibbonStrip _ribbon;
        private HBITMAP _hbitmap;
        private IUIImage? _cpIUIImage;

        /// <summary>
        /// Finalizer
        /// </summary>
        ~UIImage()
        {
            Dispose(false);
        }

        private UIImage(RibbonStrip ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon));
            _ribbon = ribbon;
        }

        /// <summary>
        /// Ctor with IUIImage, this should not happen!!!
        /// </summary>
        /// <param name="cpIUIImage"></param>
        internal unsafe UIImage(IUIImage cpIUIImage)
        {
            if (cpIUIImage == null)
                throw new ArgumentNullException(nameof(cpIUIImage));
            _cpIUIImage = cpIUIImage;
            fixed (HBITMAP* phbitmap = &_hbitmap)
                _cpIUIImage.GetBitmap(phbitmap);
            GetBitmapProperties();
        }

        /// <summary>
        /// Ctor for a bitmap file (*.bmp, *.png)
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="filename"></param>
        /// <param name="highContrast"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public UIImage(RibbonStrip ribbon, string filename, bool highContrast = false) : this(ribbon)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(nameof(filename));
            Load(filename, highContrast);
        }

        /// <summary>
        /// Ctor for a bitmap stream (*.bmp, *.png)
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="stream"></param>
        /// <param name="highContrast"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UIImage(RibbonStrip ribbon, Stream stream, bool highContrast = false) : this(ribbon)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            LoadStream(stream, highContrast);
        }

        internal UIImage(RibbonStrip ribbon, HMODULE markupHandle, string resourceName) : this(ribbon)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException(nameof(resourceName));
            Load(markupHandle, resourceName);
        }

        /// <summary>
        /// Ctor for a resource dll
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="markupHandle">Use MarkupHandle from RibbonStrip class</param>
        /// <param name="resourceName">Name in the resource file</param>
        public UIImage(RibbonStrip ribbon, IntPtr markupHandle, string resourceName) : this(ribbon, (HMODULE)markupHandle, resourceName)
        {
        }

        internal UIImage(RibbonStrip ribbon, HMODULE markupHandle, ushort resourceId) : this(ribbon)
        {
            if (resourceId < 1)
                throw new ArgumentOutOfRangeException(nameof(resourceId));
            Load(markupHandle, resourceId);
        }

        /// <summary>
        /// Ctor for a resource dll
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="markupHandle">Use MarkupHandle from RibbonStrip class</param>
        /// <param name="resourceId">Id from the RibbonMarkup.h file</param>
        public UIImage(RibbonStrip ribbon, IntPtr markupHandle, ushort resourceId) : this(ribbon, (HMODULE)markupHandle, resourceId)
        {
        }

        /// <summary>
        /// Ctor for a Bitmap. The Bitmap will be converted to a Bitmap with Alpha channel if possible.
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="bitmap"></param>
        /// <param name="highContrast"></param>
        //UI_OWNERSHIP.UI_OWNERSHIP_COPY or UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER ?
        public unsafe UIImage(RibbonStrip ribbon, Bitmap bitmap, bool highContrast = false) : this(ribbon)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            HRESULT hr;
            HBITMAP dibHBitmap;
            Bitmap bmp = (Bitmap)bitmap.Clone();
            try
            {
                //bmp = TryGetArgbBitmap(bmp);
                if (!highContrast)
                    ConvertToArgbBitmap(bmp);
                dibHBitmap = GetDibHBitmap(bmp);
                if (ribbon.CpIUIImageFromBitmap != null)
                {
                    hr = ribbon.CpIUIImageFromBitmap.CreateImage(dibHBitmap, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out _cpIUIImage);
                    if (hr.Failed)
                    {
                        PInvoke.DeleteObject(dibHBitmap);
                        return;
                    }
                    fixed (HBITMAP* phbitmap = &_hbitmap)
                        _cpIUIImage.GetBitmap(phbitmap);
                    GetBitmapProperties();
                }
            }
            finally
            {
                bmp?.Dispose();
            }
        }

        //UI_OWNERSHIP.UI_OWNERSHIP_COPY or UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER ?
        internal unsafe UIImage(RibbonStrip ribbon, ImageList pImageList, int index) : this(ribbon)
        {
            HRESULT hr;
            Bitmap lBitmap;
            HBITMAP dibHBitmap;
            lBitmap = new Bitmap(pImageList.ImageSize.Width, pImageList.ImageSize.Height, PixelFormat.Format32bppArgb);
            try
            {
                Graphics g = Graphics.FromImage(lBitmap);
                g.Clear(Color.White);
                HDC hdc = (HDC)g.GetHdc();
                BOOL result = PInvoke.ImageList_DrawEx((HIMAGELIST)pImageList.Handle, index, hdc, (lBitmap.Width - pImageList.ImageSize.Width) / 2,
                    (lBitmap.Height - pImageList.ImageSize.Height) / 2, 0, 0, new COLORREF(unchecked((uint)PInvoke.CLR_NONE)),
                    new COLORREF(unchecked((uint)PInvoke.CLR_NONE)), IMAGE_LIST_DRAW_STYLE.ILD_TRANSPARENT);
                g.ReleaseHdc();
                g.Dispose();
                dibHBitmap = GetDibHBitmap(lBitmap);
                if (result == false)
                    return;
                if (ribbon.CpIUIImageFromBitmap != null)
                {
                    hr = ribbon.CpIUIImageFromBitmap.CreateImage(dibHBitmap, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out _cpIUIImage);
                    if (hr.Failed)
                    {
                        PInvoke.DeleteObject(dibHBitmap);
                        return;
                    }
                    fixed (HBITMAP* phbitmap = &_hbitmap)
                        _cpIUIImage.GetBitmap(phbitmap);
                    GetBitmapProperties();
                }
            }
            finally
            {
                lBitmap?.Dispose();
            }
        }

        /// <summary>
        /// Height of the image
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Width of the image
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Number of bits per pixel for the bitmap image. When this value equals 32,
        /// this means that the bitmap has an alpha channel.
        /// </summary>
        public ushort BitsPerPixel { get; private set; }

        internal HBITMAP HBitmapCore { get { return GetHBitmap(); } }

        /// <summary>
        /// Bitmap handle HBITMAP
        /// </summary>
        public IntPtr HBitmap { get { return HBitmapCore; } }

        /// <summary>
        /// The IUIImage interface, Low-level handle to the image
        /// </summary>
        internal IUIImage? UIImageHandle { get { return _cpIUIImage; } }

        private void ConvertToArgbBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb ||
                bitmap.PixelFormat == PixelFormat.Format32bppRgb ||
                bitmap.Height == 0 || bitmap.Width == 0)
                return;
            bitmap.MakeTransparent();
        }

        private unsafe HBITMAP GetHBitmap()
        {
            if (!_hbitmap.IsNull)
                return _hbitmap;
            if (UIImageHandle != null)
            {
                fixed (HBITMAP* phbitmap = &_hbitmap)
                    UIImageHandle.GetBitmap(phbitmap);
                return _hbitmap;
            }
            return HBITMAP.Null;
        }

        private unsafe void GetBitmapProperties()
        {
            BITMAP hBitmap;
            if (PInvoke.GetObject(HBitmapCore, sizeof(BITMAP), &hBitmap) != 0)
            {
                Height = hBitmap.bmHeight;
                Width = hBitmap.bmWidth;
                BitsPerPixel = hBitmap.bmBitsPixel;
                //Do not call DeleteObject here
            }
        }

        private unsafe HBITMAP CreatePreMultipliedBitmap(HBITMAP hBitmap)
        {
            int height, width;
            HBITMAP result = HBITMAP.Null;
            IntPtr data = IntPtr.Zero;
            HDC dc = PInvoke.CreateCompatibleDC(HDC.Null);
            try
            {
                BITMAPINFO info = new BITMAPINFO();
                info.bmiHeader = BITMAPINFOHEADER.Create();
                if (PInvoke.GetDIBits(dc, hBitmap, 0, 0, null, &info, DIB_USAGE.DIB_RGB_COLORS) != 0)
                {
                    width = info.bmiHeader.biWidth;
                    height = Math.Abs(info.bmiHeader.biHeight);
                    info.bmiHeader.biHeight = -height;
                    data = Marshal.AllocHGlobal(width * height * 4);
                    if (PInvoke.GetDIBits(dc, hBitmap, 0, (uint)height, (void*)data, &info, DIB_USAGE.DIB_RGB_COLORS) != 0)
                    {
                        uint* p = (uint*)data;
                        for (int i = 0; i < width * height; i++)
                        {
                            uint alpha = (*p >> 24);
                            uint red = ((*p >> 16) & 0xff);
                            uint green = ((*p >> 8) & 0xff);
                            uint blue = ((*p) & 0xff);

                            //This should be: R= (R * A) div 255, but this is much faster and
                            //good enough for display purposes.
                            red = (alpha * red + 255) >> 8;
                            green = (alpha * green + 255) >> 8;
                            blue = (alpha * blue + 255) >> 8;

                            *p = (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);
                            p++;
                        }
                        result = PInvoke.CreateBitmap(width, height, 1, 32, (void*)data);
                    }
                }
                return result;
            }
            finally
            {
                PInvoke.DeleteDC(dc);
                if (data != IntPtr.Zero)
                    Marshal.FreeHGlobal(data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="xTarget"></param>
        /// <param name="yTarget"></param>
        /// <param name="wTarget"></param>
        /// <param name="hTarget"></param>
        public void Draw(Graphics target, int xTarget, int yTarget, int wTarget, int hTarget)
        {
            Bitmap? bmp = GetBitmap();
            if (bmp != null)
                target.DrawImage(bmp, xTarget, yTarget, wTarget, hTarget);
        }

        internal unsafe void Drawx(Graphics target, int xTarget, int yTarget, int wTarget, int hTarget)
        {
            HBITMAP hBitmap;
            HBITMAP oldHBitmap;
            HDC hdc;
            hBitmap = HBitmapCore;
            if (hBitmap.IsNull)
                return;

            HDC srcDc = PInvoke.CreateCompatibleDC(HDC.Null);
            try
            {
                if (BitsPerPixel == 32)
                {
                    // AlphaBlend requires that the bitmap is pre - multiplied with the Alpha
                    // values.
                    hBitmap = CreatePreMultipliedBitmap(hBitmap);
                    if (hBitmap.IsNull)
                        return;
                    try
                    {
                        oldHBitmap = new HBITMAP((nint)PInvoke.SelectObject(srcDc, hBitmap));
                        BLENDFUNCTION blendFunction = new BLENDFUNCTION()
                        {
                            BlendOp = (byte)PInvoke.AC_SRC_OVER,
                            BlendFlags = 0,
                            SourceConstantAlpha = 255,
                            AlphaFormat = (byte)PInvoke.AC_SRC_ALPHA
                        };
                        hdc = (HDC)target.GetHdc();
                        PInvoke.AlphaBlend(hdc, xTarget, yTarget, wTarget, hTarget, srcDc, 0, 0, Width, Height, blendFunction);
                        target.ReleaseHdc(hdc);
                        PInvoke.SelectObject(srcDc, oldHBitmap);
                    }
                    finally
                    {
                        PInvoke.DeleteObject(hBitmap);
                    }
                }
                else
                {
                    oldHBitmap = new HBITMAP((nint)PInvoke.SelectObject(srcDc, hBitmap));
                    hdc = (HDC)target.GetHdc();
                    PInvoke.BitBlt(hdc, xTarget, yTarget, Width, Height, srcDc, 0, 0, ROP_CODE.SRCCOPY);
                    target.ReleaseHdc(hdc);
                    PInvoke.SelectObject(srcDc, oldHBitmap);
                }
            }
            finally
            {
                PInvoke.DeleteDC(srcDc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="xTarget"></param>
        /// <param name="yTarget"></param>
        public void Draw(Graphics target, int xTarget, int yTarget)
        {
            Draw(target, xTarget, yTarget, Width, Height);
        }

        /// <summary>
        /// Get the Bitmap
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap? GetBitmap()
        {
            if (_hbitmap.IsNull)
                return null;
            return FromHbitmap(_hbitmap);
        }

        private void Load(string filename, bool highContrast)
        {
            LoadBmp(filename, highContrast);
        }

        private unsafe void Load(HMODULE markupHandle, ushort resourceId)
        {
            Load(markupHandle, new PCWSTR((char*)resourceId));
        }

        private unsafe void Load(HMODULE markupHandle, string resourceName)
        {
            fixed (char* resourceNameLocal = resourceName)
                Load(markupHandle, resourceNameLocal);
        }

        //UI_OWNERSHIP.UI_OWNERSHIP_COPY or UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER ?
        private unsafe void Load(HMODULE markupHandle, PCWSTR resourceName)
        {
            HRESULT hr;
            HBITMAP dibHBitmap;
            dibHBitmap = new HBITMAP((void*)PInvoke.LoadImage(markupHandle, resourceName, GDI_IMAGE_TYPE.IMAGE_BITMAP, 0, 0, IMAGE_FLAGS.LR_CREATEDIBSECTION));
            //int err = Marshal.GetLastPInvokeError();
            ////With following code we can get a Bitmap stream
            ////We only have to add a BITMAPFILEHEADER in front of imageData1 array
            //BITMAPFILEHEADER bfh = BITMAPFILEHEADER.Create();
            //BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            //BITMAPINFO bi = new BITMAPINFO();
            //if (!dibHBitmap.IsNull)
            //{
            //    HRSRC hResource1;
            //    hResource1 = PInvoke.FindResource(markupHandle, resourceName, PInvoke.RT_BITMAP);
            //    if (hResource1.IsNull)
            //        return;
            //    uint imageSize1 = PInvoke.SizeofResource(markupHandle, hResource1);
            //    if (imageSize1 == 0)
            //        return;
            //    HGLOBAL res1 = PInvoke.LoadResource(markupHandle, hResource1);
            //    void* pResourceData1 = PInvoke.LockResource(res1);
            //    if (pResourceData1 == null)
            //        return;
            //    byte[] imageData1 = new byte[imageSize1];
            //    Marshal.Copy((IntPtr)pResourceData1, imageData1, 0, (int)imageSize1);

            //    return;
            //}
            if (dibHBitmap.IsNull)
            {
                //Lookup for the Bitmap resource in the resource folder IMAGE
                //Maybe it is a Bitmap V5 or a PNG Bitmap
                HRSRC hResource = default;
                bool error = false;
                fixed (char* imageLocal = "IMAGE")
                {
                    hResource = PInvoke.FindResource(markupHandle, resourceName, imageLocal);
                }
                if (!hResource.IsNull)
                {
                    uint imageSize = PInvoke.SizeofResource(markupHandle, hResource);
                    if (imageSize != 0)
                    {
                        HGLOBAL res = PInvoke.LoadResource(markupHandle, hResource);
                        void* pResourceData = PInvoke.LockResource(res);
                        if (pResourceData is not null)
                        {
                            byte[] imageData = new byte[imageSize];
                            Marshal.Copy((IntPtr)pResourceData, imageData, 0, (int)imageSize);
                            MemoryStream stream = new MemoryStream(imageData);
                            Bitmap bmp = new Bitmap(stream);
                            dibHBitmap = GetDibHBitmap(bmp);
                            bmp.Dispose();
                        }
                        else
                            error = true;
                    }
                    else
                        error = true;
                }
                else
                    error = true;
                if (error)
                    throw new ArgumentException("Not found", nameof(resourceName));
            }
            try
            {
                if (_ribbon.CpIUIImageFromBitmap != null)
                {
                    hr = _ribbon.CpIUIImageFromBitmap.CreateImage(dibHBitmap, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out _cpIUIImage);
                    if (hr.Failed)
                    {
                        PInvoke.DeleteObject(dibHBitmap);
                        _hbitmap = HBITMAP.Null;
                        return;
                    }
                    fixed (HBITMAP* phbitmap = &_hbitmap)
                        _cpIUIImage.GetBitmap(phbitmap);
                    GetBitmapProperties();
                }
            }
            catch
            {
                //PInvoke.DeleteObject(_hbitmap);
                //_hbitmap = HBITMAP.Null;
            }
        }

        private unsafe void LoadStream(Stream stream, bool highContrast)
        {
            HRESULT hr;
            Bitmap? bmp = null;
            HBITMAP dibHBitmap;
            try
            {
                bmp = new Bitmap(stream);
                //bmp = TryGetArgbBitmap(bmp);
                if (!highContrast)
                    ConvertToArgbBitmap(bmp);
                dibHBitmap = GetDibHBitmap(bmp);
                if (_ribbon.CpIUIImageFromBitmap != null)
                {
                    hr = _ribbon.CpIUIImageFromBitmap.CreateImage(dibHBitmap, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out _cpIUIImage);
                    if (hr.Failed)
                    {
                        PInvoke.DeleteObject(dibHBitmap);
                        return;
                    }
                    fixed (HBITMAP* phbitmap = &_hbitmap)
                        _cpIUIImage.GetBitmap(phbitmap);
                    GetBitmapProperties();
                }
            }
            finally
            {
                bmp?.Dispose();
            }
        }

        //UI_OWNERSHIP.UI_OWNERSHIP_COPY or UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER ?
        private unsafe void LoadBmp(string filename, bool highContrast)
        {
            HRESULT hr;
            Bitmap? bmp = null;
            HBITMAP dibHBitmap;
            try
            {
                bmp = new Bitmap(filename);
                //bmp = TryGetArgbBitmap(bmp);
                if (!highContrast)
                    ConvertToArgbBitmap(bmp);
                dibHBitmap = GetDibHBitmap(bmp);
                if (_ribbon.CpIUIImageFromBitmap != null)
                {
                    hr = _ribbon.CpIUIImageFromBitmap.CreateImage(dibHBitmap, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out _cpIUIImage);
                    if (hr.Failed)
                    {
                        PInvoke.DeleteObject(dibHBitmap);
                        return;
                    }
                    fixed (HBITMAP* phbitmap = &_hbitmap)
                        _cpIUIImage.GetBitmap(phbitmap);
                    GetBitmapProperties();
                }
            }
            finally
            {
                bmp?.Dispose();
            }
        }

        private static unsafe Bitmap TryGetArgbBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format32bppRgb && bitmap.RawFormat.Guid == ImageFormat.Bmp.Guid)
            {
                BitmapData? bmpData = null;
                BitmapData? alphaData = null;
                Bitmap? alpha = null;
                try
                {
                    bmpData = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    if (BitmapHasAlpha(bmpData))
                    {
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
                        alpha.UnlockBits(alphaData!);
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
        }

        private static unsafe Bitmap FromHbitmap(HBITMAP hBitmap)
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
            //PInvoke.DeleteObject((HGDIOBJ)hBitmap);
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

        private static RGBQUAD[] RGBQUADFromColorArray(Bitmap bmp)
        {
            // Some programs as Axialis have problems with a reduced palette, so lets create a full palette
            int bits = BitsFromPixelFormat(bmp.PixelFormat);
            RGBQUAD[] rgbArray = new RGBQUAD[bits <= 8 ? (1 << bits) : 0];
            Color[] entries = bmp.Palette.Entries;
            for (int i = 0; i < entries.Length; i++)
            {
                rgbArray[i].rgbRed = entries[i].R;
                rgbArray[i].rgbGreen = entries[i].G;
                rgbArray[i].rgbBlue = entries[i].B;
            }
            return rgbArray;
        }

        private static int BitsFromPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return 1;
                case PixelFormat.Format4bppIndexed:
                    return 4;
                case PixelFormat.Format8bppIndexed:
                    return 8;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;
                default:
                    return 0;
            }
        }

        private unsafe HBITMAP GetDibHBitmap(Bitmap bitmap)
        {
            RGBQUAD[] palette = RGBQUADFromColorArray(bitmap);
            BITMAPINFOHEADER header = new BITMAPINFOHEADER();
            header.biSize = (uint)sizeof(BITMAPINFOHEADER);
            header.biWidth = bitmap.Width;
            header.biHeight = -bitmap.Height;
            header.biPlanes = 1;
            header.biBitCount = (ushort)BitsFromPixelFormat(bitmap.PixelFormat);
            header.biCompression = (uint)BI_COMPRESSION.BI_RGB;
            header.biXPelsPerMeter = 0;
            header.biYPelsPerMeter = 0;
            header.biClrUsed = (uint)palette.Length;
            header.biClrImportant = 0;
            header.biSizeImage = 0;

            HDC hDCScreen = PInvoke.GetDC(HWND.Null);
            int rgbQuadLength = palette.Length;
            byte* b = stackalloc byte[BITMAPINFO.SizeOf(rgbQuadLength)];
            BITMAPINFO* pbi = (BITMAPINFO*)b;
            pbi->bmiHeader = header;
            if (rgbQuadLength > 0)
            {
                palette.CopyTo(pbi->bmiColors.AsSpan(rgbQuadLength));
            }
            HDC hDCScreenOUTBmp = PInvoke.CreateCompatibleDC(hDCScreen);
            //bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            long scanLength = Math.Abs(bmpData.Stride) * bmpData.Height;
            header.biSizeImage = (uint)scanLength;
            //IntPtr scanColor = bmpData.Scan0;
            //byte[] XOR = new byte[Math.Abs(bmpData.Stride) * bmpData.Height];
            //Marshal.Copy(scanColor, XOR, 0, XOR.Length);
            void* bits;
            HBITMAP hBitmapOUTBmp = PInvoke.CreateDIBSection(hDCScreenOUTBmp, pbi, DIB_USAGE.DIB_RGB_COLORS, &bits, HANDLE.Null, 0);
            Buffer.MemoryCopy((void*)bmpData.Scan0, bits, scanLength, scanLength);
            bitmap.UnlockBits(bmpData);

            //Marshal.Copy(XOR, 0, (IntPtr)bits, XOR.Length);

            PInvoke.ReleaseDC(HWND.Null, hDCScreen);
            //PInvoke.DeleteObject(hBitmapOUTBmp);
            PInvoke.DeleteDC(hDCScreenOUTBmp);

            return hBitmapOUTBmp;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            IUIImage? localIUImage = _cpIUIImage;
            _cpIUIImage = null;
            if (localIUImage != null)
            {
                int refCount = Marshal.ReleaseComObject(localIUImage);
                Debug.WriteLine("IUIImage refCount " + refCount);
                //PInvoke.DeleteObject(_hbitmap);
            }
        }
    }
}
