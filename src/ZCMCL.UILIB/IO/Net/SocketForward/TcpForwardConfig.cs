// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Net;

namespace ZCMCL.UILIB.IO.Net.SocketForward;

public sealed class TcpForwardConfig
{
    public string? LocalHost { get; set; }
    public ushort LocalPort { get; set; }
    public string? RemoteHost { get; set; }
    public ushort RemotePort { get; set; }
    public ushort MaxConnection { get; set; } = 10;
    
    public const uint MaxBufferSize = 32*1024; // 32 KB
    public const uint MinBufferSize = 1024; // 1 KB
    public uint BufferSize { get; set; } = 8192;
}