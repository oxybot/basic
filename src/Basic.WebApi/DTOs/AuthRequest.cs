// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of an authentication request.
/// </summary>
public class AuthRequest : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "password")]
    public string Password { get; set; }
}
