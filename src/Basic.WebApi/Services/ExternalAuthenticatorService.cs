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
            List<LdapUser> ldapUsersList = new List<LdapUser>();
            LdapUsers ldapUsers = new LdapUsers();

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
                while ((message = queue.GetResponse()) != null)
                {
                    if (message is LdapSearchResult)
                    {
                        LdapEntry entry = (message as LdapSearchResult).Entry;

                        // Get the attribute set of the entry
                        LdapAttributeSet attributeSet = entry.GetAttributeSet();
                        var ienum = attributeSet.GetEnumerator();

                        // Parse through the attribute set to get the attributes and the corresponding values

                        LdapUser user = new LdapUser();

                        while (ienum.MoveNext())
                        {
                            user.DisplayName = entry.GetAttributeAsString("cn");
                            user.Email = entry.GetAttributeAsString("mail") ?? "-";
                            user.UserName = entry.GetAttributeAsString("sAMAccountName");
                            user.Title = entry.GetAttributeAsString("description");
                            user.Avatar = entry.GetAttributeAsBase64("thumbnailPhoto");
                        }

                        ldapUsersList.Add(user);
                    }
                }

                ldapUsers.ListOfLdapUsers = ldapUsersList;
                ldapUsers.OccurrencesNumber = ldapUsersList.Count;
            }
            catch (LdapException ex)
            {
                Logger.LogError(ex, "Can't retrieve users from Active Directory");
            }

            return ldapUsers;
        }

        /// <summary>
        /// Validates the credential of a user.
        /// </summary>
        /// <param name="username">The identifier of the user on the external authenticator.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));
            }

            string domainName = Options.DomainName;
            string userDn = $"{username}@{domainName}";
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(Options.Server, Options.Port);
                    connection.Bind(userDn, password);
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
