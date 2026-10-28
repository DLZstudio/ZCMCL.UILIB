// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.Utils;

/// <summary>
/// 用来模拟带一个参数的属性
/// </summary>
/// <typeparam name="TParam">参数值类型</typeparam>
/// <typeparam name="TValue">属性值类型</typeparam>
public class ParameterizedProperty<TParam, TValue>
{
    public Func<TParam, TValue> GetValue { private get; init; } = null!;
    public Action<TParam, TValue> SetValue { private get; init; } = null!;

    public TValue this[TParam param]
    {
        get => GetValue.Invoke(param);
        set => SetValue.Invoke(param, value);
    }
}