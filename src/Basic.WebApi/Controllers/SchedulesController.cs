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
/// Provides API to retrieve and manage schedule data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class SchedulesController : BaseModelController<Schedule, ScheduleForList, ScheduleForView, ScheduleForEdit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulesController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public SchedulesController(Context context, IMapper mapper, ILogger<SchedulesController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all schedules.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <returns>The list of schedules.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.TimeRO, Role.Time)]
    [Produces("application/json")]
    public ListResult<ScheduleForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
    {
        var entities = this.GetAllCore(definitions, sortAndFilter)
            .ToList()
            .Select(e => this.Mapper.Map<ScheduleForList>(e));

        return new ListResult<ScheduleForList>(entities)
        {
            Total = entities.Count(),
        };
    }

    /// <summary>
    /// Retrieves a specific schedule.
    /// </summary>
    /// <param name="identifier">The identifier of the schedule.</param>
    /// <returns>The detailed data about the schedule identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No schedule is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.TimeRO, Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override ScheduleForView GetOne(Guid identifier)
    {
        return base.GetOne(identifier);
    }

    /// <summary>
    /// Creates a new schedule.
    /// </summary>
    /// <param name="schedule">The schedule data.</param>
    /// <returns>The schedule data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    public override ScheduleForList Post(ScheduleForEdit schedule)
    {
        return base.Post(schedule);
    }

    /// <summary>
    /// Updates a specific schedule.
    /// </summary>
    /// <param name="identifier">The identifier of the schedule to update.</param>
    /// <param name="schedule">The schedule data.</param>
    /// <returns>The schedule data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    /// <response code="404">No schedule is associated to the provided <paramref name="identifier"/>.</response>
    [HttpPut]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override ScheduleForList Put(Guid identifier, ScheduleForEdit schedule)
    {
        return base.Put(identifier, schedule);
    }

    /// <summary>
    /// Deletes a specific schedule.
    /// </summary>
    /// <param name="identifier">The identifier of the schedule to delete.</param>
    /// <response code="404">No schedule is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete(Guid identifier)
    {
        base.Delete(identifier);
    }

    /// <summary>
    /// Checks user information.
    /// </summary>
    /// <param name="entity">The form data.</param>
    /// <param name="model">The datasource instance.</param>
    protected override void CheckDependencies(ScheduleForEdit entity, Schedule model)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        else if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        // Check and update model.User
        model.User = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == entity.UserIdentifier);
        if (model.User == null)
        {
            this.ModelState.AddModelError("UserIdentifier", "Invalid User");
        }

        // Check for conflicts
        var conflicts = this.Context.Set<Schedule>()
            .Where(s => s.User.Identifier == entity.UserIdentifier && s.Identifier != model.Identifier)
            .Where(s => (model.ActiveTo == null || s.ActiveFrom <= model.ActiveTo) && (s.ActiveTo == null || s.ActiveTo >= model.ActiveFrom))
            .ToList();
        if (conflicts.Any())
        {
            this.ModelState.AddModelError(nameof(entity.ActiveFrom), "This schedule conflicts with another schedule");
        }

        // Check and update model.WorkingSchedule
        if (model.WorkingSchedule.ToList().Any(n => n < 0) || model.WorkingSchedule.ToList().Any(n => n > 24))
        {
            this.ModelState.AddModelError("WorkingSchedule", "The schedule must contain hours of work per day (0-24)");
        }

        if (model.WorkingSchedule.ToList().All(n => n == 0))
        {
            this.ModelState.AddModelError("WorkingSchedule", "The schedule must contain at least one working day");
        }

        if (model.WorkingSchedule.Length == 7 || model.WorkingSchedule.Length == 14)
        {
            // Do nothing - the data are already correct
        }
        else if (model.WorkingSchedule.Length < 7)
        {
            model.WorkingSchedule = model.WorkingSchedule.Concat(new decimal[7 - model.WorkingSchedule.Length]).ToArray();
        }
        else if (model.WorkingSchedule.Length < 14)
        {
            model.WorkingSchedule = model.WorkingSchedule.Concat(new decimal[14 - model.WorkingSchedule.Length]).ToArray();
        }
        else if (model.WorkingSchedule.Length > 14)
        {
            this.ModelState.AddModelError("WorkingSchedule", "Invalid schedule data");
        }
    }

    /// <summary>
    /// Ensures that user data are available.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Schedule> AddIncludesForList(IQueryable<Schedule> query)
    {
        return query.Include(s => s.User);
    }

    /// <summary>
    /// Ensures that user data are available.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Schedule> AddIncludesForView(IQueryable<Schedule> query)
    {
        return query.Include(s => s.User);
    }
}
