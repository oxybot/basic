using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Basic.Model
{
    /// <summary>
    /// Represents a time management event for a user.
    /// </summary>
    public class Event : BaseModel, IWithStatus<EventStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event()
        {
            this.Statuses = new List<EventStatus>();
        }

        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        public virtual EventCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the comment associated to the request.
        /// </summary>
        public string Comment { get; set; }

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
        /// Gets the current status for this event.
        /// </summary>
        public Status CurrentStatus
        {
            get { return this.Statuses.OrderByDescending(s => s.UpdatedOn).FirstOrDefault()?.Status; }
        }

        /// <summary>
        /// Gets the history of statuses of the event.
        /// </summary>
        public virtual ICollection<EventStatus> Statuses { get; }

        /// <summary>
        /// Gets or sets the number of hours associated to the first day.
        /// </summary>
        [Required]
        public decimal DurationFirstDay { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the last day.
        /// </summary>
        [Required]
        public decimal DurationLastDay { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the event.
        /// </summary>
        [Required]
        public decimal DurationTotal { get; set; }

        /// <summary>
        /// Gets or sets the list of the attachments.
        /// </summary>
        // public List<TypedFile> Attachments { get; set; }
    }
}
