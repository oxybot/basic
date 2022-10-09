using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Indicates that the associated property is required when another property has a specific value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredWhenAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message.
        /// </summary>
        private const string DefaultErrorMessage = "The field {0} is required based on {1} value.";

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredWhenAttribute"/> class.
        /// </summary>
        /// <param name="linkedProperty">The name of the linked property.</param>
        /// <param name="linkedValue">The value for which the tagged property becomes required, could be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="linkedProperty"/> parameter is <c>null</c>.</exception>
        public RequiredWhenAttribute(string linkedProperty, object linkedValue)
            : base(DefaultErrorMessage)
        {
            LinkedProperty = linkedProperty ?? throw new ArgumentNullException(nameof(linkedProperty));
            LinkedValue = linkedValue;
        }

        /// <summary>
        /// Enforces the validation of this assertion based on a context.
        /// </summary>
        /// <value>
        /// Returns always <c>true</c>.
        /// </value>
        public override bool RequiresValidationContext => true;

        /// <summary>
        /// Gets the name of the linked property.
        /// </summary>
        public string LinkedProperty { get; }

        /// <summary>
        /// Gets the value for which the tagged property becomes required, could be <c>null</c>.
        /// </summary>
        public object LinkedValue { get; }

        /// <summary>
        /// Checks the assertion without a context.
        /// </summary>
        /// <param name="value">The value of the instance to check.</param>
        /// <returns>Always raise an exception.</returns>
        /// <exception cref="InvalidOperationException">This assertion should always be checked with a context.</exception>
        public override bool IsValid(object value)
        {
            throw new InvalidOperationException("A validation context is required");
        }

        /// <summary>
        /// Checks the assertion.
        /// </summary>
        /// <param name="value">The value of the instance to check.</param>
        /// <param name="context">The validation context.</param>
        /// <returns>The validation result associated with the assertion.</returns>
        /// <exception cref="ArgumentNullException">The context is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The context is empty.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ObjectInstance is null || context.ObjectType is null)
            {
                throw new ArgumentException("The context is not properly initialized", nameof(context));
            }

            var property = context.ObjectType.GetProperty(this.LinkedProperty);
            if (property == null)
            {
                throw new ArgumentException($"The linked property {LinkedProperty} is not present in the {context.ObjectType.Name} class");
            }

            if (object.Equals(property.GetValue(context.ObjectInstance), this.LinkedValue))
            {
                // The field is required
                if (value == null)
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Overrides the formatting so that two parameters can be used in the error message.
        /// </summary>
        /// <param name="name">The display name of the field.</param>
        /// <returns>The formatted error message.</returns>
        /// <remarks>
        /// The format parameters are:
        /// <list type="table">
        /// <item>
        /// <term>0</term>
        /// <description>The <paramref name="name"/> value.</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>The <see cref="LinkedProperty"/> value.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, LinkedProperty);
        }
    }
}
