// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Numerics;
using ZCMCL.UILIB.UI.Animation.Animatable;
using ZCMCL.UILIB.UI.Animation.ValueProcessor;

namespace ZCMCL.UILIB.UI.Animation.Core;

// public readonly struct AnimationFrame<T>(IAnimatable target, T value, T startValue) : IAnimationFrame
// {
//     public IAnimatable Target { get; init; } = target;
//     public T Value { get; init; } = value;
//     public T StartValue { get; init; } = startValue;
//     public T GetAbsoluteValue() => ValueProcessorManager.Add(StartValue, Value);
//     object IAnimationFrame.StartValue => StartValue!;
//     object IAnimationFrame.Value => Value!;
//     object IAnimationFrame.GetAbsoluteValue() => GetAbsoluteValue()!;
// }