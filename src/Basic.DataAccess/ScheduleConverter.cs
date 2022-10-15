using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Basic.DataAccess
{
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Part of a converter")]
    internal class ScheduleConverter : ValueConverter<decimal[], string>
    {
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
}
