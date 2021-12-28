using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage user data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseModelController<User, UserForList, UserForView, UserForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UsersController(Context context, IMapper mapper, ILogger<UsersController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>The list of users.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<UserForList> GetAll()
        {
            return AddIncludesForList(Context.Set<User>())
                .ToList()
                .Select(e => Mapper.Map<UserForList>(e));
        }

        /// <summary>
        /// Retrieves a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user.</param>
        /// <returns>The detailed data about the user identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
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
        [Produces("application/json")]
        [Route("{identifier}")]
        public override UserForList Put(Guid identifier, UserForEdit user)
        {
            return base.Put(identifier, user);
        }

        /// <summary>
        /// Deletes a specific user.
        /// </summary>
        /// <param name="identifier">The identifier of the user to delete.</param>
        /// <response code="404">No user is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }
    }
}
