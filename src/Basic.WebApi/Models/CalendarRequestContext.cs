using Basic.Model;

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Groups all the key data linked to a calendar request.
    /// </summary>
    public class CalendarRequestContext
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the category mapped to the request.
        /// </summary>
        public EventCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the schedule mapped to the request.
        /// </summary>
        public Schedule Schedule { get; set; }

        /// <summary>
        /// Gets or sets the number of hours associated to the request.
        /// </summary>
        public decimal? TotalHours { get; set; }

        /// <summary>
        /// Gets or sets the number of days associated to the request.
        /// </summary>
        public int? TotalDays { get; set; }
    }
}
