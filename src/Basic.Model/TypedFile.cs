using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a file added to an entity.
    /// </summary>
    [Owned]
    public class TypedFile
    {
        /// <summary>
        /// Gets or sets the data of the file.
        /// </summary>
        [Required]
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the mime-type of the file.
        /// </summary>
        [Required]
        public string MimeType { get; set; }
    }
}
