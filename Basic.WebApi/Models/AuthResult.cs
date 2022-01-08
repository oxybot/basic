namespace Basic.WebApi.Models
{
    /// <summary>
    /// Represents the result of an authentication request.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the token type
        /// </summary>
        /// <value>
        /// Default value to <c>Bearer</c>.
        /// </value>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Gets or sets the expiration date of the token.
        /// </summary>
        /// <remarks>
        /// The provided value as the number of milliseconds elapsed since
        /// January 1, 1970 00:00:00 UTC.
        /// </remarks>
        public long Expire { get; set; }
    }
}
