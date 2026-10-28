// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using ZCMCL.UILIB.Utils;

namespace ZCMCL.UILIB.App.Configuration;

public class ArgConfig<TValue> : ParameterizedProperty<object, TValue>
{
    public ArgConfig(Func<object?, TValue> getter, Action<object?, TValue> setter)
    {
        GetValue = getter;
        SetValue = setter;
    }
}
