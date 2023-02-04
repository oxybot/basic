// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Models;

/// <summary>
/// Defines the model associated with a standard sort and filter options.
/// </summary>
public class SortAndFilterModel
{
    /// <summary>
    /// Gets or sets the filter values.
    /// </summary>
    public IEnumerable<string> Filters { get; set; }

    /// <summary>
    /// Gets or sets the property name to sort on.
    /// </summary>
    public string SortKey { get; set; }

    /// <summary>
    /// Gets or sets the order for sort (asc or desc).
    /// </summary>
    public string SortValue { get; set; }
}
