// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class BackEaseOut : Easing
{
    public static BackEaseOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        var p = 1 - progress;
        return 1 - p * (p * p - Math.Sin(p * Math.PI));
    }
}