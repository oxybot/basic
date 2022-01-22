using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Provides humanized display name for the properties of the model entities.
    /// </summary>
    public class HumanizerMetadataProvider : IDisplayMetadataProvider
    {
        /// <summary>
        /// Called when creating a display metadata.
        /// </summary>
        /// <param name="context">The current context.</param>
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

        /// <summary>
        /// Checks is the current model element can be converted.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="modelMetadata">The associated model metadata.</param>
        /// <param name="propertyAttributes">The attributes of the property.</param>
        /// <returns></returns>
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
