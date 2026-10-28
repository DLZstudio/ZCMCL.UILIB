// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.Animatable;

public sealed class EmptyAnimatable : IAnimatable
{
    public static EmptyAnimatable Instance { get; } = new();

    private EmptyAnimatable() { }
    
    public object? GetValue()
    {
        return null;
    }

    public void SetValue(object value)
    {
        // 空
    }
    
    public void SetValue<T>(T value)
    {
        // 空
    }
}