using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// <returns>The list of users.</returns>
        [HttpGet]
        [AllowAnonymous]
        // [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        public IEnumerable<UserForList> GetAll(string filter, string sortKey, int sortValue)
        {
            var entities = AddIncludesForList(Context.Set<User>())
                .ToList()
                .Select(e => Mapper.Map<UserForList>(e));

            switch(sortKey)
            {
                case "UserName":
                    if(sortValue == 1)
                    {
                        entities = entities.OrderBy(o => o.UserName);
                    }
                    else if (sortValue == -1)
                    {
                        entities = entities.OrderBy(o => o.UserName).Reverse();
                    }
                    break;
            }
                
            return entities;
        }

        /// <summary>
        /// Retrieves a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user.</param>
        /// <returns>The detailed data about the user identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AllowAnonymous]
        // [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override UserForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }


        /// <summary>
        /// Retrieves a specific user.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="searchTerm">The identifier of the user.</param>
        /// <returns>The detailed data about the user identified by <paramref name="searchTerm"/>.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="searchTerm"/>.</response>
        [HttpGet]
        [AllowAnonymous]
        // [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("ldap")]
        public LdapUsers GetLdapUser([FromServices]LdapSearchService service, string searchTerm)
        {
            LdapUsers ldapUsers = service.LdapSearch(searchTerm);
            var usersFromDb = Context.Set<User>();

            ldapUsers.ListOfLdapUsers.ToList().ForEach(d => d.Importable =! usersFromDb.Any(sd => sd.Email.ToLower() == d.Email.ToLower()));
            
            if(ldapUsers.ListOfLdapUsers.Count > 0){string imageString = ldapUsers.ListOfLdapUsers[0].Avatar;}

            return ldapUsers;
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

            return Mapper.Map<UserForList>(user);
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
    }
}
