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
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the event")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        public UserReference User { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        public EntityReference Category { get; set; }

        /// <summary>
        /// Gets or sets the comment associated to the request.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the first day.
        /// </summary>
        [Required]
        public int DurationFirstDay { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the last day.
        /// </summary>
        [Required]
        public int DurationLastDay { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the event.
        /// </summary>
        [Required]
        public int DurationTotal { get; set; }
    }
}
