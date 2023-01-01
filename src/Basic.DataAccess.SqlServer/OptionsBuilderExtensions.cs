// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Configuration;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for the <see cref="DbContextOptionsBuilder"/> class.
    /// </summary>
    public static class OptionsBuilderExtensions
    {
        /// <summary>
        /// Prepares the options to support SqlServer with migrations.
        /// </summary>
        /// <typeparam name="T">The current type of options builder.</typeparam>
        /// <param name="optionsBuilder">The options builder to update.</param>
        /// <param name="configuration">The global configuration including SqlServer connection string, if any.</param>
        /// <returns>The configured options builder.</returns>
        /// <remarks>
        /// If the <paramref name="configuration"/> parameter is <c>null</c> as default and local configuration
        /// is used. This case is required for the design mode and migration management.
        /// </remarks>
        public static T UseConfiguredSqlServer<T>(this T optionsBuilder, IConfiguration configuration = null)
            where T : DbContextOptionsBuilder
        {
            string connectionString = configuration != null
                ? configuration.GetConnectionString("SqlServer")
                : "Server=(localdb)\\mssqllocaldb;Database=basic;Trusted_Connection=True;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(
                connectionString,
                options =>
                {
                    string assemblyName = typeof(Basic.DataAccess.SqlServer.Migrations.InitialCreate).Assembly.FullName;
                    options.MigrationsAssembly(assemblyName);
                });

            return optionsBuilder;
        }
    }
}
