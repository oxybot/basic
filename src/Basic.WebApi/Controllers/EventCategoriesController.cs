﻿// Copyright (c) oxybot. All rights reserved.
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

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve and manage event category data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class EventCategoriesController : BaseModelController<EventCategory, EventCategoryForList, EventCategoryForList, EventCategoryForEdit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventCategoriesController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public EventCategoriesController(Context context, IMapper mapper, ILogger<EventCategoriesController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <returns>The list of categories.</returns>
    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    public ListResult<EventCategoryForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
    {
        var entities = this.GetAllCore(definitions, sortAndFilter)
            .ToList()
            .Select(e => this.Mapper.Map<EventCategoryForList>(e));

        return new ListResult<EventCategoryForList>(entities)
        {
            Total = entities.Count(),
        };
    }

    /// <summary>
    /// Retrieves a specific category.
    /// </summary>
    /// <param name="identifier">The identifier of the category.</param>
    /// <returns>The detailed data about the category identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No category is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.SchedulesRO, Role.Schedules)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override EventCategoryForList GetOne(Guid identifier)
    {
        return base.GetOne(identifier);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="category">The category data.</param>
    /// <returns>The category data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Schedules)]
    [Produces("application/json")]
    public override EventCategoryForList Post(EventCategoryForEdit category)
    {
        return base.Post(category);
    }

    /// <summary>
    /// Updates a specific category.
    /// </summary>
    /// <param name="identifier">The identifier of the category to update.</param>
    /// <param name="category">The category data.</param>
    /// <returns>The category data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">No category is associated to the provided <paramref name="identifier"/>.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Schedules)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override EventCategoryForList Put(Guid identifier, EventCategoryForEdit category)
    {
        return base.Put(identifier, category);
    }

    /// <summary>
    /// Deletes a specific category.
    /// </summary>
    /// <param name="identifier">The identifier of the category to delete.</param>
    /// <response code="404">No category is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Schedules)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete(Guid identifier)
    {
        base.Delete(identifier);
    }

    /// <summary>
    /// Overriden to manage the value of ColorClass.
    /// </summary>
    /// <param name="entity">The received entity.</param>
    /// <param name="model">The associated modei.</param>
    protected override void ExecuteExtraChecks(EventCategoryForEdit entity, EventCategory model)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        else if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        // Check for conflicts
        var duplicates = this.Context.Set<Client>()
            .Where(c => c.DisplayName == model.DisplayName)
            .Where(c => c.Identifier != model.Identifier);
        if (duplicates.Any())
        {
            this.ModelState.AddModelError(nameof(model.DisplayName), "A event category with the same Display Name is already registered.");
        }

        // Update data if needed
        if (model.Mapping == EventTimeMapping.TimeOff)
        {
            model.ColorClass = null;
        }
    }
}
