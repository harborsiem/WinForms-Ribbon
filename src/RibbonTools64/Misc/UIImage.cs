using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.UI.Controls;

namespace WinForms.Ribbon1
{
    /// <summary>
    /// Helper class for IUIImage interface.
    /// This class supports Bitmaps with Alpha channel if available (PixelFormat.Format32bppArgb)
    /// </summary>
    public class UIImage
    {
        private static IUIImageFromBitmap s_imageFactory;
        //static UIImage()
        //{
        //    s_imageFactory = new UIRibbonImageFromBitmapFactory() as IUIImageFromBitmap;
        //}
        private static bool s_initByRibbon;

        private HBITMAP _hbitmap;
        private IUIImage _handle;
        private object _lockObject = new object();
        private bool factoryDispose;

        private UIImage()
        {
            if (s_imageFactory == null)
            {
                lock (_lockObject)
                {
                    if (s_imageFactory == null)
                        s_imageFactory = new UIRibbonImageFromBitmapFactory() as IUIImageFromBitmap;
                }
            }
        }

        /// <summary>
        /// This ctor is only called by the Ribbon framework
        /// Ribbon framework has to Dispose when framework is destroyed
        /// </summary>
        /// <param name="imageFromBitmap"></param>
        internal UIImage(IUIImageFromBitmap imageFromBitmap)
        {
            if (s_imageFactory == null)
            {
                s_imageFactory = imageFromBitmap;
                factoryDispose = true;
                s_initByRibbon = true;
            }
        }

        internal UIImage(ushort resourceId)
        {
            //?
        }

