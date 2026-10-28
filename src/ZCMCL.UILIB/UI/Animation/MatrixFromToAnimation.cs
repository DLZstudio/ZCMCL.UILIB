// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Windows.Media;
using ZCMCL.UILIB.UI.Animation.Animatable;
using ZCMCL.UILIB.UI.Animation.Core;
using ZCMCL.UILIB.UI.Animation.ValueProcessor;

namespace ZCMCL.UILIB.UI.Animation;

public class MatrixFromToAnimation : FromToAnimationBase<Matrix>
{
    public override IAnimationFrame? ComputeNextFrame(IAnimatable target)
    {
        // 应用缓动函数
        var easedProgress = Easing.Ease(CurrentFrame, TotalFrames);

        // 计算当前值
        CurrentValue = ValueType == AnimationValueType.Relative
            ? ValueProcessorManager.Add(From, ValueProcessorManager.Scale(To, easedProgress))
            : ValueProcessorManager.Add(From,
                ValueProcessorManager.Scale(ValueProcessorManager.Subtract(To, From), easedProgress));

        return base.ComputeNextFrame(target);
    }
}