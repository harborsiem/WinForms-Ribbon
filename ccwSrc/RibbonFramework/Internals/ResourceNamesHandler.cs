using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Windows.Win32;

namespace WinForms.Ribbon
{
    internal class ResourceNamesHandler
    {
        private List<string> _resourceNames = new List<string>();
        public List<string> Names => _resourceNames;

        public unsafe ResourceNamesHandler(HMODULE hModule, string pType)
        {
            PInvoke.EnumResNameCallback callback = EnumResNameProc;
            fixed (char* pTypeLocal = pType)
                PInvoke.EnumResourceNames(hModule, pTypeLocal,
                    callback);
        }

        /// <summary>
        /// Callback
        /// </summary>
        /// <param name="hModule"></param>
        /// <param name="pType"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        private unsafe BOOL EnumResNameProc(HMODULE hModule, PCWSTR pType, PWSTR pName)
        {
            _resourceNames.Add(pName.ToString());
            return true;
        }
    }
}
