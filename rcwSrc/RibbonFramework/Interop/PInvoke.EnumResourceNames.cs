// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.LibraryLoader;

namespace Windows.Win32;

internal static unsafe partial class PInvoke
{
    public delegate BOOL EnumResNameCallback(HMODULE hModule, PCWSTR pType, PWSTR pName);

    // Stash the delegate to keep it from being collected
    private static readonly ENUMRESNAMEPROCW s_enumNativeCallbackFunctionPointer = HandleEnumResNameNativeCallback;

    public static unsafe BOOL EnumResourceNames(HMODULE hModule, PCWSTR pType, EnumResNameCallback callback)
    {
        // We pass a function pointer to the native function and supply the callback as
        // reference data, so that the CLR doesn't need to generate a native code block for
        // each callback delegate instance (for storing the closure pointer).
        GCHandle gcHandle = GCHandle.Alloc(callback);
        try
        {
            return EnumResourceNames(hModule, pType, s_enumNativeCallbackFunctionPointer, (nint)gcHandle);
        }
        finally
        {
            gcHandle.Free();
        }
    }

    private static BOOL HandleEnumResNameNativeCallback(HMODULE hModule, PCWSTR pType, PWSTR pName, nint lParam)
    {
        return ((EnumResNameCallback)((GCHandle)(nint)lParam).Target!)(hModule, pType, pName);
    }
}
