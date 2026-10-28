// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Collections.Generic;

namespace ZCMCL.UILIB.App.Configuration.Storage;

public interface IEnumerableKeyProvider
{
    /// <summary>
    /// 获取文件包含的所有键。通常是一个耗时操作，慎用。
    /// </summary>
    public IEnumerable<string> Keys { get; }
}
