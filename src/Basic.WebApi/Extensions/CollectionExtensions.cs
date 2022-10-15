﻿namespace System.Collections.Generic
{
    /// <summary>
    /// Extensions methods for the <see cref="ICollection{T}"/> class.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Add a range of value to a specific collection.
        /// </summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="reference">The collection to modify.</param>
        /// <param name="source">The items to add to the <paramref name="reference"/> collection, if any.</param>
        public static void AddRange<T>(this ICollection<T> reference, IEnumerable<T> source)
        {
            if (reference is null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (source is null)
            {
                // Nothing to do
                return;
            }

            foreach (T item in source)
            {
                reference.Add(item);
            }
        }
    }
}