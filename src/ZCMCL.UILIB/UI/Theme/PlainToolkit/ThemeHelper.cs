// SPDX-License-Identifier: MIT
// Copyright (c) 2026 PCL Community
// 源自 PCL-Community/PlainToolkit.UI（MIT）

using System;
using System.Windows;

namespace ZCMCL.UILIB.UI.Theme.PlainToolkit;

/// <summary>
/// Reorders palette ResourceDictionaries at runtime.
/// Both Colors.xaml and ColorsDark.xaml are always loaded in Generic.xaml.
/// Light mode = ColorsDark.xaml before Colors.xaml (Colors.xaml wins).
/// Dark  mode = ColorsDark.xaml after  Colors.xaml (ColorsDark.xaml wins).
/// </summary>
public static class ThemeHelper
{
    private static ResourceDictionary? _genericDict;
    private static ResourceDictionary? _colorsLight;
    private static ResourceDictionary? _colorsDark;

    /// <summary>是否已成功初始化（可用作诊断）。</summary>
    public static bool IsInitialized =>
        _genericDict is not null && _colorsLight is not null && _colorsDark is not null;

    /// <summary>
    /// 初始化主题支持。应用启动时调用一次，必须在 UI 线程。</summary>
    /// <returns><c>true</c> 表示初始化成功。</returns>
    /// <exception cref="InvalidOperationException">
    /// 在顶层 <c>MergedDictionaries</c> 中找不到 <c>Generic.xaml</c>，或其中缺少
    /// <c>Colors.xaml</c> / <c>ColorsDark.xaml</c>。
    ///
    /// 失败时会抛异常而不是静默返回 —— 否则调用方只能看到「主题切换没反应」，
    /// 无法判断是没调用 Initialize 还是初始化失败（ZCMCL 补）。
    /// 若 Generic.xaml 被包在自己的资源字典里再合并，请改用三参数重载。
    /// </exception>
    public static bool Initialize(ResourceDictionary appResources)
    {
        _genericDict = null;
        _colorsLight = null;
        _colorsDark = null;

        // Find Generic.xaml inside Application.Resources.MergedDictionaries (one level deep)
        foreach (var dict in appResources.MergedDictionaries)
        {
            var src = dict.Source?.OriginalString ?? "";
            if (src.Contains("/Generic.xaml"))
            {
                _genericDict = dict;
                break;
            }
        }

        if (_genericDict is null)
        {
            throw new InvalidOperationException(
                "[ZCMCL.UILIB] ThemeHelper.Initialize 失败：在 Application.Resources.MergedDictionaries " +
                "顶层找不到 Generic.xaml。请直接合并 " +
                "pack://application:,,,/ZCMCL.UILIB;component/UI/Theme/PlainToolkit/Themes/Generic.xaml，" +
                "或改用 Initialize(generic, light, dark) 三参数重载。");
        }

        // Find Colors.xaml and ColorsDark.xaml inside Generic.xaml's MergedDictionaries
        foreach (var dict in _genericDict.MergedDictionaries)
        {
            var src = dict.Source?.OriginalString ?? "";
            if (src.Contains("/Colors.xaml") && !src.Contains("Dark"))
                _colorsLight = dict;
            if (src.Contains("/ColorsDark.xaml"))
                _colorsDark = dict;
        }

        if (_colorsLight is null || _colorsDark is null)
        {
            throw new InvalidOperationException(
                "[ZCMCL.UILIB] ThemeHelper.Initialize 失败：Generic.xaml 的 MergedDictionaries 中缺少 " +
                "Colors.xaml 或 ColorsDark.xaml，无法进行明暗切换。");
        }

        return true;
    }

    /// <summary>
    /// Initialize theme support with explicit palette references.
    /// Use this overload when Generic.xaml is nested inside another ResourceDictionary.
    /// </summary>
    /// <param name="generic">The Generic.xaml ResourceDictionary (must contain both Colors.xaml and ColorsDark.xaml in its MergedDictionaries).</param>
    /// <param name="colorsLight">The Colors.xaml ResourceDictionary from generic.MergedDictionaries.</param>
    /// <param name="colorsDark">The ColorsDark.xaml ResourceDictionary from generic.MergedDictionaries.</param>
    public static void Initialize(ResourceDictionary generic, ResourceDictionary colorsLight, ResourceDictionary colorsDark)
    {
        _genericDict = generic;
        _colorsLight = colorsLight;
        _colorsDark = colorsDark;
    }

    /// <summary>
    /// 切换浅色 / 深色。必须在 UI 线程调用。
    ///
    /// 未初始化时抛异常而不是静默返回 —— 静默会让调用方误以为「切换功能坏了」（ZCMCL 补）。
    /// </summary>
    /// <param name="isLight"><c>true</c> 浅色，<c>false</c> 深色。</param>
    /// <exception cref="InvalidOperationException">未先调用 <see cref="Initialize(ResourceDictionary)"/>。</exception>
    public static void SetTheme(bool isLight)
    {
        if (_genericDict == null || _colorsLight == null || _colorsDark == null)
        {
            throw new InvalidOperationException(
                "[ZCMCL.UILIB] ThemeHelper.SetTheme 前必须先调用 ThemeHelper.Initialize(...)。" +
                "若已在启动时调用，请确认合并的是 Generic.xaml 且位于 MergedDictionaries 顶层。");
        }

        var dicts = _genericDict.MergedDictionaries;

        if (isLight)
        {
            // ColorsDark.xaml before Colors.xaml → Colors.xaml wins
            if (dicts.IndexOf(_colorsDark) > dicts.IndexOf(_colorsLight))
            {
                dicts.Remove(_colorsDark);
                var lightIdx = dicts.IndexOf(_colorsLight);
                dicts.Insert(lightIdx, _colorsDark);
            }
        }
        else
        {
            // ColorsDark.xaml after Colors.xaml → ColorsDark.xaml wins
            if (dicts.IndexOf(_colorsDark) < dicts.IndexOf(_colorsLight))
            {
                dicts.Remove(_colorsDark);
                var lightIdx = dicts.IndexOf(_colorsLight);
                dicts.Insert(lightIdx + 1, _colorsDark);
            }
        }
    }
}
