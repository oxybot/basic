using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Represents a <see cref="IActionResult"/> linked to model state in error.
    /// </summary>
    public class InvalidModelStateActionResult : IActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelStateActionResult"/> class.
        /// </summary>
        /// <param name="modelState">The associated model state.</param>
        public InvalidModelStateActionResult(ModelStateDictionary modelState)
        {
            ModelState = modelState ?? throw new ArgumentNullException(nameof(modelState));
        }

        /// <summary>
        /// Gets the associated model state.
        /// </summary>
        public ModelStateDictionary ModelState { get; }

        /// <summary>
        /// Updates the response with the errors of the model state.
        /// </summary>
        /// <param name="context">The reference action context.</param>
        public void ExecuteResult(ActionContext context)
        {
            this.ExecuteResultAsync(context).Wait();
        }

        /// <summary>
        /// Updates the response with the errors of the model state.
        /// </summary>
        /// <param name="context">The reference action context.</param>
        /// <returns>The task associated to the current action.</returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var bodyResult = Convert(ModelState);
            await context.HttpContext.Response.WriteAsJsonAsync(bodyResult);
        }

        /// <summary>
        /// Converts a model state into an invalid result.
        /// </summary>
        /// <param name="modelState">The reference model state.</param>
        /// <returns>The converted model state.</returns>
        public static InvalidResult Convert(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                return null;
            }

            var result = new InvalidResult();
            foreach (var pair in modelState)
            {
                if (pair.Value.Errors != null && pair.Value.Errors.Count > 0)
                {
                    var errors = pair.Value.Errors.Select(e => e.ErrorMessage).ToArray();
                    var key = string.Join('.', pair.Key.Split('.').Select(s => s.ToJsonFieldName()));
                    result.Add(key, errors);
                }
            }

            return result;
        }
    }
}
