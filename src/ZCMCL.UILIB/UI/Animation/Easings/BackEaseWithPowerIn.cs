// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class BackEaseWithPowerIn(EasePower power = EasePower.Middle) : Easing
{
    private readonly double _p = 3.0 - (double)power * 0.5;

    protected override double EaseCore(double progress)
    {
        var t = Math.Clamp(progress, 0.0, 1.0);
        return Math.Pow(t, _p) * Math.Cos(1.5 * Math.PI * (1.0 - t));
    }
}