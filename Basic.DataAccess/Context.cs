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

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);

            builder.Properties<decimal>()
                .HaveColumnType("decimal(18,6)");

            builder.Properties<Enum>()
                .HaveColumnType("nvarchar(24)");
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

            // Special configuration
            builder
                .Entity<Schedule>()
                .Property(s => s.WorkingSchedule)
                    .HasConversion<ScheduleConverter, ScheduleComparer>();

            // Special configuration specific to the data source
            builder.Entity<Event>().Property(s => s.StartDate).HasColumnType("date");
            builder.Entity<Event>().Property(s => s.EndDate).HasColumnType("date");
            builder.Entity<GlobalDayOff>().Property(s => s.Date).HasColumnType("date");

            // Set-up initial data
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
                DisplayName = "John Doe",
                Title = "User Group Evangelist",
                Salt = "demo",
            };
            demoUser.Password = demoUser.HashPassword("demo");

            builder.Entity<User>()
                .HasData(demoUser);

            foreach (var role in roles)
            {
                builder.Entity("RoleUser")
                    .HasData(new { RolesIdentifier = role.Identifier, UsersIdentifier = demoUser.Identifier });
            }

            builder.Entity<Status>()
                .HasData(new Status()
                {
                    Identifier = new Guid("52bc6354-d8ef-44e2-87ca-c64deeeb22e8"),
                    DisplayName = "Requested",
                    Description = "The associated event has been created and is waiting for approval",
                    IsActive = true,
                }, new Status()
                {
                    Identifier = new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"),
                    DisplayName = "Approved",
                    Description = "The associated event has been approved",
                    IsActive = false,
                }, new Status()
                {
                    Identifier = new Guid("e7f8dcc7-57d5-4e74-ac38-1fbd5153996c"),
                    DisplayName = "Rejected",
                    Description = "The associated event has been rejected",
                    IsActive = false,
                });
        }
    }
}
