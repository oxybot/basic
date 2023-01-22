// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents a new calendar request for the connected user.
/// </summary>
public class MyEventRequest : BaseEntityDTO, IValidatableObject
{
    /// <summary>
    /// Gets or sets the associated category.
    /// </summary>
    [Required]
    [Display(Name = "Category")]
    [SwaggerSchema(Format = "ref/category")]
    public Guid? CategoryIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the comment associated to the request.
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Gets or sets the end date of the event.
    /// </summary>
    [Required]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the event.
    /// </summary>
    [Required]
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the number of hours associated to the first day.
    /// </summary>
    [SwaggerSchema(Format = "hours")]
    [Range(0, 24)]
    public int? DurationFirstDay { get; set; }

    /// <summary>
    /// Gets or sets the number of hours associated to the last day.
    /// </summary>
    [SwaggerSchema(Format = "hours")]
    [Range(0, 24)]
    public int? DurationLastDay { get; set; }

    /// <summary>
    /// Validates the current instance.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>The errors during the validation of the instance.</returns>
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.CategoryIdentifier == Guid.Empty)
        {
            yield return new ValidationResult(
                "The Category is mandatory",
                new[] { nameof(this.CategoryIdentifier) });
        }

        if (this.StartDate == DateOnly.MinValue)
        {
            yield return new ValidationResult(
                "The Start Date is mandatory",
                new[] { nameof(this.StartDate) });
        }

        if (this.EndDate == DateOnly.MinValue)
        {
            yield return new ValidationResult(
                "The End Date is mandatory",
                new[] { nameof(this.EndDate) });
        }

        if (this.StartDate > this.EndDate)
        {
            yield return new ValidationResult(
                "The End Date can't be earlier than Start Date",
                new[] { nameof(this.StartDate), nameof(this.EndDate) });
        }
    }
}
