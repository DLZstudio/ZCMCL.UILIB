// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class BackEaseInOut : Easing
{
    public static BackEaseInOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        if (progress < 0.5)
        {
            var f = 2 * progress;
            return 0.5 * f * (f * f - Math.Sin(f * Math.PI));
        }
        else
        {
            var f = 1 - (2 * progress - 1);
            return 0.5 * (1 - f * (f * f - Math.Sin(f * Math.PI))) + 0.5;
        }
    }
}