// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Windows;
using ZCMCL.UILIB.UI.Animation.Animatable;

namespace ZCMCL.UILIB.UI.Animation.Core;

public interface IAnimationFrame
{
    Action GetAction();
}