using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    public class ClientContract : BaseModel
    {
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

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Service> Services { get; }

        public ICollection<Invoice> Invoices { get; }
    }
}
