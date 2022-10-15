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
        public IEnumerable<EventForList> GetAll(string filter = "", string sortKey = "", string sortValue = "")
        {
            var entities = this.AddIncludesForList(this.Context.Set<Event>())
                .ToList()
                .Select(e => this.Mapper.Map<EventForList>(e))
                .Reverse();

            switch (sortKey)
            {
                case "User":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.User.DisplayName);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.User.DisplayName).Reverse();
                    }
                    break;

                case "Category":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.Category.DisplayName);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.Category.DisplayName).Reverse();
                    }
                    break;

                case "Start Date":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.StartDate);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.StartDate).Reverse();
                    }
                    break;

                case "End Date":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.EndDate);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.EndDate).Reverse();
                    }
                    break;

                case "Duration Total":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.DurationTotal);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.DurationTotal).Reverse();
                    }
                    break;

                case "Current Status":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.CurrentStatus.DisplayName);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.CurrentStatus.DisplayName).Reverse();
                    }
                    break;
            }
            return entities;
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
            var entity = base.GetOne(identifier);

            return entity;
        }

        /// <summary>
        /// Creates a new event with a notification.
        /// </summary>
        /// <param name="event">The event data.</param>
        /// <param name="service">The service for email notification.</param>
        /// <returns>The event data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("notify")]
        public EventForList CreateEvent([FromServices] EmailService service, EventForEdit @event)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            else if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            var usersFromDb = this.Context.Set<User>();
            User user = usersFromDb.ToList().Find(u => u.Identifier == @event.UserIdentifier);

            if (user == null)
            {
                this.ModelState.AddModelError("User", "The User is invalid");
            }

            var categories = this.Context.Set<EventCategory>();
            EventCategory category = categories.ToList().Find(c => c.Identifier == @event.CategoryIdentifier);

            if (category == null)
            {
                this.ModelState.AddModelError("Category", "The event category is invalid");
            }

            Event model = new Event()
            {
                User = user,
                Category = category,
                Comment = @event.Comment,
                StartDate = @event.StartDate.Value,
                EndDate = @event.EndDate.Value,
                DurationFirstDay = @event.DurationFirstDay ?? 8m,
                DurationLastDay = @event.DurationLastDay ?? 8m,
            };

            // Send a notification when an event is created
            service.EventCreated(model);

            return this.Post(@event);
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
        /// Retrieves the basic information about the linked entities.
        /// </summary>
        /// <param name="identifier">The identifier of the event.</param>
        /// <returns>The linked entities information.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        [Route("{identifier}/links")]
        public AttachmentLinks GetLinks(Guid identifier)
        {
            var entity = this.Context
                .Set<Event>()
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

        /// <summary>
        /// Checks and maps <see cref="Event.User"/> and <see cref="Event.Category"/> info.
        /// </summary>
        /// <param name="event">The event data.</param>
        /// <param name="model">The event model instance.</param>
        protected override void CheckDependencies(EventForEdit @event, Event model)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            else if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.User = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == @event.UserIdentifier);
            if (model.User == null)
            {
                this.ModelState.AddModelError("UserIdentifier", "Invalid User");
            }

            model.Category = this.Context.Set<EventCategory>().SingleOrDefault(c => c.Identifier == @event.CategoryIdentifier);
            if (model.Category == null)
            {
                this.ModelState.AddModelError("CategoryIdentifier", "Invalid Category");
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
                    UpdatedBy = user,
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
