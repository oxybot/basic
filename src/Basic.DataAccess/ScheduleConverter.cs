﻿// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Basic.DataAccess;

/// <summary>
/// Converts <see cref="Schedule.WorkingSchedule"/> values to and from string value.
/// </summary>
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Part of a converter")]
internal sealed class ScheduleConverter : ValueConverter<decimal[], string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduleConverter"/> class.
    /// </summary>
    public ScheduleConverter()
        : base(a => string.Join(",", a), p => ConvertFromProviderFunction(p), null)
    {
    }

    private static decimal[] ConvertFromProviderFunction(string provider)
    {
        return provider.Split(",", StringSplitOptions.None)
            .Select(s => Convert.ToDecimal(s, CultureInfo.InvariantCulture))
            .ToArray();
    }
}
