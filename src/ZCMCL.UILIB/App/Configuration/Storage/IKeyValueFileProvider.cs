// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.App.Configuration.Storage;

/// <summary>
/// 键值文件模型。
/// </summary>
public interface IKeyValueFileProvider
{
    /// <summary>
    /// 文件路径。
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// 获取一个值。
    /// </summary>
    public T Get<T>(string key);

    /// <summary>
    /// 设置一个值。
    /// </summary>
    public void Set<T>(string key, T value);

    /// <summary>
    /// 判断一个值是否存在。
    /// </summary>
    public bool Exists(string key);

    /// <summary>
    /// 移除一个值。
    /// </summary>
    public void Remove(string key);

    /// <summary>
    /// 写入文件。
    /// </summary>
    public void Sync();
}
