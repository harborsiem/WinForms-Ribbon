using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using global::System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.System.Console;
using Windows.Win32.Storage.FileSystem;
using Windows.Win32.UI.WindowsAndMessaging;
//using winmdroot = global::Windows.Win32;

namespace Windows.Win32
{
    internal static partial class CsWin32Ex
    {
        public static double DpiScaling()
        {
            using global::System.Drawing.Graphics g = global::System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            {
                HDC desktop = (HDC)g.GetHdc();
                int LogicalScreenHeight = PInvoke.GetDeviceCaps(desktop, GET_DEVICE_CAPS_INDEX.VERTRES);
                int PhysicalScreenHeight = PInvoke.GetDeviceCaps(desktop, GET_DEVICE_CAPS_INDEX.DESKTOPVERTRES);
                int logpixelsy = PInvoke.GetDeviceCaps(desktop, GET_DEVICE_CAPS_INDEX.LOGPIXELSY);
                float screenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
                float dpiScalingFactor = (float)logpixelsy / (float)96;

                if (screenScalingFactor > 1 ||
                    dpiScalingFactor > 1)
                {
                    // do something nice for people who can't see very well...
                }
                return dpiScalingFactor;
            }
        }

        public static unsafe void InitConsoleHandles()
        {
            HANDLE hStdOut, hStdErr, hStdOutDup, hStdErrDup;
            BY_HANDLE_FILE_INFORMATION bhfi;
            hStdOut = PInvoke.GetStdHandle(STD_HANDLE.STD_OUTPUT_HANDLE);
            hStdErr = PInvoke.GetStdHandle(STD_HANDLE.STD_ERROR_HANDLE);
            // Get current process handle
            HANDLE hProcess = (HANDLE)Process.GetCurrentProcess().Handle;
            // Duplicate Stdout handle to save initial value
            PInvoke.DuplicateHandle(hProcess, hStdOut, hProcess, &hStdOutDup,
            0, true, DUPLICATE_HANDLE_OPTIONS.DUPLICATE_SAME_ACCESS);
            // Duplicate Stderr handle to save initial value
            PInvoke.DuplicateHandle(hProcess, hStdErr, hProcess, &hStdErrDup,
            0, true, DUPLICATE_HANDLE_OPTIONS.DUPLICATE_SAME_ACCESS);
            // Attach to console window – this may modify the standard handles
            PInvoke.AttachConsole(PInvoke.ATTACH_PARENT_PROCESS);
            // Adjust the standard handles
            if (PInvoke.GetFileInformationByHandle(PInvoke.GetStdHandle(STD_HANDLE.STD_OUTPUT_HANDLE), out bhfi))
            {
                PInvoke.SetStdHandle(STD_HANDLE.STD_OUTPUT_HANDLE, hStdOutDup);
            }
            else
            {
                PInvoke.SetStdHandle(STD_HANDLE.STD_OUTPUT_HANDLE, hStdOut);
            }
            if (PInvoke.GetFileInformationByHandle(PInvoke.GetStdHandle(STD_HANDLE.STD_ERROR_HANDLE), out bhfi))
            {
                PInvoke.SetStdHandle(STD_HANDLE.STD_ERROR_HANDLE, hStdErrDup);
            }
            else
            {
                PInvoke.SetStdHandle(STD_HANDLE.STD_ERROR_HANDLE, hStdErr);
            }
        }
    }

    static partial class PInvoke
    {
        public static void SuspendPainting(IntPtr hWnd)
        {
            PInvoke.SendMessage((HWND)hWnd, WM_SETREDRAW, (WPARAM)0, new LPARAM());
        }

        public static void ResumePainting(IntPtr hWnd)
        {
            PInvoke.SendMessage((HWND)hWnd, WM_SETREDRAW, (WPARAM)1, new LPARAM());
        }

        public static unsafe HRSRC FindResource(HMODULE hModule, ushort lpName, string lpType)
        {
            fixed (char* lpTypeLocal = lpType)
            {
                char* lpNameLocal = (char*)lpName;
                HRSRC __result = PInvoke.FindResource(hModule, lpNameLocal, lpTypeLocal);
                return __result;
            }
        }

        public static unsafe HRSRC FindResource(HMODULE hModule, ushort lpName, ushort lpType)
        {
            char* lpTypeLocal = (char*)lpType;
            char* lpNameLocal = (char*)lpName;
            HRSRC __result = PInvoke.FindResource(hModule, lpNameLocal, lpTypeLocal);
            return __result;
        }

        public static unsafe HANDLE LoadImage(HINSTANCE hInst, ushort name, GDI_IMAGE_TYPE type, int cx, int cy, IMAGE_FLAGS fuLoad)
        {
            char* nameLocal = (char*)name;
            HANDLE __result = PInvoke.LoadImage(hInst, nameLocal, type, cx, cy, fuLoad);
            return __result;
        }

        [DllImport("GDI32.dll", ExactSpelling = true, EntryPoint = "SetBkMode")]
        public static extern int SetBkMode(Windows.Win32.Graphics.Gdi.HDC hdc, Windows.Win32.Graphics.Gdi.BACKGROUND_MODE mode);
    }

    namespace UI.Controls
    {
        partial struct COMBOBOXINFO
        {
            public static unsafe COMBOBOXINFO Create()
            {
                return new COMBOBOXINFO() { cbSize = (uint)sizeof(COMBOBOXINFO) };
            }
        }
    }

    namespace Graphics.Gdi
    {
        public enum BACKGROUND_MODE : int
        {
            OPAQUE = 2,
            TRANSPARENT = 1,
        }

        partial struct BITMAPFILEHEADER
        {
            public static unsafe BITMAPFILEHEADER Create()
            {
                return new BITMAPFILEHEADER() { bfType = 0x4d42 }; //"BM"
            }
        }

        partial struct BITMAPCOREHEADER
        {
            public static unsafe BITMAPCOREHEADER Create()
            {
                return new BITMAPCOREHEADER() { bcSize = (uint)sizeof(BITMAPCOREHEADER) };
            }
        }

        partial struct BITMAPINFOHEADER
        {
            public static unsafe BITMAPINFOHEADER Create()
            {
                return new BITMAPINFOHEADER() { biSize = (uint)sizeof(BITMAPINFOHEADER) };
            }
        }

        partial struct BITMAPV4HEADER
        {
            public static unsafe BITMAPV4HEADER Create()
            {
                return new BITMAPV4HEADER() { bV4Size = (uint)sizeof(BITMAPV4HEADER) };
            }
        }

        partial struct BITMAPV5HEADER
        {
            public static unsafe BITMAPV5HEADER Create()
            {
                return new BITMAPV5HEADER() { bV5Size = (uint)sizeof(BITMAPV5HEADER) };
            }
        }
    }
}
