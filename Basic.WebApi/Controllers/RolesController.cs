using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve roles data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public RolesController(Context context, IMapper mapper, ILogger<RolesController> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the datasource context.
        /// </summary>
        protected Context Context { get; }

        /// <summary>
        /// Gets the configured automapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<RoleForList> GetAll()
        {
            return Context.Set<Role>()
                .ToList()
                .Select(e => Mapper.Map<RoleForList>(e));
        }

        /// <summary>
        /// Retrieves the roles of the connected user.
        /// </summary>
        /// <returns>The list of roles assigned to the connected user.</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("mine")]
        public IEnumerable<RoleForList> GetMine()
        {
            var userIdClaim = this.User.Claims.SingleOrDefault(c => c.Type == "sid:guid");
            var userId = Guid.Parse(userIdClaim.Value);

            return Context.Set<User>()
                .Include(u => u.Roles)
                .Single(u => u.Identifier == userId)
                .Roles
                .ToList()
                .Select(r => Mapper.Map<RoleForList>(r));
        }
    }
}
