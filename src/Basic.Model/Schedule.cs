// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Basic.Model;

/// <summary>
/// Represents the working schedule of a user.
/// </summary>
public class Schedule : BaseModel
{
    /// <summary>
    /// Gets or sets the associated user.
    /// </summary>
    [Required]
    public virtual User User { get; set; }

    /// <summary>
    /// Gets or sets the start date of this schedule.
    /// </summary>
    [Required]
    public DateOnly ActiveFrom { get; set; }

    /// <summary>
    /// Gets or sets the end date of this schedule, if any.
    /// </summary>
    public DateOnly? ActiveTo { get; set; }

    /// <summary>
    /// Gets or sets the number of working hours per day of the week.
    /// </summary>
    /// <value>
    /// 7 or 14 values expected.
    /// </value>
    [Required]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Special conversion in place")]
    public decimal[] WorkingSchedule { get; set; }
}
