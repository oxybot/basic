using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a user's working schedule.
    /// </summary>
    public class ScheduleForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the schedule.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the schedule")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "ref/user")]
        public virtual UserReference User { get; set; }

        /// <summary>
        /// Gets or sets the start date of this schedule.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "date")]
        public DateTime ActiveFrom { get; set; }

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
