using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage attachment data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("Events/{eventId}/Attachments")]
    public class EventAttachmentsController : BaseAttachmentsController<Event, EventAttachment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAttachmentsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public EventAttachmentsController(Context context, IMapper mapper, ILogger<EventAttachmentsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all attachments for a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the parent event.</param>
        /// <returns>The list of associated attachments.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        public override IEnumerable<AttachmentForList> GetAll([FromRoute] Guid eventId)
        {
            return base.GetAll(eventId);
        }

        /// <summary>
        /// Retrieves a specific attachment.
        /// </summary>
        /// <param name="eventId">The identifier of the parent event.</param>
        /// <param name="identifier">The identifier of the attachment.</param>
        /// <returns>The detailed data about the attachment identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override AttachmentForView GetOne([FromRoute] Guid eventId, Guid identifier)
        {
            return base.GetOne(eventId, identifier);
        }

        /// <summary>
        /// Creates a new attachment.
        /// </summary>
        /// <param name="eventId">The identifier of the parent event.</param>
        /// <param name="entity">The attachment data.</param>
        /// <returns>The attachment data after creation.</returns>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override AttachmentForList Post([FromRoute] Guid eventId, AttachmentForEdit entity)
        {
            return base.Post(eventId, entity);
        }

        /// <summary>
        /// Deletes a specific attachment.
        /// </summary>
        /// <param name="eventId">The identifier of the parent event.</param>
        /// <param name="identifier">The identifier of the attachment to delete.</param>
        /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete([FromRoute] Guid eventId, Guid identifier)
        {
            base.Delete(eventId, identifier);
        }
    }
}
