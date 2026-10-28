// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;

namespace ZCMCL.UILIB.Utils.Encryption;

public interface IEncryptionProvider
{
    public byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key);
    public byte[] Decrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key);

    public static bool IsSupported { get; }
}