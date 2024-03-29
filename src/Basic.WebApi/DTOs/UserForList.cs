﻿// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.Framework;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of a user.
/// </summary>
public class UserForList : BaseEntityDTO
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
    [SwaggerSchema("The display name of the user")]
    [Required]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the user name of the user.
    /// </summary>
    [SwaggerSchema("The user name of the user")]
    [Required]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the title of the user.
    /// </summary>
    [SwaggerSchema("The e-mail of the user")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the title of the user.
    /// </summary>
    [SwaggerSchema("The title of the user")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the avatar of the user.
    /// </summary>
    [Display(Order = 1)]
    [Sortable(false)]
    [SwaggerSchema(Format = "image")]
    public Base64File Avatar { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is authorized to connect.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the roles assigned to the user.
    /// </summary>
    [Display(GroupName = "Roles")]
    [Sortable(false)]
    [SwaggerSchema(Format = "list/role")]
    public IEnumerable<RoleForList> Roles { get; set; }
}
