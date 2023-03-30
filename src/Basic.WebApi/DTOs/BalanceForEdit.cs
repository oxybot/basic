// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the balance data.
/// </summary>
public class BalanceForEdit : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the associated user.
    /// </summary>
    [Required]
    [Display(Name = "User")]
    [SwaggerSchema(Format = "ref/user")]
    public Guid? UserIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the associated category.
    /// </summary>
    [Required]
    [Display(Name = "Category")]
    [SwaggerSchema(Format = "ref/category")]
    public Guid? CategoryIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the reference year.
    /// </summary>
    [Required]
    [Range(2000, 2100)]
    public int? Year { get; set; }

    /// <summary>
    /// Gets or sets the defined standard allowance for this year, in hours.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "hours")]
    public decimal Allowed { get; set; }
}
