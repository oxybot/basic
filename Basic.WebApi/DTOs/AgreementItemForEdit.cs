using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of an agreement item.
    /// </summary>
    public class AgreementItemForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the linked product, if any.
        /// </summary>
        [Display(Name = "Product")]
        [SwaggerSchema(Format = "ref/product")]
        public Guid? ProductIdentifier { get; set; }

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
