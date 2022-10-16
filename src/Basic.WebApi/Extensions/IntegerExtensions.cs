// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="int"/> instances.
    /// </summary>
    internal static class IntegerExtensions
    {
        /// <summary>
        /// Determines if a specific value is even.
        /// </summary>
        /// <param name="reference">The reference value.</param>
        /// <returns><c>true</c> is the <paramref name="reference"/> is even; otherwise <c>false</c>.</returns>
        public static bool IsEven(this int reference)
        {
            return reference % 2 == 0;
        }
    }
}
