// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class ExponentialEaseInOut : Easing
{
    public static ExponentialEaseInOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        if (progress < 0.5)
        {
            return 0.5 * Math.Pow(2, 20 * progress - 10);
        }

        return -0.5 * Math.Pow(2, -20 * progress + 10) + 1;
    }
}