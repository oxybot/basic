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
        public Guid UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        public Guid CategoryIdentifier { get; set; }

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
