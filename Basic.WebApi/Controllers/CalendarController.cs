using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Basic.WebApi.Controllers
{
    [ApiController]
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

        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<UserCalendar> GetAll(string month)
        {
            DateTime startOfMonth = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var days = (endOfMonth - startOfMonth).TotalDays;
            var users = Context.Set<User>();

            foreach (var user in users)
            {
                var events = user.Events
                    .Where(e => e.StartDate <= endOfMonth && e.EndDate >= startOfMonth);
                var timeoff = events.Where(e => e.Category.Mapping != EventTimeMapping.Active);
                var active = events.Where(e => e.Category.Mapping == EventTimeMapping.Active);

                var calendar = new UserCalendar() { User = Mapper.Map<EntityReference>(user) };
                if (timeoff.Any())
                {
                    var line = new UserCalendar.Line() { Category = "timeoff" };
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

                foreach(var activeGroup in active.GroupBy(e => e.Category))
                {
                    var line = new UserCalendar.Line() { Category = activeGroup.Key.DisplayName };
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
    }
}
