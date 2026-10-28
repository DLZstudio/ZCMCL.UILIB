// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.Logging;

public record LoggerConfiguration(
    string StoreFolder,
    long MaxFileSize = 32 * 1024 * 1024,
    string? FileNameFormat = null,
    bool AutoDeleteOldFile = true,
    int MaxKeepOldFile = 16,
    LogLevel MinLogLevel = LogLevel.Info
);
