# 第三方依赖清单

本项目通过 NuGet 引用的组件及其许可证。所有依赖均为宽松许可（MIT / Apache-2.0），
不含 copyleft 许可，可以安全地用于闭源或开源分发。

## 运行时依赖

| 包 | 版本 | 许可 | 用途 |
|---|---|---|---|
| [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) | 8.4.2 | MIT | MVVM 基础设施 |
| [Microsoft.Xaml.Behaviors.Wpf](https://github.com/microsoft/XamlBehaviorsWpf) | 1.1.142 | MIT | XAML 行为 |
| [Wacton.Unicolour](https://github.com/waacton/Unicolour) | 8.0.0 | MIT | 色彩空间转换（小节的 OKLCH 调色） |
| [Microsoft.CSharp](https://github.com/dotnet/runtime) | 4.7.0 | MIT | `dynamic` 支持 |
| [System.Management](https://github.com/dotnet/runtime) | 10.0.9 | MIT | WMI 硬件信息 |
| [System.Security.Cryptography.ProtectedData](https://github.com/dotnet/runtime) | 10.0.9 | MIT | DPAPI 加密 |
| [PlainToolkit.CngProtectedData](https://www.nuget.org/packages/PlainToolkit.CngProtectedData) | 2.1.0 | MIT | CNG 保护的本地数据 |
| [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) | 1.4.2 | MIT | 压缩包读写 |
| [Microsoft.Data.Sqlite](https://github.com/dotnet/efcore) | 10.0.9 | MIT | 本地缓存数据库 |
| [SQLitePCLRaw.bundle_e_sqlite3](https://github.com/ericsink/SQLitePCL.raw) | 3.0.3 | Apache-2.0 | SQLite 原生绑定 |
| [System.Text.Json](https://github.com/dotnet/runtime) | 10.0.9 | MIT | JSON 序列化 |
| [YamlDotNet](https://github.com/aaubry/YamlDotNet) | 18.1.0 | MIT | YAML 配置文件读写 |
| [Ae.Dns.Client](https://github.com/Aetf/Ae.Dns) | 3.1.0 | MIT | DNS 查询（DoH） |
| [Microsoft.Extensions.Http](https://github.com/dotnet/runtime) | 10.0.9 | MIT | HttpClientFactory |
| [Polly](https://github.com/App-vNext/Polly) | 8.7.0 | BSD-3-Clause | 网络重试与熔断 |
| [Humanizer.Core.zh-CN](https://github.com/Humanizr/Humanizer) | 3.0.10 | MIT | 中文数字/时间表述 |
| [Microsoft.Extensions.Logging](https://github.com/dotnet/runtime) | 10.0.9 | MIT | 日志抽象 |
| [Microsoft.Extensions.Logging.Abstractions](https://github.com/dotnet/runtime) | 10.0.9 | MIT | 日志抽象 |

## 构建时依赖

| 包 | 版本 | 许可 | 用途 |
|---|---|---|---|
| [Microsoft.CodeAnalysis.CSharp](https://github.com/dotnet/roslyn) | 5.0.0 | MIT | 源生成器（`PrivateAssets=all`，不随产物分发） |
| [Microsoft.CodeAnalysis.Analyzers](https://github.com/dotnet/roslyn-analyzers) | 5.0.0 | MIT | 同上 |

## 说明

- `SQLitePCLRaw.bundle_e_sqlite3` 为 Apache-2.0，与项目主体的 Apache-2.0 部分兼容。
- `Polly` 为 BSD-3-Clause，属宽松许可，与本项目许可兼容。
- 源生成器依赖标记为 `PrivateAssets="all"`，不会传递给下游使用者。

如需完整许可文本，可通过 `dotnet list package --include-transitive` 结合
`~/.nuget/packages/<id>/<version>/LICENSE*` 获取。
