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

        public IConfiguration Configuration { get; }

        public List<Object> LdapSearch(string searchTerm) // IEnumerable<UserForList>
        {
            var ldapUser = new List<Object>();
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
                        LdapSearchQueue queue = connection.Search(searchBase, LdapConnection.ScopeSub, $"(cn=*{searchTerm}*)", null, false, (LdapSearchQueue)
                                                null, (LdapSearchConstraints)null);
                        LdapMessage message;
                        while ((message = queue.GetResponse()) != null)
                        {
                            if (message is LdapSearchResult)
                            {
                                LdapEntry entry = (message as LdapSearchResult).Entry;
                                System.Console.Out.WriteLine("\n" + entry.Dn);
                                System.Console.Out.WriteLine("\tAttributes: ");

                                // Get the attribute set of the entry
                                LdapAttributeSet attributeSet = entry.GetAttributeSet();
                                System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();


                                // Parse through the attribute set to get the attributes and the corresponding values
                                while (ienum.MoveNext())
                                {
                                    LdapAttribute attribute = (LdapAttribute)ienum.Current;
                                    String attributeName = attribute.Name;
                                    String attributeVal = attribute.StringValue;
                                    Console.WriteLine(attributeName + " value: " + attributeVal);

                                    switch (attribute.Name)
                                    {
                                        case "sn":
                                        case "givenName":
                                        case "mail":
                                        case "sAMAccountName":
                                            // case "thumbnailPhoto":
                                            ldapUser.Add(attribute.StringValue);
                                            break;
                                    }
                                }
                            }
                        }
                        connection.Disconnect();
                        return ldapUser;
                    }

                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex);
            }
            return ldapUser;
        }
    }
}