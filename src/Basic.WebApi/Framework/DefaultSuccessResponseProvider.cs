// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Adds a default documentation for the success of any API.
    /// </summary>
    /// <remarks>
    /// This provider is required as the default behavior is implemented only when no [ProducesXXX]
    /// is present on the API method.
    /// </remarks>
    public class DefaultSuccessResponseProvider : IApiDescriptionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSuccessResponseProvider"/> class.
        /// </summary>
        /// <param name="metadata">The metadata provider.</param>
        public DefaultSuccessResponseProvider(IModelMetadataProvider metadata)
        {
            this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        /// <summary>
        /// Gets the order of execution of this provider.
        /// </summary>
        /// <remarks>
        /// Set to act before any other providers. As a result, it will be able to correct the final result.
        /// </remarks>
        public int Order => -11000;

        /// <summary>
        /// Gets the metadata provider associated with the application.
        /// </summary>
        public IModelMetadataProvider Metadata { get; }

        /// <summary>
        /// Called to prepare the context. Does nothing.
        /// </summary>
        /// <param name="context">The current api description generation context.</param>
        public void OnProvidersExecuting(ApiDescriptionProviderContext context)
        {
        }

        /// <summary>
        /// Called to review the context.
        /// </summary>
        /// <param name="context">The current api description generation context.</param>
        public void OnProvidersExecuted(ApiDescriptionProviderContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var result in context.Results)
            {
                var successResponse = result.SupportedResponseTypes.SingleOrDefault(s => s.StatusCode == 200);
                var actionDescriptor = result.ActionDescriptor as ControllerActionDescriptor;
                var returnType = actionDescriptor.MethodInfo.ReturnType;
                var modelMetadata = this.Metadata.GetMetadataForType(returnType);
                if (successResponse == null)
                {
                    result.SupportedResponseTypes.Add(new ApiResponseType() { StatusCode = 200, ModelMetadata = modelMetadata, Type = returnType });
                }
                else if (successResponse.Type == null)
                {
                    successResponse.Type = returnType;
                    successResponse.ModelMetadata = modelMetadata;
                }
            }
        }
    }
}
