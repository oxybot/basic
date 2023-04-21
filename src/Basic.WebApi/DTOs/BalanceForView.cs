// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the balance data.
/// </summary>
public class BalanceForView : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the associated user.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "ref/user")]
    public UserReference User { get; set; }

    /// <summary>
    /// Gets or sets the associated category.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "ref/category")]
    public EntityReference Category { get; set; }

    /// <summary>
    /// Gets or sets the reference year.
    /// </summary>
    [Required]
    [Range(2000, 2100)]
    public int? Year { get; set; }

    /// <summary>
    /// Gets or sets the defined total allowance for this year, in hours.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "hours")]
    public decimal Total { get; set; }

    /// <summary>
    /// Gets or sets the details of the balance.
    /// </summary>
    [Display(GroupName = "Details")]
    public IEnumerable<BalanceItemForList> Details { get; set; }
}
