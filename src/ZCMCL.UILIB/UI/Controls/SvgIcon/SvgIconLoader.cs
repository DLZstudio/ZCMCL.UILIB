// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ZCMCL.UILIB.Logging;

namespace ZCMCL.UILIB.UI.Controls.SvgIcon;

public static class SvgIconLoader
{
    /// <summary>
    /// 默认图标包。必须是仓库内实际存在的目录（当前为 lucide）。
    ///
    /// 历史上这里是 "default"，但 IconPacks/ 下并没有 default/ 目录，
    /// 导致 <c>Icon="play"</c>（不带包名前缀）静默渲染为空白 —— 页面能跑、布局正常、
    /// 图标全没，排查成本极高。改为实际存在的 "lucide" 后，
    /// <c>Icon="play"</c> 与 <c>Icon="lucide/play"</c> 等价。
    /// </summary>
    public const string DefaultIconPack = "lucide";

    private static readonly string _AssemblyName =
        typeof(SvgIconLoader).Assembly.GetName().Name ?? "ZCMCL.UILIB";

    private static readonly ConcurrentDictionary<string, Lazy<SvgIconModel?>> _Cache =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 图标资源缺失时的回调（包名、图标名）。可用于在开发期把静默失败暴露出来。
    /// </summary>
    public static event Action<string, string>? IconMissing;

    internal static SvgIconModel? Load(string? icon, string? defaultPack = null)
    {
        var key = SvgIconKey.TryParse(icon, defaultPack ?? DefaultIconPack);
        if (key is null)
        {
            if (!string.IsNullOrWhiteSpace(icon))
                _Log($"无效的 SVG 图标标识：{icon}", warning: false);
            return null;
        }

        var cacheKey = key.Value.ToString();
        return _Cache.GetOrAdd(cacheKey, _ => new Lazy<SvgIconModel?>(() => _LoadCore(key.Value))).Value;
    }

    public static void ClearCache()
    {
        _Cache.Clear();
    }

    private static SvgIconModel? _LoadCore(SvgIconKey key)
    {
        try
        {
            var uri = new Uri(
                $"pack://application:,,,/{_AssemblyName};component/UI/Assets/IconPacks/{key.Pack}/{key.Name}.svg",
                UriKind.Absolute);
            var info = Application.GetResourceStream(uri);
            if (info is null)
            {
                // 图标缺失是「静默错误」的典型来源：控件不报错、只是空白。
                // 这里提升为 Warning 级日志（Release 下也输出），并触发 IconMissing 事件。
                _Log($"缺少 SVG 图标资源：{key}（检查包名是否正确，或改用 lucide/{key.Name}）",
                    warning: true);
                IconMissing?.Invoke(key.Pack, key.Name);
                return null;
            }

            using var stream = info.Stream;
            using var reader = new StreamReader(stream, Encoding.UTF8, true);
            var svg = reader.ReadToEnd();
            return SvgIconParser.Parse(svg);
        }
        catch (Exception ex)
        {
            _Log($"加载 SVG 图标失败：{key}", warning: true, ex);
            IconMissing?.Invoke(key.Pack, key.Name);
            return null;
        }
    }

    private static void _Log(string message, bool warning, Exception? ex = null)
    {
        // warning=true 时在 Release 下也输出，保证「图标没显示」有据可查；
        // 其余诊断信息维持仅 DEBUG 输出，避免日志噪音。
#if DEBUG
        var emit = true;
#else
        var emit = warning;
#endif
        if (!emit) return;

        if (warning)
        {
            if (ex is null) LogWrapper.Warn("SvgIcon", message);
            else LogWrapper.Warn(ex, "SvgIcon", message);
        }
        else
        {
            if (ex is null) LogWrapper.Debug("SvgIcon", message);
            else LogWrapper.Debug(ex, "SvgIcon", message);
        }
    }

    private readonly record struct SvgIconKey(string Pack, string Name)
    {
        public static SvgIconKey? TryParse(string? icon, string defaultPack)
        {
            if (string.IsNullOrWhiteSpace(icon))
                return null;

            var normalized = icon.Trim().Replace('\\', '/');
            if (normalized.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                normalized = normalized[..^4];

            normalized = normalized.Trim('/');
            if (normalized.Length == 0 || normalized.Contains("..", StringComparison.Ordinal))
                return null;

            var parts = normalized.Split('/', 2,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var pack = parts.Length == 2 ? parts[0] : defaultPack;
            var name = parts.Length == 2 ? parts[1] : parts[0];

            if (!_IsSafeResourcePath(pack) || !_IsSafeResourcePath(name))
                return null;

            return new SvgIconKey(pack, name);
        }

        public override string ToString()
        {
            return $"{Pack}/{Name}";
        }

        private static bool _IsSafeResourcePath(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.StartsWith('/') || value.EndsWith('/'))
                return false;

            return value.Split('/', StringSplitOptions.RemoveEmptyEntries).All(_IsSafeResourceSegment);
        }

        private static bool _IsSafeResourceSegment(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value is "." or "..")
                return false;

            return value.All(ch => char.IsLetterOrDigit(ch) || ch is '-' or '_' or '.');
        }
    }
}