// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Drawing;
using System.IO;
using ZCMCL.UILIB.App;

namespace ZCMCL.UILIB.Utils;

public static class IconHelper
{
    public static string GetIconPath()
    {
        var paths = Path.Combine(Paths.Temp, "icon.png");
        if (!File.Exists(paths))
        {
            CreateIcon();
        }

        return paths;
    }

    private static void CreateIcon()
    {
        using var icon = Icon.ExtractAssociatedIcon(Basics.ExecutablePath) ??
                         throw new InvalidOperationException("无法提取程序图标。");
        using var bitmap = icon.ToBitmap();
        bitmap.Save(Path.Combine(Paths.Temp, "icon.png"));
    }
}