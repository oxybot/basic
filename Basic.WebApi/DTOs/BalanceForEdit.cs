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
        public Guid UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        public Guid CategoryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the reference year.
        /// </summary>
        [Required]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the defined standard allowance for this year, in hours.
        /// </summary>
        [Required]
        public int Allowed { get; set; }

        /// <summary>
        /// Gets or sets the transfered amount to add to the balance, in hours.
        /// </summary>
        public int Transfered { get; set; }
    }
}
