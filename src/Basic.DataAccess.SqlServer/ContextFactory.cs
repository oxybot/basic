using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Basic.DataAccess.SqlServer
{
    internal class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=basic;Trusted_Connection=True;MultipleActiveResultSets=true",
                options =>
                {
                    options.MigrationsAssembly("Basic.DataAccess.SqlServer");
                });

            return new Context(optionsBuilder.Options);
        }
    }
}
