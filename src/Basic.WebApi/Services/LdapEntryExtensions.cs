namespace Novell.Directory.Ldap
{
    public static class LdapEntryExtensions
    {
        public static string GetAttributeAsString(this LdapEntry entry, string attrName)
        {
            LdapAttribute attribute;
            try
            {
                attribute = entry.GetAttribute(attrName);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

            if (attribute.StringValueArray.Length == 0)
            {
                return null;
            }
            else
            {
                return attribute.StringValue;
            }
        }

        public static string GetAttributeAsBase64(this LdapEntry entry, string attrName)
        {
            LdapAttribute attribute;
            try
            {
                attribute = entry.GetAttribute(attrName);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

            if (attribute.StringValueArray.Length == 0)
            {
                return null;
            }
            else
            {
                return Convert.ToBase64String(attribute.ByteValue);
            }
        }
    }
}
