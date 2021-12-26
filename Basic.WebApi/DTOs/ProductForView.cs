using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the summarised data of a product.
    /// </summary>
    public class ProductForView : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the name of the product as displayed in the interface.
        /// </summary>
        [Required]
        [Display(Prompt = "Awesome product", Description = "The name of the product")]
        [SwaggerSchema("The name of the product")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the default description of the product as displayed in agreement items.
        /// </summary>
        [SwaggerSchema("The default description associated with the product")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the default unit price of the product.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "currency")]
        public decimal DefaultUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the default quantity of the product.
        /// </summary>
        [Required]
        public decimal DefaultQuantity { get; set; }
    }
}
