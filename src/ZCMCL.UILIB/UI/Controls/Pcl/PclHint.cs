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
public class PclHint : Border, WeakEventSubscriber.IThemeAware
{
    /// <summary>主题切换后重新取色（由 WeakEventSubscriber 调用）。</summary>
    public void OnThemeChanged() => ApplyTheme();

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

        // 加载完成后再绑一次：构造发生在主题应用之前时（首次启动很常见），
        // SetResourceReference 找不到键会静默不生效，必须等资源就绪重试。
        Loaded += (_, _) => ApplyTheme();

        // 主题切换后重新取色（本控件的语义色是写死的，无法用 DynamicResource）。
        // 弱引用订阅：控件被回收后不阻止 GC，也不给静态事件留下悬挂引用。
        WeakEventSubscriber.Attach(this);
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PclHint h) h._text.Text = e.NewValue as string ?? string.Empty;
    }

    private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PclHint h) h.ApplyTheme();
    }

    /// <summary>
    /// 取色。
    ///
    /// 演进过程（都踩过坑，记录以免再犯）：
    ///   1. 硬编码 8 组 hex（浅/深各 4 组）→ 主题一改就失效，且要维护两份
    ///   2. 引用 ColorBrushXxxBack 资源键 → 生成时误用了 AHalfTransparent(0.5)，
    ///      底色被白底稀释成 (245,236,200) 近乎白，提示条失去辨识度
    ///   3. 现在：直接用 ToneProfile 的 L7/L2 + HSL(hue, 90%)，
    ///      与 PCL2-CE 的 MyHint.UpdateUI() 一致，不透明、无需判明暗。
    /// </summary>
    private void ApplyTheme()
    {
        var kind = Theme switch
        {
            PclHintTheme.Success => ZCMCL.UILIB.UI.Theme.SemanticColor.Kind.Success,
            PclHintTheme.Warning => ZCMCL.UILIB.UI.Theme.SemanticColor.Kind.Warning,
            PclHintTheme.Error => ZCMCL.UILIB.UI.Theme.SemanticColor.Kind.Error,
            _ => ZCMCL.UILIB.UI.Theme.SemanticColor.Kind.Info
        };

        var bg = ZCMCL.UILIB.UI.Theme.SemanticColor.Background(kind);
        var fg = ZCMCL.UILIB.UI.Theme.SemanticColor.Foreground(kind);

        var bgBrush = new SolidColorBrush(bg);
        bgBrush.Freeze();
        var fgBrush = new SolidColorBrush(fg);
        fgBrush.Freeze();

        Background = bgBrush;
        BorderBrush = fgBrush;
        _text.Foreground = fgBrush;
    }
}
