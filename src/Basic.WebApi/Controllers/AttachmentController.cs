using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage attachment data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AttachmentController : BaseModelController<Attachment, AttachmentForList, AttachmentForView, AttachmentForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public AttachmentController(Context context, IMapper mapper, ILogger<AttachmentController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all attachments.
        /// </summary>
        /// <returns>The list of attachments.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        public IEnumerable<Attachment> GetAll(Guid eventId)
        {
            var entities = Context.Set<Attachment>().Where(c => c.EventIdentifier == eventId);

            return entities
            .ToList();
        }

        /// <summary>
        /// Retrieves a specific attachment.
        /// </summary>
        /// <param name="identifier">The identifier of the attachment.</param>
        /// <returns>The detailed data about the attachment identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override AttachmentForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new attachment.
        /// </summary>
        /// <param name="attachment">The attachment data.</param>
        /// <returns>The attachment data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        public override AttachmentForList Post(AttachmentForEdit attachment)
        {
            return base.Post(attachment);
        }

        /// <summary>
        /// Updates a specific attachment.
        /// </summary>
        /// <param name="identifier">The identifier of the attachment to update.</param>
        /// <param name="attachment">The attachment data.</param>
        /// <returns>The attachment data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time, Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override AttachmentForList Put(Guid identifier, AttachmentForEdit attachment)
        {
            return base.Put(identifier, attachment);
        }

        /// <summary>
        /// Deletes a specific attachment.
        /// </summary>
        /// <param name="identifier">The identifier of the attachment to delete.</param>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.User)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }
    }
}