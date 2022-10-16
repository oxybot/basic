// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Basic.DataAccess
{
    /// <summary>
    /// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa.
    /// </summary>
    /// <seealso href="https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323" />
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateOnlyConverter"/> class.
        /// </summary>
        public DateOnlyConverter()
            : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }
}
