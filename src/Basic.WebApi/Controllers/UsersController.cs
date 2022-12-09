// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage user data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : BaseModelController<User, UserForList, UserForView, UserForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public UsersController(Context context, IMapper mapper, ILogger<UsersController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <param name="definitions">The service providing the entity definitions.</param>
        /// <param name="sortAndFilter">The sort and filter options, is any.</param>
        /// <returns>The list of users.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        public IEnumerable<UserForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
        {
            var entities = this.GetAllCore(definitions, sortAndFilter)
                .ToList()
                .Select(e => this.Mapper.Map<UserForList>(e));

            return entities;
        }

        /// <summary>
        /// Retrieves a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user.</param>
        /// <returns>The detailed data about the user identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override UserForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user data.</param>
        /// <returns>The user data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        public override UserForList Post(UserForEdit user)
        {
            return base.Post(user);
        }

        /// <summary>
        /// Updates a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user to update.</param>
        /// <param name="user">The user data.</param>
        /// <returns>The user data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override UserForList Put(Guid identifier, UserForEdit user)
        {
            return base.Put(identifier, user);
        }

        /// <summary>
        /// Updates the password of a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user to update.</param>
        /// <param name="password">The password data.</param>
        /// <returns>The user data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}/password")]
        public UserForList UpdatePassword(Guid identifier, PasswordForEdit password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == identifier);
            if (user == null)
            {
                throw new NotFoundException("No user identified by " + identifier);
            }

            user.ChangePassword(password.NewPassword);
            this.Context.SaveChanges();

            return this.Mapper.Map<UserForList>(user);
        }

        /// <summary>
        /// Deletes a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user to delete.</param>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Supports the import process from the external authenticator by providing the top results of a specific search.
        /// </summary>
        /// <param name="service">The external authenticator service.</param>
        /// <param name="searchTerm">The search criteria.</param>
        /// <returns>The top results associated with <paramref name="searchTerm"/>.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("import")]
        public LdapUsers Import([FromServices] ExternalAuthenticatorService service, string searchTerm)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            else if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException($"'{nameof(searchTerm)}' cannot be null or empty.", nameof(searchTerm));
            }

            var ldapUsers = service.Search(searchTerm);
            var usersFromDb = this.Context.Set<User>();

#pragma warning disable CA1304 // Specify CultureInfo - disabled to enable SQL conversion (ToUpperInvariant is not recognized)
            ldapUsers.ListOfLdapUsers.ToList()
                .ForEach(d => d.Importable = !usersFromDb.Any(sd => sd.Email.ToUpper() == d.Email.ToUpper()));
#pragma warning restore CA1304 // Specify CultureInfo

            return ldapUsers;
        }

        /// <summary>
        /// Retrieves the basic information about the linked entities.
        /// </summary>
        /// <param name="identifier">The identifier of the user.</param>
        /// <returns>The linked entities information.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        [Route("{identifier}/links")]
        public AttachmentLinks GetLinks(Guid identifier)
        {
            var entity = this.Context
                .Set<User>()
                .Include(c => c.Attachments)
                .SingleOrDefault(c => c.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return new AttachmentLinks()
            {
                Attachments = entity.Attachments.Count,
            };
        }
    }
}
