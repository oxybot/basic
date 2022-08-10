using System.Collections.Generic;

namespace Basic.Model
{
    /// <summary>
    /// Marks a class that supports attachments.
    /// </summary>
    public interface IWithAttachments<TAttachment>
        where TAttachment : BaseAttachment
    {
        /// <summary>
        /// Gets the associated attachments.
        /// </summary>
        public ICollection<TAttachment> Attachments { get; }
    }
}
