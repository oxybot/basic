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

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage users calendars.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public CalendarController(Context context, IMapper mapper, ILogger<CalendarController> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the datasource context.
        /// </summary>
        protected Context Context { get; }

        /// <summary>
        /// Gets the configured automapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets all events to be displayed in the calendar of a specific month.
        /// </summary>
        /// <param name="month">The month of reference (format: YYYY-MM)</param>
        /// <returns>The events to be displayed in the calendar.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<UserCalendar> GetAll(string month)
        {
            DateTime startOfMonth = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var days = (endOfMonth - startOfMonth).TotalDays;
            var users = Context.Set<User>()
                .Include(e => e.Events)
                .ThenInclude(e => e.Category);

            foreach (var user in users)
            {
                var events = user.Events
                    .Where(e => e.StartDate <= endOfMonth && e.EndDate >= startOfMonth);
                var timeoff = events.Where(e => e.Category.Mapping != EventTimeMapping.Active);
                var active = events.Where(e => e.Category.Mapping == EventTimeMapping.Active);

                var calendar = new UserCalendar() { User = Mapper.Map<EntityReference>(user) };
                if (timeoff.Any())
                {
                    var line = new UserCalendar.Line()
                    {
                        Category = "timeoff",
                        ColorClass = "bg-timeoff"
                    };

                    calendar.Lines.Add(line);
                    for (int i = 1; i <= days; i++)
                    {
                        DateTime day = startOfMonth.AddDays(i - 1);
                        if (timeoff.Any(e => e.StartDate <= day && day <= e.EndDate))
                        {
                            line.Days.Add(i);
                        }
                    }
                }

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
                        DateTime day = startOfMonth.AddDays(i - 1);
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
        /// <param name="request">The request data.</param>
        /// <returns>The balance data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public void Post(CalendarRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var context = CreateContext(request);
            if (context.Category == null)
            {
                throw new BadRequestException("Invalid category identifier");
            }
            else if (context.Schedule == null)
            {
                throw new BadRequestException("Missing working schedule for this period");
            }

            Event model = new Event()
            {
                User = context.User,
                Category = context.Category,
                Comment = request.Comment,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DurationFirstDay = request.DurationFirstDay ?? 8m,
                DurationLastDay = request.DurationLastDay ?? 8m,
                DurationTotal = context.TotalHours ?? 0m,
            };

            Context.Set<Event>().Add(model);
            Context.SaveChanges();
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
            if (request is null)
            {
                throw new BadRequestException("missing request information");
            }

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
                && request.StartDate != DateTime.MinValue
                && request.EndDate != DateTime.MinValue;

            if (!check.RequestComplete)
            {
                return check;
            }

            if (context.Schedule != null)
            {
                check.ActiveSchedule = true;
            }
            else
            {
                check.ActiveSchedule = false;
                var schedules = Context.Set<Schedule>()
                    .Where(s => s.User == context.User)
                    .Where(s => s.ActiveFrom <= request.EndDate && (s.ActiveTo == null || request.StartDate < s.ActiveTo.Value));
                check.ActiveScheduleMessage = schedules.Any()
                    ? "Multiple working schedules are impacted. Please do one request for each."
                    : "No working schedule defined for this period";
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
                // Compute total impacted hours
                context.TotalHours = 0m;
                context.TotalDays = 0;
                bool complexSchedule = context.Schedule.WorkingSchedule.Length > 7;
                Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                for (DateTime day = request.StartDate; day <= request.EndDate; day = day.AddDays(1))
                {
                    // extract the associated working schedule
                    int week = calendar.GetWeekOfYear(day, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    int index = complexSchedule && week.IsEven() ? 7 + (int)day.DayOfWeek : (int)day.DayOfWeek;
                    decimal maxHours = context.Schedule.WorkingSchedule[index];

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
