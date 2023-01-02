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
        public DatabaseConfigurationProvider(DatabaseConfigurationSource source)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.Source.Logger.LogInformation("created");
            EntityChangeObserver.Instance.Changed += this.OnEntityChangeObserverChanged;
        }

        /// <summary>
        /// Gets the source definition attached to this provider.
        /// </summary>
        public DatabaseConfigurationSource Source { get; }

        /// <summary>
        /// Loads the entry from the database.
        /// </summary>
        public override void Load()
        {
            this.Source.Logger.LogInformation("Load configuration from database");
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            DbContextInitializer.InitializeOptions(optionsBuilder, this.Source.Configuration);

            using (Context context = new Context(optionsBuilder.Options))
            {
                var values = context.Set<Setting>()
                    .ToDictionary(s => $"{s.Section}:{s.Key}", s => s.Value);

                this.Data.Clear();
                this.Data.AddRange(values);
            }

            this.OnReload();
        }

        private void OnEntityChangeObserverChanged(object sender, EntityChangeEventArgs e)
        {
            if (e.Entry.Entity.GetType() != typeof(Setting))
            {
                return;
            }

            this.Load();
        }
    }
}
