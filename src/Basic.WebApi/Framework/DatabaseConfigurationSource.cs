// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Creates configuration provider based on the entity framework context.
    /// </summary>
    public class DatabaseConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfigurationSource"/> class.
        /// </summary>
        /// <param name="services">The service provider used for configuration initialization.</param>
        /// <param name="logger">The logger associated with the <see cref="DatabaseConfigurationSource"/> class.</param>
        public DatabaseConfigurationSource(IServiceProvider services, ILogger<DatabaseConfigurationSource> logger)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the service provider used for configuration initialization.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets the logger associated with the class.
        /// </summary>
        public ILogger<DatabaseConfigurationSource> Logger { get; }

        /// <summary>
        /// Creates a new instance of the configuration builder.
        /// </summary>
        /// <param name="builder">The current configuration builder.</param>
        /// <returns>The created configuration builder.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.Logger.LogDebug("Create a new DatabaseConfigurationProvider");
            return this.Services.GetRequiredService<DatabaseConfigurationProvider>();
        }
    }
}
