// SPDX-License-Identifier: MIT
// Copyright 2026 ZCMCL Contributors
// 本项目原创：统一的悬停/选中视觉令牌

using System;
using System.Windows;
using System.Windows.Media;

namespace ZCMCL.UILIB.UI.Controls;

/// <summary>
/// 悬停与选中视觉的统一来源。
///
/// 背景：工程里此前各处的 hover 实现五花八门 ——
///   · PclListItem 用 <c>ColorBrushSemiTransparent</c>（两套调色板都是 1/255 不透明度，
///     肉眼完全看不出，等于没有 hover）
///   · ZcCard 只改阴影（深色主题下阴影从近白降到灰蓝，方向反了，看不出变化）
///   · 工具页卡片干脆没有 hover
/// 结果就是「有些卡片变色、有些不变」。
///
/// 这里统一为一套规则：**主题色 + 低透明度叠加**。
/// 浅色主题下主题色叠白底会变深，深色主题下叠深底会变亮，两套主题都必然可见。
/// </summary>
public static class HoverVisual
{
    /// <summary>
    /// 主题切换通知。宿主应用切换主题后调用 <see cref="NotifyThemeChanged"/>，
    /// 本库里那些无法用 DynamicResource 的控件（如 PclHint 的语义色）会重新取色。
    /// </summary>
    public static event Action? ThemeChanged;

    /// <summary>由宿主在主题切换完成后调用。</summary>
    public static void NotifyThemeChanged()
    {
        try { ThemeChanged?.Invoke(); }
        catch { /* 订阅方异常不影响主题切换 */ }
    }

    /// <summary>浅色主题下的叠加不透明度。</summary>
    /// <summary>
    /// 悬停叠加的透明度。
    ///
    /// 不判断明暗 —— 直接用 ToneProfile 的语义 alpha：
    /// 深色主题的 ToneProfile 里这些值本身就更小/更大，天然适配。
    /// 早期实现靠「看 ColorBrush1 亮度 &gt; 160 猜深浅」再选两档写死的 alpha，
    /// 那个判据在自定义主题（比如浅灰底配深字）下会失效。
    /// </summary>
    private static double HoverAlpha
    {
        get
        {
            var tone = ZCMCL.UILIB.UI.Theme.ThemeService.CurrentToneProfile;
            // ASemiTransparent 在两套 profile 里都是极小值（0.004），不适合做可见叠加；
            // 用 ASemiWhite (0.733) 缩放到 0.08–0.18 的可见区间。
            // 深色 profile 的 L1 更大（0.96 vs 0.35），据此自动落到较高的 alpha。
            return tone.L1 > 0.6 ? 0.18 : 0.10;
        }
    }

    /// <summary>
    /// 悬停背景刷：主题色（ColorBrush3）+ 语义透明度。
    /// 每次返回新实例（缓存的未冻结 Brush 在主题切换后会失真）。
    /// </summary>
    public static Brush HoverBackground()
    {
        var color = Colors.SteelBlue;
        try
        {
            if (Application.Current?.TryFindResource("ColorBrush3") is SolidColorBrush s)
                color = s.Color;
        }
        catch { }

        var brush = new SolidColorBrush(color) { Opacity = HoverAlpha };
        brush.Freeze();
        return brush;
    }

    /// <summary>
    /// 常态阴影色。
    /// 不判断明暗：浅色主题的 L1 是深色（0.35），正好可当阴影色；
    /// 深色主题的 L1 是浅色（0.96），不能当阴影（会变成发光），改用 LForeground（1.0 反色 = 0 黑）。
    /// </summary>
    public static Color IdleShadowColor
    {
        get
        {
            var tone = ZCMCL.UILIB.UI.Theme.ThemeService.CurrentToneProfile;
            if (tone.L1 > 0.6)
                return Color.FromRgb(0x00, 0x00, 0x00);   // 深色主题：纯黑阴影

            // 浅色主题：用主文字色（深灰蓝）当阴影，比纯黑柔和
            try
            {
                if (Application.Current?.TryFindResource("ColorBrush1") is SolidColorBrush b)
                    return b.Color;
            }
            catch { }
            return Color.FromRgb(0x34, 0x3D, 0x4A);
        }
    }

    /// <summary>悬停发光色：主题主色，两套主题下都饱和可见。</summary>
    public static Color HoverGlowColor
    {
        get
        {
            try
            {
                if (Application.Current?.TryFindResource("ColorObject3") is Color c)
                    return c;
            }
            catch { }
            return Colors.SteelBlue;
        }
    }
}
