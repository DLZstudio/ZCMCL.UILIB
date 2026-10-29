// SPDX-License-Identifier: MIT
// Copyright 2026 ZCMCL Contributors
// 本项目原创：为 PlainTextBox 提供水印与错误态支持

using System.Windows;
using System.Windows.Controls;

namespace ZCMCL.UILIB.UI.Controls;

/// <summary>
/// 为 <c>PlainTextBox</c> / <c>PlainComboBox</c> 这类基于 WPF 内置控件的样式补充
/// PCL <c>MyTextBox</c> 具备的附加能力。
///
/// 用附加属性而非新建控件类型，是为了让宿主继续使用原生 <c>TextBox</c>
/// （不破坏现有样式继承与绑定）。
///
/// 用法：
/// <code>
/// &lt;TextBox ui:TextBoxHelper.HintText="输入玩家名" /&gt;
/// &lt;TextBox ui:TextBoxHelper.HasError="True" /&gt;
/// </code>
/// 模板内会读取这些附加属性渲染水印与错误边框。
/// </summary>
public static class TextBoxHelper
{
    /// <summary>水印文字。仅当 Text 为空且未聚焦时显示。</summary>
    public static readonly DependencyProperty HintTextProperty =
        DependencyProperty.RegisterAttached(
            "HintText",
            typeof(string),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(string.Empty));

    /// <summary>是否处于错误态（错误边框）。</summary>
    public static readonly DependencyProperty HasErrorProperty =
        DependencyProperty.RegisterAttached(
            "HasError",
            typeof(bool),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(false));

    /// <summary>错误说明，由宿主自行展示（模板不强制渲染，避免挤压布局）。</summary>
    public static readonly DependencyProperty ErrorTextProperty =
        DependencyProperty.RegisterAttached(
            "ErrorText",
            typeof(string),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(string.Empty));

    public static string GetHintText(DependencyObject element)
        => (string)element.GetValue(HintTextProperty);

    public static void SetHintText(DependencyObject element, string value)
        => element.SetValue(HintTextProperty, value);

    public static bool GetHasError(DependencyObject element)
        => (bool)element.GetValue(HasErrorProperty);

    public static void SetHasError(DependencyObject element, bool value)
        => element.SetValue(HasErrorProperty, value);

    public static string GetErrorText(DependencyObject element)
        => (string)element.GetValue(ErrorTextProperty);

    public static void SetErrorText(DependencyObject element, string value)
        => element.SetValue(ErrorTextProperty, value);
}
