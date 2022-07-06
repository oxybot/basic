using Basic.Model;
using Basic.WebApi.DTOs;
using Novell.Directory.Ldap;
using System.Text.RegularExpressions;

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
        public List<LdapUser> LdapSearch(string searchTerm) // IEnumerable<UserForList>
        {
            List<LdapUser> ldapUsersList = new List<LdapUser>();
            // Object[] ldapUser = {};
            var configuration = Configuration.GetRequiredSection("ActiveDirectory");
            var searchBase = configuration.GetValue<string>("Base");
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    string userDn = configuration.GetValue<string>("UserDN");
                    connection.Connect(configuration.GetValue<string>("Server"), configuration.GetValue<int>("Port"));
                    connection.Bind(userDn, configuration.GetValue<string>("Password")); // credentials à definir
                    if (connection.Bound)
                    {
                        // count loops number to fill in the list of users to return
                        var counter = 0;
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
                                if (counter < 4)
                                {
                                    ldapUsersList.Add(user);
                                }
                                counter++;
                            }
                        }
                        connection.Disconnect();
                        Console.WriteLine("we found " + counter + " occurrences");
                        return ldapUsersList;
                    }
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex);
            }
            return ldapUsersList;
        }
    }
}