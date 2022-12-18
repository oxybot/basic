// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Standard format for file content")]
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the mime-type of the file.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MimeType { get; set; }
    }
}
