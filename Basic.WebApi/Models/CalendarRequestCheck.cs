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
        /// Gets or sets a value indicating whereas the request contains the required data.
        /// </summary>
        public bool RequestComplete { get; set; }

        /// <summary>
        /// Gets or sets the message relative to the <see cref="RequestComplete"/> status, if any.
        /// </summary>
        public string RequestCompleteMessage { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whereas an active schedule is linked to the request.
        /// </summary>
        public bool ActiveSchedule { get; set; }

        /// <summary>
        /// Gets or sets the message relative to the <see cref="ActiveSchedule"/> status, if any.
        /// </summary>
        public string ActiveScheduleMessage { get; set; }

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
