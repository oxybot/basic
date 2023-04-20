// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Basic.DataAccess;

/// <summary>
/// Represents the entity framework context specific to the project.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class.
    /// </summary>
    /// <param name="options">The configuration used to initialize the context.</param>
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class.
    /// </summary>
    protected Context()
    {
    }

    /// <inheritdoc />
    public override int SaveChanges()
    {
        var entries = this.GetChangedEntries();
        int result = base.SaveChanges();
        NotifyEntityChange(entries);
        return result;
    }

    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = this.GetChangedEntries();
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        NotifyEntityChange(entries);
        return result;
    }

    /// <summary>
    /// Configures the conventions specific to the project.
    /// </summary>
    /// <param name="configurationBuilder">The current configuration builder.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        if (configurationBuilder is null)
        {
            throw new ArgumentNullException(nameof(configurationBuilder));
        }

        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>()
            .HaveColumnType("decimal(18,6)");

        configurationBuilder.Properties<Enum>()
            .HaveColumnType("nvarchar(24)");

        configurationBuilder.Properties<DateOnly>()
            .HaveColumnType("date");

        configurationBuilder.Properties<DateOnly?>()
            .HaveColumnType("date");

        // https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>();

        // https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
        configurationBuilder.Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter>();
    }

    /// <summary>
    /// Called when the entity model should be created.
    /// </summary>
    /// <param name="modelBuilder">The current model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        var modelTypes = typeof(BaseModel).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(BaseModel).IsAssignableFrom(t));

        // Register all model types
        foreach (Type type in modelTypes)
        {
            var typeBuilder = modelBuilder.Entity(type);

            // Manage [Unique] attributes
            foreach (var property in type.GetProperties().Where(p => p.GetCustomAttribute<UniqueAttribute>() != null))
            {
                typeBuilder.HasIndex(property.Name).IsUnique();
            }
        }

        // Special configuration for Schedule.WorkingSchedule
        modelBuilder
            .Entity<Schedule>()
            .Property(s => s.WorkingSchedule)
            .HasConversion<ScheduleConverter, ScheduleComparer>();

        // Special configuration for Balance.Details
        modelBuilder
            .Entity<Balance>()
            .HasMany(e => e.Details)
            .WithOne(e => e.Balance)
            .OnDelete(DeleteBehavior.Cascade);

        // Special configuration for Settings
        modelBuilder
            .Entity<Setting>()
            .HasIndex(nameof(Setting.Section), nameof(Setting.Key))
            .IsUnique();

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
            new Role()
            {
                Identifier = new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"),
                Code = Role.Beta,
            },
            new Role()
            {
                Identifier = new Guid("e7f909f2-2af9-42d8-bfd0-5ca96022cba2"),
                Code = Role.Options,
            },
        };
        modelBuilder.Entity<Role>().HasData(roles);

        var demoUser = new User()
        {
            Identifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"),
            Username = "demo",
            DisplayName = "John Doe",
            Title = "User Group Evangelist",
            Salt = "demo",
            IsActive = true,
        };
        demoUser.Password = demoUser.HashPassword("demo");

        modelBuilder.Entity<User>()
            .HasData(demoUser);

        foreach (var role in roles)
        {
            modelBuilder.Entity("RoleUser")
                .HasData(new { RolesIdentifier = role.Identifier, UsersIdentifier = demoUser.Identifier });
        }

        modelBuilder.Entity<Status>()
            .HasData(
            new Status()
            {
                Identifier = Status.Requested,
                DisplayName = "Requested",
                Description = "The associated event has been created and is waiting for approval",
                IsActive = true,
            },
            new Status()
            {
                Identifier = Status.Approved,
                DisplayName = "Approved",
                Description = "The associated event has been approved",
                IsActive = true,
            },
            new Status()
            {
                Identifier = Status.Rejected,
                DisplayName = "Rejected",
                Description = "The associated event has been rejected",
                IsActive = false,
            },
            new Status()
            {
                Identifier = Status.Canceled,
                DisplayName = "Canceled",
                Description = "The associated event has been canceled",
                IsActive = false,
            });
    }

    /// <summary>
    /// Notifies on any entity change based on <see cref="EntityChangeObserver"/>.
    /// </summary>
    /// <param name="entries">The changed entities.</param>
    private static void NotifyEntityChange(IEnumerable<EntityEntry> entries)
    {
        if (entries == null)
        {
            return;
        }

        foreach (var entry in entries)
        {
            EntityChangeObserver.Instance.OnChanged(new EntityChangeEventArgs(entry));
        }
    }

    /// <summary>
    /// Retrieves a stable list of the changed entities.
    /// </summary>
    /// <returns>The list of changes entities.</returns>
    private IEnumerable<EntityEntry> GetChangedEntries()
    {
        return this.ChangeTracker.Entries()
            .Where(i => i.State == EntityState.Modified || i.State == EntityState.Added)
            .ToList();
    }
}
