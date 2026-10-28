// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.UI.Animation.Core;

public struct ActionAnimationFrame(Action action) : IAnimationFrame
{
    public Action Action { get; set; } = action;

    public Action GetAction() => Action;
}