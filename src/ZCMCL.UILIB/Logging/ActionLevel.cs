// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.Logging;

/// <summary>
/// 事件/意外行为等级。
/// </summary>
public enum ActionLevel
{
    TraceLog = 00,
    NormalLog = 10,
    Hint = 20,
    HintErr = 21,
    MsgBox = 30,
    MsgBoxErr = 31,
    MsgBoxFatal = 32,
}
