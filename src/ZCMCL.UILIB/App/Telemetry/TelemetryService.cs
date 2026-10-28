// SPDX-License-Identifier: MIT
// Copyright 2026 ZCMCL Contributors
// 本项目原创：遥测预留接口（默认关闭，需用户授权）

using System;

namespace ZCMCL.UILIB.App.Telemetry;

/// <summary>
/// 遥测上报的<b>预留接口</b>。当前默认关闭，且不做任何实际网络请求。
///
/// 设计约束（后续启用时必须遵守）：
/// 1. <b>默认关闭</b>：<see cref="Consent"/> 必须为用户明示同意后才可为 <c>true</c>；
/// 2. <b>必须可撤回</b>：用户关闭开关后立即停止上报，且不保留待发送队列；
/// 3. <b>不得静默采集</b>：未获同意时不得采集任何设备标识、路径、账户信息；
/// 4. <b>域名占位</b>：<see cref="Endpoint"/> 指向演示域名，启用前替换为真实端点并经评审。
///
/// 本类型的用途是让上层（日志/崩溃处理）现在就能写出「同意后上报」的调用点，
/// 而不必等到真正启用遥测时再改动业务流程。
/// </summary>
public static class TelemetryService
{
    /// <summary>
    /// 演示用端点占位。启用遥测前必须替换，且端点需支持不入库原始 IP。
    /// </summary>
    public const string Endpoint = "https://telemetry.demo.invalid/v1/events";

    /// <summary>
    /// 用户是否已明示同意上报。默认 <c>false</c>。
    /// 由设置页的「匿名使用数据」开关写入，切换即时生效。
    /// </summary>
    public static bool Consent { get; set; }

    /// <summary>
    /// 记录一条异常。未获同意时<b>直接返回</b>，不采集、不排队、不发网络请求。
    /// </summary>
    /// <param name="ex">异常对象。</param>
    /// <param name="context">上下文摘要（调用方保证不含隐私信息）。</param>
    /// <param name="level">日志级别。</param>
    public static void ReportException(Exception ex, string? context = null, object? level = null)
    {
        if (!Consent) return;

        // TODO(遥测启用时实现)：
        //   1. 组装载荷：异常类型、堆栈指纹（去参数）、UI 版本、内核版本、OS 版本；
        //   2. 剔除：绝对路径、用户名、账户标识、服务器地址、聊天内容；
        //   3. 通过独立 HttpClient 异步发送，失败静默（不得影响主流程、不得重试风暴）；
        //   4. 上报前再次检查 Consent（用户可能在请求飞行途中关闭开关）。
        _ = ex;
        _ = context;
        _ = level;
    }

    /// <summary>
    /// 记录一个匿名计数事件（如「完成一次安装」）。未获同意时直接返回。
    /// </summary>
    /// <param name="name">事件名，须为固定枚举式字符串，不得拼接用户数据。</param>
    public static void TrackEvent(string name)
    {
        if (!Consent) return;

        // TODO(遥测启用时实现)：同上，仅上报事件名与发生次数。
        _ = name;
    }
}
