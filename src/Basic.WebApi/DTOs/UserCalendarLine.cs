﻿// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents a calendar line of a specific user and category.
/// </summary>
public class UserCalendarLine
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCalendarLine"/> class.
    /// </summary>
    public UserCalendarLine()
    {
        this.Days = new List<int>();
    }

    /// <summary>
    /// Gets or sets the category name for this line.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the css class for this line.
    /// </summary>
    public string ColorClass { get; set; }

    /// <summary>
    /// Gets or sets the days part of this calendar line.
    /// </summary>
    [SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "Required for Asp.Net Core binding")]
    public ICollection<int> Days { get; set; }
}
