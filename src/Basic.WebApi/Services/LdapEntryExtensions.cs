// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Novell.Directory.Ldap;

/// <summary>
/// Defines extension methods for the <see cref="LdapEntry"/> class.
/// </summary>
public static class LdapEntryExtensions
{
    /// <summary>
    /// Retrieves an attribute a <see cref="string"/>, managing <c>null</c> cases.
    /// </summary>
    /// <param name="entry">The reference.</param>
    /// <param name="attrName">The name of the attribute to retrieve.</param>
    /// <returns>The value of the attribute identified by <paramref name="attrName"/>.</returns>
    public static string GetAttributeAsString(this LdapEntry entry, string attrName)
    {
        if (entry is null)
        {
            throw new ArgumentNullException(nameof(entry));
        }
        else if (attrName is null)
        {
            throw new ArgumentNullException(nameof(attrName));
        }

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

    /// <summary>
    /// Retrieves an attribute a base 64 <see cref="string"/>, managing <c>null</c> cases.
    /// </summary>
    /// <param name="entry">The reference.</param>
    /// <param name="attrName">The name of the attribute to retrieve.</param>
    /// <returns>The value of the attribute identified by <paramref name="attrName"/>.</returns>
    public static string GetAttributeAsBase64(this LdapEntry entry, string attrName)
    {
        if (entry is null)
        {
            throw new ArgumentNullException(nameof(entry));
        }
        else if (string.IsNullOrEmpty(attrName))
        {
            throw new ArgumentException($"'{nameof(attrName)}' cannot be null or empty.", nameof(attrName));
        }

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
