// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the balance data.
/// </summary>
public class BalanceForList : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the balance.
    /// </summary>
    [Key]
    [SwaggerSchema("The unique identifier of the balance")]
    public Guid Identifier { get; set; }

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
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the defined total allowance for this year, in hours.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "hours")]
    public decimal Total { get; set; }
}
