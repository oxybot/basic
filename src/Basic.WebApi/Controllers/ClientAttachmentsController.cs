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
[Route("Clients/{clientId}/Attachments")]
public class ClientAttachmentsController : BaseAttachmentsController<Client, ClientAttachment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientAttachmentsController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public ClientAttachmentsController(Context context, IMapper mapper, ILogger<ClientAttachmentsController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all attachments for a specific client.
    /// </summary>
    /// <param name="clientId">The identifier of the parent client.</param>
    /// <returns>The list of associated attachments.</returns>
    /// <response code="404">No client is associated to the provided <paramref name="clientId"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.Clients, Role.ClientsRO)]
    [Produces("application/json")]
    public override IEnumerable<AttachmentForList> GetAll([FromRoute] Guid clientId)
    {
        return base.GetAll(clientId);
    }

    /// <summary>
    /// Retrieves a specific attachment.
    /// </summary>
    /// <param name="clientId">The identifier of the parent client.</param>
    /// <param name="identifier">The identifier of the attachment.</param>
    /// <returns>The detailed data about the attachment identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No client is associated to the provided <paramref name="clientId"/>.</response>
    /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.Clients, Role.ClientsRO)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override AttachmentForView GetOne([FromRoute] Guid clientId, Guid identifier)
    {
        return base.GetOne(clientId, identifier);
    }

    /// <summary>
    /// Creates a new attachment.
    /// </summary>
    /// <param name="clientId">The identifier of the parent client.</param>
    /// <param name="entity">The attachment data.</param>
    /// <returns>The attachment data after creation.</returns>
    /// <response code="404">No client is associated to the provided <paramref name="clientId"/>.</response>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Clients)]
    [Produces("application/json")]
    public override AttachmentForList Post([FromRoute] Guid clientId, AttachmentForEdit entity)
    {
        return base.Post(clientId, entity);
    }

    /// <summary>
    /// Deletes a specific attachment.
    /// </summary>
    /// <param name="clientId">The identifier of the parent client.</param>
    /// <param name="identifier">The identifier of the attachment to delete.</param>
    /// <response code="404">No client is associated to the provided <paramref name="clientId"/>.</response>
    /// <response code="404">No attachment is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Clients)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete([FromRoute] Guid clientId, Guid identifier)
    {
        base.Delete(clientId, identifier);
    }
}
