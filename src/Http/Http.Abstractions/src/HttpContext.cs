// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Shared;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// Encapsulates all HTTP-specific information about an individual HTTP request.
/// </summary>
[DebuggerDisplay("{DebuggerToString(),nq}")]
[DebuggerTypeProxy(typeof(HttpContextDebugView))]
public abstract class HttpContext
{
    /// <summary>
    /// Gets the collection of HTTP features provided by the server and middleware available on this request.
    /// 获取此请求上可用的服务器和中间件提供的HTTP功能的集合。
    /// </summary>
    public abstract IFeatureCollection Features { get; }

    /// <summary>
    /// Gets the <see cref="HttpRequest"/> object for this request.
    /// </summary>
    public abstract HttpRequest Request { get; }

    /// <summary>
    /// Gets the <see cref="HttpResponse"/> object for this request.
    /// </summary>
    public abstract HttpResponse Response { get; }

    /// <summary>
    /// Gets information about the underlying connection for this request.
    /// 获取有关此请求的基础连接的信息
    /// </summary>
    public abstract ConnectionInfo Connection { get; }

    /// <summary>
    /// Gets an object that manages the establishment of WebSocket connections for this request.
    /// 获取一个对象，该对象管理此请求的WebSocket连接的建立
    /// </summary>
    public abstract WebSocketManager WebSockets { get; }

    /// <summary>
    /// Gets or sets the user for this request.
    /// 获取或设置此请求的用户。
    /// </summary>
    public abstract ClaimsPrincipal User { get; set; }

    /// <summary>
    /// Gets or sets a key/value collection that can be used to share data within the scope of this request.
    /// 获取或设置可用于在此请求范围内共享数据的键/值集合。
    /// </summary>
    public abstract IDictionary<object, object?> Items { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the request's service container.
    /// 获取或设置提供对请求的服务容器的访问的<see cref="IServiceProvider"/>。
    /// </summary>
    public abstract IServiceProvider RequestServices { get; set; }

    /// <summary>
    /// Notifies when the connection underlying this request is aborted and thus request operations should be
    /// cancelled.
    /// 通知此请求的基础连接何时中止，因此应取消请求操作。
    /// </summary>
    public abstract CancellationToken RequestAborted { get; set; }

    /// <summary>
    /// Gets or sets a unique identifier to represent this request in trace logs.
    /// 获取或设置一个唯一标识符，以在跟踪日志中表示此请求
    /// </summary>
    public abstract string TraceIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the object used to manage user session data for this request.
    /// 获取或设置用于管理此请求的用户会话数据的对象。
    /// </summary>
    public abstract ISession Session { get; set; }

    /// <summary>
    /// Aborts the connection underlying this request.
    /// 中止此请求背后的连接。
    /// </summary>
    public abstract void Abort();

    private string DebuggerToString()
    {
        return HttpContextDebugFormatter.ContextToString(this, reasonPhrase: null);
    }

    private sealed class HttpContextDebugView(HttpContext context)
    {
        private readonly HttpContext _context = context;

        // Hide server specific implementations, they combine IFeatureCollection and many feature interfaces.
        public HttpContextFeatureDebugView Features => new HttpContextFeatureDebugView(_context.Features);
        public HttpRequest Request => _context.Request;
        public HttpResponse Response => _context.Response;
        public Endpoint? Endpoint => _context.GetEndpoint();
        public ConnectionInfo Connection => _context.Connection;
        public WebSocketManager WebSockets => _context.WebSockets;
        public ClaimsPrincipal User => _context.User;
        public IDictionary<object, object?> Items => _context.Items;
        public CancellationToken RequestAborted => _context.RequestAborted;
        public IServiceProvider RequestServices => _context.RequestServices;
        public string TraceIdentifier => _context.TraceIdentifier;
        // The normal session property throws if accessed before/without the session middleware.
        public ISession? Session => _context.Features.Get<ISessionFeature>()?.Session;
    }

    [DebuggerDisplay("Count = {Items.Length}")]
    private sealed class HttpContextFeatureDebugView(IFeatureCollection features)
    {
        private readonly IFeatureCollection _features = features;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<string, object>[] Items => _features.Select(pair => new KeyValuePair<string, object>(pair.Key.FullName ?? string.Empty, pair.Value)).ToArray();
    }
}
