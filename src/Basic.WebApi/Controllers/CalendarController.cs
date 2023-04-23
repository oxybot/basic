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
using System.Globalization;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve and manage users calendars.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class CalendarController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public CalendarController(Context context, IMapper mapper, ILogger<CalendarController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Gets all events to be displayed in the calendar of a specific month.
    /// </summary>
    /// <param name="month">The month of reference (format: YYYY-MM).</param>
    /// <returns>The events to be displayed in the calendar.</returns>
    [HttpGet]
    [Produces("application/json")]
    public IEnumerable<UserCalendar> GetAll(string month)
    {
        DateOnly startOfMonth = DateOnly.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
        DateOnly endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
        var days = endOfMonth.Day;
        var users = this.Context.Set<User>()
            .Include(e => e.Events)
                .ThenInclude(e => e.Category)
            .Include(e => e.Events)
                .ThenInclude(e => e.Statuses)
                    .ThenInclude(e => e.Status)
            .Where(u => u.Schedules.Any(s => s.ActiveFrom <= endOfMonth && (s.ActiveTo == null || s.ActiveTo >= startOfMonth)))
            .OrderBy(u => u.DisplayName)
            .ToList();

        foreach (var user in users)
        {
#pragma warning disable CA1851 // Possible multiple enumerations of 'IEnumerable' collection - That is the idea
            var events = user.Events
                .Where(e => e.StartDate <= endOfMonth && e.EndDate >= startOfMonth)
                .Where(e => e.CurrentStatus.IsActive);
            var timeoff = events.Where(e => e.Category.Mapping == EventTimeMapping.TimeOff).ToList();
            var others = events.Where(e => e.Category.Mapping != EventTimeMapping.TimeOff).ToList();
#pragma warning restore CA1851 // Possible multiple enumerations of 'IEnumerable' collection

            var calendar = new UserCalendar() { User = this.Mapper.Map<EntityReference>(user) };

            // Add days off
            var workingSchedule = ScheduleHelper.CalculateWorkingSchedule(this.Context, user, startOfMonth, endOfMonth);
            foreach (var date in workingSchedule.Where(d => d.Value == 0m).Select(p => p.Key))
            {
                calendar.DaysOff.Add(date.Day);
            }

            // Add line for time-off, if any
            if (timeoff.Any())
            {
                var line = new UserCalendarLine()
                {
                    Category = "Time-off",
                    ColorClass = "bg-timeoff",
                };

                calendar.Lines.Add(line);
                for (int i = 1; i <= days; i++)
                {
                    DateOnly day = startOfMonth.AddDays(i - 1);
                    if (timeoff.Any(e => e.StartDate <= day && day <= e.EndDate))
                    {
                        line.Days.Add(i);
                    }
                }
            }

            // Add lines for active categories
            foreach (var activeGroup in others.GroupBy(e => e.Category))
            {
                var line = new UserCalendarLine()
                {
                    Category = activeGroup.Key.DisplayName,
                    ColorClass = activeGroup.Key.ColorClass,
                };

                calendar.Lines.Add(line);
                for (int i = 1; i <= days; i++)
                {
                    DateOnly day = startOfMonth.AddDays(i - 1);
                    if (activeGroup.Any(e => e.StartDate <= day && day <= e.EndDate))
                    {
                        line.Days.Add(i);
                    }
                }
            }

            yield return calendar;
        }
    }

    /// <summary>
    /// Creates a request.
    /// </summary>
    /// <param name="notification">The email service used for notification.</param>
    /// <param name="requestService">The service used to validate the request.</param>
    /// <param name="request">The request data.</param>
    /// <returns>The balance data after creation.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [Produces("application/json")]
    public EventForList Post([FromServices] EmailService notification, [FromServices] EventRequestService requestService, MyEventRequest request)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        else if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        else if (requestService is null)
        {
            throw new ArgumentNullException(nameof(requestService));
        }

        var context = requestService.Validate(request, this.ModelState);
        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        Event model = new Event()
        {
            User = context.User,
            Category = context.Category,
            Comment = request.Comment,
            StartDate = request.StartDate.Value,
            EndDate = request.EndDate.Value,
            DurationFirstDay = request.DurationFirstDay ?? context.Schedule.For(request.StartDate.Value),
            DurationLastDay = request.DurationLastDay ?? context.Schedule.For(request.StartDate.Value),
            DurationTotal = context.TotalHours ?? 0m,
        };

        User user = this.GetConnectedUser();
        var requested = this.Context.GetStatus("Requested");
        model.CurrentStatus = requested;
        model.Statuses.Add(new EventStatus()
        {
            Status = requested,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = user,
        });

        this.Context.Set<Event>().Add(model);
        this.Context.SaveChanges();

        // Send a notification when an event is created
        notification.EventCreated(model);

        return this.Mapper.Map<EventForList>(model);
    }

    /// <summary>
    /// Checks a request calendar and provides current status and impacts.
    /// </summary>
    /// <param name="requestService">The service used to validate the request.</param>
    /// <param name="request">The current request.</param>
    /// <returns>The status and impacts of the request.</returns>
    /// <response code="400">The provided data are invalid.</response>
    [HttpPost]
    [Route("check")]
    [Produces("application/json")]
    public EventRequestCheck Check([FromServices] EventRequestService requestService, MyEventRequest request)
    {
        if (requestService is null)
        {
            throw new ArgumentNullException(nameof(requestService));
        }

        // All the technical tests are managed prior to this point (i.e. required fields...)
        // Are added here the business and reference checks
        var context = requestService.Validate(request, this.ModelState);
        if (!this.ModelState.IsValid)
        {
            throw new InvalidModelStateException(this.ModelState);
        }

        return new EventRequestCheck
        {
            TotalHours = context.TotalHours,
            TotalDays = context.TotalDays,
        };
    }
}
