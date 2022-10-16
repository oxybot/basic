// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Basic.DataAccess
{
    /// <summary>
    /// Converts <see cref="Nullable{DateOnly}" /> to <see cref="Nullable{DateTime}"/> and vice versa.
    /// </summary>
    /// <seealso href="https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323" />
    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public NullableDateOnlyConverter()
            : base(
            d => d == null
                ? null
                : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
            d => d == null
                ? null
                : new DateOnly?(DateOnly.FromDateTime(d.Value)))
        { }
    }
}
