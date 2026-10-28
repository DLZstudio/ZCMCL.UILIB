// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.Utils;

public sealed class AtomicVariable<T>
{
    private T? _value;
    
    public T? Value {
        get => _value;
        set => SetValue(value);
    }

    public bool ReadOnly { get; set; }
    
    public bool Nullable { get; set; }

    public void SetValue(in T? value)
    {
        if (ReadOnly) throw new NotSupportedException("Read-only variable");
        if (!Nullable && value is null) throw new NotSupportedException("Non-null variable");
        _value = value;
    }

    public AtomicVariable(T? value = default, bool readOnly = false, bool? nullable = null)
    {
        Nullable = nullable ?? value is null;
        SetValue(value);
        ReadOnly = readOnly;
    }
}
