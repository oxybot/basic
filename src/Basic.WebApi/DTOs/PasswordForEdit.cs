// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

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
        /// Error message associated with a password and its confirmation being different.
        /// </summary>
        protected const string ErrorPasswordNotConfirmed = "The password and its confirmation should match";

        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password of the user.
        /// </summary>
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

            if (!string.Equals(this.NewPassword, this.ConfirmPassword, StringComparison.Ordinal))
            {
                yield return new ValidationResult(ErrorPasswordNotConfirmed, new[] { nameof(this.NewPassword) });
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

            int score = 0;
            score += numbers > 0 ? 1 : 0;
            score += lowers > 0 ? 1 : 0;
            score += uppers > 0 ? 1 : 0;
            score += specials > 0 ? 1 : 0;

            if (score < 3)
            {
                yield return new ValidationResult(ErrorPasswordTooWeak, new[] { nameof(this.NewPassword) });
            }
        }
    }
}
