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
        /// Gets or sets the expiration time of the token in seconds.
        /// </summary>
        public int ExpireIn { get; set; }
    }
}
