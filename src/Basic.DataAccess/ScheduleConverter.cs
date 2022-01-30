using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace Basic.DataAccess
{
    internal class ScheduleConverter : ValueConverter<decimal[], string>
    {
        public ScheduleConverter()
            : base(a => string.Join(",", a), p => ConvertFromProviderFunction(p), null)
        {
        }

        private static decimal[] ConvertFromProviderFunction(string provider)
        {
            return provider.Split(",", StringSplitOptions.None)
                .Select(s => Convert.ToDecimal(s))
                .ToArray();
        }
    }
}
