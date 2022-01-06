using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the event data.
    /// </summary>
    public class EventForView : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "ref/user")]
        public UserReference User { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "ref/category")]
        public EntityReference Category { get; set; }

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
        [Required]
        [SwaggerSchema(Format = "number/hours")]
        public int DurationFirstDay { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the last day.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "number/hours")]
        public int DurationLastDay { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the event, in hours.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "number/hours")]
        public int DurationTotal { get; set; }
    }
}
