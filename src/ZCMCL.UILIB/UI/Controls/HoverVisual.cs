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
    private const double AlphaLight = 0.10;

    /// <summary>深色主题下的叠加不透明度（深底上同样的 alpha 更不明显，故提高一档）。</summary>
    private const double AlphaDark = 0.18;

    /// <summary>当前是否处于深色主题。判据：ColorBrush1（主文字色）为浅色即深色主题。</summary>
    public static bool IsDarkTheme
    {
        get
        {
            try
            {
                if (Application.Current?.TryFindResource("ColorBrush1") is SolidColorBrush b)
                    return b.Color.R + b.Color.G + b.Color.B > 3 * 160;
            }
            catch { }
            return false;
        }
    }

    /// <summary>
    /// 悬停背景刷：主题色（ColorBrush3）+ 低透明度。
    /// 每次调用返回新实例（未冻结的缓存会在主题切换后失真）。
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

        var brush = new SolidColorBrush(color)
        {
            Opacity = IsDarkTheme ? AlphaDark : AlphaLight
        };
        brush.Freeze();
        return brush;
    }

    /// <summary>常态阴影色：浅色用近黑（正常投影），深色用纯黑（近白会变成发光）。</summary>
    public static Color IdleShadowColor =>
        IsDarkTheme ? Color.FromRgb(0x00, 0x00, 0x00) : Color.FromRgb(0x34, 0x3D, 0x4A);

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
