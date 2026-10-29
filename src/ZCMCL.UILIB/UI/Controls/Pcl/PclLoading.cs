// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 结构衍生自 PCL-Community/PCL-CE 的 MyLoading（Apache-2.0），由 ZCMCL Contributors 重写实现
// Modified by ZCMCL Contributors (2026): 用 WPF 动画重写，不依赖上游的图片序列帧资源

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ZCMCL.UILIB.UI.Controls.Pcl;

/// <summary>
/// PCL 风格的加载指示器。
///
/// 上游 MyLoading 是「镐子挥动」的图片序列帧（依赖内置 PNG 资源）。
/// 本实现改用矢量 + 动画重绘：一个旋转的圆弧环，
/// 视觉语义与「正在加载」一致，且不引入二进制资源、可随主题变色。
/// </summary>
public class PclLoading : Grid
{
    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register(nameof(Size), typeof(double), typeof(PclLoading),
            new PropertyMetadata(36d, OnSizeChanged));

    public static readonly DependencyProperty IsRunningProperty =
        DependencyProperty.Register(nameof(IsRunning), typeof(bool), typeof(PclLoading),
            new PropertyMetadata(true, OnRunningChanged));

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    private readonly Path _arc;
    private readonly RotateTransform _rotate;

    public PclLoading()
    {
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;

        _rotate = new RotateTransform(0);
        _arc = new Path
        {
            Data = BuildArcGeometry(),
            StrokeThickness = 2.4,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            Stretch = Stretch.Uniform,
            RenderTransformOrigin = new Point(0.5, 0.5),
            RenderTransform = _rotate,
            Stroke = new SolidColorBrush(Color.FromRgb(0x13, 0x70, 0xF3))   // ColorBrush3
        };
        Children.Add(_arc);

        Width = Size;
        Height = Size;

        Loaded += (_, _) => UpdateAnimation();
        Unloaded += (_, _) => StopAnimation();
    }

    /// <summary>画一个约 270° 的圆弧（比整圈更有「转动」的视觉参考）。</summary>
    private static Geometry BuildArcGeometry()
    {
        const double r = 10;
        const double cx = 12, cy = 12;
        var start = new Point(cx + r, cy);
        var end = new Point(cx, cy + r);

        var fig = new PathFigure { StartPoint = start, IsClosed = false };
        fig.Segments.Add(new ArcSegment
        {
            Point = end,
            Size = new Size(r, r),
            IsLargeArc = true,
            SweepDirection = SweepDirection.Clockwise
        });
        var geo = new PathGeometry();
        geo.Figures.Add(fig);
        geo.Freeze();
        return geo;
    }

    private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclLoading l) return;
        var s = (double)e.NewValue;
        l.Width = s;
        l.Height = s;
    }

    private static void OnRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PclLoading l) return;
        if ((bool)e.NewValue) l.UpdateAnimation();
        else l.StopAnimation();
    }

    private void UpdateAnimation()
    {
        if (!IsRunning || !IsLoaded) return;
        var anim = new DoubleAnimation(0, 360, TimeSpan.FromMilliseconds(900))
        {
            RepeatBehavior = RepeatBehavior.Forever
        };
        _rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
    }

    private void StopAnimation() => _rotate.BeginAnimation(RotateTransform.AngleProperty, null);
}
