using System.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Specifies the minimum value for the associated numeric field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MinimumAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message.
        /// </summary>
        private const string DefaultErrorMessage = "The field {0} must be greated or equal to {1}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumAttribute"/> class with a <c>int</c> value.
        /// </summary>
        /// <param name="minimum">The minimum value for the associated field.</param>
        public MinimumAttribute(int minimum)
            : base(() => DefaultErrorMessage)
        {
            this.Minimum = minimum;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumAttribute"/> class with a <c>decimal</c> value.
        /// </summary>
        /// <param name="minimum">The minimum value for the associated field.</param>
        public MinimumAttribute(decimal minimum)
            : base(() => DefaultErrorMessage)
        {
            this.Minimum = minimum;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumAttribute"/> class with a <c>double</c> value.
        /// </summary>
        /// <param name="minimum">The minimum value for the associated field.</param>
        public MinimumAttribute(double minimum)
            : base(() => DefaultErrorMessage)
        {
            this.Minimum = minimum;
        }

        /// <summary>
        /// Gets the minimal value allowed for the associated field.
        /// </summary>
        public IComparable Minimum { get; }

        /// <summary>
        /// Checks that a provided value is superior or equal to <see cref="Minimum"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if the value is valid; <c>false</c> otherwise.</returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value.GetType() != this.Minimum.GetType())
            {
                return false;
            }

            return this.Minimum.CompareTo(value) <= 0;
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
        /// <description>The <see cref="Minimum"/> value.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Minimum);
        }
    }
}
