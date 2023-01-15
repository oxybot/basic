// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the calendar of a specific user.
/// </summary>
public class UserCalendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCalendar"/> class.
    /// </summary>
    public UserCalendar()
    {
        this.Lines = new List<UserCalendarLine>();
        this.DaysOff = new List<int>();
    }

    /// <summary>
    /// Gets or sets the associated users.
    /// </summary>
    public EntityReference User { get; set; }

    /// <summary>
    /// Gets or sets the standard days off for this user.
    /// </summary>
    [SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "Required for Asp.Net Core binding")]
    public ICollection<int> DaysOff { get; set; }

    /// <summary>
    /// Gets or sets the lines for this user.
    /// </summary>
    [SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "Required for Asp.Net Core binding")]
    public ICollection<UserCalendarLine> Lines { get; set; }
}
