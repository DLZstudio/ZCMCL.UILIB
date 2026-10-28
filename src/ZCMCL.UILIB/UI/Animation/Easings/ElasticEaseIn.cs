// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using ZCMCL.UILIB.Utils;

namespace ZCMCL.UILIB.UI.Animation.Easings;

public class ElasticEaseIn : Easing
{
    public static ElasticEaseIn Shared { get; } = new();
    
    protected override double EaseCore(double progress)
    {
        return Math.Sin(EaseUtils.ElasticPiTimes6Point5 * progress) *
               Math.Exp(EaseUtils.ElasticLn2Times10 * (progress - 1d));
    }
}