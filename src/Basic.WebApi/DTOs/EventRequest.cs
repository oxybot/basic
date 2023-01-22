// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents a new calendar request for a specific user.
/// </summary>
public class EventRequest : MyEventRequest
{
    /// <summary>
    /// Gets or sets the user for which the request is done.
    /// </summary>
    [Required]
    [Display(Name = "User")]
    [SwaggerSchema(Format = "ref/user")]
    public Guid? UserIdentifier { get; set; }

    /// <inheritdoc/>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.UserIdentifier == Guid.Empty)
        {
            yield return new ValidationResult(
                "The User is mandatory",
                new[] { nameof(this.UserIdentifier) });
        }

        foreach (var result in base.Validate(validationContext))
        {
            yield return result;
        }
    }
}
