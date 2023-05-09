// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve and manage attachment data.
/// </summary>
/// <typeparam name="TModel">The type of the entity model.</typeparam>
/// <typeparam name="TAttachment">The type of associated with the attachements.</typeparam>
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
    protected BaseAttachmentsController(Context context, IMapper mapper, ILogger<BaseAttachmentsController<TModel, TAttachment>> logger)
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
        var parent = this.Context.Set<TModel>()
            .Include(a => a.Attachments)
            .SingleOrDefault(a => a.Identifier == parentId);
        if (parent == null)
        {
            throw new NotFoundException("Unknown entity");
        }

        return parent.Attachments.Select(a => this.Mapper.Map<AttachmentForList>(a));
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
        var parent = this.Context.Set<TModel>()
           .Include(a => a.Attachments)
           .SingleOrDefault(a => a.Identifier == parentId);
        if (parent == null)
        {
            throw new NotFoundException("Unknown entity");
        }

        var attachment = parent.Attachments.SingleOrDefault(a => a.Identifier == identifier);
        return this.Mapper.Map<AttachmentForView>(attachment);
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
    [AuthorizeRoles(Role.Clients)]
    [Produces("application/json")]
    public virtual AttachmentForList Post([FromRoute] Guid parentId, AttachmentForEdit entity)
    {
        var parent = this.Context.Set<TModel>()
           .Include(a => a.Attachments)
           .SingleOrDefault(a => a.Identifier == parentId);
        if (parent == null)
        {
            throw new NotFoundException("Unknown entity");
        }

        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        TAttachment model = this.Mapper.Map<AttachmentForEdit, TAttachment>(entity, new TAttachment());
        model.Parent = parent;

        this.Context.Set<TAttachment>().Add(model);
        this.Context.SaveChanges();

        return this.Mapper.Map<AttachmentForList>(model);
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
        var parent = this.Context.Set<TModel>()
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

        this.Context.Set<TAttachment>().Remove(entity);
        this.Context.SaveChanges();
    }
}
