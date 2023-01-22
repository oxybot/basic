// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Model;

/// <summary>
/// Extensions methods for the <see cref="Schedule"/> class.
/// </summary>
public static class ScheduleExtensions
{
    /// <summary>
    /// Retrieves the working time of specific date and schedule.
    /// </summary>
    /// <param name="schedule">The reference.</param>
    /// <param name="date">The specific date.</param>
    /// <returns>
    /// The working time defined in the <paramref name="schedule"/> for this <paramref name="date"/>.
    /// </returns>
    public static decimal For(this Schedule schedule, DateOnly date)
    {
        if (schedule is null)
        {
            throw new ArgumentNullException(nameof(schedule));
        }

        if (date < schedule.ActiveFrom)
        {
            throw new ArgumentException("The date is not part of the working schedule", nameof(date));
        }

        if (schedule.ActiveTo != null && date > schedule.ActiveTo.Value)
        {
            throw new ArgumentException("The date is not part of the working schedule", nameof(date));
        }

        Calendar stdCalendar = CultureInfo.InvariantCulture.Calendar;
        int dayOfWeek = (int)date.DayOfWeek;
        int week = stdCalendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        if (schedule.WorkingSchedule.Length > 7 && week.IsEven())
        {
            dayOfWeek += 7;
        }

        return schedule.WorkingSchedule[dayOfWeek];
    }
}
