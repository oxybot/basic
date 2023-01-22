// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.Controllers;
using Basic.WebApi.DTOs;

namespace Basic.WebApi.Models;

/// <summary>
/// Provides a detailed check on the status and impacts of a <see cref="MyEventRequest"/>.
/// </summary>
/// <seealso cref="CalendarController.Check"/>
public class EventRequestCheck
{
    /// <summary>
    /// Gets or sets the number of hours associated to the request.
    /// </summary>
    public decimal? TotalHours { get; set; }

    /// <summary>
    /// Gets or sets the number of days associated to the request.
    /// </summary>
    public int? TotalDays { get; set; }
}
