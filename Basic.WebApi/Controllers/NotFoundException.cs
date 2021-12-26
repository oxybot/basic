namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Raises a <c>404</c> http error code.
    /// </summary>
    public class NotFoundException : BadHttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">The associated error message.</param>
        public NotFoundException(string message)
            : base(message, 404) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">The associated error message.</param>
        /// <param name="inner">The underlying error description.</param>
        public NotFoundException(string message, Exception inner)
            : base(message, 404, inner) { }
    }
}
