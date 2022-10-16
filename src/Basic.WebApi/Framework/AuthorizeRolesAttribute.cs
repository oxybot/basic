// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Supercharges the <see cref="AuthorizeAttribute"/> to simplify roles usage.
    /// </summary>
    public sealed class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeRolesAttribute"/> class.
        /// </summary>
        /// <param name="roles">The list of authorized roles.</param>
        [SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "Surcharge of the default Roles property")]
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}
