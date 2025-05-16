// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Hosting.Server;

/// <summary>
/// Represents an application.
/// 代表一个应用程序。
/// </summary>
/// <typeparam name="TContext">The context associated with the application.</typeparam>
public interface IHttpApplication<TContext> where TContext : notnull
{
    /// <summary>
    /// Create a TContext given a collection of HTTP features.
    /// 根据HTTP功能的集合创建TContext。
    /// </summary>
    /// <param name="contextFeatures">A collection of HTTP features to be used for creating the TContext.</param>
    /// <returns>The created TContext.</returns>
    TContext CreateContext(IFeatureCollection contextFeatures);

    /// <summary>
    /// Asynchronously processes an TContext.
    /// 异步处理TContext。
    /// </summary>
    /// <param name="context">The TContext that the operation will process.</param>
    Task ProcessRequestAsync(TContext context);

    /// <summary>
    /// Dispose a given TContext.
    /// 处理给定的TContext。
    /// </summary>
    /// <param name="context">The TContext to be disposed.</param>
    /// <param name="exception">The Exception thrown when processing did not complete successfully, otherwise null.</param>
    void DisposeContext(TContext context, Exception? exception);
}
