// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class ExponentialEaseOut : Easing
{
    public static ExponentialEaseOut Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        return Math.Abs(progress - 1.0) < 1e-4 ? progress : 1 - Math.Pow(2, -10 * progress);
    }
}