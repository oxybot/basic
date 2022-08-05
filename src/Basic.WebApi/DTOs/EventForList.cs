using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the event data.
    /// </summary>
    public class EventForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the event")]
        public Guid Identifier { get; set; }

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
        /// Gets or sets the end date of the event.
        /// </summary>
        [Required]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        [Required]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the event, in hours.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "hours")]
        public decimal DurationTotal { get; set; }

        /// <summary>
        /// Gets or sets the current status for the event.
        /// </summary>
        [SwaggerSchema(Format = "ref/status")]
        public StatusReference CurrentStatus { get; set; }
    }
}
