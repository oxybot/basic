using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a user's working schedule.
    /// </summary>
    public class ScheduleForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "ref/user")]
        public Guid? UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the start date of this schedule.
        /// </summary>
        [Required]
        public DateOnly? ActiveFrom { get; set; }

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
        [SwaggerSchema(Format = "schedule")]
        public decimal[] WorkingSchedule { get; set; }
    }
}
