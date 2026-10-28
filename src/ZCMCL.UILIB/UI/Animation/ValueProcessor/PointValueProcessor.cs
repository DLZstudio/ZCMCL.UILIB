// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Windows;

namespace ZCMCL.UILIB.UI.Animation.ValueProcessor;

public class PointValueProcessor : IValueProcessor<Point>
{
    public Point Filter(Point value) => value;

    public Point Add(Point value1, Point value2) => new(value1.X + value2.X, value1.Y + value2.Y);

    public Point Subtract(Point value1, Point value2) => new(value1.X - value2.X, value1.Y - value2.Y);

    public Point Scale(Point value, double factor) =>  new(value.X * factor, value.Y * factor);
    
    public Point DefaultValue() => new();
    
    public bool Equal(Point value1, Point value2) => value1 == value2;
}