        /// <summary>
        /// Ctor with IUIImage
        /// </summary>
        /// <param name="handle"></param>
        public unsafe UIImage(IUIImage handle) : this()
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));
            _handle = handle;
            fixed (HBITMAP* bitmapLocal = &_hbitmap)
                _handle.GetBitmap(bitmapLocal);
            GetBitmapProperties();
        }

        /// <summary>
        /// Ctor for a bitmap file (*.bmp, *.png)
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="highContrast"></param>
        public UIImage(string filename, bool highContrast = false) : this()
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(nameof(filename));
            Load(filename, highContrast);
        }

        /// <summary>
        /// Ctor for a resource dll
        /// </summary>
        /// <param name="instance">Use MarkupHandle from Ribbon class</param>
        /// <param name="resourceName"></param>
        public UIImage(HMODULE instance, string resourceName) : this()
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException(nameof(resourceName));
            Load(instance, resourceName);
        }

        /// <summary>
        /// Ctor for a resource dll
        /// </summary>
        /// <param name="instance">Use MarkupHandle from Ribbon class</param>
        /// <param name="resourceId">Id from the RibbonMarkup.h file</param>
        public UIImage(HMODULE instance, ushort resourceId) : this()
        {
            if (resourceId < 1)
                throw new ArgumentOutOfRangeException(nameof(resourceId));
            Load(instance, resourceId);
        }

        /// <summary>
        /// Ctor for a Bitmap. The Bitmap will be converted to a Bitmap with Alpha channel
        /// </summary>
        /// <param name="bitmap"></param>
        public unsafe UIImage(Bitmap bitmap) : this()
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            bitmap = TryGetAlphaBitmap(bitmap);
            s_imageFactory.CreateImage((HBITMAP)bitmap.GetHbitmap(), UI_OWNERSHIP.UI_OWNERSHIP_COPY, out _handle);
            fixed (HBITMAP* bitmapLocal = &_hbitmap)
                _handle.GetBitmap(bitmapLocal);
            GetBitmapProperties();
        }

        internal unsafe UIImage(ImageList pImageList, int index) : this()
        {
            Bitmap lBitmap;
            lBitmap = new Bitmap(pImageList.ImageSize.Width, pImageList.ImageSize.Height, PixelFormat.Format32bppArgb);
            try
            {
                Graphics g = Graphics.FromImage(lBitmap);
                HDC hdc = (HDC)g.GetHdc();
                BOOL result = PInvoke.ImageList_DrawEx((HIMAGELIST)pImageList.Handle, index, hdc, (lBitmap.Width - pImageList.ImageSize.Width) / 2,
                    (lBitmap.Height - pImageList.ImageSize.Height) / 2, 0, 0, new COLORREF(unchecked((uint)PInvoke.CLR_NONE)),
                    new COLORREF(unchecked((uint)PInvoke.CLR_NONE)), IMAGE_LIST_DRAW_STYLE.ILD_TRANSPARENT);
                g.ReleaseHdc();
                g.Dispose();
                if (result == false)
                    return;
                s_imageFactory.CreateImage((HBITMAP)lBitmap.GetHbitmap(), UI_OWNERSHIP.UI_OWNERSHIP_COPY, out _handle);
                fixed (HBITMAP* bitmapLocal = &_hbitmap)
                    _handle.GetBitmap(bitmapLocal);
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

        /// <summary>
        /// Bitmap handle HBITMAP
        /// </summary>
        public HBITMAP HBitmap { get { return GetHBitmap(); } }

        /// <summary>
        /// The IUIImage interface, Low-level handle to the image
        /// </summary>
        public IUIImage Handle { get { return _handle; } }

        private void ConvertToAlphaBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb || bitmap.Height == 0 || bitmap.Width == 0)
                return;
            bitmap.MakeTransparent();
        }

        private unsafe HBITMAP GetHBitmap()
        {
            if (_hbitmap != HBITMAP.Null)
                return _hbitmap;
            if (Handle != null)
            {
                fixed (HBITMAP* bitmapLocal = &_hbitmap)
                    Handle.GetBitmap(bitmapLocal);
                return _hbitmap;
            }
            return HBITMAP.Null;
        }

        private unsafe void GetBitmapProperties()
        {
            BITMAP props;
            if (PInvoke.GetObject(HBitmap, sizeof(BITMAP), &props) != 0)
            {
                Height = props.bmHeight;
                Width = props.bmWidth;
                BitsPerPixel = props.bmBitsPixel;
            }
        }

        private unsafe HBITMAP CreatePreMultipliedBitmap(HBITMAP bitmap)
        {
            int height, width;
            HBITMAP result = HBITMAP.Null;
            IntPtr data = IntPtr.Zero;
            HDC dc = PInvoke.CreateCompatibleDC(new HDC(0));
            try
            {
                BITMAPINFO info = new BITMAPINFO();
                info.bmiHeader = BITMAPINFOHEADER.Create();
                if (PInvoke.GetDIBits(dc, bitmap, 0, 0, null, &info, DIB_USAGE.DIB_RGB_COLORS) != 0)
                {
                    width = info.bmiHeader.biWidth;
                    height = Math.Abs(info.bmiHeader.biHeight);
                    info.bmiHeader.biHeight = -height;
                    data = Marshal.AllocHGlobal(width * height * 4);
                    if (PInvoke.GetDIBits(dc, bitmap, 0, (uint)height, (void*)data, &info, DIB_USAGE.DIB_RGB_COLORS) != 0)
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
            target.DrawImage(GetBitmap(), xTarget, yTarget, wTarget, hTarget);
        }

        internal unsafe void Drawx(Graphics target, int xTarget, int yTarget, int wTarget, int hTarget)
        {
            HBITMAP bitmap = new HBITMAP();
            HBITMAP oldBitmap;
            HDC hdc;
            bitmap = HBitmap;
            if (bitmap == HBITMAP.Null)
                return;

            HDC srcDc = PInvoke.CreateCompatibleDC(new HDC(0));
            try
            {
                if (BitsPerPixel == 32)
                {
                    //bitmap = CreatePreMultipliedBitmap(bitmap);
                    if (bitmap == HBITMAP.Null)
                        return;
                    try
                    {
                        oldBitmap = new HBITMAP(PInvoke.SelectObject(srcDc, bitmap));
                        BLENDFUNCTION blendFunction = new BLENDFUNCTION()
                        {
                            BlendOp = (byte)PInvoke.AC_SRC_OVER,
                            BlendFlags = 0,
                            SourceConstantAlpha = 255,
                            AlphaFormat = (byte)PInvoke.AC_SRC_ALPHA
                        };
                        hdc = (HDC)target.GetHdc();
                        //PInvoke.BitBlt(hdc, xTarget, yTarget, Width, Height, srcDc, 0, 0, ROP_CODE.SRCPAINT);
                        PInvoke.AlphaBlend(hdc, xTarget, yTarget, wTarget, hTarget, srcDc, 0, 0, Width, Height, blendFunction);
                        target.ReleaseHdc(hdc);
                        PInvoke.SelectObject(srcDc, oldBitmap);
                    }
                    finally
                    {
                        PInvoke.DeleteObject(bitmap);
                    }
                }
                else
                {
                    try
                    {
                        oldBitmap = new HBITMAP(PInvoke.SelectObject(srcDc, bitmap));
                        hdc = (HDC)target.GetHdc();
                        PInvoke.BitBlt(hdc, xTarget, yTarget, Width, Height, srcDc, 0, 0, ROP_CODE.SRCCOPY);
                        target.ReleaseHdc(hdc);
                        PInvoke.SelectObject(srcDc, oldBitmap);
                    }
                    finally
                    {
                        PInvoke.DeleteObject(bitmap);
                    }
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
        public Bitmap GetBitmap()
        {
            if (_hbitmap == HBITMAP.Null)
                return null;
            return FromHbitmap(_hbitmap);
        }

        private void Load(string filename, bool highContrast)
        {
            _handle = null;
            _hbitmap = HBITMAP.Null;
            string ext = Path.GetExtension(filename).ToUpperInvariant();
            if (ext == ".BMP")
                LoadBmp(filename, highContrast);
            else if (ext == ".PNG")
                LoadPng(filename, highContrast);
            else
                throw new ArgumentException("Unsupported image file extensions ()", nameof(filename));
            GetBitmapProperties();
            DoChanged();
        }

        void DoChanged()
        {

        }

        private unsafe void Load(HMODULE instance, ushort resourceId)
        {
            Load(instance, new PCWSTR((char*)resourceId));
        }

        void Load(ushort resourceId)
        {
            // ?
        }

        private unsafe void Load(HMODULE instance, string resourceName)
        {
            fixed (char* resourceNameLocal = resourceName)
                Load(instance, resourceNameLocal);
        }

        private unsafe void Load(HMODULE instance, PCWSTR resourceName)
        {
            _hbitmap = new HBITMAP(PInvoke.LoadImage(instance, resourceName, GDI_IMAGE_TYPE.IMAGE_BITMAP, 0, 0, IMAGE_FLAGS.LR_CREATEDIBSECTION));
            if (_hbitmap == HBITMAP.Null)
            {
                //Lookup for the Bitmap resource in the resource folder IMAGE
                //Maybe it is a Bitmap V5 or a PNG Bitmap
                HRSRC hResource = default;
                bool error = false;
                fixed (char* imageLocal = "IMAGE")
                {
                    hResource = PInvoke.FindResource(instance, resourceName, imageLocal);
                }
                if (hResource != IntPtr.Zero)
                {
                    uint imageSize = PInvoke.SizeofResource(instance, hResource);
                    if (imageSize != 0)
                    {
                        HGLOBAL res = PInvoke.LoadResource(instance, hResource);
                        void* pResourceData = PInvoke.LockResource(res);
                        if (pResourceData is not null)
                        {
                            byte[] imageData = new byte[imageSize];
                            Marshal.Copy((IntPtr)pResourceData, imageData, 0, (int)imageSize);
                            MemoryStream stream = new MemoryStream(imageData);
                            Bitmap bmp = new Bitmap(stream);
                            _hbitmap = (HBITMAP)bmp.GetHbitmap();
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
                    throw new ArgumentException("Can not find resourceName");
            }
            try
            {
                s_imageFactory.CreateImage(_hbitmap, UI_OWNERSHIP.UI_OWNERSHIP_TRANSFER, out _handle);
                GetBitmapProperties();
            }
            catch
            {
                PInvoke.DeleteObject(_hbitmap);
                _hbitmap = HBITMAP.Null;
            }
        }

        private unsafe void LoadBmp(string filename, bool highContrast)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(filename);
                bmp = TryGetAlphaBitmap(bmp);
                if (!highContrast)
                    ConvertToAlphaBitmap(bmp);
                s_imageFactory.CreateImage((HBITMAP)bmp.GetHbitmap(), UI_OWNERSHIP.UI_OWNERSHIP_COPY, out _handle);
                fixed (HBITMAP* bitmapLocal = &_hbitmap)
                    _handle.GetBitmap(bitmapLocal);
            }
            finally
            {
                bmp?.Dispose();
            }
        }

        private unsafe void LoadPng(string filename, bool highContrast)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(filename);
                if (!highContrast)
                    ConvertToAlphaBitmap(bmp);
                s_imageFactory.CreateImage((HBITMAP)bmp.GetHbitmap(), UI_OWNERSHIP.UI_OWNERSHIP_COPY, out _handle);
                fixed (HBITMAP* bitmapLocal = &_hbitmap)
                    _handle.GetBitmap(bitmapLocal);
            }
            finally
            {
                bmp?.Dispose();
            }
        }

        private static unsafe Bitmap TryGetAlphaBitmap(Bitmap bitmap)
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
            if (_handle != null)
            {
                PInvoke.DeleteObject(_hbitmap);
                _handle = null;
            }
            if (s_imageFactory != null && factoryDispose)
            {
                s_imageFactory = null;
                s_initByRibbon = false;
            }
        }

        /// <summary>
        /// Should only be used at the end of an application (FormClose)
        /// </summary>
        public static void Destroy()
        {
            if (!s_initByRibbon && s_imageFactory != null)
            {
                Marshal.ReleaseComObject(s_imageFactory);
                s_imageFactory = null;
            }
        }
    }
}
