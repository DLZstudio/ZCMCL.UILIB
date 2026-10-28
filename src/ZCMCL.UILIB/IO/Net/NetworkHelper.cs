// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ZCMCL.UILIB.IO.Net;

public static class NetworkHelper
{
    public static int NewTcpPort()
    {
        using var so = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        so.Bind(new IPEndPoint(IPAddress.Loopback, 0));
        return so.LocalEndPoint == null ? 0 : ((IPEndPoint)so.LocalEndPoint).Port;
    }
    
    public static bool IsNetworkAvailable()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }
}
