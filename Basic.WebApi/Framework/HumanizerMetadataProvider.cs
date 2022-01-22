using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Framework
{
    public class HumanizerMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var propertyAttributes = context.Attributes;
            var modelMetadata = context.DisplayMetadata;
            var propertyName = context.Key.Name;

            if (IsTransformRequired(propertyName, modelMetadata, propertyAttributes))
            {
                modelMetadata.DisplayName = () => propertyName.Humanize().Transform(To.TitleCase);
            }
        }

        private static bool IsTransformRequired(string propertyName, DisplayMetadata modelMetadata, IReadOnlyList<object> propertyAttributes)
        {
            if (!string.IsNullOrEmpty(modelMetadata.SimpleDisplayProperty))
                return false;

            if (propertyAttributes.OfType<DisplayNameAttribute>().Any())
                return false;

            if (propertyAttributes.OfType<DisplayAttribute>().Any(d => d.Name != null))
                return false;

            if (string.IsNullOrEmpty(propertyName))
                return false;

            return true;
        }
    }
}
