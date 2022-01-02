namespace Basic.WebApi.Models
{
    /// <summary>
    /// Represents the data of an authentication request.
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}
