// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace Basic.DataAccess.MySql
{
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Instancied automatically when creating new migration")]
    internal class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();

            string connectionString = "Server=localhost;Database=basic;User=basic;Password=basic";
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                options =>
                {
                    options.MigrationsAssembly("Basic.DataAccess.MySql");
                });

            return new Context(optionsBuilder.Options);
        }
    }
}
