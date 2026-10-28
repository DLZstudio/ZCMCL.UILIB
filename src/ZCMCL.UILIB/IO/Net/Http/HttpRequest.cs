// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZCMCL.UILIB.IO.Net.Http;

public static class HttpRequest
{
    public static HttpRequestMessage Create(string url)
    {
        return new HttpRequestMessage(HttpMethod.Get, new Uri(url));
    }
    public static HttpRequestMessage CreateHead(string url)
    {
        return new HttpRequestMessage(HttpMethod.Head, new Uri(url));
    }
    public static HttpRequestMessage CreatePost(string url)
    {
        return new HttpRequestMessage(HttpMethod.Post, new Uri(url));
    }
    public static HttpRequestMessage CreatePut(string url)
    {
        return new HttpRequestMessage(HttpMethod.Put, new Uri(url));
    }
    public static HttpRequestMessage CreateDelete(string url)
    {
        return new HttpRequestMessage(HttpMethod.Delete, new Uri(url));
    }

    public static async Task<string> GetStringAsync(string url)
    {
        using var resp = await Create(url).SendAsync().ConfigureAwait(false);
        return await resp.AsStringAsync().ConfigureAwait(false);
    }

    public static async Task<T?> GetJsonAsync<T>(string url)
    {
        using var resp = await Create(url).SendAsync().ConfigureAwait(false);
        return await resp.AsJsonAsync<T>().ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T data, string? contentType = null)
    {
        return await CreatePost(url)
            .WithJsonContent(data, contentType)
            .SendAsync()
            .ConfigureAwait(false);
    }
}
