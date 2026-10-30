// SPDX-License-Identifier: MIT
// Copyright 2026 ZCMCL Contributors
// 本项目原创：弱引用主题订阅辅助

using System;
using System.Collections.Generic;
using System.Linq;

namespace ZCMCL.UILIB.UI.Controls;

/// <summary>
/// 让控件订阅 <see cref="HoverVisual.ThemeChanged"/> 而不阻止其被回收。
///
/// 为什么需要：<see cref="HoverVisual.ThemeChanged"/> 是静态事件，
/// 直接用 <c>+=</c> 订阅会让静态事件持有控件引用 —— 页面反复创建销毁时
/// 控件永远无法回收，长时间运行会累积成内存泄漏。
/// 这里用弱引用登记，收到通知时自动清理已回收的项。
/// </summary>
internal static class WeakEventSubscriber
{
    /// <summary>需要响应主题变化的控件必须实现本接口。</summary>
    internal interface IThemeAware
    {
        void OnThemeChanged();
    }

    private static readonly List<WeakReference<IThemeAware>> Watchers = new();
    private static readonly object Gate = new();
    private static bool _hooked;

    internal static void Attach(IThemeAware target)
    {
        lock (Gate)
        {
            if (!_hooked)
            {
                HoverVisual.ThemeChanged += OnThemeChanged;
                _hooked = true;
            }

            Watchers.Add(new WeakReference<IThemeAware>(target));
        }
    }

    private static void OnThemeChanged()
    {
        List<IThemeAware> alive;

        lock (Gate)
        {
            // 清理已回收的引用
            Watchers.RemoveAll(w => !w.TryGetTarget(out _));
            alive = Watchers
                .Select(w => w.TryGetTarget(out var t) ? t : null)
                .Where(t => t is not null)
                .Select(t => t!)
                .ToList();
        }

        foreach (var t in alive)
        {
            try { t.OnThemeChanged(); }
            catch { /* 单个控件失败不影响其它控件 */ }
        }
    }
}
