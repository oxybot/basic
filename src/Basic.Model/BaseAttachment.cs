using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents an attachment file.
    /// </summary>
    public abstract class BaseAttachment : BaseModel
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

    /// <summary>
    /// Represents an attachment file.
    /// </summary>
    public abstract class BaseAttachment<TParentModel> : BaseAttachment
        where TParentModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the parent instance.
        /// </summary>
        [Required]
        public TParentModel Parent { get; set; }
    }
}
