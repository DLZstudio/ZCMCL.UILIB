// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 结构衍生自 PCL-Community/PCL-CE 的 MyCard（Apache-2.0），由 ZCMCL Contributors 重写实现
// Modified by ZCMCL Contributors (2026): 用 WPF 原生 DropShadowEffect/ContentPresenter 重写，
// 去掉对 MyDropShadow / BlurBorder / AnimatedBackgroundGrid 的依赖

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace ZCMCL.UILIB.UI.Controls.Pcl;

/// <summary>
/// PCL 风格的卡片容器：标题行 + 可选折叠 + 悬停阴影。
///
/// 规格取自 PCL2-CE 的 <c>MyCard</c>：
///   圆角 5；标题 Margin 15,12，FontSize 13，Bold；
///   折叠箭头 10×6 位于右上 Margin 0,17,16,0；
///   内容区起点 y=38（标题行高度）；
///   阴影常态 opacity 0.07，悬停升到 0.4 并转为主题色。
///
/// 与上游实现的差异：这里用 <see cref="DropShadowEffect"/> + <see cref="ContentPresenter"/>
/// 直接构建视觉树（控件自包含，引用程序集即可用，无需合并任何 ResourceDictionary），
/// 不依赖上游的 MyDropShadow / BlurBorder / AnimatedBackgroundGrid。
///
/// 注意：本类继承 <see cref="Grid"/>，XAML 里 <c>&lt;PclCard&gt;内容&lt;/PclCard&gt;</c>
/// 会经由 <see cref="ContentPropertyAttribute"/> 落到 <see cref="Content"/> 属性，
/// 而不是 <see cref="Panel.Children"/> —— 否则内容会盖在标题行上。
/// </summary>
[System.Windows.Markup.ContentProperty(nameof(Content))]
public class PclCard : Grid
{
    // ── 依赖属性 ──────────────────────────────────────────────

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(PclCard),
            new PropertyMetadata(string.Empty, OnTitleChanged));

    public static readonly DependencyProperty IsCollapsibleProperty =
        DependencyProperty.Register(nameof(IsCollapsible), typeof(bool), typeof(PclCard),
            new PropertyMetadata(false, OnCollapsibleChanged));

    public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(PclCard),
            new PropertyMetadata(true, OnExpandedChanged));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PclCard),
            new PropertyMetadata(new CornerRadius(5), OnCornerRadiusChanged));

    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(PclCard),
            new PropertyMetadata(null, OnContentChanged));

    public static readonly DependencyProperty HeaderContentProperty =
        DependencyProperty.Register(nameof(HeaderContent), typeof(object), typeof(PclCard),
            new PropertyMetadata(null, OnHeaderContentChanged));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 标题行右侧的附加内容（折叠箭头左侧）。
    /// 用于 CE 组件卡那种「标题 + 已选版本/图标」的形态。
    /// </summary>
    public object HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public bool IsCollapsible
    {
        get => (bool)GetValue(IsCollapsibleProperty);
        set => SetValue(IsCollapsibleProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>卡片内容。会放进标题行下方的内容区。</summary>
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    // ── 视觉树 ────────────────────────────────────────────────

    private const double TitleRowHeight = 38;   // 内容区起点（CE 的内容 Margin 顶部为 38）
    private const double ShadowIdleOpacity = 0.07;
    private const double ShadowHoverOpacity = 0.40;

    private readonly Border _border;
    private readonly ContentPresenter _host;
    private readonly ContentPresenter _headerSuffix;
    private readonly TextBlock _titleBlock;
    private readonly Path _arrow;
    private readonly DropShadowEffect _shadow;
    private readonly RowDefinitionCollection _innerRows;

    public PclCard()
    {
        // 阴影层（同时承担背景与圆角）
        _shadow = new DropShadowEffect
        {
            BlurRadius = 6,
            ShadowDepth = 1,
            Direction = 270,
            Opacity = ShadowIdleOpacity,
            Color = Color.FromRgb(0x34, 0x3D, 0x4A)   // ColorObject1
        };

        _border = new Border
        {
            CornerRadius = new CornerRadius(5),
            Effect = _shadow,
            Background = Resolve<Brush>("ColorBrushTransparentBackground", Brushes.White),
            BorderBrush = Resolve<Brush>("ColorBrushGray7", Brushes.Gainsboro),
            BorderThickness = new Thickness(1),
            SnapsToDevicePixels = true
        };
        Children.Add(_border);

        // 内容网格：标题行(固定 38) + 内容(*)
        var inner = new Grid();
        inner.RowDefinitions.Add(new RowDefinition { Height = new GridLength(TitleRowHeight) });
        inner.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        _innerRows = inner.RowDefinitions;
        _border.Child = inner;

        // 标题
        _titleBlock = new TextBlock
        {
            Margin = new Thickness(15, 12, 0, 0),
            FontSize = 13,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            IsHitTestVisible = false,
            Foreground = Resolve<Brush>("ColorBrush1", Brushes.Black)
        };
        Grid.SetRow(_titleBlock, 0);
        inner.Children.Add(_titleBlock);

        // 标题行右侧附加内容（折叠箭头左侧）
        _headerSuffix = new ContentPresenter
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 36, 0),   // 给右侧箭头留位
            Visibility = Visibility.Collapsed,
            IsHitTestVisible = true
        };
        Grid.SetRow(_headerSuffix, 0);
        inner.Children.Add(_headerSuffix);

        // 折叠箭头（10×6，右上，指向下方=展开）
        _arrow = new Path
        {
            Data = Geometry.Parse("M2,4 l-2,2 10,10 10,-10 -2,-2 -8,8 -8,-8 z"),
            Width = 10,
            Height = 6,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 17, 16, 0),
            RenderTransformOrigin = new Point(0.5, 0.5),
            RenderTransform = new RotateTransform(180),
            Fill = Resolve<Brush>("ColorBrush1", Brushes.Black),
            Visibility = Visibility.Collapsed,
            IsHitTestVisible = false
        };
        Grid.SetRow(_arrow, 0);
        inner.Children.Add(_arrow);

        // 内容宿主
        _host = new ContentPresenter
        {
            Margin = new Thickness(25, 0, 23, 15),   // 左右 25/23，底部 15（CE 的内容边距）
            Visibility = Visibility.Collapsed
        };
        Grid.SetRow(_host, 1);
        inner.Children.Add(_host);

        // 悬停：阴影加深变主题色 + 标题/箭头变蓝
        MouseEnter += OnMouseEnter;
        MouseLeave += OnMouseLeave;

        RefreshTitleRow();   // 初始无标题时把标题行高度收为 0
    }

    // ── 属性变化 ──────────────────────────────────────────────

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclCard c) return;
        c._titleBlock.Text = e.NewValue as string ?? string.Empty;
        c.RefreshTitleRow();
    }

    private static void OnHeaderContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclCard c) return;
        c._headerSuffix.Content = e.NewValue;
        c._headerSuffix.Visibility = e.NewValue is null ? Visibility.Collapsed : Visibility.Visible;
        c.RefreshTitleRow();
    }

    /// <summary>
    /// 标题行高度随有无标题自适应：
    /// 有标题 → 38（CE 的内容起点）；无标题 → 0，内容改为顶部 15 边距，
    /// 避免无标题卡片顶部空出 38px。
    /// </summary>
    private void RefreshTitleRow()
    {
        var hasTitle = !string.IsNullOrWhiteSpace(_titleBlock.Text);
        var hasHeaderContent = _headerSuffix.Content is not null;
        var showRow = hasTitle || hasHeaderContent;

        _titleBlock.Visibility = hasTitle ? Visibility.Visible : Visibility.Collapsed;
        _headerSuffix.Visibility = hasHeaderContent ? Visibility.Visible : Visibility.Collapsed;
        if (hasTitle)
            _arrow.Visibility = IsCollapsible ? Visibility.Visible : Visibility.Collapsed;

        _innerRows[0].Height = showRow ? new GridLength(TitleRowHeight) : new GridLength(0);
        _host.Margin = showRow
            ? new Thickness(25, 0, 23, 15)
            : new Thickness(15, 12, 15, 12);
    }

    private static void OnCollapsibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclCard c) return;
        var on = (bool)e.NewValue;
        // 只有存在标题时才显示折叠箭头
        c._arrow.Visibility = on && !string.IsNullOrWhiteSpace(c._titleBlock.Text)
            ? Visibility.Visible
            : Visibility.Collapsed;
        c.Cursor = on ? System.Windows.Input.Cursors.Hand : null;
    }

    private static void OnExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclCard c) return;
        var expanded = (bool)e.NewValue;
        c._arrow.RenderTransform = new RotateTransform(expanded ? 180 : 0);
        c._host.Visibility = expanded ? Visibility.Visible : Visibility.Collapsed;
    }

    private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PclCard c) c._border.CornerRadius = (CornerRadius)e.NewValue;
    }

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclCard c) return;
        c._host.Content = e.NewValue;
        // 有内容且处于展开态时显示内容区
        c._host.Visibility = c.IsExpanded ? Visibility.Visible : Visibility.Collapsed;
    }

    // ── 交互 ──────────────────────────────────────────────────

    protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        // 只在标题行区域点击时折叠（避免内容里的按钮误触）
        var pos = e.GetPosition(this);
        if (!IsCollapsible || pos.Y > TitleRowHeight) return;
        IsExpanded = !IsExpanded;
        e.Handled = true;
    }

    private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        AnimateShadow(ShadowHoverOpacity, Resolve<Color>("ColorObject4", Colors.SteelBlue));
        _titleBlock.Foreground = Resolve<Brush>("ColorBrush2", Brushes.SteelBlue);
        if (IsCollapsible) _arrow.Fill = Resolve<Brush>("ColorBrush2", Brushes.SteelBlue);
    }

    private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        AnimateShadow(ShadowIdleOpacity, Resolve<Color>("ColorObject1", Color.FromRgb(0x34, 0x3D, 0x4A)));
        _titleBlock.Foreground = Resolve<Brush>("ColorBrush1", Brushes.Black);
        if (IsCollapsible) _arrow.Fill = Resolve<Brush>("ColorBrush1", Brushes.Black);
    }

    private void AnimateShadow(double opacity, Color color)
    {
        var dur = TimeSpan.FromMilliseconds(90);
        _shadow.BeginAnimation(DropShadowEffect.OpacityProperty,
            new DoubleAnimation(opacity, dur));
        _shadow.BeginAnimation(DropShadowEffect.ColorProperty,
            new ColorAnimation(color, dur));
    }

    // ── 资源查找（找不到时回退，保证控件独立可用） ────────────

    private static T Resolve<T>(string key, T fallback)
    {
        try
        {
            if (Application.Current?.TryFindResource(key) is T v) return v;
        }
        catch { /* 设计期或资源未就绪 */ }
        return fallback;
    }
}
