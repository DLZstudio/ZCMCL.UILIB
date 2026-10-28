// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Windows.Media;

namespace ZCMCL.UILIB.UI.Controls.SvgIcon;

internal readonly record struct SvgIconPaintOptions(
    Brush IconBrush,
    double StrokeThickness,
    bool UseOriginalColor);