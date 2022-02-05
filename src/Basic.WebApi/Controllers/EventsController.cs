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
    /// Provides API to retrieve and manage events data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EventsController
        : BaseImmutableModelController<Event, EventForList, EventForView, EventForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public EventsController(Context context, IMapper mapper, ILogger<EventsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>The list of events.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        public IEnumerable<EventForList> GetAll()
        {
            var entities = AddIncludesForList(Context.Set<Event>());

            return entities
                .ToList()
                .Select(e => Mapper.Map<EventForList>(e));
        }

        /// <summary>
        /// Retrieves all events associated to the connected user.
        /// </summary>
        /// <param name="limit">The maximum numbers of events to return.</param>
        /// <returns>The list of events associated to the connected user.</returns>
        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        [Route("mine")]
        public IEnumerable<EventForList> GetMine(int? limit)
        {
            var entities = AddIncludesForList(Context.Set<Event>());
            var user = this.GetConnectedUser();

            IQueryable<Event> query = entities
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
        /// Retrieves a specific event.
        /// </summary>
        /// <param name="identifier">The identifier of the event.</param>
        /// <returns>The detailed data about the event identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override EventForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="event">The event data.</param>
        /// <returns>The event data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override EventForList Post(EventForEdit @event)
        {
            return base.Post(@event);
        }

        /// <summary>
        /// Deletes a specific agreement.
        /// </summary>
        /// <param name="identifier">The identifier of the agreement to delete.</param>
        /// <response code="404">No event is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Checks and maps <see cref="Event.User"/> and <see cref="Event.Category"/> info.
        /// </summary>
        /// <param name="event">The event data.</param>
        /// <param name="model">The event model instance.</param>
        protected override void CheckDependencies(EventForEdit @event, Event model)
        {
            model.User = Context.Set<User>().SingleOrDefault(u => u.Identifier == @event.UserIdentifier);
            if (model.User == null)
            {
                ModelState.AddModelError("UserIdentifier", "Invalid User");
            }

            model.Category = Context.Set<EventCategory>().SingleOrDefault(c => c.Identifier == @event.CategoryIdentifier);
            if (model.Category == null)
            {
                ModelState.AddModelError("CategoryIdentifier", "Invalid Category");
            }

            // Add the default status for a new event
            if (model.Identifier == Guid.Empty)
            {
                User user = this.GetConnectedUser();
                var requested = this.Context.GetStatus("requested");
                model.Statuses.Add(new EventStatus()
                {
                    Status = requested,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = user
                });
            }
        }

        /// <summary>
        /// Adds the <see cref="Event.User"/> and <see cref="Event.Category"/> values.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Event> AddIncludesForList(IQueryable<Event> query)
        {
            return query
                .Include(c => c.User)
                .Include(c => c.Category)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.Status);
        }

        /// <summary>
        /// Adds the <see cref="Event.User"/> and <see cref="Event.Category"/> values.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Event> AddIncludesForView(IQueryable<Event> query)
        {
            return query
                .Include(c => c.User)
                .Include(c => c.Category)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.Status);
        }
    }
}
