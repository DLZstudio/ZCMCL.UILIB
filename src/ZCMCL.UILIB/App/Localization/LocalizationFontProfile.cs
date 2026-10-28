// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.App.Localization;

/// <summary>
///     UI 字体策略。
/// </summary>
public enum LocalizationFontProfile
{
    /// <summary>
    ///     英文与拉丁文字优先。
    /// </summary>
    English,

    /// <summary>
    ///     简体中文标准字形。
    /// </summary>
    SimplifiedChinese,

    /// <summary>
    ///     繁体中文标准字形。
    /// </summary>
    TraditionalChinese,

    /// <summary>
    ///     日文标准字形。
    /// </summary>
    Japanese,

    /// <summary>
    ///     韩文标准字形。
    /// </summary>
    Korean,

    /// <summary>
    ///     其他语言。
    /// </summary>
    Other
}