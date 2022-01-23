using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Raises a <c>400</c> http error code.
    /// </summary>
    public class BadRequestException : BadHttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        public BadRequestException(ModelStateDictionary modelState)
            : base("Bad Request", 400)
        {
            ModelState = Convert(modelState) ?? throw new ArgumentNullException(nameof(modelState));
        }

        /// <summary>
        /// Gets the model state used as a reference for the Json payload.
        /// </summary>
        public ModelStateDictionary ModelState { get; }

        /// <summary>
        /// Converts a model state from c# property names to javascript naming convention.
        /// </summary>
        /// <param name="current">The current model state.</param>
        /// <returns>The updated model state.</returns>
        public static ModelStateDictionary Convert(ModelStateDictionary current)
        {
            if (current == null)
            {
                return null;
            }

            var result = new ModelStateDictionary();
            foreach (var pair in current)
            {
                foreach (var error in pair.Value.Errors)
                {
                    result.AddModelError(pair.Key.ToJsonFieldName(), error.ErrorMessage);
                }
            }

            return result;
        }
    }
}
