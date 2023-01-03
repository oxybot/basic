// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Basic.WebApi.Framework
{
    public class DefaultSuccessResponseProvider : IApiDescriptionProvider
    {
        public DefaultSuccessResponseProvider(IModelMetadataProvider metadata)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public int Order => -11000;

        public IModelMetadataProvider Metadata { get; }

        public void OnProvidersExecuting(ApiDescriptionProviderContext context)
        {
        }

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
                var modelMetadata = Metadata.GetMetadataForType(returnType);
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
