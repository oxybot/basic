// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to initialize the database as needed.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class InitController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InitController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="logger">The associated logger.</param>
    public InitController(Context context, ILogger<InitController> logger)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the datasource context.
    /// </summary>
    protected Context Context { get; }

    /// <summary>
    /// Gets the associated logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Initializes the data.
    /// </summary>
    /// <returns>The status of the initialization.</returns>
    [HttpGet]
    [AuthorizeRoles(Role.Options)]
    [Produces("application/json")]
    public IActionResult Get()
    {
        // Update demo user to provide an email
        var demoUser = this.Context.Set<User>().FirstOrDefault(u => u.Username == "demo");
        if (demoUser != null)
        {
            demoUser.Email = "jeandoe@example.com";
            this.Context.SaveChanges();
        }

        // Create the default event categories
        if (!this.Context.Set<EventCategory>().Any())
        {
            // Create the default event categories
            this.Context.Set<EventCategory>().AddRange(new[]
            {
                new EventCategory { DisplayName = "Holidays", Mapping = EventTimeMapping.TimeOff, RequireBalance = true },
                new EventCategory { DisplayName = "Sickness Leave", Mapping = EventTimeMapping.TimeOff, RequireBalance = false },
                new EventCategory { DisplayName = "Children Sickness Leave", Mapping = EventTimeMapping.TimeOff, RequireBalance = true },
                new EventCategory { DisplayName = "Travel", Mapping = EventTimeMapping.Informational, RequireBalance = false, ColorClass = "bg-blue" },
                new EventCategory { DisplayName = "Working from Home", Mapping = EventTimeMapping.Active, RequireBalance = false, ColorClass = "bg-pink" },
            });

            this.Context.SaveChanges();
        }

        return this.Ok();
    }
}
