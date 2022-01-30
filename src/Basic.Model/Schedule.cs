using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the working schedule of a user.
    /// </summary>
    public class Schedule : BaseModel
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the start date of this schedule.
        /// </summary>
        [Required]
        public DateOnly ActiveFrom { get; set; }

        /// <summary>
        /// Gets or sets the end date of this schedule, if any.
        /// </summary>
        public DateOnly? ActiveTo { get; set; }

        /// <summary>
        /// Gets or sets the number of working hours per day of the week.
        /// </summary>
        /// <value>
        /// 7 or 14 values expected.
        /// </value>
        [Required]
        public decimal[] WorkingSchedule { get; set; }
    }
}
