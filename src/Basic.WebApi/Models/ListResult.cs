// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;

namespace Basic.WebApi.Models;

/// <summary>
/// Represents the result content of a <c>GetAll</c> api call.
/// </summary>
/// <typeparam name="T"> The type of returned elements.</typeparam>
public class ListResult<T>
    where T : BaseEntityDTO
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListResult{T}"/> class.
    /// </summary>
    /// <param name="values">
    /// The returned values.
    /// </param>
    public ListResult(IEnumerable<T> values)
    {
        this.Values = values;
    }

    /// <summary>
    /// Gets the returned values.
    /// </summary>
    public IEnumerable<T> Values { get; }

    /// <summary>
    /// Gets or sets the total number of elements in this collection of entities (without filtered).
    /// </summary>
    public int Total { get; set; }
}
