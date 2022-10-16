// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.Framework;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> instances
    /// to register the <see cref="ExceptionMiddleware"/> middleware.
    /// </summary>
    public static class ExceptionMiddlewareApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="ExceptionMiddleware"/> in the current pipeline.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
