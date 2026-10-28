// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.App.IoC;

public class PropertyAccessor<TProperty>(Func<TProperty>? getter = null, Action<TProperty>? setter = null)
{
    public TProperty Value
    {
        get => (getter ?? throw new NotSupportedException("Write-only property"))();
        set => (setter ?? throw new NotSupportedException("Read-only property"))(value);
    }

    public bool CanGet => getter is not null;
    public bool CanSet => setter is not null;
    public bool IsReadOnly => setter is null;
    public bool IsWriteOnly => getter is null;
}
