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
    /// Provides API to retrieve and manage attachment data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("Agreements/{agreementId}/Attachments")]
    public class AgreementAttachmentsController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public AgreementAttachmentsController(Context context, IMapper mapper, ILogger<AgreementAttachmentsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all attachments for a specific agreement.
        /// </summary>
        /// <param name="agreementId">The identifier of the parent agreement.</param>
        /// <returns>The list of associated attachments.</returns>
        /// <response code="404">No agreement is associated to the provided <paramref name="agreementId"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.Client, Role.ClientRO)]
        [Produces("application/json")]
        public virtual IEnumerable<AttachmentForList> GetAll([FromRoute] Guid agreementId)
        {
            var parent = Context.Set<Agreement>()
                .Include(a => a.Attachments)
                .SingleOrDefault(a => a.Identifier == agreementId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            return parent.Attachments.Select(a => Mapper.Map<AttachmentForList>(a));
        }

        /// <summary>
        /// Retrieves a specific attachment.
        /// </summary>
        /// <param name="agreementId">The identifier of the parent agreement.</param>
        /// <param name="identifier">The identifier of the attachment.</param>
        /// <returns>The detailed data about the attachment identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No agreement is associated to the provided <paramref name="agreementId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.Client, Role.ClientRO)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual AttachmentForView GetOne([FromRoute] Guid agreementId, Guid identifier)
        {
            var parent = Context.Set<Agreement>()
               .Include(a => a.Attachments)
               .SingleOrDefault(a => a.Identifier == agreementId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            var attachment = parent.Attachments.SingleOrDefault(a => a.Identifier == identifier);
            return Mapper.Map<AttachmentForView>(attachment);
        }

        /// <summary>
        /// Creates a new attachment.
        /// </summary>
        /// <param name="agreementId">The identifier of the parent agreement.</param>
        /// <param name="entity">The attachment data.</param>
        /// <returns>The attachment data after creation.</returns>
        /// <response code="404">No agreement is associated to the provided <paramref name="agreementId"/>.</response>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Client)]
        [Produces("application/json")]
        public virtual AttachmentForList Post([FromRoute] Guid agreementId, AttachmentForEdit entity)
        {
            var parent = Context.Set<Agreement>()
               .Include(a => a.Attachments)
               .SingleOrDefault(a => a.Identifier == agreementId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
            }

            Attachment model = Mapper.Map<Attachment>(entity);
            model.AgreementIdentifier = agreementId;

            Context.Set<Attachment>().Add(model);
            Context.SaveChanges();

            return Mapper.Map<AttachmentForList>(model);
        }

        /// <summary>
        /// Deletes a specific attachment.
        /// </summary>
        /// <param name="agreementId">The identifier of the parent agreement.</param>
        /// <param name="identifier">The identifier of the attachment to delete.</param>
        /// <response code="404">No agreement is associated to the provided <paramref name="agreementId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Client)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual void Delete([FromRoute] Guid agreementId, Guid identifier)
        {
            var parent = Context.Set<Agreement>()
               .Include(a => a.Attachments)
               .SingleOrDefault(a => a.Identifier == agreementId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            var entity = parent.Attachments.SingleOrDefault(a => a.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            Context.Set<Attachment>().Remove(entity);
            Context.SaveChanges();
        }
    }
}
