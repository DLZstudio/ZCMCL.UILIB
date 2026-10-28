// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.App.Configuration.Storage;

public class ConfigFileInitException(string path, string message, Exception? inner = null)
    : Exception(message, inner)
{
    /// <summary>
    /// Relative file path.
    /// </summary>
    public string Path { get; } = path;
}
