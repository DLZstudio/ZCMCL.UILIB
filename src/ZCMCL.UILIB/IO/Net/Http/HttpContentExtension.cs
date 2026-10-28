// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ZCMCL.UILIB.Utils;

namespace ZCMCL.UILIB.IO.Net.Http;

public static class HttpContentExtension
{
    extension (HttpRequestMessage requestMessage)
    {
        public HttpRequestMessage WithContent(HttpContent content, string? contentType = null)
        {
            requestMessage.Content = content;
            if (contentType is not null) requestMessage.WithHeader("Content-Type", contentType);
            return requestMessage;
        }

        public HttpRequestMessage WithContent(string content, string? contentType = null)
        {
            requestMessage.Content = contentType is null
                ? new StringContent(content, Encoding.UTF8)
                : new StringContent(content, Encoding.UTF8, contentType);
            return requestMessage;
        }

        public HttpRequestMessage WithBinaryContent(byte[] content)
        {
            requestMessage.Content = new ByteArrayContent(content);
            return requestMessage;
        }

        public HttpRequestMessage WithJsonContent<T>(T content, string? contentType = null, JsonSerializerOptions? options = null)
        {
            requestMessage.Content = new StringContent(
                JsonSerializer.Serialize(content, options ?? JsonCompat.SerializerOptions),
                Encoding.UTF8,
                contentType ?? "application/json");
            return requestMessage;
        }

        public HttpRequestMessage WithFormContent(string form)
        {
            return requestMessage.WithContent(
                new ByteArrayContent(Encoding.UTF8.GetBytes(form)),
                "application/x-www-form-urlencoded");
        }

        public HttpRequestMessage WithFormContent(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            requestMessage.Content = new FormUrlEncodedContent(pairs);
            return requestMessage;
        }
    }
}
