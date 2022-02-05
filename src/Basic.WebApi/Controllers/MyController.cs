using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides the information related to the current user.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MyController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public MyController(Context context, IMapper mapper, ILogger<MyController> logger)
            : base(context, mapper, logger)
        {
        }


        /// <summary>
        /// Retrieves all events associated to the connected user.
        /// </summary>
        /// <param name="limit">The maximum numbers of events to return.</param>
        /// <returns>The list of events associated to the connected user.</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("Events")]
        public IEnumerable<EventForList> GetEvents(int? limit)
        {
            var user = this.GetConnectedUser();
            IQueryable<Event> query = Context.Set<Event>()
                .Include(e => e.Category)
                .Include(e => e.Statuses).ThenInclude(s => s.Status)
                .Where(e => e.User == user)
                .OrderByDescending(e => e.StartDate);

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return query
                .ToList()
                .Select(e => Mapper.Map<EventForList>(e));
        }

        /// <summary>
        /// Retrieves the connected user data.
        /// </summary>
        /// <returns>The detailed data about the connected user.</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("User")]
        public UserForView GetUser()
        {
            var user = this.GetConnectedUser();
            return Mapper.Map<UserForView>(user);
        }

        /// <summary>
        /// Retrieves the roles of the connected user.
        /// </summary>
        /// <returns>The list of roles assigned to the connected user.</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("Roles")]
        public IEnumerable<RoleForList> GetRoles()
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

        /// <summary>
        /// Retrieves the time-off consumption per category for the connected user.
        /// </summary>
        /// <param name="consumptionService">The calculation service associated to consumption.</param>
        /// <returns>The time-off consumption for the connected user.</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("Consumption")]
        public IEnumerable<ConsumptionForList> GetMyConsumption([FromServices] ConsumptionService consumptionService)
        {
            var user = this.GetConnectedUser();
            return consumptionService.GetConsumptionForUser(user);
        }
    }
}
