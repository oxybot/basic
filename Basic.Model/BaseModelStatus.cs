using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a change of status for a model instance.
    /// </summary>
    public abstract class BaseModelStatus : BaseModel
    {
        /// <summary>
        /// Gets or sets the status of the associated entity.
        /// </summary>
        [Required]
        public virtual Status Status { get; set; }

        /// <summary>
        /// Gets or sets the user that did the update of status.
        /// </summary>
        [Required]
        public virtual User UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the update.
        /// </summary>
        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}
