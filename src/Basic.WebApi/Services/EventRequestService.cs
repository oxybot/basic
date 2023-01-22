// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Services;

/// <summary>
/// Provides helpers to validate <see cref="EventRequest"/> and <see cref="MyEventRequest"/> instances.
/// </summary>
public class EventRequestService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventRequestService"/> class.
    /// </summary>
    /// <param name="httpContext">The context associated with the current request.</param>
    /// <param name="context">The entity framework context.</param>
    public EventRequestService(IHttpContextAccessor httpContext, Context context)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        this.HttpContext = httpContext.HttpContext;
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets the context associated with the current request.
    /// </summary>
    public HttpContext HttpContext { get; }

    /// <summary>
    /// Gets the entity framework context.
    /// </summary>
    public Context Context { get; }

    /// <summary>
    /// Retrieves the data around the request.
    /// </summary>
    /// <param name="request">The current request.</param>
    /// <returns>The context associated to the request.</returns>
    public EventRequestContext CreateContext(MyEventRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var userIdClaim = this.HttpContext.User.Claims.Single(c => c.Type == "sid:guid");
        var userIdentifier = Guid.Parse(userIdClaim.Value);

        return this.CreateContextCore(userIdentifier, request);
    }

    /// <summary>
    /// Retrieves the data around the request.
    /// </summary>
    /// <param name="request">The current request.</param>
    /// <returns>The context associated to the request.</returns>
    public EventRequestContext CreateContext(EventRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return this.CreateContextCore(request.UserIdentifier, request);
    }

    /// <summary>
    /// Validates a specific request and updates the associated model state.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="modelState">The model state to update.</param>
    /// <returns>The context associated with the validation.</returns>
    public EventRequestContext Validate(MyEventRequest request, ModelStateDictionary modelState)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        else if (modelState is null)
        {
            throw new ArgumentNullException(nameof(modelState));
        }

        EventRequestContext context = this.CreateContext(request);

        if (context.Category == null)
        {
            string message = "Invalid category selected";
            modelState.AddModelError(nameof(request.CategoryIdentifier), message);
        }

        if (context.Schedule == null)
        {
            var schedules = this.Context.Set<Schedule>()
                .Where(s => s.User == context.User)
                .Where(s => s.ActiveFrom <= request.EndDate && (s.ActiveTo == null || request.StartDate < s.ActiveTo.Value));

            string errorMessage = schedules.Any()
                ? "Multiple working schedules are impacted. Please do one request for each."
                : "No working schedule defined for this period";
            modelState.AddModelError(string.Empty, errorMessage);
        }

        if (!modelState.IsValid)
        {
            // Errors already present on the basic elements
            return context;
        }

        if (request.DurationFirstDay != null)
        {
            decimal firstDay = context.Schedule.For(request.StartDate.Value);
            if (firstDay == 0m)
            {
                string message = "Can't define a partial day on a non-working day";
                modelState.AddModelError(nameof(request.DurationFirstDay), message);
            }
            else if (firstDay < request.DurationFirstDay.Value)
            {
                string message = "Duration above a normal working day duration";
                modelState.AddModelError(nameof(request.DurationFirstDay), message);
            }
        }

        if (request.DurationLastDay != null)
        {
            decimal lastDay = context.Schedule.For(request.EndDate.Value);
            if (lastDay == 0m)
            {
                string message = "Can't define a partial day on a non-working day";
                modelState.AddModelError(nameof(request.DurationLastDay), message);
            }
            else if (lastDay < request.DurationLastDay.Value)
            {
                string message = "Duration above a normal working day duration";
                modelState.AddModelError(nameof(request.DurationLastDay), message);
            }
        }

        if (context.Category.Mapping == EventTimeMapping.TimeOff)
        {
            // Conflicts on time-off
            var conflicts = this.Context.Set<Event>()
                .Include(e => e.Statuses)
                .ThenInclude(e => e.Status)
                .Where(e => e.Category.Mapping == EventTimeMapping.TimeOff)
                .Where(e => e.StartDate <= request.EndDate && request.StartDate <= e.EndDate)
                .Where(e => e.User == context.User)
                .ToList()
                .Where(e => e.CurrentStatus.IsActive);

            if (conflicts.Any())
            {
                string message = "There is already registered time-off on the same period";
                modelState.AddModelError(nameof(request.StartDate), message);
                modelState.AddModelError(nameof(request.EndDate), message);
            }

            // Refuse time-off request for 0 hours
            if (context.TotalHours.HasValue && context.TotalHours.Value == 0m)
            {
                string message = "Can't request a time-off on non-working time";
                modelState.AddModelError(string.Empty, message);
            }
        }
        else
        {
            // Conflicts on an active or informational request
            var conflicts = this.Context.Set<Event>()
                .Where(e => e.Category == context.Category)
                .Where(e => e.StartDate <= request.EndDate && request.StartDate <= e.EndDate)
                .Where(e => e.User == context.User)
                .ToList()
                .Where(e => e.CurrentStatus.IsActive);

            if (conflicts.Any())
            {
                string message = $"There is already registered '{context.Category.DisplayName}' on the same period";
                modelState.AddModelError(nameof(request.StartDate), message);
                modelState.AddModelError(nameof(request.EndDate), message);
            }
        }

        return context;
    }

    /// <summary>
    /// Retrieves the data around the request.
    /// </summary>
    /// <param name="userIdentifier">The identifier of the user.</param>
    /// <param name="request">The current request.</param>
    /// <returns>The context associated to the request.</returns>
    protected EventRequestContext CreateContextCore(Guid? userIdentifier, MyEventRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var context = new EventRequestContext();

        // Retrieve user
        context.User = this.Context.Set<User>()
            .SingleOrDefault(u => u.Identifier == userIdentifier);

        // Retrieve category
        context.Category = this.Context.Set<EventCategory>()
            .SingleOrDefault(c => c.Identifier == request.CategoryIdentifier);

        // Retrieve schedule
        context.Schedule = this.Context.Set<Schedule>()
            .Where(s => s.User.Identifier == userIdentifier)
            .Where(s => s.ActiveFrom <= request.StartDate && (s.ActiveTo == null || request.EndDate < s.ActiveTo.Value))
            .SingleOrDefault();

        if (context.User == null || context.Category == null || context.Schedule == null)
        {
            return context;
        }

        if (context.Category.Mapping == EventTimeMapping.Informational)
        {
            context.TotalDays = request.EndDate.Value.DayNumber - request.StartDate.Value.DayNumber + 1;
        }
        else
        {
            var workingSchedule = ScheduleHelper.CalculateWorkingSchedule(this.Context, context.User, request.StartDate.Value, request.EndDate.Value);

            // Compute total impacted hours
            context.TotalHours = 0m;
            context.TotalDays = 0;
            for (DateOnly day = request.StartDate.Value; day <= request.EndDate.Value; day = day.AddDays(1))
            {
                decimal maxHours = workingSchedule[day];

                if (maxHours == 0m)
                {
                    // Skip the day
                    continue;
                }

                context.TotalDays++;
                if (day == request.StartDate && request.DurationFirstDay.HasValue)
                {
                    context.TotalHours += request.DurationFirstDay.Value;
                }
                else if (day == request.EndDate && request.DurationLastDay.HasValue)
                {
                    context.TotalHours += request.DurationLastDay.Value;
                }
                else
                {
                    context.TotalHours += maxHours;
                }
            }
        }

        return context;
    }
}
