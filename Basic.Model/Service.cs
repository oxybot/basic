using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    public class Service : BaseModel
    {
        /// <summary>
        /// Gets or sets the parent agreement.
        /// </summary>
        [Required]
        public Agreement Agreement { get; set; }

        /// <summary>
        /// Gets or sets the linked product, if any.
        /// </summary>
        public Product Product { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}
