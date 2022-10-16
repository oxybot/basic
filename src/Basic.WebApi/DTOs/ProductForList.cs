// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the summarised data of a product.
    /// </summary>
    public class ProductForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the product")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the product as displayed in the interface.
        /// </summary>
        [Required]
        [Display(Prompt = "Awesome product", Description = "The name of the product")]
        [SwaggerSchema("The name of the product")]
        public string DisplayName { get; set; }
    }
}
