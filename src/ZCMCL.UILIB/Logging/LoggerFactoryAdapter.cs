// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ZCMCL.UILIB.Logging;

/// <summary>
/// ILoggerFactory 实现，用于创建 LoggerAdapter 实例
/// </summary>
public class LoggerFactoryAdapter(Logger logger) : ILoggerFactory
{
    private readonly Logger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly List<IDisposable> _disposables = [];

    public void AddProvider(ILoggerProvider provider)
    {
        _disposables.Add(provider);
        // 不需要实现，因为我们只有一个固定的 Logger
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new LoggerAdapter(_logger, categoryName);
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
    }
}