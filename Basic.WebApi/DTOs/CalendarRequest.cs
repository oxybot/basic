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
        public Guid CategoryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the comment associated to the request.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the first day.
        /// </summary>
        [SwaggerSchema(Format = "number/hours")]
        public int? DurationFirstDay { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the last day.
        /// </summary>
        [SwaggerSchema(Format = "number/hours")]
        public int? DurationLastDay { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CategoryIdentifier == Guid.Empty)
            {
                yield return new ValidationResult(
                    "The Category is mandatory",
                    new[] { nameof(CategoryIdentifier) });
            }

            if (StartDate == DateTime.MinValue)
            {
                yield return new ValidationResult(
                    "The Start Date is mandatory",
                    new[] { nameof(StartDate) });
            }

            if (EndDate == DateTime.MinValue)
            {
                yield return new ValidationResult(
                    "The Start Date is mandatory",
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
