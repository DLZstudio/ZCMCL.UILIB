// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ZCMCL.UILIB.Utils.OS;

public static partial class NtInterop
{
    [LibraryImport("ntdll.dll")]
    private static partial void RtlGetNtVersionNumbers(
        out int major,
        out int minor,
        out int build);

    private static void _ThrowLastWin32Error(int? errorCode = null) => throw new Win32Exception(errorCode ?? Marshal.GetLastWin32Error());
    
    /// <summary>
    /// Retrieve the kernel version number of the current operating system (unaffected by compatibility settings)
    /// </summary>
    /// <returns>A <see cref="Version"/> instance, used to represent the current operating system kernel version number.</returns>
    public static Version GetCurrentOsVersion()
    {
        RtlGetNtVersionNumbers(out var major, out var minor, out var build);
        build &= 0xFFFF;
        return new Version(major, minor, build);
    }
}
