// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Diagnostics;

namespace ZCMCL.UILIB.App.IoC;

partial class Lifecycle
{
    private static void _RunCurrentExecutable(string? arguments)
    {
        var fileName = Environment.ProcessPath!;
        if (arguments is null) Process.Start(fileName);
        else Process.Start(fileName, arguments);
    }

    private static void _KillCurrentProcess()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "taskkill.exe",
            Arguments = $"/f /t /pid {Environment.ProcessId}",
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process.Start(psi);
    }
}
