// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Basic.WebApi
{
    /// <summary>
    /// Manages the web test server.
    /// </summary>
    public sealed class TestServer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestServer"/> class.
        /// </summary>
        [SuppressMessage(
            "Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "WithWebHostBuilder is sending back the initialized instance")]
        public TestServer()
        {
            this.Application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(InitializeBuilder);
        }

        /// <summary>
        /// Gets the web test server factory.
        /// </summary>
        private WebApplicationFactory<Program> Application { get; }

        /// <summary>
        /// Creates the default client.
        /// </summary>
        /// <returns>The default client for the test application.</returns>
        public HttpClient CreateClient()
        {
            return this.Application.CreateClient();
        }

        /// <summary>
        /// Creates a client with a specific configuration.
        /// </summary>
        /// <param name="options">The configuration for the application.</param>
        /// <returns>The client for the test application.</returns>
        public HttpClient CreateClient(WebApplicationFactoryClientOptions options)
        {
            return this.Application.CreateClient(options);
        }

        /// <summary>
        /// Called to stop the test server.
        /// </summary>
        public void Dispose()
        {
            // Ensure database to be deleted at the end of test execution.
            using (var scope = this.Application.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<Context>())
            {
                context.Database.EnsureDeleted();
            }

            this.Application.Dispose();
        }

        private static void InitializeBuilder(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace the database initialization service
                services.RemoveAll<DbContextOptions<Context>>();
                services.AddSingleton<DbContextOptions<Context>>(sp =>
                {
                    var builder = new DbContextOptionsBuilder<Context>(
                        new DbContextOptions<Context>(new Dictionary<Type, IDbContextOptionsExtension>()));

                    builder.UseApplicationServiceProvider(sp);

                    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                    configuration.GetSection("ConnectionStrings")["SqlServer"]
                        = "Server=(localdb)\\mssqllocaldb;Database=basic-test;Trusted_Connection=True;MultipleActiveResultSets=true";
                    builder.UseConfiguredSqlServer(configuration);

                    return builder.Options;
                });
            });
        }
    }
}
