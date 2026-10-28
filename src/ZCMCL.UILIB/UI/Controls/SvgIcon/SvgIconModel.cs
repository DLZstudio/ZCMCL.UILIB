// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Collections.Generic;

namespace ZCMCL.UILIB.UI.Controls.SvgIcon;

internal sealed class SvgIconModel
{
    public double MinX { get; init; }
    public double MinY { get; init; }
    public double Width { get; init; } = 24D;
    public double Height { get; init; } = 24D;
    public IReadOnlyList<SvgIconElement> Elements { get; init; } = [];
}