// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the data of a client.
    /// </summary>
    public class Client : BaseModel, IWithAttachments<ClientAttachment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
            this.Agreements = new List<Agreement>();
            this.Attachments = new List<ClientAttachment>();
        }

        /// <summary>
        /// Gets or sets the name of the client as displayed in the interface.
        /// </summary>
        [Required]
        [Unique]
        [MaxLength(255)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the client as displayed in official papers.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the address of the client.
        /// </summary>
        [Required]
        public StreetAddress Address { get; set; }

        /// <summary>
        /// Gets the associated agreements.
        /// </summary>
        public virtual ICollection<Agreement> Agreements { get; }

        /// <summary>
        /// Gets the list of the attachments.
        /// </summary>
        public virtual ICollection<ClientAttachment> Attachments { get; }
    }
}
