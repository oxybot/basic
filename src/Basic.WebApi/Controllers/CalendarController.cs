using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Basic.WebApi.Services;

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
        /// <param name="month">The month of reference (format: YYYY-MM)</param>
        /// <returns>The events to be displayed in the calendar.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<UserCalendar> GetAll(string month)
        {
            DateOnly startOfMonth = DateOnly.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            DateOnly endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var days = endOfMonth.Day;
            var users = Context.Set<User>()
                .Include(e => e.Events)
                    .ThenInclude(e => e.Category)
                .Include(e => e.Events)
                    .ThenInclude(e => e.Statuses)
                        .ThenInclude(e => e.Status)
                .Where(u => u.Schedules.Any(s => s.ActiveFrom <= endOfMonth && (s.ActiveTo == null || s.ActiveTo >= startOfMonth)))
                .ToList();

            foreach (var user in users)
            {
                var events = user.Events
                    .Where(e => e.StartDate <= endOfMonth && e.EndDate >= startOfMonth)
                    .Where(e => e.CurrentStatus.IsActive);
                var timeoff = events.Where(e => e.Category.Mapping != EventTimeMapping.Active);
                var active = events.Where(e => e.Category.Mapping == EventTimeMapping.Active);

                var calendar = new UserCalendar() { User = Mapper.Map<EntityReference>(user) };

                // Add days off
                var workingSchedule = ScheduleHelper.CalculateWorkingSchedule(Context, user, startOfMonth, endOfMonth);
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
                        ColorClass = "bg-timeoff"
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
                        ColorClass = activeGroup.Key.ColorClass
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
        public EventForList Post([FromServices]EmailService notification, CalendarRequest request)
        {
            var context = CreateContext(request);
            if (context.Category == null)
            {
                ModelState.AddModelError("CategoryIdentifier", "Invalid category");
            }

            if (context.Schedule == null)
            {
                ModelState.AddModelError("", "Missing working schedule for this period");
            }

            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
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
                UpdatedBy = user
            });

            User userRequest = model.User;
            if (userRequest == null)
            {
                ModelState.AddModelError("User", "The User is invalid");
            }

            EventCategory category = model.Category;
            if (category == null)
            {
                ModelState.AddModelError("Category", "The event category is invalid");
            }

            // Send an email notification when an event is created
            notification.EmailToManagers(category, userRequest, model);

            Context.Set<Event>().Add(model);
            Context.SaveChanges();

            return Mapper.Map<EventForList>(model);
        }

        /// <summary>
        /// Checks a request calendar and provides current status and impacts.
        /// </summary>
        /// <param name="request">The current request.</param>
        /// <returns>The status and impacts of the request.</returns>
        [HttpPost]
        [Route("check")]
        [Produces("application/json")]
        public CalendarRequestCheck Check(CalendarRequest request)
        {
            var check = new CalendarRequestCheck();
            if (!ModelState.IsValid)
            {
                check.RequestCompleteMessage = ModelState.SelectMany(m => m.Value.Errors).First().ErrorMessage;
                return check;
            }

            var errors = request.Validate(null);
            if (errors.Any())
            {
                check.RequestCompleteMessage = errors.First().ErrorMessage;
                return check;
            }

            var context = CreateContext(request);
            check.RequestComplete = context.Category != null
                && request.StartDate != DateOnly.MinValue
                && request.EndDate != DateOnly.MinValue;

            if (!check.RequestComplete)
            {
                return check;
            }

            check.ActiveSchedule = context.Schedule != null;
            if (!check.ActiveSchedule)
            {
                check.ActiveSchedule = false;
                var schedules = Context.Set<Schedule>()
                    .Where(s => s.User == context.User)
                    .Where(s => s.ActiveFrom <= request.EndDate && (s.ActiveTo == null || request.StartDate < s.ActiveTo.Value));
                check.ActiveScheduleMessage = schedules.Any()
                    ? "Multiple working schedules are impacted. Please do one request for each."
                    : "No working schedule defined for this period";

                return check;
            }

            if (context.Category.Mapping != EventTimeMapping.Active)
            {
                // Conflicts on time-off
                var conflicts = Context.Set<Event>()
                    .Where(e => e.Category.Mapping != EventTimeMapping.Active)
                    .Where(e => e.StartDate <= request.EndDate && request.StartDate <= e.EndDate);

                check.NoConflict = !conflicts.Any();
                if (!check.NoConflict)
                {
                    check.NoConflictMessage = "There is already registered time-off on the same period";
                    return check;
                }
            }
            else
            {
                // Conflicts on active days
                var conflicts = Context.Set<Event>()
                    .Where(e => e.Category == context.Category)
                    .Where(e => e.StartDate <= request.EndDate && request.StartDate <= e.EndDate);

                check.NoConflict = !conflicts.Any();
                if (!check.NoConflict)
                {
                    check.NoConflictMessage = "There is already registered '" + context.Category.DisplayName + "' on the same period";
                    return check;
                }
            }

            check.TotalHours = context.TotalHours;
            check.TotalDays = context.TotalDays;

            return check;
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
            context.User = Context.Set<User>().SingleOrDefault(u => u.Identifier == userId);

            // Retrieve category
            context.Category = Context.Set<EventCategory>().SingleOrDefault(c => c.Identifier == request.CategoryIdentifier);

            // Retrieve schedule
            context.Schedule = Context.Set<Schedule>()
                .Where(s => s.User.Identifier == userId)
                .Where(s => s.ActiveFrom <= request.StartDate && (s.ActiveTo == null || request.EndDate < s.ActiveTo.Value))
                .SingleOrDefault();

            if (context.User == null || context.Category == null || context.Schedule == null)
            {
                return context;
            }

            if (context.Category.Mapping != EventTimeMapping.Active)
            {
                var workingSchedule = ScheduleHelper.CalculateWorkingSchedule(Context, context.User, request.StartDate.Value, request.EndDate.Value);

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
