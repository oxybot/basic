using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the balance data.
    /// </summary>
    public class BalanceForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        [Display(Name = "User")]
        [SwaggerSchema(Format = "ref/user")]
        public Guid? UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        [Display(Name = "Category")]
        [SwaggerSchema(Format = "ref/category")]
        public Guid? CategoryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the reference year.
        /// </summary>
        [Required]
        [Range(2000, 2100)]
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the defined standard allowance for this year, in hours.
        /// </summary>
        [Required]
        [Minimum(1)]
        [SwaggerSchema(Format = "hours")]
        public int? Allowed { get; set; }

        /// <summary>
        /// Gets or sets the transfered amount to add to the balance, in hours.
        /// </summary>
        [Required]
        [Minimum(0)]
        [SwaggerSchema(Format = "hours")]
        public int? Transfered { get; set; }
    }
}
