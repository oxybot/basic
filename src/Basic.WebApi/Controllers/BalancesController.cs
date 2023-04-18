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
/// Provides API to retrieve and manage balance data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class BalancesController : BaseModelController<Balance, BalanceForList, BalanceForView, BalanceForEdit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BalancesController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public BalancesController(Context context, IMapper mapper, ILogger<BalancesController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all balances.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <returns>The list of balances.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.TimeRO, Role.Time)]
    [Produces("application/json")]
    public ListResult<BalanceForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
    {
        var entities = this.GetAllCore(definitions, sortAndFilter)
            .ToList()
            .Select(e => this.Mapper.Map<BalanceForList>(e));

        return new ListResult<BalanceForList>(entities)
        {
            Total = entities.Count(),
        };
    }

    /// <summary>
    /// Retrieves a specific balance.
    /// </summary>
    /// <param name="identifier">The identifier of the balance.</param>
    /// <returns>The detailed data about the balance identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No balance is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.TimeRO, Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override BalanceForView GetOne(Guid identifier)
    {
        return base.GetOne(identifier);
    }

    /// <summary>
    /// Creates a new balance.
    /// </summary>
    /// <param name="balance">The balance data.</param>
    /// <returns>The balance data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    public override BalanceForList Post(BalanceForEdit balance)
    {
        return base.Post(balance);
    }

    /// <summary>
    /// Updates a specific balance.
    /// </summary>
    /// <param name="identifier">The identifier of the balance to update.</param>
    /// <param name="balance">The balance data.</param>
    /// <returns>The balance data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">No balance is associated to the provided <paramref name="identifier"/>.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override BalanceForList Put(Guid identifier, BalanceForEdit balance)
    {
        return base.Put(identifier, balance);
    }

    /// <summary>
    /// Deletes a specific balance.
    /// </summary>
    /// <param name="identifier">The identifier of the balance to delete.</param>
    /// <response code="404">No balance is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete(Guid identifier)
    {
        base.Delete(identifier);
    }

    /// <summary>
    /// Creates a new balance item.
    /// </summary>
    /// <param name="balanceId">The identifier of the balance.</param>
    /// <param name="item">The balance item data.</param>
    /// <returns>The balance item data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">The <paramref name="balanceId"/> is not associated to any balance.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Time)]
    [Route("{balanceId}/items")]
    [Produces("application/json")]
    public BalanceItemForList PostItem([FromRoute] Guid balanceId, BalanceItemForEdit item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var entity = this.Context.Set<Balance>().SingleOrDefault(e => e.Identifier == balanceId);
        if (entity == null)
        {
            throw new NotFoundException("Unknown balance");
        }

        BalanceItem model = this.Mapper.Map<BalanceItem>(item);
        model.Balance = entity;

        this.Context.Set<BalanceItem>().Add(model);
        this.Context.SaveChanges();

        return this.Mapper.Map<BalanceItemForList>(model);
    }

    /// <summary>
    /// Updates an existing balance item.
    /// </summary>
    /// <param name="balanceId">The identifier of the balance.</param>
    /// <param name="itemId">The identifier of the updated item.</param>
    /// <param name="item">The balance item data.</param>
    /// <returns>The balance item data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">The <paramref name="balanceId"/> is not associated to any balance.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Time)]
    [Route("{balanceId}/items/{itemId}")]
    [Produces("application/json")]
    public BalanceItemForList PutItem([FromRoute] Guid balanceId, Guid itemId, BalanceItemForEdit item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var entity = this.Context.Set<Balance>().SingleOrDefault(e => e.Identifier == balanceId);
        if (entity == null)
        {
            throw new NotFoundException("Unknown balance");
        }

        var model = this.Context.Set<BalanceItem>().SingleOrDefault(e => e.Identifier == itemId && e.Balance == entity);
        if (model == null)
        {
            throw new NotFoundException("Unknown balance item");
        }

        this.Mapper.Map(item, model);
        this.Context.SaveChanges();

        return this.Mapper.Map<BalanceItemForList>(model);
    }

    /// <summary>
    /// Deletes a specific balance item.
    /// </summary>
    /// <param name="balanceId">The identifier of the balance.</param>
    /// <param name="itemId">The identifier of the item to delete.</param>
    /// <response code="404">No item is associated to the provided <paramref name="itemId"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Client)]
    [Route("{balanceId}/items/{itemId}")]
    [Produces("application/json")]
    public void DeleteItem([FromRoute] Guid balanceId, Guid itemId)
    {
        var balance = this.Context.Set<Balance>()
            .SingleOrDefault(e => e.Identifier == balanceId);
        if (balance == null)
        {
            throw new NotFoundException("Unknown balance");
        }

        var entity = this.Context.Set<BalanceItem>()
            .SingleOrDefault(e => e.Identifier == itemId && e.Balance == balance);
        if (entity == null)
        {
            throw new NotFoundException($"Not existing entity");
        }

        this.Context.Set<BalanceItem>().Remove(entity);
        this.Context.SaveChanges();
    }

    /// <summary>
    /// Checks and maps <see cref="Balance.User"/> and <see cref="Balance.Category"/> info.
    /// </summary>
    /// <param name="balance">The event data.</param>
    /// <param name="model">The event model instance.</param>
    protected override void ExecuteExtraChecks(BalanceForEdit balance, Balance model)
    {
        if (balance is null)
        {
            throw new ArgumentNullException(nameof(balance));
        }
        else if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        // Check dependencies
        model.User = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == balance.UserIdentifier);
        if (model.User == null)
        {
            this.ModelState.AddModelError("UserIdentifier", "Invalid User");
        }

        model.Category = this.Context.Set<EventCategory>().SingleOrDefault(c => c.Identifier == balance.CategoryIdentifier);
        if (model.Category == null)
        {
            this.ModelState.AddModelError("CategoryIdentifier", "Invalid Category");
        }

        // Check conflict
        bool conflict = this.Context.Set<Balance>().Any(b => b.User == model.User && b.Category == model.Category && b.Year == model.Year && b.Identifier != model.Identifier);
        if (conflict)
        {
            this.ModelState.AddModelError(string.Empty, "Such balance is already defined (Same user, category and year)");
        }
    }

    /// <summary>
    /// Adds the <see cref="Balance.User"/> and <see cref="Balance.Category"/> values.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Balance> AddIncludesForList(IQueryable<Balance> query)
    {
        return query
            .Include(c => c.User)
            .Include(c => c.Category);
    }

    /// <summary>
    /// Adds the <see cref="Balance.User"/> and <see cref="Balance.Category"/> values.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Balance> AddIncludesForView(IQueryable<Balance> query)
    {
        return query
            .Include(c => c.User)
            .Include(c => c.Category)
            .Include(c => c.Details);
    }
}
