using System;
using System.Collections.Generic;

namespace Basic.Model
{
    /// <summary>
    /// Represents an attachment file.
    /// </summary>
    public class Attachment : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment"/> class.
        /// </summary>
        public Attachment()
        {
        }

        /// <summary>
        /// Gets or sets the display name of the attachment.
        /// </summary>
        // [Required]
        public Guid DisplayName { get; set; }

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

        /// <summary>
        /// Gets the type of entitie the attachment is associated to.
        /// </summary>
        // [Required]
        public TypeOfEntitie type { get; }

        /// <summary>
        /// Gets the entity identifier associated to the attachment
        /// </summary>
        // [Required]
        public virtual Guid EntitieIdentifier { get; }
    }
}
