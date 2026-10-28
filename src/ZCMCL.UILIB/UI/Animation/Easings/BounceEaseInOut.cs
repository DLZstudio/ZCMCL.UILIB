// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using ZCMCL.UILIB.Utils;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class BounceEaseInOut : Easing
{
    public static BounceEaseInOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        if (progress < 0.5)
        {
            return 0.5 * (1 - EaseUtils.Bounce(1 - progress * 2));
        }

        return 0.5 * EaseUtils.Bounce(progress * 2 - 1) + 0.5;
    }
}