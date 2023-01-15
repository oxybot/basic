// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of an agreement item.
/// </summary>
public class AgreementItemForEditWithIdentifier : AgreementItemForEdit
{
    /// <summary>
    /// Gets or sets the unique identifier of the item, if any.
    /// </summary>
    [Key]
    [SwaggerSchema("The unique identifier of the item", ReadOnly = true)]
    public Guid? Identifier { get; set; }
}
