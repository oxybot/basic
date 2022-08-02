using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents an attachment file.
    /// </summary>
    public class Attachment : BaseModel
    {        

        /// <summary>
        /// Gets or sets the display name of the attachment.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the attachment content.
        /// </summary>
        [Required]
        public TypedFile AttachmentContent { get; set; }
    }
}
