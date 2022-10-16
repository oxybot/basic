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

namespace Basic.WebApi.Controllers
{
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
                var events = user.Events
                    .Where(e => e.StartDate <= endOfMonth && e.EndDate >= startOfMonth)
                    .Where(e => e.CurrentStatus.IsActive);
                var timeoff = events.Where(e => e.Category.Mapping != EventTimeMapping.Active);
                var active = events.Where(e => e.Category.Mapping == EventTimeMapping.Active);

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
                    var line = new UserCalendar.Line()
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
                foreach (var activeGroup in active.GroupBy(e => e.Category))
                {
                    var line = new UserCalendar.Line()
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
        /// <param name="request">The request data.</param>
        /// <returns>The balance data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public EventForList Post([FromServices] EmailService notification, CalendarRequest request)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            else if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var context = this.CreateContext(request);
            this.Validate(context, request);

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
                DurationFirstDay = request.DurationFirstDay ?? 8m,
                DurationLastDay = request.DurationLastDay ?? 8m,
                DurationTotal = context.TotalHours ?? 0m,
            };

            User user = this.GetConnectedUser();
            var requested = this.Context.GetStatus("Requested");
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
        /// <param name="request">The current request.</param>
        /// <returns>The status and impacts of the request.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Route("check")]
        [Produces("application/json")]
        public CalendarRequestCheck Check(CalendarRequest request)
        {
            // All the technical tests are managed prior to this point (i.e. required fields...)
            // Are added here the business and reference checks

            var context = this.CreateContext(request);
            this.Validate(context, request);
            if (!this.ModelState.IsValid)
            {
                throw new InvalidModelStateException(this.ModelState);
            }

            return new CalendarRequestCheck
            {
                TotalHours = context.TotalHours,
                TotalDays = context.TotalDays,
            };
        }

        private void Validate(CalendarRequestContext context, CalendarRequest request)
        {
            if (context.Category == null)
            {
                this.ModelState.AddModelError(nameof(request.CategoryIdentifier), "Invalid category selected");
            }

            if (context.Schedule == null)
            {
                var schedules = this.Context.Set<Schedule>()
                    .Where(s => s.User == context.User)
                    .Where(s => s.ActiveFrom <= request.EndDate && (s.ActiveTo == null || request.StartDate < s.ActiveTo.Value));

                this.ModelState.AddModelError(string.Empty, schedules.Any()
                    ? "Multiple working schedules are impacted. Please do one request for each."
                    : "No working schedule defined for this period");
            }

            if (!this.ModelState.IsValid)
            {
                // Errors already present on the basic elements
                return;
            }

            if (context.Category.Mapping != EventTimeMapping.Active)
            {
                // Conflicts on time-off
                var conflicts = this.Context.Set<Event>()
                    .Where(e => e.Category.Mapping != EventTimeMapping.Active)
                    .Where(e => e.StartDate <= request.EndDate && request.StartDate <= e.EndDate)
                    .Where(e => e.User == context.User);

                if (conflicts.Any())
                {
                    string message = "There is already registered time-off on the same period";
                    this.ModelState.AddModelError(nameof(request.StartDate), message);
                    this.ModelState.AddModelError(nameof(request.EndDate), message);
                }

                // Refuse time-off request for 0 hours
                if (context.TotalHours.HasValue && context.TotalHours.Value == 0m)
                {
                    this.ModelState.AddModelError(string.Empty, "Can't request a time-off on non-working time");
                }
            }
            else
            {
                // Conflicts on active days
                var conflicts = this.Context.Set<Event>()
                    .Where(e => e.Category == context.Category)
                    .Where(e => e.StartDate <= request.EndDate && request.StartDate <= e.EndDate);

                if (conflicts.Any())
                {
                    string message = $"There is already registered '{context.Category.DisplayName}' on the same period";
                    this.ModelState.AddModelError(nameof(request.StartDate), message);
                    this.ModelState.AddModelError(nameof(request.EndDate), message);
                }
            }
        }

        /// <summary>
        /// Retrieves the data around the request.
        /// </summary>
        /// <param name="request">The current request.</param>
        /// <returns>The context associated to the request.</returns>
        private CalendarRequestContext CreateContext(CalendarRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userIdClaim = this.User.Claims.Single(c => c.Type == "sid:guid");
            var userId = Guid.Parse(userIdClaim.Value);
            var context = new CalendarRequestContext();

            // Retrieve user
            context.User = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == userId);

            // Retrieve category
            context.Category = this.Context.Set<EventCategory>().SingleOrDefault(c => c.Identifier == request.CategoryIdentifier);

            // Retrieve schedule
            context.Schedule = this.Context.Set<Schedule>()
                .Where(s => s.User.Identifier == userId)
                .Where(s => s.ActiveFrom <= request.StartDate && (s.ActiveTo == null || request.EndDate < s.ActiveTo.Value))
                .SingleOrDefault();

            if (context.User == null || context.Category == null || context.Schedule == null)
            {
                return context;
            }

            if (context.Category.Mapping != EventTimeMapping.Active)
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
}
