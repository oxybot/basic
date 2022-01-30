using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Applies the security requirements on the api operations.
    /// </summary>
    public class RoleRequirementsOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRequirementsOperationFilter"/> class.
        /// </summary>
        /// <param name="securitySchemaName">The name of the security schema.</param>
        public RoleRequirementsOperationFilter(string securitySchemaName = "oauth2")
        {
            SecuritySchemaName = securitySchemaName ?? throw new ArgumentNullException(nameof(securitySchemaName));
        }

        /// <summary>
        /// Gets the name of the security schema.
        /// </summary>
        public string SecuritySchemaName { get; }

        /// <summary>
        /// Applies the filter on a specific operation.
        /// </summary>
        /// <param name="operation">The current operation.</param>
        /// <param name="context">The current context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var authorizeAttributes = context.GetControllerAndActionAttributes<AuthorizeAttribute>();
            var rolesAttributes = context.GetControllerAndActionAttributes<AuthorizeRolesAttribute>();

            if (!authorizeAttributes.Any() && !rolesAttributes.Any())
            {
                return;
            }

            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }

            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = SecuritySchemaName
                }
            };

            var policies = rolesAttributes
                .SelectMany(a => a.Roles.Split(",", StringSplitOptions.TrimEntries));

            var requirement = new OpenApiSecurityRequirement() { { key, policies.ToList() } };
            operation.Security.Add(requirement);
        }
    }
}
