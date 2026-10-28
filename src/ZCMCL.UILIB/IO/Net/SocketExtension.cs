// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Net.Sockets;

namespace ZCMCL.UILIB.IO.Net;

public static class SocketExtensions
{
    public static void CloseGracefully(this Socket? socket)
    {
        if (socket is null) return;

        if (socket is { IsBound: false, Connected: false })
        {
            socket.Dispose();
            return;
        }
        
        if (socket.Connected)
        {
            try
            {
                    socket.Shutdown(SocketShutdown.Both);
            }
            catch { /* 忽略关闭时的任何错误 */ }
        }
        
        try
        {
            socket.Close();
        }
        catch { /* 忽略关闭时的任何错误 */ }
        
    }
}