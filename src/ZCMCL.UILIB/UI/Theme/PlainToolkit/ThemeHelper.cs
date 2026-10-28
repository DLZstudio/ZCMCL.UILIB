// SPDX-License-Identifier: MIT
// Copyright (c) 2026 PCL Community
// 源自 PCL-Community/PlainToolkit.UI（MIT）

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

    /// <summary>
    /// Initialize theme support. Must be called once at app startup, on the UI thread.
    /// If Generic.xaml is nested inside another ResourceDictionary, use the overload
    /// that accepts explicit dictionary references instead.
    /// </summary>
    public static void Initialize(ResourceDictionary appResources)
    {
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
            System.Diagnostics.Debug.WriteLine(
                "[PlainToolkit.UI] ThemeHelper.Initialize: Generic.xaml not found in top-level " +
                "MergedDictionaries. If Generic.xaml is nested inside another ResourceDictionary, " +
                "use Initialize(ResourceDictionary generic, ResourceDictionary light, ResourceDictionary dark) instead.");
            return;
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
    /// Set light (true) or dark (false) mode. Must be called on the UI thread.
    /// </summary>
    public static void SetTheme(bool isLight)
    {
        if (_genericDict == null || _colorsLight == null || _colorsDark == null) return;

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
