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
            // KEVIN : créé 3 variables pour essais 
            DateOnly dt1 = new DateOnly(2022, 06, 24);
            DateOnly dt2 = new DateOnly(2022, 06, 01);
            Guid user1 = new Guid("08da52c8-2869-4ad8-8b26-c57cc12751a4");
           

            var schedules = context.Set<Schedule>()
                .Where(s => s.User == user)
                // .Where(s => s.User.Identifier == user1)
                .Where(s => s.ActiveFrom <= end && (s.ActiveTo == null || s.ActiveTo >= start))
                // .Where(s => s.ActiveFrom <= dt1 && (s.ActiveTo == null || s.ActiveTo >= dt2))

                .ToList();

            var globalDaysOff = context.Set<GlobalDayOff>()
                .Where(d => start <= d.Date && d.Date <= end)
                // .Where(d => dt2 <= d.Date && d.Date <= dt1)
                .ToList();

            Calendar stdCalendar = CultureInfo.InvariantCulture.Calendar;

            IDictionary<DateOnly, decimal> results = new Dictionary<DateOnly, decimal>();
            // for (DateOnly day = dt2; day <= dt1; day = day.AddDays(1))
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
