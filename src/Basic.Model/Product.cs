// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the product definition sells by the company.
    /// </summary>
    public class Product : BaseModel
    {
        /// <summary>
        /// Gets or sets the display name of the product.
        /// </summary>
        [Required]
        [Unique]
        [MaxLength(255)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the default description of the product.
        /// </summary>
        [MaxLength(255)]
        public string DefaultDescription { get; set; }

        /// <summary>
        /// Gets or sets the default unit price of the product.
        /// </summary>
        [Required]
        public decimal DefaultUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the default quantity of the product.
        /// </summary>
        [Required]
        public decimal DefaultQuantity { get; set; }
    }
}
