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
/// Provides API to retrieve and manage agreements data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class AgreementsController : BaseModelController<Agreement, AgreementForList, AgreementForView, AgreementForEdit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AgreementsController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public AgreementsController(Context context, IMapper mapper, ILogger<AgreementsController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all agreements.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <param name="clientId">The identifier of the client to filter the result list, if any.</param>
    /// <returns>The list of agreements.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.ClientRO, Role.Client)]
    [Produces("application/json")]
    public ListResult<AgreementForList> GetAll(
        [FromServices] DefinitionsService definitions,
        [FromQuery] SortAndFilterModel sortAndFilter,
        [FromQuery] Guid? clientId)
    {
        var entities = this.GetAllCore(definitions, sortAndFilter);

        if (clientId.HasValue)
        {
            entities = entities.Where(c => c.Client.Identifier == clientId.Value);
        }

        var values = entities
            .ToList()
            .Select(e => this.Mapper.Map<AgreementForList>(e));

        return new ListResult<AgreementForList>(values)
        {
            Total = values.Count(),
        };
    }

    /// <summary>
    /// Retrieves a specific agreement.
    /// </summary>
    /// <param name="identifier">The identifier of the agreement.</param>
    /// <returns>The detailed data about the agreement identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No agreement is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.ClientRO, Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override AgreementForView GetOne(Guid identifier)
    {
        return base.GetOne(identifier);
    }

    /// <summary>
    /// Creates a new agreement.
    /// </summary>
    /// <param name="agreement">The agreement data.</param>
    /// <returns>The agreement data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Client)]
    [Produces("application/json")]
    public override AgreementForList Post(AgreementForEdit agreement)
    {
        return base.Post(agreement);
    }

    /// <summary>
    /// Updates a specific agreement.
    /// </summary>
    /// <param name="identifier">The identifier of the agreement to update.</param>
    /// <param name="agreement">The agreement data.</param>
    /// <returns>The agreement data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">No agreement is associated to the provided <paramref name="identifier"/>.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override AgreementForList Put(Guid identifier, AgreementForEdit agreement)
    {
        return base.Put(identifier, agreement);
    }

    /// <summary>
    /// Deletes a specific agreement.
    /// </summary>
    /// <param name="identifier">The identifier of the agreement to delete.</param>
    /// <response code="404">No agreement is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Client)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete(Guid identifier)
    {
        base.Delete(identifier);
    }

    /// <summary>
    /// Creates a new agreement item.
    /// </summary>
    /// <param name="agreementId">The identifier of the agreement.</param>
    /// <param name="item">The agreement item data.</param>
    /// <returns>The agreement item data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">The <paramref name="agreementId"/> is not associated to any agreement.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Client)]
    [Route("{agreementId}/items")]
    [Produces("application/json")]
    public AgreementItemForList PostItem([FromRoute] Guid agreementId, AgreementItemForEdit item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var agreement = this.Context.Set<Agreement>().SingleOrDefault(e => e.Identifier == agreementId);
        if (agreement == null)
        {
            throw new NotFoundException("Unknown agreement");
        }

        AgreementItem model = this.Mapper.Map<AgreementItem>(item);
        model.Agreement = agreement;
        if (item.ProductIdentifier.HasValue)
        {
            model.Product = this.Context.Set<Product>()
                .SingleOrDefault(p => p.Identifier == item.ProductIdentifier.Value);
            if (model.Product == null)
            {
                this.ModelState.AddModelError("ProductIdentifier", "Invalid product");
                throw new InvalidModelStateException(this.ModelState);
            }
        }

        this.Context.Set<AgreementItem>().Add(model);
        this.Context.SaveChanges();

        return this.Mapper.Map<AgreementItemForList>(model);
    }

    /// <summary>
    /// Updates an existing agreement item.
    /// </summary>
    /// <param name="agreementId">The identifier of the agreement.</param>
    /// <param name="itemId">The identifier of the updated item.</param>
    /// <param name="item">The agreement item data.</param>
    /// <returns>The agreement item data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">The <paramref name="agreementId"/> is not associated to any agreement.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Client)]
    [Route("{agreementId}/items/{itemId}")]
    [Produces("application/json")]
    public AgreementItemForList PutItem([FromRoute] Guid agreementId, Guid itemId, AgreementItemForEdit item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var agreement = this.Context.Set<Agreement>().SingleOrDefault(e => e.Identifier == agreementId);
        if (agreement == null)
        {
            throw new NotFoundException("Unknown agreement");
        }

        var model = this.Context.Set<AgreementItem>().SingleOrDefault(e => e.Identifier == itemId && e.Agreement == agreement);
        if (model == null)
        {
            throw new NotFoundException("Unknown agreement item");
        }

        this.Mapper.Map(item, model);
        if (item.ProductIdentifier.HasValue)
        {
            model.Product = this.Context.Set<Product>()
                .SingleOrDefault(p => p.Identifier == item.ProductIdentifier.Value);
            if (model.Product == null)
            {
                this.ModelState.AddModelError("ProductIdentifier", "Invalid product identifier");
                throw new InvalidModelStateException(this.ModelState);
            }
        }

        this.Context.SaveChanges();

        return this.Mapper.Map<AgreementItemForList>(model);
    }

    /// <summary>
    /// Deletes a specific agreement item.
    /// </summary>
    /// <param name="agreementId">The identifier of the agreement.</param>
    /// <param name="itemId">The identifier of the item to delete.</param>
    /// <response code="404">No item is associated to the provided <paramref name="itemId"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Client)]
    [Route("{agreementId}/items/{itemId}")]
    [Produces("application/json")]
    public void DeleteItem([FromRoute] Guid agreementId, Guid itemId)
    {
        var agreement = this.Context.Set<Agreement>()
            .SingleOrDefault(e => e.Identifier == agreementId);
        if (agreement == null)
        {
            throw new NotFoundException("Unknown agreement");
        }

        var entity = this.Context.Set<AgreementItem>()
            .SingleOrDefault(e => e.Identifier == itemId && e.Agreement == agreement);
        if (entity == null)
        {
            throw new NotFoundException($"Not existing entity");
        }

        this.Context.Set<AgreementItem>().Remove(entity);
        this.Context.SaveChanges();
    }

    /// <summary>
    /// Checks and maps Agrrements info.
    /// </summary>
    /// <param name="agreement">The agreement data.</param>
    /// <param name="model">The agreement model instance.</param>
    protected override void ExecuteExtraChecks(AgreementForEdit agreement, Agreement model)
    {
        if (agreement is null)
        {
            throw new ArgumentNullException(nameof(agreement));
        }
        else if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Client = this.Context.Set<Client>().SingleOrDefault(c => c.Identifier == agreement.ClientIdentifier);
        if (model.Client == null)
        {
            this.ModelState.AddModelError("ClientIdentifier", "Invalid client");
        }

        if (agreement.Items == null)
        {
            agreement.Items = Array.Empty<AgreementItemForEditWithIdentifier>();
        }

        // Updated items
        foreach (var item in agreement.Items.Where(i => i.Identifier.HasValue))
        {
            var modelItem = model.Items.SingleOrDefault(i => i.Identifier == item.Identifier.Value);
            if (modelItem == null)
            {
                this.ModelState.AddModelError("Items", "Unknown item identified by: " + item.Identifier.Value);
            }

            this.Mapper.Map(item, modelItem);
            if (item.ProductIdentifier.HasValue)
            {
                modelItem.Product = this.Context.Set<Product>().SingleOrDefault(i => i.Identifier == item.ProductIdentifier);
                if (modelItem.Product == null)
                {
                    this.ModelState.AddModelError("ProductIdentifier", "Unknown product identified by: " + item.ProductIdentifier.Value);
                }
            }
        }

        // Deleted items
        var keys = agreement.Items.Where(i => i.Identifier.HasValue).Select(i => i.Identifier).ToList();
        foreach (var modelItem in model.Items.Where(i => !keys.Contains(i.Identifier)).ToArray())
        {
            model.Items.Remove(modelItem);
        }

        // New items
        foreach (var item in agreement.Items.Where(i => !i.Identifier.HasValue))
        {
            var modelItem = this.Mapper.Map<AgreementItem>(item);
            modelItem.Agreement = model;
            if (item.ProductIdentifier.HasValue)
            {
                modelItem.Product = this.Context.Set<Product>().SingleOrDefault(i => i.Identifier == item.ProductIdentifier);
                if (modelItem.Product == null)
                {
                    this.ModelState.AddModelError("ProductIdentifier", "Unknown product identified by: " + item.ProductIdentifier.Value);
                }
            }

            model.Items.Add(modelItem);
        }
    }

    /// <summary>
    /// Adds the <see cref="Client"/> values.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Agreement> AddIncludesForList(IQueryable<Agreement> query)
    {
        return query.Include(c => c.Client);
    }

    /// <summary>
    /// Adds the <see cref="Client"/> values.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Agreement> AddIncludesForView(IQueryable<Agreement> query)
    {
        return query
            .Include(c => c.Client)
            .Include(c => c.Items)
                .ThenInclude(i => i.Product);
    }
}
