﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a detail item part of an agreement.
    /// </summary>
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

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity associated.
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price associated.
        /// </summary>
        [Required]
        public decimal UnitPrice { get; set; }
    }
}
