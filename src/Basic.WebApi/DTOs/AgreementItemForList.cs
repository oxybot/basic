// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of an agreement item.
    /// </summary>
    public class AgreementItemForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the item")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the linked product, if any.
        /// </summary>
        [SwaggerSchema(Format = "ref/product")]
        public EntityReference Product { get; set; }

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
        [SwaggerSchema(Format = "currency")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets the total price associated.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "currency")]
        public decimal TotalPrice { get => this.Quantity * this.UnitPrice; }
    }
}
