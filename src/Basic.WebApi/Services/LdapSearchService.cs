using Basic.WebApi.DTOs;
using Novell.Directory.Ldap;


namespace Basic.WebApi.Services
{
    /// <summary>
    /// Global variables.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Global variable to know if a Ldap connection is on.
        /// </summary>
        public static bool isConnected;
    }

    /// <summary>
    /// Provides ldap search services.
    /// </summary>
    public class LdapSearchService
    {
        /// <summary>
        /// Ldap search service constructor.
        /// </summary>
        public LdapSearchService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Provides a generic connection to the Active Directory.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Establish a connection to the Active Directory.
        /// </summary>
        public LdapConnection LdapConnect()
        {
            var configuration = Configuration.GetRequiredSection("ActiveDirectory");

            var connection = new LdapConnection { SecureSocketLayer = false };

            string userDn = configuration.GetValue<string>("UserDN");
            connection.Connect(configuration.GetValue<string>("Server"), configuration.GetValue<int>("Port"));
            connection.Bind(userDn, configuration.GetValue<string>("Password"));

            Global.isConnected = true;
            return connection;
        }

        /// <summary>
        /// Disconnect from the Active Directory.
        /// </summary>
        public void LdapDisconnect(LdapConnection connection)
        {
            Global.isConnected = false;
            connection.Disconnect();
        }

        /// <summary>
        /// Keyword search for an user in the Active Directory.
        /// </summary>
        public LdapUsers LdapSearch(bool isConnected, string searchTerm)
        {
            List<LdapUser> ldapUsersList = new List<LdapUser>();
            LdapUsers ldapUsers = new LdapUsers();

            var connection = new LdapConnection();
            var configuration = Configuration.GetRequiredSection("ActiveDirectory");
            var ldapSearchConfiguration = Configuration.GetRequiredSection("LdapUserSearch");
            var searchBase = configuration.GetValue<string>("Base");

            try
            {
                do
                {
                    connection = LdapConnect();
                }
                while (!Global.isConnected);

                if (connection.Bound)
                {
                    // Count loops number to fill in the list of users to return:
                    var counter = 0;
                    // Get the occurrences number to display:
                    int occurrencesToDisplay = ldapSearchConfiguration.GetValue<int>("occurrencesToDisplay");

                    LdapSearchQueue queue = connection.Search(searchBase, LdapConnection.ScopeSub, $"(&(objectClass=person)(cn=*{searchTerm}*))",
                                            null, false, (LdapSearchQueue) null, (LdapSearchConstraints)null);
                    LdapMessage message;

                    while ((message = queue.GetResponse()) != null)
                    {
                        if (message is LdapSearchResult)
                        {
                            LdapEntry entry = (message as LdapSearchResult).Entry;
                            // Get the attribute set of the entry
                            LdapAttributeSet attributeSet = entry.GetAttributeSet();
                            System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                            // Parse through the attribute set to get the attributes and the corresponding values

                            LdapUser user = new LdapUser();

                            while (ienum.MoveNext())
                            {
                                // LdapAttribute attribute = (LdapAttribute)ienum.Current;
                                // String attributeName = attribute.Name;
                                // String attributeVal = attribute.StringValue;
                                // Console.WriteLine(attributeName + " value: " + attributeVal);

                                user.DisplayName = entry.GetAttributeAsString("sn") + " " + entry.GetAttributeAsString("givenName");
                                if(entry.GetAttributeAsString("mail") == null){user.Email = "-";}
                                else{user.Email = entry.GetAttributeAsString("mail");}
                                user.UserName = entry.GetAttributeAsString("sAMAccountName");
                                user.Title = entry.GetAttributeAsString("description");
                                user.Avatar = entry.GetAttributeAsBase64("thumbnailPhoto");
                            } 

                            if (counter < occurrencesToDisplay)
                            {
                                ldapUsersList.Add(user);
                            }
                            counter++;
                        }

                    } 
                    LdapDisconnect(connection);

                    ldapUsers.ListOfLdapUsers = ldapUsersList;
                    ldapUsers.OccurrencesNumber = counter;
                    
                    return ldapUsers;
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex);
            }
            return ldapUsers;
        }
    }
}