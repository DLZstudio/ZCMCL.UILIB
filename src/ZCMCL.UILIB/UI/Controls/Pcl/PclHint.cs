// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 结构衍生自 PCL-Community/PCL-CE 的 MyHint（Apache-2.0），由 ZCMCL Contributors 重写实现

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZCMCL.UILIB.UI.Controls.Pcl;

/// <summary>提示条主题（对应 CE 的 MyHint.Themes）。</summary>
public enum PclHintTheme
{
    /// <summary>信息（蓝，色相 210）。</summary>
    Info = 0,
    /// <summary>成功（绿）。</summary>
    Success = 1,
    /// <summary>警告（黄，色相 40）。</summary>
    Warning = 2,
    /// <summary>错误（红，色相 355）。</summary>
    Error = 3
}

/// <summary>
/// PCL 风格的提示条。
///
/// 规格取自 PCL2-CE 的 <c>MyHint</c>：
///   左侧 3px 色条（BorderThickness 3,0,0,0）、圆角 2、
///   文字 Padding 12,9、LineHeight 16、TextWrapping=Wrap。
///   颜色由 HSL(色相, 90%, L7 背景 / L2 边框与文字) 生成；
///   这里改用调色板中已有的等价语义色，避免依赖未移植的 ToneProfile。
///
/// 与上游差异：上游颜色随 ToneProfile 动态计算；本实现用固定语义色，
/// 兼顾浅/深色主题时通过 <see cref="PclHintTheme"/> + 资源键回退。
/// </summary>
public class PclHint : Border
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(PclHint),
            new PropertyMetadata(string.Empty, OnTextChanged));

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(nameof(Theme), typeof(PclHintTheme), typeof(PclHint),
            new PropertyMetadata(PclHintTheme.Info, OnThemeChanged));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public PclHintTheme Theme
    {
        get => (PclHintTheme)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    private readonly TextBlock _text;

    public PclHint()
    {
        BorderThickness = new Thickness(3, 0, 0, 0);
        CornerRadius = new CornerRadius(2);
        HorizontalAlignment = HorizontalAlignment.Stretch;
        SnapsToDevicePixels = true;
        UseLayoutRounding = true;

        _text = new TextBlock
        {
            Padding = new Thickness(12, 9, 12, 9),
            Margin = new Thickness(0, 0, 0, -1),
            LineHeight = 16,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = false
        };
        Child = _text;

        ApplyTheme();
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PclHint h) h._text.Text = e.NewValue as string ?? string.Empty;
    }

    private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PclHint h) h.ApplyTheme();
    }

    private void ApplyTheme()
    {
        // 语义色：与 CE 的 HSL 生成结果观感一致（背景浅、边框与文字深）
        (string bg, string fg) = Theme switch
        {
            PclHintTheme.Info => ("#FFE8F1FD", "#FF1A6FD4"),
            PclHintTheme.Success => ("#FFE9F7EE", "#FF1E7A45"),
            PclHintTheme.Warning => ("#FFFDF6E3", "#FFA8791B"),
            PclHintTheme.Error => ("#FFFDECEC", "#FFC0362C"),
            _ => ("#FFE8F1FD", "#FF1A6FD4")
        };

        Background = ParseBrush(bg);
        BorderBrush = ParseBrush(fg);
        _text.Foreground = ParseBrush(fg);
    }

    private static Brush ParseBrush(string hex)
    {
        var b = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        b.Freeze();
        return b;
    }
}
