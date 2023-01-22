// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace System.Globalization;

/// <summary>
/// Extensions methods for the <see cref="Calendar"/> class.
/// </summary>
public static class CalendarExtensions
{
    /// <summary>
    /// Returns the week of year for the specified <see cref="DateOnly"/>.
    /// The returned value is an integer between 1 and 53.
    /// </summary>
    /// <param name="calendar">The reference calendar.</param>
    /// <param name="date">THe reference date.</param>
    /// <param name="rule">The rule for counting the week.</param>
    /// <param name="firstDayOfWeek">The day used as the first day of the week.</param>
    /// <returns>The week of the year for <paramref name="date"/>.</returns>
    public static int GetWeekOfYear(this Calendar calendar, DateOnly date, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
    {
        if (calendar is null)
        {
            throw new ArgumentNullException(nameof(calendar));
        }

        DateTime time = date.ToDateTime(TimeOnly.MinValue);
        return calendar.GetWeekOfYear(time, rule, firstDayOfWeek);
    }
}
