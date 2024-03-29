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
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides the information related to the current user.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class MyController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public MyController(Context context, IMapper mapper, ILogger<MyController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all events associated to the connected user.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <param name="limit">The maximum numbers of events to return.</param>
    /// <returns>The list of events associated to the connected user.</returns>
    [HttpGet]
    [Produces("application/json")]
    [Route("Events")]
    public ListResult<EventForList> GetEvents([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter, [FromQuery] int? limit)
    {
        if (definitions is null)
        {
            throw new ArgumentNullException(nameof(definitions));
        }
        else if (sortAndFilter == null)
        {
            sortAndFilter = new SortAndFilterModel();
        }

        if (sortAndFilter.SortValue == null)
        {
            sortAndFilter.SortKey = "startDate";
            sortAndFilter.SortValue = "desc";
        }

        var user = this.GetConnectedUser();
        IQueryable<Event> query = this.Context.Set<Event>()
            .Include(e => e.Category)
            .Include(e => e.Statuses).ThenInclude(s => s.Status)
            .Where(e => e.User == user)
            .ApplySortAndFilter(sortAndFilter, definitions.GetOne(nameof(EventForList)));

        int total = query.Count();

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var entities = query
            .ToList()
            .Select(e => this.Mapper.Map<EventForList>(e));

        return new ListResult<EventForList>(entities)
        {
            Total = total,
        };
    }

    /// <summary>
    /// Retrieves a specific event associated to the connected user.
    /// </summary>
    /// <param name="eventId">The identifier of the event.</param>
    /// <returns>The detailed data about the event identified by <paramref name="eventId"/>.</returns>
    /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
    [HttpGet]
    [Produces("application/json")]
    [Route("Events/{eventId}")]
    public EventForView GetEvent(Guid eventId)
    {
        var user = this.GetConnectedUser();
        Event entity = this.Context.Set<Event>()
            .Include(e => e.Category)
            .Include(e => e.Statuses).ThenInclude(s => s.Status)
            .SingleOrDefault(e => e.Identifier == eventId && e.User == user);

        if (entity == null)
        {
            throw new NotFoundException("Not existing entity");
        }

        return this.Mapper.Map<EventForView>(entity);
    }

    /// <summary>
    /// Retrieves the statuses associated to a specific event.
    /// </summary>
    /// <param name="eventId">The identifier of the event.</param>
    /// <returns>The statuses of the event.</returns>
    /// <response code="404">No event is associated to the provided <paramref name="eventId"/>.</response>
    [HttpGet]
    [Produces("application/json")]
    [Route("Events/{eventId}/Statuses")]
    public ListResult<ModelStatusForList> GetEventStatuses(Guid eventId)
    {
        var user = this.GetConnectedUser();
        var entity = this.Context.Set<Event>()
            .Include(e => e.Statuses).ThenInclude(s => s.Status)
            .Include(e => e.Statuses).ThenInclude(s => s.UpdatedBy)
            .Include(e => e.User)
            .SingleOrDefault(e => e.Identifier == eventId && e.User == user);
        if (entity == null)
        {
            throw new NotFoundException("Not existing entity");
        }

        var entities = entity.Statuses
            .OrderByDescending(s => s.UpdatedOn)
            .Select(s => this.Mapper.Map<ModelStatusForList>(s));

        return new ListResult<ModelStatusForList>(entities)
        {
            Total = entities.Count(),
        };
    }

    /// <summary>
    /// Provides the possible future status for a specific event.
    /// </summary>
    /// <param name="eventId">The identifier of the event.</param>
    /// <returns>The possible statuses for the event.</returns>
    /// <response code="404">No event is associated to the provided <paramref name="eventId"/> for the current user.</response>
    [HttpGet]
    [Produces("application/json")]
    [Route("Events/{eventId}/Statuses/Next")]
    public IEnumerable<StatusReference> GetEventStatusesNext(Guid eventId)
    {
        var user = this.GetConnectedUser();
        var entity = this.Context.Set<Event>()
            .Include(e => e.CurrentStatus)
            .SingleOrDefault(e => e.Identifier == eventId && e.User == user);
        if (entity == null)
        {
            throw new NotFoundException("Not existing entity");
        }

        if (entity.CurrentStatus.Identifier == Status.Requested
            || entity.CurrentStatus.Identifier == Status.Approved)
        {
            if (entity.StartDate > DateOnly.FromDateTime(DateTime.Today))
            {
                // The event is cancellable by the user
                return this.Context.Set<Status>()
                    .Where(s => s.Identifier == Status.Canceled)
                    .ToArray()
                    .Select(s => this.Mapper.Map<StatusReference>(s));
            }
        }

        return Array.Empty<StatusReference>();
    }

    /// <summary>
    /// Updates the current status of a specific event.
    /// </summary>
    /// <param name="notification">The notification service.</param>
    /// <param name="eventId">The identifier of the event.</param>
    /// <param name="update">The details of the update.</param>
    /// <returns>The identifier of the created status.</returns>
    /// <response code="404">No event is associated to the provided <paramref name="eventId"/> for the current user.</response>
    [HttpPost]
    [Produces("application/json")]
    [Route("Events/{eventId}/Statuses")]
    public EntityReference EditEventStatus([FromServices] EmailService notification, Guid eventId, StatusUpdate update)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        else if (update is null)
        {
            throw new ArgumentNullException(nameof(update));
        }

        var user = this.GetConnectedUser();
        var entity = this.Context.Set<Event>()
            .Include(e => e.User)
            .Include(e => e.Statuses)
            .Include(e => e.Category)
            .SingleOrDefault(e => e.Identifier == eventId && e.User == user);
        if (entity == null)
        {
            throw new NotFoundException("Not existing entity");
        }

        var from = this.Context.Set<Status>().SingleOrDefault(s => s.Identifier == update.From);
        var to = this.Context.Set<Status>().SingleOrDefault(s => s.Identifier == update.To);

        if (from == null)
        {
            this.ModelState.AddModelError("From", "The From status is invalid");
        }

        if (to == null)
        {
            this.ModelState.AddModelError("To", "The To status is invalid");
        }

        if (update.To == update.From)
        {
            this.ModelState.AddModelError("To", "The statuses From and To should be different");
        }
        else if (entity.CurrentStatus.Identifier == update.To)
        {
            this.ModelState.AddModelError(string.Empty, "A similar transition was already applied");
        }
        else if (entity.CurrentStatus.Identifier != update.From)
        {
            this.ModelState.AddModelError("From", "The event is not in the right state");
        }
        else if (this.GetEventStatusesNext(eventId).All(s => s.Identifier != update.To))
        {
            // Check if the transition is valid for this event
            this.ModelState.AddModelError("From", "This transition is not authorized for this event");
        }

        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        var status = new EventStatus() { Status = to, UpdatedBy = user, UpdatedOn = DateTime.UtcNow };
        entity.Statuses.Add(status);
        entity.CurrentStatus = to;

        this.Context.SaveChanges();
        notification.EventStatusChanged(entity, status);

        return this.Mapper.Map<EntityReference>(status);
    }

    /// <summary>
    /// Retrieves the connected user data.
    /// </summary>
    /// <returns>The detailed data about the connected user.</returns>
    [HttpGet]
    [Produces("application/json")]
    [Route("User")]
    [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Expected Behaviour")]
    public MyUserForView GetUser()
    {
        var user = this.GetConnectedUser();
        return this.Mapper.Map<MyUserForView>(user);
    }

    /// <summary>
    /// Updates the connected user data.
    /// </summary>
    /// <param name="user">The user data.</param>
    /// <returns>The user data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPut]
    [Produces("application/json")]
    [Route("User")]
    public MyUserForView UpdateUser(MyUserForEdit user)
    {
        var model = this.GetConnectedUser();
        if (!string.IsNullOrEmpty(model.ExternalIdentifier))
        {
            // The user has been imported and his profile can't be updated
            string message = "Your profile is defined and managed outside of the system and can't be updated.";
            this.ModelState.AddModelError(string.Empty, message);
            throw new InvalidModelStateException(this.ModelState);
        }

        this.Mapper.Map(user, model);
        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        this.Context.SaveChanges();

        return this.Mapper.Map<MyUserForView>(model);
    }

    /// <summary>
    /// Retrieves the roles of the connected user.
    /// </summary>
    /// <returns>The list of roles assigned to the connected user.</returns>
    [HttpGet]
    [Produces("application/json")]
    [Route("Roles")]
    public IEnumerable<RoleForList> GetRoles()
    {
        var userIdClaim = this.User.Claims.SingleOrDefault(c => c.Type == "sid:guid");
        var userId = Guid.Parse(userIdClaim.Value);

        return this.Context.Set<User>()
            .Include(u => u.Roles)
            .Single(u => u.Identifier == userId)
            .Roles
            .ToList()
            .Select(r => this.Mapper.Map<RoleForList>(r));
    }

    /// <summary>
    /// Retrieves the time-off consumption per category for the connected user.
    /// </summary>
    /// <param name="consumptionService">The calculation service associated to consumption.</param>
    /// <returns>The time-off consumption for the connected user.</returns>
    [HttpGet]
    [Produces("application/json")]
    [Route("Consumption")]
    public IEnumerable<ConsumptionForList> GetMyConsumption([FromServices] ConsumptionService consumptionService)
    {
        if (consumptionService is null)
        {
            throw new ArgumentNullException(nameof(consumptionService));
        }

        var user = this.GetConnectedUser();
        return consumptionService.GetConsumptionForUser(user);
    }

    /// <summary>
    /// Updates the connected user password.
    /// </summary>
    /// <param name="password">The password data.</param>
    /// <returns>The user data after update.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPut]
    [Produces("application/json")]
    [Route("password")]
    public UserForList UpdateMyPassword(PasswordForEdit password)
    {
        if (password is null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        var user = this.GetConnectedUser();

        if (user.ExternalIdentifier != null)
        {
            // The user has been imported and his password can't be updated
            string message = "Your password is defined and managed outside of the system and can't be updated.";
            this.ModelState.AddModelError(string.Empty, message);
        }
        else if (user.HashPassword(password.OldPassword) != user.Password)
        {
            // The provided password is invalid
            string message = "The provided password is invalid.";
            this.ModelState.AddModelError(nameof(password.OldPassword), message);
        }

        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        user.ChangePassword(password.NewPassword);

        this.Context.SaveChanges();

        return this.Mapper.Map<UserForList>(user);
    }
}
