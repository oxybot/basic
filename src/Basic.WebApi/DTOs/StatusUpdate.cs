using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents a request for status update.
    /// </summary>
    public class StatusUpdate
    {
        /// <summary>
        /// Gets or sets the identifier of the current status.
        /// </summary>
        [Required]
        public Guid? From { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the targeted status.
        /// </summary>
        [Required]
        public Guid? To { get; set; }
    }
}
