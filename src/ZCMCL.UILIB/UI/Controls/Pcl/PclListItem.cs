// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 结构衍生自 PCL-Community/PCL-CE 的 MyListItem（Apache-2.0），由 ZCMCL Contributors 重写实现
// Modified by ZCMCL Contributors (2026): 以 Grid 重写并简化列结构，
// 去掉上游的 Inlines / ContentHandler / 动画缩放等启动器内部依赖

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ZCMCL.UILIB.UI.Controls.SvgIcon;

namespace ZCMCL.UILIB.UI.Controls.Pcl;

/// <summary>
/// PCL 风格的列表项 / 导航行。
///
/// 规格取自 PCL2-CE 的 <c>MyListItem</c>：
///   行高 42（列表内常用 36）；
///   图标区宽 34（图标约 18）；
///   选中时左侧 5px 圆角 2 的主题色竖条，Margin -1,6,0,6；
///   选中 / 悬停时图标与文字同步变主题色。
///
/// 与上游差异：上游基于 <c>IMyRadio</c> 接口自管理互斥并带缩放动画；
/// 这里只保留视觉与选中态，互斥交给宿主（<see cref="PclListItemGroup"/>）。
/// </summary>
public class PclListItem : Grid
{
    // ── 依赖属性 ──────────────────────────────────────────────

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(PclListItem),
            new PropertyMetadata(string.Empty, OnTitleChanged));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(PclListItem),
            new PropertyMetadata(string.Empty, OnIconChanged));

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(PclListItem),
            new PropertyMetadata(false, OnSelectedChanged));

    public static readonly DependencyProperty ShowIndicatorProperty =
        DependencyProperty.Register(nameof(ShowIndicator), typeof(bool), typeof(PclListItem),
            new PropertyMetadata(true, OnShowIndicatorChanged));

    public static readonly DependencyProperty RightContentProperty =
        DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(PclListItem),
            new PropertyMetadata(null, OnRightContentChanged));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>Lucide 图标名，如 "lucide/play"（省略包名时用默认包）。</summary>
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>是否显示选中竖条（纯导航项可关掉，只变色）。</summary>
    public bool ShowIndicator
    {
        get => (bool)GetValue(ShowIndicatorProperty);
        set => SetValue(ShowIndicatorProperty, value);
    }

    /// <summary>右侧附加内容（如刷新按钮）。</summary>
    public object RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>选中状态变化（由宿主用于互斥）。</summary>
    public event EventHandler<bool>? SelectedChanged;

    // ── 视觉树 ────────────────────────────────────────────────

    private const double DefaultHeight = 36;
    private const double IndicatorWidth = 5;
    private const double LogoColumnWidth = 34;

    private readonly Border _indicator;
    private readonly SvgIcon.SvgIcon _glyph;
    private readonly TextBlock _label;
    private readonly ContentPresenter _right;
    private readonly Brush _normalBrush;
    private readonly Brush _accentBrush;

    public PclListItem()
    {
        Height = DefaultHeight;
        Background = Brushes.Transparent;
        RenderTransformOrigin = new Point(0.5, 0.5);
        Cursor = Cursors.Hand;
        SnapsToDevicePixels = true;

        _normalBrush = Resolve<Brush>("ColorBrush1", Brushes.Black);
        _accentBrush = Resolve<Brush>("ColorBrush3", Brushes.SteelBlue);

        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });                 // 选中条
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LogoColumnWidth) }); // 图标
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });                 // 右侧

        // 选中竖条
        _indicator = new Border
        {
            Width = IndicatorWidth,
            CornerRadius = new CornerRadius(2),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(-1, 0, 0, 0),
            Height = 0,
            Background = _accentBrush,
            IsHitTestVisible = false
        };
        Grid.SetColumn(_indicator, 0);
        Children.Add(_indicator);

        // 图标
        _glyph = new SvgIcon.SvgIcon
        {
            Width = 18,
            Height = 18,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Stretch = Stretch.Uniform,
            IconBrush = _normalBrush,
            Visibility = Visibility.Collapsed
        };
        Grid.SetColumn(_glyph, 1);
        Children.Add(_glyph);

        // 标题
        _label = new TextBlock
        {
            Margin = new Thickness(4, 0, 0, 0),
            FontSize = 13,
            VerticalAlignment = VerticalAlignment.Center,
            TextTrimming = TextTrimming.CharacterEllipsis,
            Foreground = _normalBrush,
            IsHitTestVisible = false
        };
        Grid.SetColumn(_label, 2);
        Children.Add(_label);

        // 右侧内容
        _right = new ContentPresenter
        {
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 8, 0),
            Visibility = Visibility.Collapsed
        };
        Grid.SetColumn(_right, 3);
        Children.Add(_right);

        MouseEnter += OnMouseEnter;
        MouseLeave += OnMouseLeave;
        MouseLeftButtonUp += OnMouseUp;
    }

    // ── 属性变化 ──────────────────────────────────────────────

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PclListItem i) i._label.Text = e.NewValue as string ?? string.Empty;
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclListItem i) return;
        var icon = e.NewValue as string;
        if (string.IsNullOrEmpty(icon))
        {
            i._glyph.Visibility = Visibility.Collapsed;
            return;
        }
        i._glyph.Icon = icon;
        i._glyph.Visibility = Visibility.Visible;
    }

    private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclListItem i) return;
        var sel = (bool)e.NewValue;
        i.RefreshVisual();
        i.SelectedChanged?.Invoke(i, sel);
    }

    private static void OnShowIndicatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclListItem i) return;
        if (!(bool)e.NewValue) i._indicator.Height = 0;
        else i.RefreshVisual();
    }

    private static void OnRightContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclListItem i) return;
        i._right.Content = e.NewValue;
        i._right.Visibility = e.NewValue is null ? Visibility.Collapsed : Visibility.Visible;
    }

    // ── 视觉状态 ──────────────────────────────────────────────

    private void RefreshVisual()
    {
        var accent = IsSelected;
        _label.Foreground = accent ? _accentBrush : _normalBrush;
        _glyph.IconBrush = accent ? _accentBrush : _normalBrush;

        if (ShowIndicator)
        {
            // 选中：竖条撑满（留上下 6 的边距）；未选中：高度收 0
            var target = accent ? Height - 12 : 0;
            if (target < 0) target = 0;
            _indicator.BeginAnimation(HeightProperty,
                new DoubleAnimation(target, TimeSpan.FromMilliseconds(120)));
            _indicator.Margin = accent
                ? new Thickness(-1, 6, 0, 6)
                : new Thickness(-1, 0, 0, 0);
            _indicator.VerticalAlignment = accent ? VerticalAlignment.Stretch : VerticalAlignment.Center;
        }
        else
        {
            _indicator.Height = 0;
        }
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
        => Background = Resolve<Brush>("ColorBrushSemiTransparent", Brushes.Transparent);

    private void OnMouseLeave(object sender, MouseEventArgs e) => Background = Brushes.Transparent;

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (!IsEnabled) return;
        Click?.Invoke(this, EventArgs.Empty);
        e.Handled = true;
    }

    /// <summary>点击（由宿主接管选中逻辑）。</summary>
    public event EventHandler? Click;

    private static T Resolve<T>(string key, T fallback)
    {
        try
        {
            if (Application.Current?.TryFindResource(key) is T v) return v;
        }
        catch { }
        return fallback;
    }
}
