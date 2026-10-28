// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

using System;
using System.Net.Http;

namespace ZCMCL.UILIB.IO.Net.Http;

public class HttpResponseException:HttpRequestException,IDisposable
{
    public HttpResponseMessage? Response { get; init; }

    public HttpResponseException()
    {
        
    }

    public HttpResponseException(string message) : base(message)
    {
        
    }

    public HttpResponseException(string message, Exception? inner) : base(message, inner)
    {
        
    }

    public HttpResponseException(HttpResponseMessage? response) : this(
        $"{(int?)response?.StatusCode} {response?.ReasonPhrase ?? (response is null ? "undefined" : Enum.GetName(response.StatusCode))})")
    {
        Response = response;
    }

    public void Dispose()
    {
        Response?.Dispose();
        GC.SuppressFinalize(this);
    }

    ~HttpResponseException()
    {
        try
        {
            Response?.Dispose();
        }
        catch
        {
            // Suppress Exception
        }
    }
}