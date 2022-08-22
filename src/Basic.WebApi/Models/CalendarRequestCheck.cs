using Basic.WebApi.Controllers;
using Basic.WebApi.DTOs;

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Provides a detailed check on the status and impacts of a <see cref="CalendarRequest"/>.
    /// </summary>
    /// <seealso cref="CalendarController.Check(CalendarRequest)"/>
    public class CalendarRequestCheck
    {
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
