using Basic.DataAccess;
using Basic.Model;
using System.Globalization;

namespace Basic.WebApi
{
    /// <summary>
    /// Helpers to manage working schedule and associated information (global days-off).
    /// </summary>
    public static class ScheduleHelper
    {
        /// <summary>
        /// Provides the working schedule of a specific user for a defined period.
        /// </summary>
        /// <param name="context">The current datasource context.</param>
        /// <param name="user">The reference user.</param>
        /// <param name="start">The start date of the period (included).</param>
        /// <param name="end">The end date of the period (included).</param>
        /// <returns>The list of days in the period and the associated working time.</returns>
        public static IDictionary<DateOnly, decimal> CalculateWorkingSchedule(Context context, User user, DateOnly start, DateOnly end)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            else if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var schedules = context.Set<Schedule>()
                .Where(s => s.User == user)
                .Where(s => s.ActiveFrom <= end && (s.ActiveTo == null || s.ActiveTo >= start))
                .ToList();

            var globalDaysOff = context.Set<GlobalDayOff>()
                .Where(d => start <= d.Date && d.Date <= end)
                .ToList();

            Calendar stdCalendar = CultureInfo.InvariantCulture.Calendar;

            IDictionary<DateOnly, decimal> results = new Dictionary<DateOnly, decimal>();
            for (DateOnly day = start; day <= end; day = day.AddDays(1))
            {
                if (globalDaysOff.Any(d => d.Date == day))
                {
                    results.Add(day, 0m);
                    continue;
                }

                var schedule = schedules
                    .FirstOrDefault(s => s.ActiveFrom <= day && (s.ActiveTo == null || s.ActiveTo >= day));
                if (schedule == null)
                {
                    // The user can't work on this day
                    results.Add(day, 0m);
                    continue;
                }

                int dayOfWeek = (int)day.DayOfWeek;
                int week = stdCalendar.GetWeekOfYear(day.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                if (schedule.WorkingSchedule.Length > 7 && week.IsEven())
                {
                    dayOfWeek += 7;
                }

                results.Add(day, schedule.WorkingSchedule[dayOfWeek]);
            }

            return results;
        }
    }
}
