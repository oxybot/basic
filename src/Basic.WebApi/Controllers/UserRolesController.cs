// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage the roles of a user.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("Users/{userId}/Roles")]
    public class UserRolesController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRolesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public UserRolesController(Context context, IMapper mapper, ILogger<UserAttachmentsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all roles for a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>The list of associated roles.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="userId"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        public IEnumerable<EntityReference> GetAll([FromRoute] Guid userId)
        {
            var user = this.Context.Set<User>()
                .Include(a => a.Roles)
                .SingleOrDefault(a => a.Identifier == userId);
            if (user == null)
            {
                throw new NotFoundException("Unknown entity");
            }

            return user.Roles.Select(r => this.Mapper.Map<EntityReference>(r));
        }

        /// <summary>
        /// Updates the roles associated to a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="roles">The codes of the roles assigned to the user.</param>
        /// <returns>The updated list of associated roles.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="userId"/>.</response>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPut]
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        public IEnumerable<EntityReference> Put([FromRoute] Guid userId, IEnumerable<string> roles)
        {
            var user = this.Context.Set<User>()
                .Include(a => a.Roles)
                .SingleOrDefault(a => a.Identifier == userId);
            if (user == null)
            {
                throw new NotFoundException("Unknown entity");
            }

            user.Roles.Clear();
            if (roles != null)
            {
                foreach (var code in roles)
                {
                    var role = this.Context.Set<Role>().SingleOrDefault(r => r.Code == code);
                    if (role == null)
                    {
                        this.ModelState.AddModelError(string.Empty, $"Invalid role {role}");
                        throw new InvalidModelStateException(this.ModelState);
                    }

                    user.Roles.Add(role);
                }
            }

            this.Context.SaveChanges();

            return user.Roles.Select(r => this.Mapper.Map<EntityReference>(r));
        }
    }
}
