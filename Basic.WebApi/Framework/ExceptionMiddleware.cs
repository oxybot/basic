using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Framework
{
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
        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline.
                await this.next(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;

                var result = new BadRequestObjectResult(ex.ModelState);
                await result.ExecuteResultAsync(new ActionContext(context, null, null));
            }
            catch(NotFoundException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;
            }
            catch(UnauthorizedRequestException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Uncatched exception");
                context.Response.Clear();
                context.Response.StatusCode = 500;
            }
        }
    }
}
