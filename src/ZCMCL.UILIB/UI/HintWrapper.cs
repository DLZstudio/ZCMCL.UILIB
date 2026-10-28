// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI;

public enum HintTheme
{
    Info,
    Success,
    Error,
    Warning
}

public delegate void HintHandler(
    string message,
    HintTheme theme
);

public static class HintWrapper
{
    public static event HintHandler? OnShow;
    
    public static void Show(string message, HintTheme theme = HintTheme.Info)
    {
        OnShow?.Invoke(message, theme);
    }
}
