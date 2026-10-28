// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.SourceGenerators;

public static class SharedConstants
{
    public const string AppNamespace = "ZCMCL.UILIB.App";
    public const string IocNamespace = $"{AppNamespace}.IoC";
    public const string DependencyCollectorAttribute = $"{IocNamespace}.DependencyCollectorAttribute";
    public const string DependencyInjectionPointAttribute = $"{IocNamespace}.DependencyInjectionPointAttribute";
    public const string LifecycleScopeAttribute = $"{IocNamespace}.LifecycleScopeAttribute";
    public const string LifecycleStartAttribute = $"{IocNamespace}.LifecycleStartAttribute";
    public const string LifecycleStopAttribute = $"{IocNamespace}.LifecycleStopAttribute";
    public const string LifecycleCommandHandlerAttribute = $"{IocNamespace}.LifecycleCommandHandlerAttribute";
    public const string LifecycleDependencyInjectionAttribute = $"{IocNamespace}.LifecycleDependencyInjectionAttribute";
}
