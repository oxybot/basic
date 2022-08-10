using System.Collections.Generic;

namespace Basic.Model
{
    /// <summary>
    /// Marks a class that supports attachments.
    /// </summary>
    public interface IWithAttachments
    {
        /// <summary>
        /// Gets the associated attachments.
        /// </summary>
        public ICollection<Attachment> Attachments { get; }
    }
}
