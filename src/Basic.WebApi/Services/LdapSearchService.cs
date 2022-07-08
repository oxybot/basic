using Basic.WebApi.DTOs;
using Novell.Directory.Ldap;

namespace Basic.WebApi.Services
{
    /// <summary>
    /// Provides consumption calculation services.
    /// </summary>
    public class LdapSearchService
    {
        public LdapSearchService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Provides a generic connection to the Active Directory.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Keyword search for an user in the Active Directory.
        /// </summary>
        public LdapUsers LdapSearch(string searchTerm)
        {
            List<LdapUser> ldapUsersList = new List<LdapUser>();
            LdapUsers ldapUsers = new LdapUsers();

            var configuration = Configuration.GetRequiredSection("ActiveDirectory");
            var ldapSearchConfiguration = Configuration.GetRequiredSection("LdapUserSearch");
            var searchBase = configuration.GetValue<string>("Base");

            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    string userDn = configuration.GetValue<string>("UserDN");
                    connection.Connect(configuration.GetValue<string>("Server"), configuration.GetValue<int>("Port"));
                    connection.Bind(userDn, configuration.GetValue<string>("Password")); // ICI a modifier : maintien de la connexion

                    if (connection.Bound)
                    {
                        // Count loops number to fill in the list of users to return:
                        var counter = 0;
                        // Get the number of occurrences to display:
                        int occurrencesToDisplay = ldapSearchConfiguration.GetValue<int>("occurrencesToDisplay");

                        LdapSearchQueue queue = connection.Search(searchBase, LdapConnection.ScopeSub, $"(&(objectClass=person)(cn=*{searchTerm}*))", null, false, (LdapSearchQueue)
                                                null, (LdapSearchConstraints)null);
                        LdapMessage message;

                        while ((message = queue.GetResponse()) != null)
                        {
                            if (message is LdapSearchResult)
                            {
                                LdapEntry entry = (message as LdapSearchResult).Entry;
                                Console.WriteLine(entry.Dn);
                                var entryDn = entry.Dn.ToLower();
                                System.Console.Out.WriteLine("\n" + entry.Dn);
                                System.Console.Out.WriteLine("\tAttributes: ");

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

                                    user.DisplayName = entry.GetAttributeAsString("cn");
                                    user.Email = entry.GetAttributeAsString("mail");
                                    user.Username = entry.GetAttributeAsString("sAMAccountName");
                                    // user.Title = entry.GetAttribute("ou").StringValue;
                                    user.Avatar = entry.GetAttributeAsBase64("thumbnailPhoto");
                                } 

                                if (counter < occurrencesToDisplay)
                                {
                                    ldapUsersList.Add(user);
                                }

                                counter++;
                            }

                        }
                        connection.Disconnect();
                        Console.WriteLine( counter + " occurrences");

                        ldapUsers.ListOfLdapUsers = ldapUsersList;
                        ldapUsers.OccurrencesNumber = counter;
                        
                        return ldapUsers;
                    }
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