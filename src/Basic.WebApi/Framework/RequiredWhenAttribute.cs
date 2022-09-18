using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredWhenAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message.
        /// </summary>
        private const string DefaultErrorMessage = "The field {0} is required based on {1} value.";

        public RequiredWhenAttribute(string linkedProperty, object linkedValue)
            :base(DefaultErrorMessage)
        {
            LinkedProperty = linkedProperty ?? throw new ArgumentNullException(nameof(linkedProperty));
            LinkedValue = linkedValue;
        }

        public override bool RequiresValidationContext => true;

        public string LinkedProperty { get; }

        public object LinkedValue { get; }

        public override bool IsValid(object value)
        {
            throw new InvalidOperationException("A validation context is required");
        }

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
