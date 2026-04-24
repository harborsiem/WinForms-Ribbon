using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Windows.Win32.UI.Controls;

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

        public static void AllowDarkModeForApp(BOOL allow) =>
            DarkModeHelper.Instance.AllowDarkModeForApp(allow);

        public static void AllowDarkModeForWindow(HWND hwnd, BOOL allow) =>
            DarkModeHelper.Instance.AllowDarkModeForWindow(hwnd, allow);

        public static BOOL ShouldAppsUseDarkMode() =>
            DarkModeHelper.Instance.ShouldAppsUseDarkMode();

        public static BOOL IsDarkModeAllowedForWindow(HWND hwnd) =>
            DarkModeHelper.Instance.IsDarkModeAllowedForWindow(hwnd);

        public static BOOL IsDarkModeAllowedForApp() =>
            DarkModeHelper.Instance.IsDarkModeAllowedForApp();

        public static HTHEME OpenNCThemeData(HWND hwnd, PCWSTR pszClassList) =>
            DarkModeHelper.Instance.OpenNCThemeData(hwnd, pszClassList);

        public static BOOL ShouldSystemUseDarkMode() =>
            DarkModeHelper.Instance.ShouldSystemUseDarkMode();

        public static void RefreshImmersiveColorPolicyState() =>
            DarkModeHelper.Instance.RefreshImmersiveColorPolicyState();

        public static BOOL GetIsImmersiveColorUsingHighContrast(IMMERSIVE_HC_CACHE_MODE mode) =>
            DarkModeHelper.Instance.GetIsImmersiveColorUsingHighContrast(mode);

        public static void FlushMenuThemes() =>
            DarkModeHelper.Instance.FlushMenuThemes();

        public static unsafe BOOL SetWindowCompositionAttribute(HWND hWnd, WINDOWCOMPOSITIONATTRIBDATA* data) =>
            DarkModeHelper.Instance.SetWindowCompositionAttribute(hWnd, data);

        public static void RefreshTitleBarThemeColor(HWND hWnd, BOOL dark) =>
            DarkModeHelper.Instance.RefreshTitleBarThemeColor(hWnd, dark);
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
