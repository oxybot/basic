// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Basic.WebApi;

/// <summary>
/// Contains reference to model instances created a reference for the various tests.
/// </summary>
public class TestReferences
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestReferences"/> class.
    /// </summary>
    private TestReferences()
    {
    }

    /// <summary>
    /// Gets a client reference.
    /// </summary>
    public EntityReference Client { get; private set; }

    /// <summary>
    /// Gets a user reference.
    /// </summary>
    public UserReference User { get; private set; }

    /// <summary>
    /// Gets a category reference.
    /// </summary>
    public EntityReference Category { get; private set; }

    /// <summary>
    /// Gets the reference associated with the status "requested".
    /// </summary>
    public StatusReference StatusRequested { get; private set; }

    /// <summary>
    /// Builds the references.
    /// </summary>
    /// <param name="services">The service provider to retrieve the entity framework context.</param>
    /// <returns>The initialized references.</returns>
    public static TestReferences Build(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using Context context = scope.ServiceProvider.GetRequiredService<Context>();

        var client = new Client() { DisplayName = "ref", FullName = "ref client" };
        context.Set<Client>().Add(client);

        var user = new User() { Username = "ref", DisplayName = "ref user" };
        context.Set<User>().Add(user);

        var category = new EventCategory() { DisplayName = "ref category", ColorClass = "#ff0000", Mapping = EventTimeMapping.TimeOff };
        context.Set<EventCategory>().Add(category);

        var requested = context.Set<Status>().Single(e => e.Identifier == Status.Requested);

        context.SaveChanges();

        return new TestReferences
        {
            Client = new() { Identifier = client.Identifier, DisplayName = client.DisplayName },
            User = new() { Identifier = user.Identifier, DisplayName = user.DisplayName },
            Category = new() { Identifier = category.Identifier, DisplayName = category.DisplayName },
            StatusRequested = new() { Identifier = requested.Identifier, DisplayName = requested.DisplayName, Description = requested.Description },
        };
    }
}
