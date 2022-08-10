using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the event data.
    /// </summary>
    public class EventForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        [Display(Name = "User")]
        [SwaggerSchema(Format = "ref/user")]
        public Guid UserIdentifier { get; set; }

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
        [Required]
        [SwaggerSchema(Format = "hours")]
        public decimal? DurationFirstDay { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the last day.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "hours")]
        public decimal? DurationLastDay { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the event, in hours.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "hours")]
        public decimal? DurationTotal { get; set; }
    }
}
