using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data to update the password of a specific user.
    /// </summary>
    public class PasswordForEdit : BaseEntityDTO, IValidatableObject
    {
        /// <summary>
        /// Error message associated with a short password.
        /// </summary>
        protected const string ErrorPasswordTooShort = "The password is too short";

        /// <summary>
        /// Error message associated with a weak password.
        /// </summary>
        protected const string ErrorPasswordTooWeak = "The password is too weak";

        /// <summary>
        /// Gets or sets the old password of the user.
        /// </summary>
        /// <value>
        /// A <c>null</c> or empty value indicates that the user should be disabled.</value>
        // [Required]
        public string OldPassword { get; }

        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        /// <value>
        /// A <c>null</c> or empty value indicates that the user should be disabled.</value>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password of the user.
        /// </summary>
        /// <value>
        /// A <c>null</c> or empty value indicates that the user should be disabled.</value>
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Validates the current instance.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>The errors during the validation of the instance.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (validationContext is null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            if (string.IsNullOrEmpty(this.NewPassword))
            {
                yield break;
            }

            if (this.NewPassword.Length >= 16)
            {
                yield break;
            }

            if (this.NewPassword.Length < 12)
            {
                yield return new ValidationResult(ErrorPasswordTooShort, new[] { nameof(this.NewPassword) });
            }

            int numbers = Regex.Matches(this.NewPassword, "[0-9]").Count;
            int lowers = Regex.Matches(this.NewPassword, "[a-z]").Count;
            int uppers = Regex.Matches(this.NewPassword, "[A-Z]").Count;
            int specials = this.NewPassword.Length - numbers - lowers - uppers;

            if (specials == this.NewPassword.Length)
            {
                yield break;
            }

            int score = 0;
            score += numbers > 0 ? 1 : 0;
            score += lowers > 0 ? 1 : 0;
            score += uppers > 0 ? 1 : 0;
            score += specials > 0 ? 1 : 0;

            if (score < 3 && this.TestOfNewPassword())
            {
                yield return new ValidationResult(ErrorPasswordTooWeak, new[] { nameof(this.NewPassword) });
            }
        }

        /// <summary>
        /// Test if the new password is equal to the confirm password.
        /// </summary>
        /// <returns> "true" if the new password match the confirm password.</returns>
        public bool TestOfNewPassword()
        {
                return this.NewPassword == this.ConfirmPassword;
        }
    }
}
