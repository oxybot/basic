// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents the data of an agreement.
/// </summary>
public class AgreementForEdit : BaseEntityDTO
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
    /// Gets or sets the identifier of the associated client.
    /// </summary>
    [Required]
    [Display(Name = "Client")]
    [SwaggerSchema(Format = "ref/client")]
    public Guid? ClientIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the signature date of the agreement.
    /// </summary>
    public DateOnly? SignatureDate { get; set; }

    /// <summary>
    /// Gets or sets the private notes associated to the agreement.
    /// </summary>
    public string PrivateNotes { get; set; }

    /// <summary>
    /// Gets or sets the associated items.
    /// </summary>
    [SwaggerSchema(ReadOnly = false)]
    [SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "Required for Asp.Net Core binding")]
    public ICollection<AgreementItemForEditWithIdentifier> Items { get; set; }
}
