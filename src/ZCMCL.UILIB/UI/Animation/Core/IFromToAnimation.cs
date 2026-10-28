// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.Core;

public interface IFromToAnimation : IAnimation
{
    object? CurrentValue { get; internal set; }
    int TotalFrames { get; }
}