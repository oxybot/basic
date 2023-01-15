// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve and manage client data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class ClientsController : BaseModelController<Client, ClientForList, ClientForView, ClientForEdit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientsController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public ClientsController(Context context, IMapper mapper, ILogger<ClientsController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all clients.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <returns>The list of clients.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.ClientRO, Role.Client)]
    [Produces("application/json")]
    public IEnumerable<ClientForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
    {
        var entities = this.GetAllCore(definitions, sortAndFilter)
            .ToList()
            .Select(e => this.Mapper.Map<ClientForList>(e));

        return entities;
    }

    /// <summary>
    /// Retrieves a specific client.
    /// </summary>
    /// <param name="identifier">The identifier of the client.</param>
    /// <returns>The detailed data about the client identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.ClientRO, Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override ClientForView GetOne(Guid identifier)
    {
        return base.GetOne(identifier);
    }

    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="client">The client data.</param>
    /// <returns>The client data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Client)]
    [Produces("application/json")]
    public override ClientForList Post(ClientForEdit client)
    {
        return base.Post(client);
    }

    /// <summary>
    /// Updates a specific client.
    /// </summary>
    /// <param name="identifier">The identifier of the client to update.</param>
    /// <param name="client">The client data.</param>
    /// <returns>The client data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override ClientForList Put(Guid identifier, ClientForEdit client)
    {
        return base.Put(identifier, client);
    }

    /// <summary>
    /// Deletes a specific client.
    /// </summary>
    /// <param name="identifier">The identifier of the client to delete.</param>
    /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete(Guid identifier)
    {
        base.Delete(identifier);
    }

    /// <summary>
    /// Retrieves the basic information about the linked entities.
    /// </summary>
    /// <param name="identifier">The identifier of the client.</param>
    /// <returns>The linked entities information.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.ClientRO, Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}/links")]
    public ClientLinks GetLinks(Guid identifier)
    {
        var entity = this.Context
            .Set<Client>()
            .Include(c => c.Agreements)
            .SingleOrDefault(c => c.Identifier == identifier);
        if (entity == null)
        {
            throw new NotFoundException("Not existing entity");
        }

        return new ClientLinks()
        {
            Agreements = entity.Agreements.Count,
        };
    }

    /// <inheritdoc />
    protected override void CheckDependencies(ClientForEdit entity, Client model)
    {
        int duplicate = this.Context.Set<Client>().Where(c => c.DisplayName == model.DisplayName).Count(c => c.Identifier != model.Identifier);
        if (duplicate > 0)
        {
            this.ModelState.AddModelError(nameof(model.DisplayName), "A client with the same Display Name is already registered.");
        }
    }
}
