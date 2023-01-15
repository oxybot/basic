// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents a link from a DTO to user.
/// </summary>
public class UserReference
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [Key]
    [SwaggerSchema("The unique identifier of the user")]
    public Guid Identifier { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    [Required]
    [SwaggerSchema("The display name of the user")]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the title of the user.
    /// </summary>
    [SwaggerSchema("The title of the user")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the avatar of the user.
    /// </summary>
    [Display(Order = 1)]
    [SwaggerSchema(Format = "image")]
    public Base64File Avatar { get; set; }
}
