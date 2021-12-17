using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Model
{
    /// <summary>
    /// Represents a client contract.
    /// </summary>
    public class ClientContract : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContract"/> class.
        /// </summary>
        public ClientContract()
        {
            this.Services = new List<Service>();
            this.Invoices = new List<Invoice>();
        }

        /// <summary>
        /// Gets or sets the parent client.
        /// </summary>
        [Required]
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the internal code associated to the contract.
        /// </summary>
        [Required]
        public string InternalCode { get; set; }

        /// <summary>
        /// Gets or sets the title of the contract.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the signature date of the contract.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Gets or sets the private notes associated to the contract.
        /// </summary>
        public string PrivateNotes { get; set; }

        /// <summary>
        /// Gets the services associated to the contract.
        /// </summary>
        public ICollection<Service> Services { get; }

        /// <summary>
        /// Gets the invoices attached to the contract.
        /// </summary>
        public ICollection<Invoice> Invoices { get; }
    }
}
