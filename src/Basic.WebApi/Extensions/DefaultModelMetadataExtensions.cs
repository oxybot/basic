namespace Microsoft.AspNetCore.Mvc.ModelBinding.Metadata
{
    /// <summary>
    /// Extension methods for the <see cref="DefaultModelMetadata"/> class.
    /// </summary>
    public static class DefaultModelMetadataExtensions
    {
        /// <summary>
        /// Retrieves the first attribute of a specific type on the underlying element, if any.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <param name="metadata">The reference element.</param>
        /// <returns>The attribute, if any.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this DefaultModelMetadata metadata)
            where TAttribute : Attribute
        {
            if (metadata is null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            object attribute = metadata.Attributes.Attributes.SingleOrDefault(o => o is TAttribute);
            return (TAttribute)attribute;
        }
    }
}
