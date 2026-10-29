// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 参考 PCL-Community/PCL-CE 的 MyListItem 单选行为（Apache-2.0），由 ZCMCL Contributors 独立实现

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ZCMCL.UILIB.UI.Controls.Pcl;

/// <summary>
/// <see cref="PclListItem"/> 的互斥容器：保证同一组内最多一项处于选中态，
/// 语义等同 RadioButton 的 GroupName。
///
/// 上游把互斥逻辑写在 MyListItem 内部（遍历 Parent.Children），
/// 这里抽到容器上，列表项本身保持「纯视觉 + 点击事件」的简单职责。
/// </summary>
public class PclListItemGroup : StackPanel
{
    /// <summary>当前选中项变化。</summary>
    public event EventHandler<PclListItem?>? SelectionChanged;

    /// <summary>当前选中项。</summary>
    public PclListItem? SelectedItem { get; private set; }

    public PclListItemGroup()
    {
        Orientation = Orientation.Vertical;
    }

    /// <summary>
    /// 子项变化时统一接线。
    ///
    /// XAML 里直接写 <c>&lt;PclListItemGroup&gt;&lt;PclListItem/&gt;&lt;/...&gt;</c> 时，
    /// 子项是经由 Panel.Children 加入的，不会走 <see cref="AddItem"/>，
    /// 早期实现因此对 XAML 声明的项完全不生效（点击无选中）。
    /// 这里重写视觉子项变化，保证两种添加方式行为一致。
    /// </summary>
    protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
    {
        base.OnVisualChildrenChanged(visualAdded, visualRemoved);

        if (visualAdded is PclListItem added)
            Wire(added);

        if (visualRemoved is PclListItem removed)
            removed.Click -= OnItemClick;

        // 移除了当前选中项时清空选中
        if (visualRemoved is PclListItem gone && ReferenceEquals(SelectedItem, gone))
        {
            SelectedItem = null;
            SelectionChanged?.Invoke(this, null);
        }
    }

    private void Wire(PclListItem item)
    {
        // 避免重复接线（同名委托可通过 -= 再 += 归一，这里直接先摘后挂）
        item.Click -= OnItemClick;
        item.Click += OnItemClick;
    }

    private void OnItemClick(object? sender, EventArgs e)
    {
        if (sender is PclListItem item) Select(item);
    }

    /// <summary>添加并接入互斥逻辑（代码方式的便捷入口，与 XAML 声明等价）。</summary>
    public PclListItem AddItem(PclListItem item)
    {
        Children.Add(item);   // 接线由 OnVisualChildrenChanged 统一处理
        return item;
    }

    /// <summary>选中指定项；传 null 表示清空。</summary>
    public void Select(PclListItem? item)
    {
        // 已选中则忽略（对齐 RadioButton 行为：重复点击不重入）
        if (ReferenceEquals(SelectedItem, item)) return;

        foreach (var child in Children.OfType<PclListItem>())
        {
            if (!ReferenceEquals(child, item)) child.IsSelected = false;
        }

        if (item != null) item.IsSelected = true;
        SelectedItem = item;
        SelectionChanged?.Invoke(this, item);
    }

    /// <summary>按索引选中（便于宿主用 Tag/下标映射）。</summary>
    public void SelectIndex(int index)
    {
        if (index < 0 || index >= Children.Count) return;
        if (Children[index] is PclListItem it) Select(it);
    }
}
