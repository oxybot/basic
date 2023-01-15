// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of an agreement.
/// </summary>
public class AgreementForView : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the internal code of the agreement.
    /// </summary>
    [Required]
    public string InternalCode { get; set; }

    /// <summary>
    /// Gets or sets the title of the agreement.
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the refence to the associated client.
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "ref/client")]
    public EntityReference Client { get; set; }

    /// <summary>
    /// Gets or sets the signature date of the agreement.
    /// </summary>
    public DateOnly? SignatureDate { get; set; }

    /// <summary>
    /// Gets or sets the private notes associated to the agreement.
    /// </summary>
    public string PrivateNotes { get; set; }

    /// <summary>
    /// Gets or sets the items of the agreement.
    /// </summary>
    [Display(GroupName = "Items")]
    public IEnumerable<AgreementItemForList> Items { get; set; }
}
