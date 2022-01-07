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

            builder
                .Entity<Schedule>()
                .Property(s => s.WorkingSchedule)
                    .HasConversion<ScheduleConverter, ScheduleComparer>();

            var roles = new[]
            {
                new Role()
                {
                    Identifier = new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                    Code = Role.ClientRO,
                },
                new Role()
                {
                    Identifier = new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                    Code = Role.Client,
                },
                new Role()
                {
                    Identifier = new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                    Code = Role.TimeRO,
                },
                new Role()
                {
                    Identifier = new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                    Code = Role.Time,
                },
                new Role()
                {
                    Identifier = new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"),
                    Code = Role.User,
                },
            };
            builder.Entity<Role>().HasData(roles);

            var demoUser = new User()
            {
                Identifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"),
                Username = "demo",
                Password = "demo",
                DisplayName = "John Doe",
                Title = "User Group Evangelist",
            };

            builder.Entity<User>()
                .HasData(demoUser);

            foreach (var role in roles)
            {
                builder.Entity("RoleUser")
                    .HasData(new { RolesIdentifier = role.Identifier, UsersIdentifier = demoUser.Identifier });
            }

            // Define conventions
            builder.Properties().Where(p => p.ClrType == typeof(decimal))
                .Configure(p => p.SetColumnType("decimal(18,6)"));

            builder.Properties().Where(p => p.ClrType.IsEnum)
                .Configure(p => p.SetColumnType("nvarchar(24)"));
        }
    }
}
