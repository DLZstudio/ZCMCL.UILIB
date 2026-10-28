// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Net.Http;

namespace ZCMCL.UILIB.IO.Net.Http;

public static class HttpBasicExtension
{
    extension(HttpRequestMessage requestMessage)
    {
        public HttpRequestMessage WithHttpVersionOption(Version httpVersion)
        {
            requestMessage.Version = httpVersion;
            return requestMessage;
        }
    }
}
