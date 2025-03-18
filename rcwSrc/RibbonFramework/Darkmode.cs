using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Dwm;

namespace WinForms.Ribbon
{
    /// <summary>
    /// 
    /// </summary>
    public static class Darkmode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd">Handle of the Form</param>
        /// <param name="darkmode"></param>
        public static unsafe void DarkModeTitleBar(HWND hwnd, bool darkmode)
        {
            HRESULT hr;
            int trueValue = 0x01, falseValue = 0x00;
            if (darkmode)
            {
                //hr = PInvoke.SetWindowTheme(hwnd, "Explorer", null);
                //PInvoke.SendMessage(hwnd, PInvoke.WM_NCACTIVATE, new WPARAM(1), new LPARAM());
                //hr = PInvoke.SetWindowTheme(hwnd, "DarkMode_Explorer", null);
                hr = PInvoke.DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, &trueValue, (uint)sizeof(int));
                PInvoke.SendMessage(hwnd, PInvoke.WM_NCACTIVATE, new WPARAM(0), new LPARAM());
                PInvoke.UpdateWindow(hwnd);
            }
            else
            {
                //hr = PInvoke.SetWindowTheme(hwnd, "DarkMode_Explorer", null);
                hr = PInvoke.DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, &falseValue, (uint)sizeof(int));
                PInvoke.SendMessage(hwnd, PInvoke.WM_NCACTIVATE, new WPARAM(1), new LPARAM());
                //hr = PInvoke.SetWindowTheme(hwnd, "Explorer", null);
                //PInvoke.SendMessage(hwnd, PInvoke.WM_NCACTIVATE, new WPARAM(0), new LPARAM());
                PInvoke.UpdateWindow(hwnd);
            }
        }

        internal static IntPtr SetClassLong(HWND hwnd, GET_CLASS_LONG_INDEX nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size > 4)
                return SetClassLongPtr(hwnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetClassLong(hwnd, nIndex, unchecked(dwNewLong.ToInt32())));
        }

        [DllImport("user32.dll", EntryPoint = "SetClassLongW", SetLastError = true)]
        internal static extern uint SetClassLong(HWND hwnd, GET_CLASS_LONG_INDEX nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetClassLongPtr", SetLastError = true)]
        internal static extern IntPtr SetClassLongPtr(HWND hwnd, GET_CLASS_LONG_INDEX nIndex, IntPtr dwNewLong);

    }
}
