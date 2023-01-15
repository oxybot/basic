// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Basic.WebApi.Framework;

/// <summary>
/// Extension methods for <see cref="OperationFilterContext"/> instances.
/// </summary>
internal static class OperationFilterContextExtensions
{
    /// <summary>
    /// Extracts the attributes from both the action and its controller.
    /// </summary>
    /// <typeparam name="T">The type of attribute to extract.</typeparam>
    /// <param name="context">The reference context.</param>
    /// <returns>The attributes.</returns>
    /// <seealso href="https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/blob/master/src/Swashbuckle.AspNetCore.Filters/Extensions/OperationFilterContextExtensions.cs"/>
    public static IEnumerable<T> GetControllerAndActionAttributes<T>(this OperationFilterContext context)
        where T : Attribute
    {
        var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes<T>();
        var actionAttributes = context.MethodInfo.GetCustomAttributes<T>();

        var result = new List<T>(controllerAttributes);
        result.AddRange(actionAttributes);
        return result;
    }
}
