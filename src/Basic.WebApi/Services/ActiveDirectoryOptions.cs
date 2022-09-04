namespace Basic.WebApi.Services
{
    /// <summary>
    /// Defines the configuration options to connect to an Active Directory instance.
    /// </summary>
    /// <remarks>
    /// This configuration is used for user authentication and user creation.
    /// </remarks>
    public class ActiveDirectoryOptions
    {
        /// <summary>
        /// Gets the default section used to extract the options in the configuration.
        /// </summary>
        public const string Section = "ActiveDirectory";

        /// <summary>
        /// Gets or sets the server name of the Active Directory.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the port used to connect to the server.
        /// </summary>
        /// <value>
        /// Default to <c>389</c>.
        /// </value>
        public int Port { get; set; } = 389;

        /// <summary>
        /// Gets or sets the DN of the account used to connect to the Active Directory.
        /// </summary>
        public string UserDN { get; set; }

        /// <summary>
        /// Gets or sets the password of the account used to connect to the Active Directory.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the base DN used during search.
        /// </summary>
        public string BaseDN { get; set; }

        /// <summary>
        /// Gets or sets the limit for the search of users.
        /// </summary>
        /// <value>
        /// Default to <c>4</c>.
        /// </value>
        public int UserSearchLimit { get; set; } = 4;
    }
}
