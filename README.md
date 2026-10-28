# ZCMCL.UILIB

从 [PCL2-CE](https://github.com/PCL-Community/PCL-CE) 的 `PCL.Core` 抽取、精简并重命名的
WPF UI 基础库，供 ZCMCL 启动器使用。

提供动画引擎、主题服务、SVG 图标、以及一套从 PCL2-CE 提取的控件样式（PlainToolkit.UI）。

## 为什么会有这个项目

`PCL.Core` 里同时包含 UI 基础设施和启动器业务逻辑（Minecraft 版本管理、下载、认证、联机）。
ZCMCL 的架构把业务全部放在独立的 Go 内核里，UI 只通过 IPC 取数据，
因此这里只保留 UI 需要的那部分：

```
保留：动画引擎、主题/配色、SvgIcon、控件样式、配置系统、日志、HTTP/DNS
移除：Minecraft 业务、Link 联机、下载器、遥测上报、账户/实例管理
```

同时把命名空间由 `PCL.Core.*` 改为 `ZCMCL.UILIB.*` —— 避免与启动器内核的
`core` 目录、`core.*` RPC 命名在阅读时混淆。

## 许可

**混合许可**。各文件头部的 `SPDX-License-Identifier` 为准：

| 部分 | 许可 |
|---|---|
| 主体（衍生自 PCL.Core） | Apache-2.0 |
| `UI/Theme/PlainToolkit/` | MIT |
| 本项目原创 | MIT |

详见 [LICENSE](LICENSE)、[NOTICE](NOTICE)、[THIRD-PARTY.md](THIRD-PARTY.md)。

## 目录结构

```
src/
├── ZCMCL.UILIB/
│   ├── App/                配置系统、IoC、本地化、遥测预留接口
│   ├── IO/                 网络（HTTP/DNS）、本地缓存
│   ├── Logging/            日志服务
│   ├── UI/
│   │   ├── Animation/      动画引擎（时钟、缓动、帧动画）
│   │   ├── Controls/       BlurBorder、SvgIcon、WaterfallPanel、Tooltip
│   │   ├── Theme/          ThemeService + PlainToolkit 控件样式
│   │   ├── Effects/        视觉效果
│   │   └── Converters/     值转换器
│   └── Utils/              OS、扩展、哈希、加密、编解码
└── ZCMCL.UILIB.SourceGenerators/   配置与依赖注入的源生成器
```

## 构建

需要 **.NET 10 SDK**（目标框架 `net10.0-windows`）。

```bash
dotnet build src/ZCMCL.UILIB/ZCMCL.UILIB.csproj -c Release
```

注意：源生成器依赖 Roslyn `Microsoft.CodeAnalysis.CSharp` **5.0.0**。
上游用 5.6.0，但 .NET 10.0.1xx SDK 内置编译器为 5.0，版本过新会导致
生成器被跳过、进而 `partial` 属性大量报错（CS9248）。

## 在 WPF 项目中使用

```xml
<ProjectReference Include="path/to/ZCMCL.UILIB.csproj" />
```

合并控件样式（含调色板）：

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary
                Source="pack://application:,,,/ZCMCL.UILIB;component/UI/Theme/PlainToolkit/Themes/Generic.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

也可以只合并调色板、自行控制控件样式：

```xml
<ResourceDictionary
    Source="pack://application:,,,/ZCMCL.UILIB;component/UI/Theme/PlainToolkit/Themes/PaletteOnly.xaml" />
```

### 已知注意点

- `PlainButton` / `PlainRadioButton` 等模板只支持**纯文本 Content**（内部为
  `TextBlock.Text="{TemplateBinding Content}"`）。需要「图标 + 文字」等复合内容时，
  请自定义 `ControlTemplate` 使用 `ContentPresenter`。
- `PlainCard` 是 `ContentControl` 样式，不能套在 `Border` 上。
- 调色板默认顺序已调整为浅色优先（见 `Generic.xaml` 内的注释）。

## 与上游的差异

见 [LICENSE](LICENSE) 第二节的逐项说明，以及各文件中带 `ZCMCL` 字样的注释。
主要改动：命名空间更名、裁掉业务层、移除遥测、修复生成器的硬编码偏移、
调整 PlainToolkit 的若干模板缺陷。
