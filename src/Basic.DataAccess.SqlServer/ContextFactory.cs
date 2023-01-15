// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace Basic.DataAccess.SqlServer
{
    /// <summary>
    /// Internal class managing context creation for migration.
    /// </summary>
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Instancied automatically when creating new migration")]
    internal sealed class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        /// <summary>
        /// Creates a new context for migration management.
        /// </summary>
        /// <param name="args">The parameter is not used.</param>
        /// <returns>The created and initialized context.</returns>
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>()
                .UseConfiguredSqlServer();

            return new Context(optionsBuilder.Options);
        }
    }
}
