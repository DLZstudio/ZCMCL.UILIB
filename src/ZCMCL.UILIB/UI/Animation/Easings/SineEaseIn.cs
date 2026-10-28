// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class SineEaseIn : Easing
{
    public static SineEaseIn Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        return 1 - Math.Cos(progress * Math.PI / 2);
    }
}