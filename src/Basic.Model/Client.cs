using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the data of a client.
    /// </summary>
    public class Client : BaseModel, IWithAttachments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
            this.Agreements = new List<Agreement>();
            this.Attachments = new List<Attachment>();
        }

        /// <summary>
        /// Gets or sets the name of the client as displayed in the interface.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the client as displayed in official papers.
        /// </summary>
        [Required]
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
        /// Gets or sets the list of the attachments.
        /// </summary>
        public virtual ICollection<Attachment> Attachments { get; }
    }
}
