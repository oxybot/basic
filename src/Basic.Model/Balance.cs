using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the user balance for a specific year and a specific category of event.
    /// </summary>
    public class Balance : BaseModel
    {
        /// <summary>
        /// Gets or sets the associated user.
        /// </summary>
        [Required]
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the associated category.
        /// </summary>
        [Required]
        public virtual EventCategory Category { get; set; }

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
