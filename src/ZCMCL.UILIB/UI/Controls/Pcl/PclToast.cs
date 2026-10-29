// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 参考 PCL-Community/PCL-CE 的 HintService 视觉规格（Apache-2.0），由 ZCMCL Contributors 独立实现

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ZCMCL.UILIB.UI.Controls.Pcl;

/// <summary>应用内提示的级别。</summary>
public enum PclToastKind
{
    Success,
    Info,
    Warning,
    Error
}

/// <summary>
/// 窗口内浮动提示（PCL 的 HintService 对应物）。
///
/// 与 <c>UI/ToastNotification</c> 的区别：那个是**系统级** Windows 通知（需要 AUMID 注册、
/// 出现在操作中心）；本类是 PCL 那种**窗口内**提示条，浮在宿主窗口下方。
///
/// 用法：
/// <code>
/// PclToast.Show("已保存", this);                       // 用 this 所在窗口作为宿主
/// PclToast.Show("下载失败", this, PclToastKind.Error);
/// </code>
///
/// 定位：宿主窗口左下角，距底 20（对齐 CE 的 PanHint Margin 20，可配置）。
/// </summary>
public static class PclToast
{
    private sealed class ToastHost
    {
        public Canvas Layer = null!;
        public readonly List<Border> Visible = new();
    }

    private static readonly Dictionary<Window, ToastHost> Hosts = new();

    /// <summary>距底边距（默认 20，对齐 CE 的 PanHint）。</summary>
    public static double BottomMargin { get; set; } = 20;

    /// <summary>距左边距。</summary>
    public static double LeftMargin { get; set; } = 20;

    /// <summary>同时最多显示的条数。</summary>
    public static int MaxVisible { get; set; } = 3;

