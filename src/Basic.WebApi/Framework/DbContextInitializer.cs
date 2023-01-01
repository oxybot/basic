// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Helper class for <see cref="Context"/> initialization.
    /// </summary>
    public static class DbContextInitializer
    {
        /// <summary>
        /// Initializes the options of the EF context for the application.
        /// </summary>
        /// <param name="options">The options to update.</param>
        /// <param name="configuration">The current configuration.</param>
        /// <exception cref="NotImplementedException">
        /// The <paramref name="configuration"/> doesn't contains a valid value for the <c>DatabaseDriver</c> setting.
        /// </exception>
        public static void InitializeOptions(DbContextOptionsBuilder options, IConfiguration configuration)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            string driver = configuration["DatabaseDriver"]?.ToUpperInvariant();
            switch (driver)
            {
                case "SQLSERVER":
                    options.UseConfiguredSqlServer(configuration);
                    break;

                case "MYSQL":
                    options.UseConfiguredMySql(configuration);
                    break;

                default:
                    string message = string.Format(CultureInfo.InvariantCulture, "No database configuration defined for [{0}]", configuration["DatabaseDriver"]);
                    throw new NotImplementedException(message);
            }
        }
    }
}
