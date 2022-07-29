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

        // /// <summary>
        // /// Gets or sets the buffer of the attachment.
        // /// </summary>
        // [Required]
        // public int Blob { get; set; }

        // /// <summary>
        // /// Gets or sets the extension of the attachment.
        // /// </summary>
        // [Required]
        // public string Extension { get; set; }

        /// <summary>
        /// Gets the type of entitie the attachment is associated to.
        /// </summary>
        // [Required]
        public TypeOfEntitie Type { get; set; }

        /// <summary>
        /// Gets the entity identifier associated to the attachment
        /// </summary>
        // [Required]
        public virtual Guid EntitieIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the attachment content.
        /// </summary>
        [Required]
        public TypedFile AttachmentContent { get; set; }
    }
}
