namespace Microsoft.AspNetCore.Mvc.ModelBinding.Metadata
{
    public static class DefaultModelMetadataExtensions
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this DefaultModelMetadata metadata)
            where TAttribute: Attribute
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
