namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Raises a <c>400</c> http error code.
    /// </summary>
    public class BadRequestException : BadHttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        /// <param name="message">The associated error message.</param>
        public BadRequestException(string message)
            : base(message, 400)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        /// <param name="message">The associated error message.</param>
        /// <param name="inner">The underlying error description.</param>
        public BadRequestException(string message, Exception inner)
            : base(message, 400, inner)
        { }
    }
}
