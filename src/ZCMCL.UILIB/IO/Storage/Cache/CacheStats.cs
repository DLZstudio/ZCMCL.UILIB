// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.IO.Storage.Cache;

public record CacheStats
{
    public long TotalEntries { get; init; }
    public long TotalSizeBytes { get; init; }
    public long ExpiredEntries { get; init; }
    public long InlineEntries { get; init; }
    public long FileEntries { get; init; }
    public long CacheHits { get; init; }
    public long CacheMisses { get; init; }
    public double HitRate => (CacheHits + CacheMisses) > 0 ? (double)CacheHits / (CacheHits + CacheMisses) : 0;
    public DateTime? LastCleanup { get; init; }
}