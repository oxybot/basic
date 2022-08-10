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
    public abstract class BaseAttachmentsController<TModel, TAttachment> : BaseController
        where TModel : BaseModel, IWithAttachments<TAttachment>, new()
        where TAttachment : BaseAttachment<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAttachmentsController{TModel, TAttachment}"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public BaseAttachmentsController(Context context, IMapper mapper, ILogger<BaseAttachmentsController<TModel, TAttachment>> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all attachments for a specific entity.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity.</param>
        /// <returns>The list of associated attachments.</returns>
        /// <response code="404">No entity is associated to the provided <paramref name="parentId"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        public virtual IEnumerable<AttachmentForList> GetAll([FromRoute] Guid parentId)
        {
            var parent = Context.Set<TModel>()
                .Include(a => a.Attachments)
                .SingleOrDefault(a => a.Identifier == parentId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown entity");
            }

            return parent.Attachments.Select(a => Mapper.Map<AttachmentForList>(a));
        }

        /// <summary>
        /// Retrieves a specific attachment.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity.</param>
        /// <param name="identifier">The identifier of the attachment.</param>
        /// <returns>The detailed data about the attachment identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No entity is associated to the provided <paramref name="parentId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual AttachmentForView GetOne([FromRoute] Guid parentId, Guid identifier)
        {
            var parent = Context.Set<TModel>()
               .Include(a => a.Attachments)
               .SingleOrDefault(a => a.Identifier == parentId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown entity");
            }

            var attachment = parent.Attachments.SingleOrDefault(a => a.Identifier == identifier);
            return Mapper.Map<AttachmentForView>(attachment);
        }

        /// <summary>
        /// Creates a new attachment.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity.</param>
        /// <param name="entity">The attachment data.</param>
        /// <returns>The attachment data after creation.</returns>
        /// <response code="404">No entity is associated to the provided <paramref name="parentId"/>.</response>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Client)]
        [Produces("application/json")]
        public virtual AttachmentForList Post([FromRoute] Guid parentId, AttachmentForEdit entity)
        {
            var parent = Context.Set<TModel>()
               .Include(a => a.Attachments)
               .SingleOrDefault(a => a.Identifier == parentId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown entity");
            }

            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
            }

            TAttachment model = Mapper.Map<AttachmentForEdit, TAttachment>(entity, new TAttachment());
            model.Parent = parent;

            Context.Set<TAttachment>().Add(model);
            Context.SaveChanges();

            return Mapper.Map<AttachmentForList>(model);
        }

        /// <summary>
        /// Deletes a specific attachment.
        /// </summary>
        /// <param name="parentId">The identifier of the parent entity.</param>
        /// <param name="identifier">The identifier of the attachment to delete.</param>
        /// <response code="404">No entity is associated to the provided <paramref name="parentId"/>.</response>
        /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual void Delete([FromRoute] Guid parentId, Guid identifier)
        {
            var parent = Context.Set<TModel>()
               .Include(a => a.Attachments)
               .SingleOrDefault(a => a.Identifier == parentId);
            if (parent == null)
            {
                throw new NotFoundException("Unknown entity");
            }

            var entity = parent.Attachments.SingleOrDefault(a => a.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            Context.Set<TAttachment>().Remove(entity);
            Context.SaveChanges();
        }
    }
}
