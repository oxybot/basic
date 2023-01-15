// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve and manage attachment data.
/// </summary>
[ApiController]
[Authorize]
[Route("Agreements/{agreementId}/Attachments")]
public class AgreementAttachmentsController : BaseAttachmentsController<Agreement, AgreementAttachment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AgreementAttachmentsController"/> class.
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
    public override IEnumerable<AttachmentForList> GetAll([FromRoute] Guid agreementId)
    {
        return base.GetAll(agreementId);
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
    public override AttachmentForView GetOne([FromRoute] Guid agreementId, Guid identifier)
    {
        return base.GetOne(agreementId, identifier);
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
    public override AttachmentForList Post([FromRoute] Guid agreementId, AttachmentForEdit entity)
    {
        return base.Post(agreementId, entity);
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
    public override void Delete([FromRoute] Guid agreementId, Guid identifier)
    {
        base.Delete(agreementId, identifier);
    }
}
