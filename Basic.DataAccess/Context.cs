using Basic.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Basic.DataAccess
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected Context()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var modelTypes = typeof(BaseModel).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(BaseModel).IsAssignableFrom(t));

            // Register all model types
            foreach (Type type in modelTypes)
            {
                builder.Entity(type);
            }

            // Define conventions
            builder.Properties().Where(p => p.ClrType == typeof(decimal))
                .Configure(p => p.SetColumnType("decimal(18,6)"));
        }
    }
}
