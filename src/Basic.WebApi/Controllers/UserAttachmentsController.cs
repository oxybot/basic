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
    [Route("Users/{userId}/Attachments")]
    public class UserAttachmentsController : BaseAttachmentsController<User, UserAttachment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAttachmentsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public UserAttachmentsController(Context context, IMapper mapper, ILogger<UserAttachmentsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all attachments for a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the parent user.</param>
        /// <returns>The list of associated attachments.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="userId"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        public override IEnumerable<AttachmentForList> GetAll([FromRoute] Guid userId)
        {
            return base.GetAll(userId);
        }

        /// <summary>
        /// Retrieves a specific attachment.
        /// </summary>
        /// <param name="userId">The identifier of the parent user.</param>
        /// <param name="identifier">The identifier of the attachment.</param>
        /// <returns>The detailed data about the attachment identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="userId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.Time, Role.TimeRO)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override AttachmentForView GetOne([FromRoute] Guid userId, Guid identifier)
        {
            return base.GetOne(userId, identifier);
        }

        /// <summary>
        /// Creates a new attachment.
        /// </summary>
        /// <param name="userId">The identifier of the parent user.</param>
        /// <param name="entity">The attachment data.</param>
        /// <returns>The attachment data after creation.</returns>
        /// <response code="404">No user is associated to the provided <paramref name="userId"/>.</response>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override AttachmentForList Post([FromRoute] Guid userId, AttachmentForEdit entity)
        {
            return base.Post(userId, entity);
        }

        /// <summary>
        /// Deletes a specific attachment.
        /// </summary>
        /// <param name="userId">The identifier of the parent user.</param>
        /// <param name="identifier">The identifier of the attachment to delete.</param>
        /// <response code="404">No user is associated to the provided <paramref name="userId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete([FromRoute] Guid userId, Guid identifier)
        {
            base.Delete(userId, identifier);
        }
    }
}
