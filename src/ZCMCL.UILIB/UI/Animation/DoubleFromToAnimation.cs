// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using ZCMCL.UILIB.UI.Animation.Animatable;
using ZCMCL.UILIB.UI.Animation.Core;

namespace ZCMCL.UILIB.UI.Animation;

public sealed class DoubleFromToAnimation : FromToAnimationBase<double>
{
    public override IAnimationFrame? ComputeNextFrame(IAnimatable target)
    {
        // 应用缓动函数
        var easedProgress = Easing.Ease(CurrentFrame, TotalFrames);

        // 计算当前值
        CurrentValue = ValueType == AnimationValueType.Relative
            ? From + To * easedProgress
            : From + (To - From) * easedProgress;

        return base.ComputeNextFrame(target);
    }
}