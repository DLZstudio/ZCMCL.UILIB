// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.IO.Net.Http.Cache;

public record HttpCacheMetadata
{
    public string? ETag { get; init; }
    public string? LastModified { get; init; }
    public int StatusCode { get; init; }
}