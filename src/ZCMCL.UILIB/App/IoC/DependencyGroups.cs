// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Collections.Generic;
using System.Linq;

namespace ZCMCL.UILIB.App.IoC;

public static partial class DependencyGroups
{
    private static readonly AttributeTargets[] _TargetsToTry = [
        AttributeTargets.Method,
        AttributeTargets.Property,
        AttributeTargets.Class
    ];
    
    public static bool InvokeInjection(Delegate injection, string identifier, AttributeTargets targets)
    {
        if (!_GroupMap.TryGetValue(identifier, out var groups)) return false;
        var targetGroups = new List<DependencyGroup>();
        foreach (var targetToTry in _TargetsToTry.Where(t => targets.HasFlag(t)))
        {
            if (groups.GetValueOrDefault(targetToTry) is not { } group) return false;
            targetGroups.Add(group);
        }
        foreach (var group in targetGroups) group.InvokeInjection(injection);
        return true;
    }
}
