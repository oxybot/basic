// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Framework;

/// <summary>
/// Defines a http pipeline middleware that catches all exceptions.
/// </summary>
public class ExceptionMiddleware
{
    /// <summary>
    /// The delegate to the next middleware.
    /// </summary>
    private readonly RequestDelegate next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate to the next middleware.</param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Called when the middleware is invoked.
    /// </summary>
    /// <param name="context">The current http context.</param>
    /// <param name="logger">The associated logger.</param>
    /// <returns>A task associated to the current action.</returns>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Intended behavior - final catch of any exceptions")]
    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        else if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        try
        {
            // Call the next delegate/middleware in the pipeline.
            await this.next(context).ConfigureAwait(false);
        }
        catch (InvalidModelStateException ex)
        {
            // 400
            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(InvalidModelStateActionResult.Convert(ex.ModelState)).ConfigureAwait(false);
        }
        catch (UnauthorizedRequestException ex)
        {
            // 401
            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
        }
        catch (ForbiddenRequestException ex)
        {
            // 403
            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
        }
        catch (NotFoundException ex)
        {
            // 404
            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
        }
        catch (Exception ex)
        {
            // 500
            logger.LogCritical(ex, "Uncatched exception");
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
