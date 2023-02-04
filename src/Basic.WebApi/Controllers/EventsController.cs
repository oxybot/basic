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
/// Provides API to retrieve and manage events data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class EventsController
    : BaseImmutableModelController<Event, EventForList, EventForView, EventForEdit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventsController"/> class.
    /// </summary>
    /// <param name="notification">The notification service.</param>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public EventsController(EmailService notification, Context context, IMapper mapper, ILogger<EventsController> logger)
        : base(context, mapper, logger)
    {
        this.Notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    /// <summary>
    /// Gets the notification service.
    /// </summary>
    public EmailService Notification { get; }

    /// <summary>
    /// Retrieves all events.
    /// </summary>
    /// <param name="definitions">The service providing the entity definitions.</param>
    /// <param name="sortAndFilter">The sort and filter options, is any.</param>
    /// <returns>The list of events.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.TimeRO, Role.Time)]
    [Produces("application/json")]
    public ListResult<EventForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
    {
        IDictionary<string, Func<Event, bool>> filters = new Dictionary<string, Func<Event, bool>>
        {
            { "status/requested", e => e.CurrentStatus.Identifier == Status.Requested },
            { "status/approved", e => e.CurrentStatus.Identifier == Status.Approved },
            { "status/rejected", e => e.CurrentStatus.Identifier == Status.Rejected },
            { "status/canceled", e => e.CurrentStatus.Identifier == Status.Canceled },
        };

        var entities = this.GetAllCore(definitions, sortAndFilter)
            .ToList()
            .ApplyFilters(filters, sortAndFilter?.Filters)
            .Select(e => this.Mapper.Map<EventForList>(e));

        if (sortAndFilter == null || string.IsNullOrEmpty(sortAndFilter.SortKey))
        {
            entities = entities.Reverse();
        }

        return new ListResult<EventForList>(entities)
        {
            Total = this.Context.Set<Event>().Count(),
        };
    }

    /// <summary>
    /// Retrieves a specific event.
    /// </summary>
    /// <param name="identifier">The identifier of the event.</param>
    /// <returns>The detailed data about the event identified by <paramref name="identifier"/>.</returns>
    /// <response code="404">No event is associated to the provided <paramref name="identifier"/>.</response>
    [HttpGet]
    [AuthorizeRoles(Role.TimeRO, Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override EventForView GetOne(Guid identifier)
    {
        var entity = base.GetOne(identifier);

        return entity;
    }

    /// <summary>
    /// Creates a new event.
    /// </summary>
    /// <param name="event">The event data.</param>
    /// <returns>The event data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    public override EventForList Post(EventForEdit @event)
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        Event model = this.Mapper.Map<Event>(@event);

        this.CheckDependencies(@event, model);
        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        this.Context.Set<Event>().Add(model);
        this.Context.SaveChanges();

        // Send a notification when an event is created
        this.Notification.EventCreated(model);

        return this.Mapper.Map<EventForList>(model);
    }

    /// <summary>
    /// Deletes a specific agreement.
    /// </summary>
    /// <param name="identifier">The identifier of the agreement to delete.</param>
    /// <remarks>
    /// Deletes as well all attached <see cref="EventStatus"/>.
    /// </remarks>
    /// <response code="404">No event is associated to the provided <paramref name="identifier"/>.</response>
    [HttpDelete]
    [AuthorizeRoles(Role.Time)]
    [Produces("application/json")]
    [Route("{identifier}")]
    public override void Delete(Guid identifier)
    {
        var entity = this.Context.Set<Event>()
            .Include(e => e.Statuses)
            .SingleOrDefault(e => e.Identifier == identifier);
        if (entity == null)
        {
            throw new NotFoundException($"Not existing entity");
        }

        this.Context.Set<EventStatus>().RemoveRange(entity.Statuses);
        this.Context.Set<Event>().Remove(entity);
        this.Context.SaveChanges();
    }

    /// <summary>
    /// Retrieves the basic information about the linked entities.
    /// </summary>
    /// <param name="identifier">The identifier of the event.</param>
    /// <returns>The linked entities information.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.Time, Role.TimeRO)]
    [Produces("application/json")]
    [Route("{identifier}/links")]
    public AttachmentLinks GetLinks(Guid identifier)
    {
        var entity = this.Context
            .Set<Event>()
            .Include(c => c.Attachments)
            .SingleOrDefault(c => c.Identifier == identifier);
        if (entity == null)
        {
            throw new NotFoundException("Not existing entity");
        }

        return new AttachmentLinks()
        {
            Attachments = entity.Attachments.Count,
        };
    }

    /// <summary>
    /// Checks and maps <see cref="Event.User"/> and <see cref="Event.Category"/> info.
    /// </summary>
    /// <param name="event">The event data.</param>
    /// <param name="model">The event model instance.</param>
    protected override void CheckDependencies(EventForEdit @event, Event model)
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        else if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.User = this.Context.Set<User>()
            .SingleOrDefault(u => u.Identifier == @event.UserIdentifier);
        if (model.User == null)
        {
            this.ModelState.AddModelError(nameof(@event.UserIdentifier), "The User is invalid");
        }

        model.Category = this.Context.Set<EventCategory>()
            .SingleOrDefault(c => c.Identifier == @event.CategoryIdentifier);
        if (model.Category == null)
        {
            this.ModelState.AddModelError(nameof(@event.CategoryIdentifier), "The Event Category is invalid");
        }

        // Add the default status for a new event
        if (model.Identifier == Guid.Empty)
        {
            User user = this.GetConnectedUser();
            var requested = this.Context.GetStatus("requested");
            model.Statuses.Add(new EventStatus()
            {
                Status = requested,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = user,
            });
        }
    }

    /// <summary>
    /// Adds the <see cref="Event.User"/> and <see cref="Event.Category"/> values.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Event> AddIncludesForList(IQueryable<Event> query)
    {
        return query
            .Include(c => c.User)
            .Include(c => c.Category)
            .Include(c => c.Statuses)
            .ThenInclude(s => s.Status);
    }

    /// <summary>
    /// Adds the <see cref="Event.User"/> and <see cref="Event.Category"/> values.
    /// </summary>
    /// <param name="query">The current query.</param>
    /// <returns>The updated query.</returns>
    protected override IQueryable<Event> AddIncludesForView(IQueryable<Event> query)
    {
        return query
            .Include(c => c.User)
            .Include(c => c.Category)
            .Include(c => c.Statuses)
            .ThenInclude(s => s.Status);
    }
}
