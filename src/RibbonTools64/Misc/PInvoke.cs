using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Windows.Win32
{
    static partial class PInvoke
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern void CopyMemory(HandleRef destData, HandleRef srcData, int size);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static unsafe extern void RtlMoveMemory(void* destData, void* srcData, nuint size);
    }
}