    /// <summary>
    /// 在 <paramref name="anchor"/> 所属窗口中显示一条提示。
    /// </summary>
    /// <param name="message">提示文字。</param>
    /// <param name="anchor">用于定位宿主的元素（通常传 <c>this</c>）。</param>
    /// <param name="kind">级别，影响左侧色条与文字颜色。</param>
    /// <param name="duration">停留时长；传 <see cref="TimeSpan.Zero"/> 表示不自动消失。</param>
    public static void Show(string message, FrameworkElement anchor,
        PclToastKind kind = PclToastKind.Info, TimeSpan? duration = null)
    {
        var win = Window.GetWindow(anchor);
        if (win is null) return;

        var host = GetOrCreateHost(win);
        var toast = Build(message, kind);

        // 超上限时先移除最早的
        while (host.Visible.Count >= MaxVisible)
        {
            Remove(host, host.Visible[0], immediate: true);
        }

        host.Visible.Add(toast);
        host.Layer.Children.Add(toast);
        host.Layer.UpdateLayout();

        Layout(host);

        // 淡入
        toast.Opacity = 0;
        toast.BeginAnimation(UIElement.OpacityProperty,
            new DoubleAnimation(1, TimeSpan.FromMilliseconds(120)));

        var stay = duration ?? DefaultDuration(kind);
        if (stay <= TimeSpan.Zero) return;

        var timer = new DispatcherTimer { Interval = stay };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            Remove(host, toast, immediate: false);
        };
        timer.Start();
    }

    /// <summary>清除指定窗口的全部提示。</summary>
    public static void Clear(FrameworkElement anchor)
    {
        var win = Window.GetWindow(anchor);
        if (win is null || !Hosts.TryGetValue(win, out var host)) return;

        foreach (var t in host.Visible.ToArray())
            Remove(host, t, immediate: true);
    }

    private static TimeSpan DefaultDuration(PclToastKind kind) => kind switch
    {
        PclToastKind.Success => TimeSpan.FromSeconds(3),
        PclToastKind.Info => TimeSpan.FromSeconds(4),
        PclToastKind.Warning => TimeSpan.FromSeconds(5),
        PclToastKind.Error => TimeSpan.FromSeconds(0),   // 错误常驻，等用户或下一条顶掉
        _ => TimeSpan.FromSeconds(4)
    };

    private static ToastHost GetOrCreateHost(Window win)
    {
        if (Hosts.TryGetValue(win, out var existing) && existing.Layer.Parent != null)
            return existing;

        // 用一个覆盖全窗口、不拦截命中的 Canvas 作为浮层
        var layer = new Canvas
        {
            IsHitTestVisible = false,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        FrameworkElement root = win;
        if (win.Content is Grid g)
        {
            // 直接叠加到现有根 Grid 上，跨所有行
            Grid.SetRowSpan(layer, Math.Max(1, g.RowDefinitions.Count));
            Grid.SetColumnSpan(layer, Math.Max(1, g.ColumnDefinitions.Count));
            g.Children.Add(layer);
        }
        else if (win.Content is Panel p)
        {
            p.Children.Add(layer);
        }
        else
        {
            // 退化为包裹一层
            var wrapper = new Grid();
            var old = win.Content as UIElement;
            win.Content = null;
            if (old != null) wrapper.Children.Add(old);
            wrapper.Children.Add(layer);
            win.Content = wrapper;
            root = wrapper;
        }

        var host = new ToastHost { Layer = layer };
        Hosts[win] = host;

        // 窗口尺寸变化时重排
        win.SizeChanged += (_, _) => Layout(host);
        return host;
    }

    private static void Layout(ToastHost host)
    {
        var layer = host.Layer;
        var availableWidth = layer.ActualWidth > 0 ? layer.ActualWidth : layer.Width;
        if (double.IsNaN(availableWidth) || availableWidth <= 0)
        {
            // 还没测量：等下一次
            layer.Dispatcher.BeginInvoke(new Action(() => Layout(host)));
            return;
        }

        double offset = BottomMargin;
        for (int i = host.Visible.Count - 1; i >= 0; i--)
        {
            var t = host.Visible[i];
            t.Measure(new Size(availableWidth, double.PositiveInfinity));
            var h = t.DesiredSize.Height;
            var y = layer.ActualHeight - offset - h;
            Canvas.SetLeft(t, LeftMargin);
            Canvas.SetTop(t, y < 0 ? 0 : y);
            offset += h + 8;
        }
    }

    private static void Remove(ToastHost host, Border toast, bool immediate)
    {
        host.Visible.Remove(toast);
        if (immediate)
        {
            host.Layer.Children.Remove(toast);
            Layout(host);
            return;
        }

        var fade = new DoubleAnimation(0, TimeSpan.FromMilliseconds(140));
        fade.Completed += (_, _) =>
        {
            host.Layer.Children.Remove(toast);
            Layout(host);
        };
        toast.BeginAnimation(UIElement.OpacityProperty, fade);
    }

    private static Border Build(string message, PclToastKind kind)
    {
        (Color accent, Color bg, Color fg) = kind switch
        {
            PclToastKind.Success => (C(0x1E, 0x7A, 0x45), C(0xE9, 0xF7, 0xEE), C(0x1E, 0x7A, 0x45)),
            PclToastKind.Warning => (C(0xA8, 0x79, 0x1B), C(0xFD, 0xF6, 0xE3), C(0xA8, 0x79, 0x1B)),
            PclToastKind.Error => (C(0xC0, 0x36, 0x2C), C(0xFD, 0xEC, 0xEC), C(0xC0, 0x36, 0x2C)),
            _ => (C(0x1A, 0x6F, 0xD4), C(0xE8, 0xF1, 0xFD), C(0x1A, 0x6F, 0xD4))
        };

        return new Border
        {
            Background = new SolidColorBrush(bg),
            BorderBrush = new SolidColorBrush(accent),
            BorderThickness = new Thickness(3, 0, 0, 0),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(12, 9, 14, 9),
            MaxWidth = 420,
            Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 10, ShadowDepth = 2, Opacity = 0.14, Direction = 270
            },
            Child = new TextBlock
            {
                Text = message,
                FontSize = 12.5,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(fg)
            }
        };
    }

    private static Color C(byte r, byte g, byte b) => Color.FromRgb(r, g, b);
}
