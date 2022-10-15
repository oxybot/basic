using Basic.WebApi.DTOs;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace Basic.WebApi.Services
{
    /// <summary>
    /// Provides authentication of user using an external service.
    /// </summary>
    /// <remarks>
    /// The current implementation uses LDAP / Active Directory as a reference.
    /// </remarks>
    public class ExternalAuthenticatorService : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalAuthenticatorService"/> class.
        /// </summary>
        /// <param name="options">The active directory configuration options.</param>
        /// <param name="logger">The logger associated to the class.</param>
        public ExternalAuthenticatorService(IOptions<ActiveDirectoryOptions> options, ILogger<ExternalAuthenticatorService> logger)
        {
            this.Options = options.Value;
            this.Logger = logger;
            this.Connection = new LdapConnection();
        }

        /// <summary>
        /// Gets a configuration for the connection to the Active Directory.
        /// </summary>
        public ActiveDirectoryOptions Options { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        public ILogger<ExternalAuthenticatorService> Logger { get; }

        /// <summary>
        /// Gets a connection to the Active Directory.
        /// </summary>
        public LdapConnection Connection { get; }

        /// <summary>
        /// Disconnects from the Active Directory.
        /// </summary>
        public void Dispose()
        {
            this.Connection.Disconnect();
            this.Connection.Dispose();
        }

        /// <summary>
        /// Keyword search for an user in the Active Directory.
        /// </summary>
        public LdapUsers Search(string searchTerm)
        {
            LdapUsers results = new LdapUsers();

            try
            {
                if (!this.Connection.Connected)
                {
                    this.LdapConnect();
                }

                string filter = $"(&(objectClass=person)(cn=*{searchTerm}*))";
                LdapSearchConstraints constraints = new LdapSearchConstraints() { MaxResults = Options.UserSearchLimit };
                LdapSearchQueue queue = Connection.Search(Options.BaseDN, LdapConnection.ScopeSub, filter, null, false, (LdapSearchQueue)null, constraints);

                LdapMessage message;
                List<ExternalUser> users = new List<ExternalUser>();
                while ((message = queue.GetResponse()) != null)
                {
                    if (message is LdapSearchResult searchResult)
                    {
                        LdapEntry entry = searchResult.Entry;
                        ExternalUser user = new ExternalUser
                        {
                            DisplayName = entry.GetAttributeAsString("cn"),
                            Email = entry.GetAttributeAsString("mail"),
                            UserName = entry.GetAttributeAsString("sAMAccountName"),
                            Title = entry.GetAttributeAsString("description"),
                            Avatar = new Base64File() { MimeType = "image/jpeg", Data = entry.GetAttributeAsBase64("thumbnailPhoto") },
                            ExternalIdentifier = entry.GetAttributeAsString("userPrincipalName"),
                        };

                        users.Add(user);
                    }
                }

                users.Sort((u, v) => u.DisplayName.CompareTo(v.DisplayName));
                results.ListOfLdapUsers.AddRange(users);
            }
            catch (LdapException ex)
            {
                Logger.LogError(ex, "Can't retrieve users from Active Directory");
            }

            results.OccurrencesNumber = results.ListOfLdapUsers.Count;
            return results;
        }

        /// <summary>
        /// Validates the credential of a user.
        /// </summary>
        /// <param name="externalIdentifier">The identifier of the user on the external authenticator.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns></returns>
        public bool ValidateUser(string externalIdentifier, string password)
        {
            if (string.IsNullOrEmpty(externalIdentifier))
            {
                throw new ArgumentException($"'{nameof(externalIdentifier)}' cannot be null or empty.", nameof(externalIdentifier));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));
            }

            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(Options.Server, Options.Port);
                    connection.Bind(externalIdentifier, password);
                    if (connection.Bound)
                    {
                        return true;
                    }
                }
            }
            catch (LdapException ex)
            {
                Logger.LogError(ex, "Error while connecting to the external authenticator");
            }

            return false;
        }

        /// <summary>
        /// Establishes a connection to the Active Directory.
        /// </summary>
        protected void LdapConnect()
        {
            this.Connection.SecureSocketLayer = false;
            Connection.Connect(Options.Server, Options.Port);
            Connection.Bind(Options.UserDN, Options.Password);
        }
    }
}
