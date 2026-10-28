// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class QuarticEaseOut : Easing
{
    public static QuarticEaseOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        var f = progress - 1;
        var f2 = f * f;
        return -f2 * f2 + 1;
    }
}