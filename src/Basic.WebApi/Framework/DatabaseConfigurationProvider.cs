// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Implements a configuration provider that use the database as its source.
    /// </summary>
    public class DatabaseConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfigurationProvider" /> class.
        /// </summary>
        /// <param name="source">The source definition attached to this provider.</param>
        /// <param name="logger">The logger associated with the <see cref="DatabaseConfigurationProvider"/> class.</param>
        public DatabaseConfigurationProvider(DatabaseConfigurationSource source, ILogger<DatabaseConfigurationProvider> logger)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the source definition attached to this provider.
        /// </summary>
        public DatabaseConfigurationSource Source { get; }

        /// <summary>
        /// Gets the logger associated with the class.
        /// </summary>
        public ILogger<DatabaseConfigurationProvider> Logger { get; }

        /// <summary>
        /// Loads the entry from the database.
        /// </summary>
        public override void Load()
        {
            this.Logger.LogInformation("Load configuration from database");
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            DbContextInitializer.InitializeOptions(optionsBuilder, this.Source.Services.GetRequiredService<IConfiguration>());

            using (Context context = new Context(optionsBuilder.Options))
            {
                this.Data = context.Set<Setting>()
                    .ToDictionary(s => $"{s.Section}:{s.Key}", s => s.Value);
            }
        }
    }
}
