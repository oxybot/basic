using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a detail item part of an agreement.
    /// </summary>
    public class AgreementItem : BaseModel
    {
        /// <summary>
        /// Gets or sets the parent agreement.
        /// </summary>
        [Required]
        public virtual Agreement Agreement { get; set; }

        /// <summary>
        /// Gets or sets the linked product, if any.
        /// </summary>
        public virtual Product Product { get; set; }

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
