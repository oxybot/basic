using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
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
        /// Retrieves a specific event associated to the connected user.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>The detailed data about the event identified by <paramref name="eventId"/>.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("Events/{eventId}")]
        public EventForView GetEvent(Guid eventId)
        {
            var user = this.GetConnectedUser();
            Event entity = Context.Set<Event>()
                .Include(e => e.Category)
                .Include(e => e.Statuses).ThenInclude(s => s.Status)
                .SingleOrDefault(e => e.Identifier == eventId && e.User == user);

            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return Mapper.Map<EventForView>(entity);
        }


        /// <summary>
        /// Retrieves the statuses associated to a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>The statuses of the event.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("Events/{eventId}/Statuses")]
        public IEnumerable<ModelStatusForList> GetEventStatuses(Guid eventId)
        {
            var user = this.GetConnectedUser();
            var entity = Context.Set<Event>()
                .Include(e => e.Statuses).ThenInclude(s => s.Status)
                .Include(e => e.User)
                .SingleOrDefault(e => e.Identifier == eventId && e.User == user);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return entity.Statuses
                .OrderByDescending(s => s.UpdatedOn)
                .Select(s => Mapper.Map<ModelStatusForList>(s));
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
        /// Updates the connected user data.
        /// </summary>
        /// <param name="user">The user data.</param>
        /// <returns>The user data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("User")]
        public UserForView UpdateUser(UserForEdit user)
        {
            var model = this.GetConnectedUser();
            Mapper.Map(user, model);
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
            }

            Context.SaveChanges();

            return Mapper.Map<UserForView>(model);
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

        /// <summary>
        /// Updates the connected user password.
        /// </summary>
        /// <param name="user">The current user.</param>
        /// <param name="password">The password data.</param>
        /// <returns>The user data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No user is associated to the provided <paramref name="user"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("password")]
        public UserForList UpdateMyPassword(PasswordForEdit password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var model = this.GetConnectedUser();

            Context.SaveChanges();

            return Mapper.Map<UserForList>(model);

            // return Mapper.Map<UserForList>(user);
        }
        // FAIRE UN MIX DES DEUX
        // public UserForView UpdateUser(UserForEdit user)
        // {
        //     var model = this.GetConnectedUser();
        //     Mapper.Map(user, model);
        //     if (!ModelState.IsValid)
        //     {
        //         throw new InvalidModelStateException(ModelState);
        //     }

        //     Context.SaveChanges();

        //     return Mapper.Map<UserForView>(model);
        // }

        // public UserForList UpdatePassword(Guid identifier, PasswordForEdit password)
        // {
        //     if (password is null)
        //     {
        //         throw new ArgumentNullException(nameof(password));
        //     }

        //     var user = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == identifier);
        //     if (user == null)
        //     {
        //         throw new NotFoundException("No user identified by " + identifier);
        //     }

        //     user.ChangePassword(password.NewPassword);
        //     this.Context.SaveChanges();

        //     return Mapper.Map<UserForList>(user);
        // }

    }
}

// default :
// UserForEdit user
// PasswordForEdit password'