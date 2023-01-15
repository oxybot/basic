// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace System.Reflection;

/// <summary>
/// Extension methods for the <see cref="Assembly"/> class.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Read the content of a specific text file.
    /// </summary>
    /// <param name="assembly">The reference assembly.</param>
    /// <param name="name">The name of the file.</param>
    /// <returns>The extracted content of the file.</returns>
    public static string ReadResource(this Assembly assembly, string name)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        using (Stream stream = assembly.GetManifestResourceStream(name))
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}
