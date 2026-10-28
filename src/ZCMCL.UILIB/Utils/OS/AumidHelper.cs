// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using Microsoft.Win32;

namespace ZCMCL.UILIB.Utils.OS;

public static class AumidHelper
{
    public const string Aumid = "PCLCommunity.PCLCE";
    
    public static bool HasAumid()
    {
        using var key = Registry.CurrentUser.OpenSubKey(string.Concat(@"Software\Classes\AppUserModelId\", Aumid));
        return key is not null;
    }
    
    public static void RegisterAumid()
    {
        // .NET 8 在正常情况下不可能返回 null，如果炸了不应该包住而是让他炸下去
        using var key = Registry.CurrentUser.CreateSubKey(string.Concat(@"Software\Classes\AppUserModelId\", Aumid));
        key.SetValue("DisplayName", "Plain Craft Launcher Community Edition");
        key.SetValue("IconUri", IconHelper.GetIconPath());
        key.SetValue("IconBackgroundColor", "FFDDDD");
    }

    public static void UnregisterAumid()
    {
        Registry.CurrentUser.DeleteSubKey(string.Concat(@"Software\Classes\AppUserModelId\", Aumid), false);
    }
}