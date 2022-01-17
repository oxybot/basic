using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a status as part of a workflow that could be applied to an entity.
    /// </summary>
    public class Status : BaseModel
    {
        /// <summary>
        /// Gets or sets the name of the status - as displayed.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description of the status, if any.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the status is associated with active entities.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }
    }
}
