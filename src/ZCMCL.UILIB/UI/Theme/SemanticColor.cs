// SPDX-License-Identifier: MIT
// Copyright 2026 ZCMCL Contributors
// 本项目原创：语义提示色的统一生成

using System;
using System.Windows.Media;

namespace ZCMCL.UILIB.UI.Theme;

/// <summary>
/// 提示条的语义色生成。
///
/// 实现依据 PCL2-CE 的 <c>MyHint.UpdateUI()</c>（Apache-2.0，同仓 PCL-CE 项目）：
/// <code>
/// hue = 蓝 210 / 红 355 / 黄 40
/// Background   = HSL(hue, 90%, tone.L7 * 100)
/// BorderBrush  = HSL(hue, 90%, tone.L2 * 100)
/// Foreground   = HSL(hue, 90%, tone.L2 * 100)
/// </code>
///
/// 要点（此前几版都做错了）：
///   · **不透明**，不要加 alpha。早期实现叠了 AHalfTransparent(0.5)，
///     颜色被白底稀释成 (245,236,200) 这种近乎白的色，提示条失去辨识度。
///   · **HSL 不是 OKLCH**。PCL 这里用 HSL，饱和度固定 90%；
///     用 OKLCH 的 C 值（0.25）再叠加 alpha 会得到完全不同的观感。
///   · 亮度直接用 ToneProfile 的 L7（底）/ L2（字），深色主题下自动变暗/变亮，
///     不需要再判断明暗、也不需要两套硬编码 hex。
/// </summary>
public static class SemanticColor
{
    /// <summary>提示语义。</summary>
    public enum Kind
    {
        /// <summary>信息（蓝，色相 210）。</summary>
        Info,
        /// <summary>成功（绿，色相 145）。</summary>
        Success,
        /// <summary>警告（黄，色相 40）。</summary>
        Warning,
        /// <summary>错误（红，色相 355）。</summary>
        Error
    }

    /// <summary>取语义色相（与 PCL 的 MyHint 一致）。</summary>
    public static double HueOf(Kind kind) => kind switch
    {
        Kind.Error => 355,
        Kind.Warning => 40,
        Kind.Success => 145,
        _ => 210
    };

    /// <summary>提示条背景：HSL(hue, 90%, tone.L7)。不透明。</summary>
    public static Color Background(Kind kind) => FromHsl(HueOf(kind), 0.90, CurrentL7);

    /// <summary>提示条边框与文字：HSL(hue, 90%, tone.L2)。不透明。</summary>
    public static Color Foreground(Kind kind) => FromHsl(HueOf(kind), 0.90, CurrentL2);

    // 当前色阶的 L7 / L2。用 ThemeService 的公开属性，
    // 不在这里判断明暗 —— 明暗差异已经体现在 ToneProfile 的数值里。
    private static double CurrentL7 => ThemeService.CurrentToneProfile.L7;
    private static double CurrentL2 => ThemeService.CurrentToneProfile.L2;

    /// <summary>HSL → RGB（h 度，s/l 为 0–1，不透明）。</summary>
    public static Color FromHsl(double h, double s, double l)
    {
        h = ((h % 360) + 360) % 360;
        s = Math.Clamp(s, 0, 1);
        l = Math.Clamp(l, 0, 1);

        var c = (1 - Math.Abs(2 * l - 1)) * s;
        var x = c * (1 - Math.Abs(h / 60 % 2 - 1));
        var m = l - c / 2;

        (double r, double g, double b) = h switch
        {
            < 60 => (c, x, 0.0),
            < 120 => (x, c, 0.0),
            < 180 => (0.0, c, x),
            < 240 => (0.0, x, c),
            < 300 => (x, 0.0, c),
            _ => (c, 0.0, x)
        };

        return Color.FromRgb(
            (byte)Math.Round(Math.Clamp(r + m, 0, 1) * 255),
            (byte)Math.Round(Math.Clamp(g + m, 0, 1) * 255),
            (byte)Math.Round(Math.Clamp(b + m, 0, 1) * 255));
    }
}
