// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Collections.Frozen;

namespace ZCMCL.UILIB.UI.Animation.Core;

public record AnimationData<T> : IAnimationData
{
    public IAnimation Animation { get; init; } = null!;
    public FrozenDictionary<int, T> Values { get; init; } = null!;
}