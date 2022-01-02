using Microsoft.AspNetCore.Authorization;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Supercharges the <see cref="AuthorizeAttribute"/> to simplify roles usage.
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeRolesAttribute"/> class.
        /// </summary>
        /// <param name="roles">The list of authorized roles.</param>
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}
