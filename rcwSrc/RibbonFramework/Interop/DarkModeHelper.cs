using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.LibraryLoader;
using Windows.Win32.UI.Controls;

namespace Windows.Win32.Foundation
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS8625
    //#include <Uxtheme.h>
    public class DarkModeHelper
    {
        // TortoiseGit - a Windows shell extension for easy version control

        // Copyright (C) 2020 - TortoiseSVN

        // This program is free software; you can redistribute it and/or
        // modify it under the terms of the GNU General Public License
        // as published by the Free Software Foundation; either version 2
        // of the License, or (at your option) any later version.

        // This program is distributed in the hope that it will be useful,
        // but WITHOUT ANY WARRANTY; without even the implied warranty of
        // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        // GNU General Public License for more details.

        // You should have received a copy of the GNU General Public License
        // along with this program; if not, write to the Free Software Foundation,
        // 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

        /// helper class for the Windows 10 dark mode
        /// note: we use undocumented APIs here, so be careful!

        ~DarkModeHelper()
        {
            if (!_hUxthemeLib.IsNull)
            {
                PInvoke.FreeLibrary(_hUxthemeLib);
            }
        }

        private static unsafe string GetVersionFromFile(string strFilename)
        {
            uint dwReserved;
            uint dwBufferSize;
            fixed (char* p_strFilename = strFilename)
                dwBufferSize = PInvoke.GetFileVersionInfoSize(p_strFilename, &dwReserved);
            if (dwBufferSize > 0)
            {
                uint nInfoSize = 0,
                nFixedLength = 0;
                void* lpVersion;
                void* lpFixedPointer;
                TRANSARRAY* lpTransArray;

                byte[] buffer = new byte[dwBufferSize];
                dwReserved = 0;
                fixed (char* p_strFilename = strFilename)
                fixed (byte* pBuffer = buffer)
                    PInvoke.GetFileVersionInfo(p_strFilename,
                    dwReserved,
                    dwBufferSize,
                    pBuffer);
                // Check the current language
                fixed (char* p_subBlock = "\\VarFileInfo\\Translation")
                fixed (byte* pBuffer = buffer)
                    PInvoke.VerQueryValue(pBuffer, p_subBlock, &lpFixedPointer, &nFixedLength);
                lpTransArray = (TRANSARRAY*)lpFixedPointer;

                string strLangProductVersion = string.Empty;
                strLangProductVersion = string.Format("\\StringFileInfo\\{0:X4}{1:X4}\\ProductVersion", lpTransArray[0].wLanguageID, lpTransArray[0].wCharacterSet);

                fixed (char* p_strLangProductVersion = strLangProductVersion)
                fixed (byte* pBuffer = buffer)
                    PInvoke.VerQueryValue(pBuffer, p_strLangProductVersion, &lpVersion, &nInfoSize);
                if (nInfoSize > 0 && lpVersion != null)
                {
                    PCWSTR pStr = new PCWSTR((char*)lpVersion);
                    return pStr.ToString();
                }
            }
            return string.Empty;
        }

#if NET8_0_OR_GREATER

        public unsafe readonly struct RtlGetNtVersionNumbersFP
        {
            public readonly delegate* unmanaged<uint*, uint*, uint*, void> Value;

            public RtlGetNtVersionNumbersFP(delegate* unmanaged<uint*, uint*, uint*, void> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator RtlGetNtVersionNumbersFP(delegate* unmanaged<uint*, uint*, uint*, void> value)
                => new(value);
            public static implicit operator delegate* unmanaged<uint*, uint*, uint*, void>(RtlGetNtVersionNumbersFP value)
                => value.Value;
        }

        internal unsafe readonly struct AllowDarkModeForAppFP
        {
            public readonly delegate* unmanaged<BOOL, void> Value;

            public AllowDarkModeForAppFP(delegate* unmanaged<BOOL, void> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator AllowDarkModeForAppFP(delegate* unmanaged<BOOL, void> value)
                => new(value);
            public static implicit operator delegate* unmanaged<BOOL, void>(AllowDarkModeForAppFP value)
                => value.Value;
        }

        internal unsafe readonly struct SetPreferredAppModeFP
        {
            public readonly delegate* unmanaged<PreferredAppMode, PreferredAppMode> Value;

            public SetPreferredAppModeFP(delegate* unmanaged<PreferredAppMode, PreferredAppMode> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator SetPreferredAppModeFP(delegate* unmanaged<PreferredAppMode, PreferredAppMode> value)
                => new(value);
            public static implicit operator delegate* unmanaged<PreferredAppMode, PreferredAppMode>(SetPreferredAppModeFP value)
                => value.Value;
        }

        internal unsafe readonly struct AllowDarkModeForWindowFP
        {
            public readonly delegate* unmanaged<HWND, BOOL, void> Value;

            public AllowDarkModeForWindowFP(delegate* unmanaged<HWND, BOOL, void> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator AllowDarkModeForWindowFP(delegate* unmanaged<HWND, BOOL, void> value)
                => new(value);
            public static implicit operator delegate* unmanaged<HWND, BOOL, void>(AllowDarkModeForWindowFP value)
                => value.Value;
        }

        internal unsafe readonly struct ShouldAppsUseDarkModeFP
        {
            public readonly delegate* unmanaged<BOOL> Value;

            public ShouldAppsUseDarkModeFP(delegate* unmanaged<BOOL> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator ShouldAppsUseDarkModeFP(delegate* unmanaged<BOOL> value)
                => new(value);
            public static implicit operator delegate* unmanaged<BOOL>(ShouldAppsUseDarkModeFP value)
                => value.Value;
        }

        internal unsafe readonly struct IsDarkModeAllowedForWindowFP
        {
            public readonly delegate* unmanaged<HWND, BOOL> Value;

            public IsDarkModeAllowedForWindowFP(delegate* unmanaged<HWND, BOOL> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator IsDarkModeAllowedForWindowFP(delegate* unmanaged<HWND, BOOL> value)
                => new(value);
            public static implicit operator delegate* unmanaged<HWND, BOOL>(IsDarkModeAllowedForWindowFP value)
                => value.Value;
        }

        internal unsafe readonly struct IsDarkModeAllowedForAppFP
        {
            public readonly delegate* unmanaged<BOOL> Value;

            public IsDarkModeAllowedForAppFP(delegate* unmanaged<BOOL> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator IsDarkModeAllowedForAppFP(delegate* unmanaged<BOOL> value)
                => new(value);
            public static implicit operator delegate* unmanaged<BOOL>(IsDarkModeAllowedForAppFP value)
                => value.Value;
        }

        internal unsafe readonly struct ShouldSystemUseDarkModeFP
        {
            public readonly delegate* unmanaged<BOOL> Value;

            public ShouldSystemUseDarkModeFP(delegate* unmanaged<BOOL> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator ShouldSystemUseDarkModeFP(delegate* unmanaged<BOOL> value)
                => new(value);
            public static implicit operator delegate* unmanaged<BOOL>(ShouldSystemUseDarkModeFP value)
                => value.Value;
        }

        internal unsafe readonly struct RefreshImmersiveColorPolicyStateFP
        {
            public readonly delegate* unmanaged<void> Value;

            public RefreshImmersiveColorPolicyStateFP(delegate* unmanaged<void> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator RefreshImmersiveColorPolicyStateFP(delegate* unmanaged<void> value)
                => new(value);
            public static implicit operator delegate* unmanaged<void>(RefreshImmersiveColorPolicyStateFP value)
                => value.Value;
        }

        internal unsafe readonly struct GetIsImmersiveColorUsingHighContrastFP
        {
            public readonly delegate* unmanaged<IMMERSIVE_HC_CACHE_MODE, BOOL> Value;

            public GetIsImmersiveColorUsingHighContrastFP(delegate* unmanaged<IMMERSIVE_HC_CACHE_MODE, BOOL> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator GetIsImmersiveColorUsingHighContrastFP(delegate* unmanaged<IMMERSIVE_HC_CACHE_MODE, BOOL> value)
                => new(value);
            public static implicit operator delegate* unmanaged<IMMERSIVE_HC_CACHE_MODE, BOOL>(GetIsImmersiveColorUsingHighContrastFP value)
                => value.Value;
        }

        internal unsafe readonly struct FlushMenuThemesFP
        {
            public readonly delegate* unmanaged<void> Value;

            public FlushMenuThemesFP(delegate* unmanaged<void> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator FlushMenuThemesFP(delegate* unmanaged<void> value)
                => new(value);
            public static implicit operator delegate* unmanaged<void>(FlushMenuThemesFP value)
                => value.Value;
        }

        internal unsafe readonly struct OpenNCThemeDataFP
        {
            public readonly delegate* unmanaged<HWND, PCWSTR, HTHEME> Value;

            public OpenNCThemeDataFP(delegate* unmanaged<HWND, PCWSTR, HTHEME> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator OpenNCThemeDataFP(delegate* unmanaged<HWND, PCWSTR, HTHEME> value)
                => new(value);
            public static implicit operator delegate* unmanaged<HWND, PCWSTR, HTHEME>(OpenNCThemeDataFP value)
                => value.Value;
        }

        internal unsafe readonly struct SetWindowCompositionAttributeFP
        {
            public readonly delegate* unmanaged<HWND, WINDOWCOMPOSITIONATTRIBDATA*, BOOL> Value;

            public SetWindowCompositionAttributeFP(delegate* unmanaged<HWND, WINDOWCOMPOSITIONATTRIBDATA*, BOOL> value) => Value = value;
            public bool IsNull => Value is null;

            public static implicit operator SetWindowCompositionAttributeFP(delegate* unmanaged<HWND, WINDOWCOMPOSITIONATTRIBDATA*, BOOL> value)
                => new(value);
            public static implicit operator delegate* unmanaged<HWND, WINDOWCOMPOSITIONATTRIBDATA*, BOOL>(SetWindowCompositionAttributeFP value)
                => value.Value;
        }
#else
        private unsafe delegate void RtlGetNtVersionNumbersFP(uint* major, uint* minor, uint* build);
        private delegate void AllowDarkModeForAppFP(BOOL allow);
        private delegate PreferredAppMode SetPreferredAppModeFP(PreferredAppMode appMode);
        private delegate void AllowDarkModeForWindowFP(HWND hwnd, BOOL allow);
        private delegate BOOL ShouldAppsUseDarkModeFP();
        private delegate BOOL IsDarkModeAllowedForWindowFP(HWND hwnd);
        private delegate BOOL IsDarkModeAllowedForAppFP();
        private delegate BOOL ShouldSystemUseDarkModeFP();
        private delegate void RefreshImmersiveColorPolicyStateFP();
        private delegate BOOL GetIsImmersiveColorUsingHighContrastFP(IMMERSIVE_HC_CACHE_MODE mode);
        private delegate void FlushMenuThemesFP();
        private delegate HTHEME OpenNCThemeDataFP(HWND hWnd, PCWSTR pszClassList); //LPCWSTR
        private unsafe delegate BOOL SetWindowCompositionAttributeFP(HWND hwnd, WINDOWCOMPOSITIONATTRIBDATA* data);

#endif

        private RtlGetNtVersionNumbersFP _pRtlGetNtVersionNumbersFP = null;
        private AllowDarkModeForAppFP _pAllowDarkModeForAppFP = null;
        private SetPreferredAppModeFP _pSetPreferredAppModeFP = null;
        private AllowDarkModeForWindowFP _pAllowDarkModeForWindowFP = null;
        private ShouldAppsUseDarkModeFP _pShouldAppsUseDarkModeFP = null;
        private IsDarkModeAllowedForWindowFP _pIsDarkModeAllowedForWindowFP = null;
        private IsDarkModeAllowedForAppFP _pIsDarkModeAllowedForAppFP = null;
        private ShouldSystemUseDarkModeFP _pShouldSystemUseDarkModeFP = null;
        private RefreshImmersiveColorPolicyStateFP _pRefreshImmersiveColorPolicyStateFP = null;
        private GetIsImmersiveColorUsingHighContrastFP _pGetIsImmersiveColorUsingHighContrastFP = null;
        private FlushMenuThemesFP _pFlushMenuThemesFP = null;
        private OpenNCThemeDataFP _pOpenNCThemeDataFP = null;
        private SetWindowCompositionAttributeFP _pSetWindowCompositionAttributeFP = null;

        private HINSTANCE _hUxthemeLib = default; //HMODULE
        private bool _bCanHaveDarkMode = false;
        private static DarkModeHelper s_instance = new DarkModeHelper();
        public static DarkModeHelper Instance { get { return s_instance; } }
        public static int MajorVersion { get; private set; }
        public static int MinorVersion { get; private set; }
        public static int BuildVersion { get; private set; }

        private unsafe DarkModeHelper()
        {
            FARPROC farProc = new FARPROC();
            INITCOMMONCONTROLSEX used = new INITCOMMONCONTROLSEX()
            {
                dwSize = (uint)sizeof(INITCOMMONCONTROLSEX),
                dwICC = INITCOMMONCONTROLSEX_ICC.ICC_STANDARD_CLASSES | INITCOMMONCONTROLSEX_ICC.ICC_BAR_CLASSES | INITCOMMONCONTROLSEX_ICC.ICC_COOL_CLASSES
            };
            PInvoke.InitCommonControlsEx(&used);

            string uxtheme = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "uxtheme.dll");
            int micro = 0;
            _bCanHaveDarkMode = false;

            //var fileVersionInfo = FileVersionInfo.GetVersionInfo(uxtheme);
            //var fileVersion = fileVersionInfo.FileVersion;
            //string trimmedFileVersion = fileVersion.Substring(0, fileVersion.IndexOf("(")).TrimEnd();
            string trimmedFileVersion = GetVersionFromFile("uxtheme.dll");
            var versionToken = trimmedFileVersion.Split('.');

            int[] tokens = new int[versionToken.Length];
            for (int i = 0; i < versionToken.Length; i++)
            {
                tokens[i] = int.Parse(versionToken[i]);
            }
            if (tokens.Length == 4)
            {
                int major = tokens[0];
                int minor = tokens[1];
                micro = tokens[2];

                // the windows 10 update 1809 has the version
                // number as 10.0.17763.1
                if (major > 10)
                    _bCanHaveDarkMode = true;
                else if (major == 10)
                {
                    if (minor > 0)
                        _bCanHaveDarkMode = true;
                    else if (micro > 17762)
                        _bCanHaveDarkMode = true;
                }
                MajorVersion = major;
                MinorVersion = minor;
                BuildVersion = micro;
            }
            fixed (char* uxthemeLocal = uxtheme)
                _hUxthemeLib = PInvoke.LoadLibraryEx(uxthemeLocal, HANDLE.Null, LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_SEARCH_SYSTEM32);

            fixed (char* ntdll = "ntdll.dll")
            {
                fixed (byte* versionNumbers = global::System.Text.Encoding.Default.GetBytes("RtlGetNtVersionNumbers"))
                    farProc = PInvoke.GetProcAddress(PInvoke.GetModuleHandle(ntdll), new PCSTR(versionNumbers));
            }
            if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                _pRtlGetNtVersionNumbersFP = (delegate* unmanaged<uint*, uint*, uint*, void>)farProc.Value;
#else
                _pRtlGetNtVersionNumbersFP = farProc.CreateDelegate<RtlGetNtVersionNumbersFP>();
#endif

            if (!_hUxthemeLib.IsNull && _bCanHaveDarkMode)
            {
                // Note: these functions are undocumented! Which means I shouldn't even use them.
                // So, since MS decided to keep this new feature to themselves, I have to use
                // undocumented functions to adjust.
                // Let's just hope they change their minds and document these functions one day...

                // first try with the names, just in case MS decides to properly export these functions

                if (micro < 18362)
                {
                    fixed (byte* allowDarkModeForApp = global::System.Text.Encoding.Default.GetBytes("AllowDarkModeForApp"))
                        farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(allowDarkModeForApp));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pAllowDarkModeForAppFP = (delegate* unmanaged<BOOL, void>)farProc.Value;
#else
                        _pAllowDarkModeForAppFP = farProc.CreateDelegate<AllowDarkModeForAppFP>();
#endif
                }
                else
                {
                    fixed (byte* setPreferredAppMode = global::System.Text.Encoding.Default.GetBytes("SetPreferredAppMode"))
                        farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(setPreferredAppMode));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pSetPreferredAppModeFP = (delegate* unmanaged<PreferredAppMode, PreferredAppMode>)farProc.Value;
#else
                        _pSetPreferredAppModeFP = farProc.CreateDelegate<SetPreferredAppModeFP>();
#endif
                }
                fixed (byte* allowDarkModeForWindow = global::System.Text.Encoding.Default.GetBytes("AllowDarkModeForWindow"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(allowDarkModeForWindow));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pAllowDarkModeForWindowFP = ((delegate* unmanaged<HWND, BOOL, void>)farProc.Value);
#else
                    _pAllowDarkModeForWindowFP = farProc.CreateDelegate<AllowDarkModeForWindowFP>();
#endif

                fixed (byte* shouldAppsUseDarkMode = global::System.Text.Encoding.Default.GetBytes("ShouldAppsUseDarkMode"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(shouldAppsUseDarkMode));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pShouldAppsUseDarkModeFP = (delegate* unmanaged<BOOL>)farProc.Value;
#else
                    _pShouldAppsUseDarkModeFP = farProc.CreateDelegate<ShouldAppsUseDarkModeFP>();
#endif
                fixed (byte* isDarkModeAllowedForWindow = global::System.Text.Encoding.Default.GetBytes("IsDarkModeAllowedForWindow"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(isDarkModeAllowedForWindow));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pIsDarkModeAllowedForWindowFP = (delegate* unmanaged<HWND, BOOL>)farProc.Value;
#else
                    _pIsDarkModeAllowedForWindowFP = farProc.CreateDelegate<IsDarkModeAllowedForWindowFP>();
#endif
                fixed (byte* isDarkModeAllowedForApp = global::System.Text.Encoding.Default.GetBytes("IsDarkModeAllowedForApp"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(isDarkModeAllowedForApp));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pIsDarkModeAllowedForAppFP = ((delegate* unmanaged<BOOL>)farProc.Value);
#else
                    _pIsDarkModeAllowedForAppFP = farProc.CreateDelegate<IsDarkModeAllowedForAppFP>();
#endif
                fixed (byte* shouldSystemUseDarkMode = global::System.Text.Encoding.Default.GetBytes("ShouldSystemUseDarkMode"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(shouldSystemUseDarkMode));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pShouldSystemUseDarkModeFP = (delegate* unmanaged<BOOL>)farProc.Value;
#else
                    _pShouldSystemUseDarkModeFP = farProc.CreateDelegate<ShouldSystemUseDarkModeFP>();
#endif
                fixed (byte* refreshImmersiveColorPolicyState = global::System.Text.Encoding.Default.GetBytes("RefreshImmersiveColorPolicyState"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(refreshImmersiveColorPolicyState));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pRefreshImmersiveColorPolicyStateFP = (delegate* unmanaged<void>)farProc.Value;
#else
                    _pRefreshImmersiveColorPolicyStateFP = farProc.CreateDelegate<RefreshImmersiveColorPolicyStateFP>();
#endif
                fixed (byte* getIsImmersiveColorUsingHighContrast = global::System.Text.Encoding.Default.GetBytes("GetIsImmersiveColorUsingHighContrast"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(getIsImmersiveColorUsingHighContrast));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pGetIsImmersiveColorUsingHighContrastFP = (delegate* unmanaged<IMMERSIVE_HC_CACHE_MODE, BOOL>)farProc.Value;
#else
                    _pGetIsImmersiveColorUsingHighContrastFP = farProc.CreateDelegate<GetIsImmersiveColorUsingHighContrastFP>();
#endif
                fixed (byte* flushMenuThemes = global::System.Text.Encoding.Default.GetBytes("FlushMenuThemes"))
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, new PCSTR(flushMenuThemes));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pFlushMenuThemesFP = (delegate* unmanaged<void>)farProc.Value;
#else
                    _pFlushMenuThemesFP = farProc.CreateDelegate<FlushMenuThemesFP>();
#endif
                fixed (char* user32 = "user32.dll")
                fixed (byte* setWindowCompositionAttribute = global::System.Text.Encoding.Default.GetBytes("SetWindowCompositionAttribute"))
                    farProc = PInvoke.GetProcAddress(PInvoke.GetModuleHandle(user32), new PCSTR(setWindowCompositionAttribute));
                if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                    _pSetWindowCompositionAttributeFP = (delegate* unmanaged<HWND, WINDOWCOMPOSITIONATTRIBDATA*, BOOL>)farProc.Value;
#else
                    _pSetWindowCompositionAttributeFP = farProc.CreateDelegate<SetWindowCompositionAttributeFP>();
#endif

#if NET8_0_OR_GREATER
                if ((_pAllowDarkModeForAppFP.IsNull) && micro < 18362)
#else
                if ((_pAllowDarkModeForAppFP == null) && micro < 18362)
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(135));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pAllowDarkModeForAppFP = (delegate* unmanaged<BOOL, void>)farProc.Value;
#else
                        _pAllowDarkModeForAppFP = farProc.CreateDelegate<AllowDarkModeForAppFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pSetPreferredAppModeFP.IsNull) && micro >= 18362)
#else
                if ((_pSetPreferredAppModeFP == null) && micro >= 18362)
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(135));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pSetPreferredAppModeFP = (delegate* unmanaged<PreferredAppMode, PreferredAppMode>)farProc.Value;
#else
                        _pSetPreferredAppModeFP = farProc.CreateDelegate<SetPreferredAppModeFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pAllowDarkModeForWindowFP.IsNull))
#else
                if ((_pAllowDarkModeForWindowFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(133));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pAllowDarkModeForWindowFP = ((delegate* unmanaged<HWND, BOOL, void>)farProc.Value);
#else
                        _pAllowDarkModeForWindowFP = farProc.CreateDelegate<AllowDarkModeForWindowFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pShouldAppsUseDarkModeFP.IsNull))
#else
                if ((_pShouldAppsUseDarkModeFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(132));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pShouldAppsUseDarkModeFP = (delegate* unmanaged<BOOL>)farProc.Value;
#else
                        _pShouldAppsUseDarkModeFP = farProc.CreateDelegate<ShouldAppsUseDarkModeFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pIsDarkModeAllowedForWindowFP.IsNull))
#else
                if ((_pIsDarkModeAllowedForWindowFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(137));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pIsDarkModeAllowedForWindowFP = (delegate* unmanaged<HWND, BOOL>)farProc.Value;
#else
                        _pIsDarkModeAllowedForWindowFP = farProc.CreateDelegate<IsDarkModeAllowedForWindowFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pIsDarkModeAllowedForAppFP.IsNull))
#else
                if ((_pIsDarkModeAllowedForAppFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(139));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pIsDarkModeAllowedForAppFP = ((delegate* unmanaged<BOOL>)farProc.Value);
#else
                        _pIsDarkModeAllowedForAppFP = farProc.CreateDelegate<IsDarkModeAllowedForAppFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if (_pOpenNCThemeDataFP.IsNull && micro >= 18290)
#else
                if (_pOpenNCThemeDataFP == null && micro >= 18290)
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(49));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pOpenNCThemeDataFP = (delegate* unmanaged<HWND, PCWSTR, HTHEME>)farProc.Value;
#else
                        _pOpenNCThemeDataFP = farProc.CreateDelegate<OpenNCThemeDataFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if (_pShouldSystemUseDarkModeFP.IsNull)
#else
                if (_pShouldSystemUseDarkModeFP == null)
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(138));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pShouldSystemUseDarkModeFP = (delegate* unmanaged<BOOL>)farProc.Value;
#else
                        _pShouldSystemUseDarkModeFP = farProc.CreateDelegate<ShouldSystemUseDarkModeFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pRefreshImmersiveColorPolicyStateFP.IsNull))
#else
                if ((_pRefreshImmersiveColorPolicyStateFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(104));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pRefreshImmersiveColorPolicyStateFP = (delegate* unmanaged<void>)farProc.Value;
#else
                        _pRefreshImmersiveColorPolicyStateFP = farProc.CreateDelegate<RefreshImmersiveColorPolicyStateFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pGetIsImmersiveColorUsingHighContrastFP.IsNull))
#else
                if ((_pGetIsImmersiveColorUsingHighContrastFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(106));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pGetIsImmersiveColorUsingHighContrastFP = (delegate* unmanaged<IMMERSIVE_HC_CACHE_MODE, BOOL>)farProc.Value;
#else
                        _pGetIsImmersiveColorUsingHighContrastFP = farProc.CreateDelegate<GetIsImmersiveColorUsingHighContrastFP>();
#endif
                }
#if NET8_0_OR_GREATER
                if ((_pFlushMenuThemesFP.IsNull))
#else
                if ((_pFlushMenuThemesFP == null))
#endif
                {
                    farProc = PInvoke.GetProcAddress(_hUxthemeLib, MAKEINTRESOURCEA(136));
                    if (!farProc.IsNull)
#if NET8_0_OR_GREATER
                        _pFlushMenuThemesFP = (delegate* unmanaged<void>)farProc.Value;
#else
                        _pFlushMenuThemesFP = farProc.CreateDelegate<FlushMenuThemesFP>();
#endif
                }
            }
        }

        public unsafe void RtlGetNtVersionNumbers(out uint major, out uint minor, out uint build)
        {
#if NET8_0_OR_GREATER
            if (!_pRtlGetNtVersionNumbersFP.IsNull)
            {
                fixed (uint* majorLocal = &major)
                fixed (uint* minorLocal = &minor)
                fixed (uint* buildLocal = &build)
                    _pRtlGetNtVersionNumbersFP.Value(majorLocal, minorLocal, buildLocal);
                return;
            }
#else
            if (_pRtlGetNtVersionNumbersFP != null)
            {
                fixed (uint* majorLocal = &major)
                fixed (uint* minorLocal = &minor)
                fixed (uint* buildLocal = &build)
                    _pRtlGetNtVersionNumbersFP(majorLocal, minorLocal, buildLocal);
                return;
            }
#endif
            major = 0;
            minor = 0;
            build = 0;
        }

        public bool CanHaveDarkMode()
        {
            return _bCanHaveDarkMode;
        }

        internal unsafe void AllowDarkModeForApp(BOOL allow)
        {
#if NET8_0_OR_GREATER
            if (!_pAllowDarkModeForAppFP.IsNull)
                _pAllowDarkModeForAppFP.Value(allow ? true : false);
            if (!_pSetPreferredAppModeFP.IsNull)
                _pSetPreferredAppModeFP.Value(allow ? PreferredAppMode.ForceDark : PreferredAppMode.Default);
#else
            if (_pAllowDarkModeForAppFP != null)
                _pAllowDarkModeForAppFP(allow ? true : false);
            if (_pSetPreferredAppModeFP != null)
                _pSetPreferredAppModeFP(allow ? PreferredAppMode.ForceDark : PreferredAppMode.Default);
#endif
        }

        internal unsafe void AllowDarkModeForWindow(HWND hwnd, BOOL allow)
        {
#if NET8_0_OR_GREATER
            if (!_pAllowDarkModeForWindowFP.IsNull)
                _pAllowDarkModeForWindowFP.Value(hwnd, allow);
#else
            if (_pAllowDarkModeForWindowFP != null)
                _pAllowDarkModeForWindowFP(hwnd, allow);
#endif
        }

        internal unsafe BOOL ShouldAppsUseDarkMode()
        {
#if NET8_0_OR_GREATER
            if (!_pShouldAppsUseDarkModeFP.IsNull)
                return _pShouldAppsUseDarkModeFP.Value();
#else
            if (_pShouldAppsUseDarkModeFP != null)
                return _pShouldAppsUseDarkModeFP();
#endif
            return false;
        }

        internal unsafe BOOL IsDarkModeAllowedForWindow(HWND hwnd)
        {
#if NET8_0_OR_GREATER
            if (!_pIsDarkModeAllowedForWindowFP.IsNull)
                return _pIsDarkModeAllowedForWindowFP.Value(hwnd);
#else
            if (_pIsDarkModeAllowedForWindowFP != null)
                return _pIsDarkModeAllowedForWindowFP(hwnd);
#endif
            return false;
        }

        internal unsafe BOOL IsDarkModeAllowedForApp()
        {
#if NET8_0_OR_GREATER
            if (!_pIsDarkModeAllowedForAppFP.IsNull)
                return _pIsDarkModeAllowedForAppFP.Value();
#else
            if (_pIsDarkModeAllowedForAppFP != null)
                return _pIsDarkModeAllowedForAppFP();
#endif
            return false;
        }

        internal unsafe HTHEME OpenNCThemeData(HWND hwnd, PCWSTR pszClassList)
        {
#if NET8_0_OR_GREATER
            if (!_pOpenNCThemeDataFP.IsNull)
                return _pOpenNCThemeDataFP.Value(hwnd, pszClassList);
#else
            if (_pOpenNCThemeDataFP != null)
                return _pOpenNCThemeDataFP(hwnd, pszClassList);
#endif
            return HTHEME.Null;
        }

        internal unsafe BOOL ShouldSystemUseDarkMode()
        {
#if NET8_0_OR_GREATER
            if (!_pShouldSystemUseDarkModeFP.IsNull)
                return _pShouldSystemUseDarkModeFP.Value();
#else
            if (_pShouldSystemUseDarkModeFP != null)
                return _pShouldSystemUseDarkModeFP();
#endif
            return false;
        }

        internal unsafe void RefreshImmersiveColorPolicyState()
        {
#if NET8_0_OR_GREATER
            if (!_pRefreshImmersiveColorPolicyStateFP.IsNull)
                _pRefreshImmersiveColorPolicyStateFP.Value();
#else
            if (_pRefreshImmersiveColorPolicyStateFP != null)
                _pRefreshImmersiveColorPolicyStateFP();
#endif
        }

        internal unsafe BOOL GetIsImmersiveColorUsingHighContrast(IMMERSIVE_HC_CACHE_MODE mode)
        {
#if NET8_0_OR_GREATER
            if (!_pGetIsImmersiveColorUsingHighContrastFP.IsNull)
                return _pGetIsImmersiveColorUsingHighContrastFP.Value(mode);
#else
            if (_pGetIsImmersiveColorUsingHighContrastFP != null)
                return _pGetIsImmersiveColorUsingHighContrastFP(mode);
#endif
            return false;
        }

        internal unsafe void FlushMenuThemes()
        {
#if NET8_0_OR_GREATER
            if (!_pFlushMenuThemesFP.IsNull)
                _pFlushMenuThemesFP.Value();
#else
            if (_pFlushMenuThemesFP != null)
                _pFlushMenuThemesFP();
#endif
        }

        internal unsafe BOOL SetWindowCompositionAttribute(HWND hWnd, WINDOWCOMPOSITIONATTRIBDATA* data)
        {
#if NET8_0_OR_GREATER
            if (!_pSetWindowCompositionAttributeFP.IsNull)
                return _pSetWindowCompositionAttributeFP.Value(hWnd, data);
#else
            if (_pSetWindowCompositionAttributeFP != null)
                return _pSetWindowCompositionAttributeFP(hWnd, data);
#endif
            return false;
        }

        internal unsafe void RefreshTitleBarThemeColor(HWND hWnd, BOOL dark)
        {
            WINDOWCOMPOSITIONATTRIBDATA data = new WINDOWCOMPOSITIONATTRIBDATA()
            {
                Attrib = WINDOWCOMPOSITIONATTRIB.WCA_USEDARKMODECOLORS,
                pvData = &dark,
                cbData = (nuint)sizeof(BOOL)
            };
            SetWindowCompositionAttribute(hWnd, &data);
        }

        internal unsafe static PCSTR MAKEINTRESOURCEA(int value)
        {
            return new PCSTR((byte*)new IntPtr(value));
        }
    }

    public enum IMMERSIVE_HC_CACHE_MODE
    {
        IHCM_USE_CACHED_VALUE,
        IHCM_REFRESH
    };

    public enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    };

    public enum WINDOWCOMPOSITIONATTRIB
    {
        WCA_UNDEFINED = 0,
        WCA_NCRENDERING_ENABLED = 1,
        WCA_NCRENDERING_POLICY = 2,
        WCA_TRANSITIONS_FORCEDISABLED = 3,
        WCA_ALLOW_NCPAINT = 4,
        WCA_CAPTION_BUTTON_BOUNDS = 5,
        WCA_NONCLIENT_RTL_LAYOUT = 6,
        WCA_FORCE_ICONIC_REPRESENTATION = 7,
        WCA_EXTENDED_FRAME_BOUNDS = 8,
        WCA_HAS_ICONIC_BITMAP = 9,
        WCA_THEME_ATTRIBUTES = 10,
        WCA_NCRENDERING_EXILED = 11,
        WCA_NCADORNMENTINFO = 12,
        WCA_EXCLUDED_FROM_LIVEPREVIEW = 13,
        WCA_VIDEO_OVERLAY_ACTIVE = 14,
        WCA_FORCE_ACTIVEWINDOW_APPEARANCE = 15,
        WCA_DISALLOW_PEEK = 16,
        WCA_CLOAK = 17,
        WCA_CLOAKED = 18,
        WCA_ACCENT_POLICY = 19,
        WCA_FREEZE_REPRESENTATION = 20,
        WCA_EVER_UNCLOAKED = 21,
        WCA_VISUAL_OWNER = 22,
        WCA_HOLOGRAPHIC = 23,
        WCA_EXCLUDED_FROM_DDA = 24,
        WCA_PASSIVEUPDATEMODE = 25,
        WCA_USEDARKMODECOLORS = 26,
        WCA_LAST = 27
    };

    public unsafe struct WINDOWCOMPOSITIONATTRIBDATA
    {
        public WINDOWCOMPOSITIONATTRIB Attrib;
        public void* pvData;
        public nuint cbData; //SIZE_T
    };

#pragma warning disable CS0649
    internal struct TRANSARRAY
    {
        public ushort wLanguageID;
        public ushort wCharacterSet;
    };
}
