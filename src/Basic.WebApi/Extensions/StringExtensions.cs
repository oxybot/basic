// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace System;

/// <summary>
/// Defines extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a field name to its json representation.
    /// </summary>
    /// <param name="value">The field name to be converted.</param>
    /// <returns>The field name as part of a json payload.</returns>
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Expected behaviour")]
    public static string ToJsonFieldName(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        else if (value.Length == 1)
        {
            return value.ToLowerInvariant();
        }

        string start = value[..1];
        string remaining = value[1..];

        return start.ToLowerInvariant() + remaining;
    }
}
