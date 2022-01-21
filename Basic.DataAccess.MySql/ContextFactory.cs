using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Basic.DataAccess.MySql
{
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
                }
            );

            return new Context(optionsBuilder.Options);
        }
    }
}
