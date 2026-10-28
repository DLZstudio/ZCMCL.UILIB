// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.IO;

namespace ZCMCL.UILIB.Utils.OS;

public static class SystemPaths {
    /// <summary>
    /// 系统盘符（含冒号和反斜杠），例如 "C:\"。
    /// </summary>
    public static string DriveLetter { get; } = Path.GetPathRoot(Environment.SystemDirectory)!;
}
