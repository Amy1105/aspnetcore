// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http.Features;

/// <summary>
/// Represents a collection of HTTP features.
/// 表示HTTP功能的集合。
/// </summary>
public interface IFeatureCollection : IEnumerable<KeyValuePair<Type, object>>
{
    /// <summary>
    /// Indicates if the collection can be modified.
    /// 指示是否可以修改集合。
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// Incremented for each modification and can be used to verify cached results.
    /// 每次修改都会递增，可用于验证缓存结果。
    /// </summary>
    int Revision { get; }

    /// <summary>
    /// Gets or sets a given feature. Setting a null value removes the feature.
    /// 获取或设置给定的功能。设置null值会删除该功能。
    /// </summary>
    /// <param name="key"></param>
    /// <returns>The requested feature, or null if it is not present.</returns>
    object? this[Type key] { get; set; }

    /// <summary>
    /// Retrieves the requested feature from the collection.
    /// 从集合中检索请求的功能。
    /// </summary>
    /// <typeparam name="TFeature">The feature key.</typeparam>
    /// <returns>The requested feature, or null if it is not present.</returns>
    TFeature? Get<TFeature>();

    /// <summary>
    /// Sets the given feature in the collection.
    /// 设置集合中的给定特征。
    /// </summary>
    /// <typeparam name="TFeature">The feature key.</typeparam>
    /// <param name="instance">The feature value.</param>
    void Set<TFeature>(TFeature? instance);
}
