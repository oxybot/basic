// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Framework;

/// <summary>
/// Creates configuration provider based on the entity framework context.
/// </summary>
public class DatabaseConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConfigurationSource"/> class.
    /// </summary>
    /// <param name="logger">The logger associated with the <see cref="DatabaseConfigurationSource"/> class.</param>
    /// <param name="configuration">The configuration associated to the application and to the database connection.</param>
    public DatabaseConfigurationSource(ILogger<DatabaseConfigurationSource> logger, IConfiguration configuration)
    {
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Gets the logger associated with the class.
    /// </summary>
    public ILogger<DatabaseConfigurationSource> Logger { get; }

    /// <summary>
    /// Gets the configuration associated to the application and to the database connection.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Creates a new instance of the configuration builder.
    /// </summary>
    /// <param name="builder">The current configuration builder.</param>
    /// <returns>The created configuration builder.</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        this.Logger.LogDebug("Create a new DatabaseConfigurationProvider");
        return new DatabaseConfigurationProvider(this);
    }
}
