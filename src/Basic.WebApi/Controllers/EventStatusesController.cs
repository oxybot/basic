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
    [Route("/Events/{eventId}/Statuses")]
    public class EventStatusesController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStatusesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public EventStatusesController(Context context, IMapper mapper, ILogger<EventStatusesController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves the statuses associated to a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>The statuses of the event.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        public IEnumerable<ModelStatusForList> GetAll(Guid eventId)
        {
            var entity = Context.Set<Event>()
                .Include(e => e.Statuses).ThenInclude(s => s.Status)
                .Include(e => e.User)
                .SingleOrDefault(c => c.Identifier == eventId);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return entity.Statuses
                .OrderByDescending(s => s.UpdatedOn)
                .Select(s => Mapper.Map<ModelStatusForList>(s));
        }


        /// <summary>
        /// Provides the possible future status for a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>The possible statuses for the event.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        [Route("Next")]
        public IEnumerable<StatusReference> GetNext(Guid eventId)
        {
            var entity = Context.Set<Event>()
                .Include(e => e.Statuses).ThenInclude(s => s.Status)
                .SingleOrDefault(c => c.Identifier == eventId);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            if (entity.CurrentStatus.Identifier == Status.Requested)
            {
                return new[] {
                    Context.Set<Status>().SingleOrDefault(s => s.Identifier == Status.Approved),
                    Context.Set<Status>().SingleOrDefault(s => s.Identifier == Status.Rejected),
                }.Select(s => Mapper.Map<StatusReference>(s));
            }

            if (entity.CurrentStatus.Identifier == Status.Approved)
            {
                return new[] {
                    Context.Set<Status>().SingleOrDefault(s => s.Identifier == Status.Canceled),
                }.Select(s => Mapper.Map<StatusReference>(s));
            }

            return Array.Empty<StatusReference>();
        }

        /// <summary>
        /// Updates the current status of a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <param name="update">The details of the update.</param>
        /// <returns>The identifier of the created status.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public EntityReference EditStatus(Guid eventId, StatusUpdate update)
        {
            var entity = Context.Set<Event>().Include(e => e.Statuses)
                .SingleOrDefault(c => c.Identifier == eventId);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            var from = Context.Set<Status>().SingleOrDefault(s => s.Identifier == update.From);
            var to = Context.Set<Status>().SingleOrDefault(s => s.Identifier == update.To);

            if (from == null)
            {
                ModelState.AddModelError("From", "The From status is invalid");
            }

            if (to == null)
            {
                ModelState.AddModelError("To", "The To status is invalid");
            }

            if (update.To == update.From)
            {
                ModelState.AddModelError("To", "The statuses From and To should be different");
            }
            else if (entity.CurrentStatus.Identifier == update.To)
            {
                ModelState.AddModelError("", "A similar transition was already applied");
            }
            else if (entity.CurrentStatus.Identifier != update.From)
            {
                ModelState.AddModelError("From", "The event is not in the right state");
            }

            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
            }

            var user = this.GetConnectedUser();
            var status = new EventStatus() { Status = to, UpdatedBy = user, UpdatedOn = DateTime.UtcNow };
            entity.Statuses.Add(status);
            Context.SaveChanges();

            return Mapper.Map<EntityReference>(status);
        }
    }
}
