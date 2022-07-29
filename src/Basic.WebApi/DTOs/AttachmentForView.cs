using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a user.
    /// </summary>
    public class AttachmentForView : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the display name of the attachment.
        /// </summary>
        [SwaggerSchema("The display name of the attachment")]
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the buffer of the attachment.
        /// </summary>
        // [Required]
        public int Blob { get; set; }

        /// <summary>
        /// Gets or sets the extension of the attachment.
        /// </summary>
        // [Required]
        public string Extension { get; set; }
    }
}