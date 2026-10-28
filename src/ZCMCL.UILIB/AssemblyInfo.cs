// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）
// Modified by ZCMCL Contributors (2026): 移除 Validate 命名空间的 XmlnsDefinition（该目录已裁剪）

using System.Runtime.CompilerServices;
using System.Windows.Markup;

[assembly: XmlnsDefinition("https://ce.pclc.cc/core/ui/animation", "ZCMCL.UILIB.UI.Animation")]
[assembly: XmlnsDefinition("https://ce.pclc.cc/core/ui/animation", "ZCMCL.UILIB.UI.Animation.Core")]
[assembly: XmlnsDefinition("https://ce.pclc.cc/core/ui/animation", "ZCMCL.UILIB.UI.Animation.Easings")]
[assembly: XmlnsPrefix("https://ce.pclc.cc/core/ui/animation", "ani")]

[assembly:XmlnsPrefix("https://ce.pclc.cc/core/utils/validate", "val")]

[assembly: DisableRuntimeMarshalling]
[assembly: InternalsVisibleTo("ZCMCL.UILIB.Test")]
