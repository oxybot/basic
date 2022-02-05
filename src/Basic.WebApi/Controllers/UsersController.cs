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
        [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
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
        [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override UserForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Retrieves the connected user data.
        /// </summary>
        /// <returns>The detailed data about the connected user.</returns>
        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        [Route("me")]
        public UserForView GetMe()
        {
            var userIdClaim = this.User.Claims.SingleOrDefault(c => c.Type == "sid:guid");
            var userId = Guid.Parse(userIdClaim.Value);
            return base.GetOne(userId);
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
        [AuthorizeRoles(Role.User)]
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
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Retrieves the time-off consumption per category for a user.
        /// </summary>
        /// <returns>The time-off consumption for the connected user.</returns>
        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        [Route("me/consumption")]
        public IEnumerable<ConsumptionForList> GetMyConsumption()
        {
            var userIdClaim = this.User.Claims.SingleOrDefault(c => c.Type == "sid:guid");
            var userId = Guid.Parse(userIdClaim.Value);

            return GetConsumption(userId);
        }

        /// <summary>
        /// Retrieves the time-off consumption per category for a user.
        /// </summary>
        /// <param name="identifier">The identifier of the user.</param>
        /// <returns>The time-off consumption for the user identified by <paramref name="identifier"/>.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        [Route("{identifier}/consumption")]
        public IEnumerable<ConsumptionForList> GetConsumption(Guid identifier)
        {
            var user = Context.Set<User>()
                .SingleOrDefault(c => c.Identifier == identifier);
            if (user == null)
            {
                throw new NotFoundException("Not existing user");
            }

            var year = DateTime.Now.Year;
            var startOfYear = new DateOnly(DateTime.Now.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1).AddDays(-1);

            var balances = Context.Set<Balance>()
                .Include(b => b.Category)
                .Where(b => b.User == user && b.Year == startOfYear.Year).ToList();
            var groupedEvents = Context.Set<Event>()
                .Include(e => e.Category)
                .Include(e => e.Statuses)
                    .ThenInclude(s => s.Status)
                .Where(e => e.User == user && e.StartDate >= startOfYear && e.StartDate <= endOfYear && e.Category.Mapping == EventTimeMapping.TimeOff)
                .ToList()
                .Where(e => e.CurrentStatus.IsActive)
                .GroupBy(e => e.Category);

            foreach (var category in groupedEvents)
            {
                var balance = balances.SingleOrDefault(b => b.Category == category.Key);
                var planned = category.Where(e => e.CurrentStatus.Identifier == Status.Approved && e.StartDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Today).ToList();
                var taken = category.Where(e => e.CurrentStatus.Identifier == Status.Approved && e.StartDate.ToDateTime(TimeOnly.MinValue) < DateTime.Today).ToList();
                var requested = category.Where(e => e.CurrentStatus.Identifier == Status.Requested).ToList();
                yield return new ConsumptionForList()
                {
                    Category = Mapper.Map<EntityReference>(category.Key),
                    Allowed = balance?.Allowed,
                    Transfered = balance?.Transfered,
                    Planned = planned.Sum(e => e.DurationTotal),
                    Taken = taken.Sum(e => e.DurationTotal),
                    Requested = requested.Sum(e => e.DurationTotal),
                };
            }

            foreach (var balance in balances.Where(b => !groupedEvents.Any(c => c.Key == b.Category)))
            {
                yield return new ConsumptionForList()
                {
                    Category = Mapper.Map<EntityReference>(balance.Category),
                    Allowed = balance.Allowed,
                    Transfered = balance.Transfered,
                };
            }
        }
    }
}
