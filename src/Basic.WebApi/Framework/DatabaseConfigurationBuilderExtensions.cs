// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.Framework;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for <see cref="IConfigurationBuilder"/> to add
    /// the <see cref="DatabaseConfigurationProvider"/> instance.
    /// </summary>
    public static class DatabaseConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the database configuration provider to the current configuration builder.
        /// </summary>
        /// <param name="builder">The current configuration builder.</param>
        /// <param name="loggingBuilder">The builder associated to logging.</param>
        public static void AddDatabase(this IConfigurationBuilder builder, ILoggingBuilder loggingBuilder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (loggingBuilder is null)
            {
                throw new ArgumentNullException(nameof(loggingBuilder));
            }

            // Register services for the source and provider
            loggingBuilder.Services.AddSingleton<DatabaseConfigurationSource>();
            loggingBuilder.Services.AddSingleton<DatabaseConfigurationProvider>();

            // Retrieve a logger factory for the database configuration source and provider
            IServiceProvider services = loggingBuilder.Services.BuildServiceProvider();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            // Temporary configuration to retrieve the connection string
            IConfigurationRoot temp = builder.Build();

            // Register the database configuration source
            builder.Add(services.GetRequiredService<DatabaseConfigurationSource>());
        }
    }
}
