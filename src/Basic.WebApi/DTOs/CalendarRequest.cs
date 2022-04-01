using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents a new calendar request for the connected user.
    /// </summary>
    public class CalendarRequest : BaseEntityDTO, IValidatableObject
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
        public int? DurationFirstDay { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the last day.
        /// </summary>
        [SwaggerSchema(Format = "hours")]
        public int? DurationLastDay { get; set; }

        /// <summary>
        /// Validates the current instance.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The errors during the validation of the instance.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CategoryIdentifier == Guid.Empty)
            {
                yield return new ValidationResult(
                    "The Category is mandatory",
                    new[] { nameof(CategoryIdentifier) });
            }

            if (StartDate == DateOnly.MinValue)
            {
                yield return new ValidationResult(
                    "The Start Date is mandatory",
                    new[] { nameof(StartDate) });
            }

            if (EndDate == DateOnly.MinValue)
            {
                yield return new ValidationResult(
                    "The End Date is mandatory",
                    new[] { nameof(EndDate) });
            }

            if (StartDate > EndDate)
            {
                yield return new ValidationResult(
                    "The End Date can't be earlier than Start Date",
                    new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}
