// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of an balance item.
/// </summary>
public class BalanceItemForEdit : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the item, if any.
    /// </summary>
    [Key]
    [SwaggerSchema("The unique identifier of the item", ReadOnly = true)]
    public Guid? Identifier { get; set; }

    /// <summary>
    /// Gets or sets the description of the item.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the value associated.
    /// </summary>
    [Required]
    public decimal Value { get; set; }
}
