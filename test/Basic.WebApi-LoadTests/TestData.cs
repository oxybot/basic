// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper.Internal;
using Basic.DataAccess;
using Basic.DataAccess.SqlServer.Migrations;
using Basic.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Basic.WebApi;

/// <summary>
/// Creates the test data for this test suite.
/// </summary>
public class TestData
{
    /// <summary>
    /// Defines the number of generated entities per model.
    /// </summary>
    public const int NumberOfEntities = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestData"/> class.
    /// </summary>
    /// <param name="context">The current context.</param>
    public TestData(Context context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets the current entity framework context.
    /// </summary>
    public Context Context { get; }

    /// <summary>
    /// Initializes a <see cref="TestData"/> instance.
    /// </summary>
    /// <param name="services">The services providing the <see cref="Context"/> instance.</param>
    /// <returns>An initialized <see cref="TestData"/> instance.</returns>
    public static TestData PopulateAll(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using Context context = scope.ServiceProvider.GetRequiredService<Context>();

        var result = new TestData(context);
        result.PopulateAll();
        return result;
    }

    /// <summary>
    /// Generates all data.
    /// </summary>
    public void PopulateAll()
    {
        this.Populate<User>();
        this.Populate<EventCategory>();
        this.Populate<Event>();
        this.PopulateEventStatus();
    }

    /// <summary>
    /// Generates the data for the <see cref="EventStatus"/> model.
    /// </summary>
    public void PopulateEventStatus()
    {
        this.Populate<Event>();

        var requested = this.Context.Set<Status>().Single(s => s.Identifier == Status.Requested);
        this.Context.Set<Event>().ToList().ForEach(e =>
        {
            e.CurrentStatus = requested;
            e.Statuses.Add(new EventStatus()
            {
                Status = requested,
                UpdatedBy = (User)this.GetRandomValueFor(typeof(User)),
                UpdatedOn = (DateTime)this.GetRandomValueFor(typeof(DateTime)),
            });
        });

        this.Context.SaveChanges();
    }

    /// <summary>
    /// Generates the data for a specific model class.
    /// </summary>
    /// <typeparam name="TModel">The reference model class.</typeparam>
    public void Populate<TModel>()
        where TModel : BaseModel, new()
    {
        for (var i = 0; i < NumberOfEntities; i++)
        {
            var entity = this.Create<TModel>();
            this.Context.Set<TModel>().Add(entity);
        }

        this.Context.SaveChanges();
    }

    /// <summary>
    /// Creates a new entity for a specific model class.
    /// </summary>
    /// <typeparam name="TModel">The reference model class.</typeparam>
    /// <returns>The initialized entity.</returns>
    public TModel Create<TModel>()
        where TModel : BaseModel, new()
    {
        TModel entity = new TModel();
        foreach (var property in typeof(TModel).GetProperties())
        {
            if (property.Name == nameof(BaseModel.Identifier))
            {
                // Do nothing for the identifier
                continue;
            }

            if (property.CanBeSet())
            {
                object value = this.GetRandomValueFor(property.PropertyType);
                property.SetValue(entity, value);
            }
        }

        return entity;
    }

    /// <summary>
    /// Creates a random value for a specific type.
    /// </summary>
    /// <param name="propertyType">The reference type.</param>
    /// <returns>Thr random value.</returns>
    [SuppressMessage(
        "Security",
        "CA5394:Do not use insecure randomness",
        Justification = "For testing only")]
    public object GetRandomValueFor(Type propertyType)
    {
        if (propertyType == null
            || propertyType == typeof(TypedFile))
        {
            return null;
        }
        else if (propertyType == typeof(string))
        {
            return Guid.NewGuid().ToString();
        }
        else if (propertyType == typeof(bool))
        {
            return Random.Shared.Next() % 2 == 1;
        }
        else if (propertyType == typeof(decimal))
        {
            return (decimal)Random.Shared.NextDouble();
        }
        else if (propertyType == typeof(EventTimeMapping))
        {
            return (EventTimeMapping)(Random.Shared.Next() % 3);
        }
        else if (propertyType == typeof(DateOnly))
        {
            int days = Random.Shared.Next(2000 * 365, 3000 * 365);
            return DateOnly.FromDayNumber(days);
        }
        else if (propertyType == typeof(DateTime))
        {
            long ticks = Random.Shared.NextInt64(2000L * 365L * 24L * 60L * 60L * 10L, 3000L * 365L * 24L * 60L * 60L * 10L);
            return new DateTime(ticks);
        }
        else if (propertyType == typeof(EventCategory))
        {
            var entities = this.Context.Set<EventCategory>();
            return entities.Skip(Random.Shared.Next(0, entities.Count())).Take(1).First();
        }
        else if (propertyType == typeof(User))
        {
            var entities = this.Context.Set<User>();
            return entities.Skip(Random.Shared.Next(0, entities.Count())).Take(1).First();
        }
        else
        {
            throw new NotImplementedException($"Random value generation not implemented for {propertyType.Name}");
        }
    }
}
