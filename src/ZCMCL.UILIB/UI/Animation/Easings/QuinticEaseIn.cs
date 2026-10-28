// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class QuinticEaseIn : Easing
{
    public static QuinticEaseIn Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        var p2 = progress * progress;
        return p2 * p2 * progress;
    }
}