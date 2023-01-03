// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
        [MaxLength(255)]
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
    /// <typeparam name="TParentModel">The parent entity.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Same class, two simple content")]
    public abstract class BaseAttachment<TParentModel> : BaseAttachment
        where TParentModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the parent instance.
        /// </summary>
        [Required]
        public virtual TParentModel Parent { get; set; }
    }
}
