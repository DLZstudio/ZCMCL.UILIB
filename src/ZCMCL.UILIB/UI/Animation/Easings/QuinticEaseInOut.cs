// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class QuinticEaseInOut : Easing
{
    public static QuinticEaseInOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        if (progress < 0.5)
        {
            var p2 = progress * progress;
            return 16 * p2 * p2 * progress;
        }

        var f = 2 * progress - 2;
        var f2 = f * f;
        return 0.5 * f2 * f2 * f + 1;
    }
}