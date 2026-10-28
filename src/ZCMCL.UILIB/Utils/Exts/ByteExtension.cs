// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.Utils.Exts;

public static class ByteExtension
{
    extension(ReadOnlySpan<byte> bytes)
    {
        public string ToHexString() => Convert.ToHexString(bytes).ToLower();
        public string FromByteToB64() => Convert.ToBase64String(bytes);
        public string FromBytesToB64UrlSafe() => bytes.FromByteToB64().FromB64ToB64UrlSafe();
    }
}
