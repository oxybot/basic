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
    public class RolesController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public RolesController(Context context, IMapper mapper, ILogger<RolesController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<RoleForList> GetAll()
        {
            return this.Context.Set<Role>()
                .ToList()
                .Select(e => this.Mapper.Map<RoleForList>(e));
        }
    }
}
