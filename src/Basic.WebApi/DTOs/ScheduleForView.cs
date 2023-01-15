// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of a user's working schedule.
/// </summary>
public class ScheduleForView : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the associated user.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "ref/user")]
    public virtual UserReference User { get; set; }

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
    [SwaggerSchema(Format = "schedule")]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "DTO")]
    public decimal[] WorkingSchedule { get; set; }
}
