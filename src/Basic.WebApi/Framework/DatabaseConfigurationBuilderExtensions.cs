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

            // Retrieve the current services
            IServiceProvider services = loggingBuilder.Services.BuildServiceProvider();

            // Temporary configuration to retrieve the connection string
            IConfigurationRoot temp = builder.Build();

            // Register the database configuration source
            builder.Add(new DatabaseConfigurationSource(
                services.GetRequiredService<ILogger<DatabaseConfigurationSource>>(),
                temp));
        }
    }
}
