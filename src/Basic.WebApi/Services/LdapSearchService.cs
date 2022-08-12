using Basic.WebApi.DTOs;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace Basic.WebApi.Services
{
    /// <summary>
    /// Provides ldap search services.
    /// </summary>
    public class LdapSearchService : IDisposable
    {
        /// <summary>
        /// Ldap search service constructor.
        /// </summary>
        /// <param name="options">The active directory configuration options.</param>
        /// <param name="logger">The logger associated to the class.</param>
        public LdapSearchService(IOptions<ActiveDirectoryOptions> options, ILogger<LdapSearchService> logger)
        {
            this.Options = options.Value;
            this.Logger = logger;
            this.Connection = new LdapConnection();
        }

        /// <summary>
        /// Provides a configuration for the connection to the Active Directory.
        /// </summary>
        public ActiveDirectoryOptions Options { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        public ILogger<LdapSearchService> Logger { get; }

        /// <summary>
        /// Provides a connection to the Active Directory.
        /// </summary>
        public LdapConnection Connection { get; }

        /// <summary>
        /// Establish a connection to the Active Directory.
        /// </summary>
        public void LdapConnect()
        {
            this.Connection.SecureSocketLayer = false;
            Connection.Connect(Options.Server, Options.Port);
            Connection.Bind(Options.UserDN, Options.Password);
        }

        /// <summary>
        /// Disconnect from the Active Directory.
        /// </summary>
        public void Dispose()
        {
            this.Connection.Disconnect();
            this.Connection.Dispose();
        }

        /// <summary>
        /// Keyword search for an user in the Active Directory.
        /// </summary>
        public LdapUsers LdapSearch(string searchTerm)
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
                            user.DisplayName = entry.GetAttributeAsString("givenName") + " " + entry.GetAttributeAsString("sn");
                            if (entry.GetAttributeAsString("mail") == null) { user.Email = "-"; }
                            else { user.Email = entry.GetAttributeAsString("mail"); }
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
    }
}