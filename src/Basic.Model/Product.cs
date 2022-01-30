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
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the default description of the product.
        /// </summary>
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
