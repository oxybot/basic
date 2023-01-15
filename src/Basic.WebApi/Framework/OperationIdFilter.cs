// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basic.WebApi.Framework;

/// <summary>
/// Defines <c>operationId</c> on all operations.
/// </summary>
public class OperationIdFilter : IOperationFilter
{
    /// <summary>
    /// Defines <c>operationId</c> on a specific operation.
    /// </summary>
    /// <param name="operation">The operation to update.</param>
    /// <param name="context">The current generation context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation is null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!string.IsNullOrEmpty(operation.OperationId))
        {
            // Already defined
            return;
        }

        if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor action)
        {
            operation.OperationId = $"{action.ControllerName}.{action.ActionName}";
        }
    }
}
