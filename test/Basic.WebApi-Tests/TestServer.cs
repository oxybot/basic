// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

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

            this.TestReferences = TestReferences.Build(this.Application.Services);
        }

        /// <summary>
        /// Gets identifiers of references instances for testing.
        /// </summary>
        public TestReferences TestReferences { get; }

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
        /// Creates a client initialized with an account with all roles.
        /// </summary>
        /// <returns>The default client for the test application.</returns>
        public async Task<HttpClient> CreateAuthenticatedClientAsync()
        {
            // Global initialization and authentication
            var client = this.CreateClient();
            var content = new { Username = "demo", Password = "demo" };
            using (var response = await client.PostAsJsonAsync(new Uri("/auth", UriKind.Relative), content).ConfigureAwait(false))
            {
                Assert.True(response.IsSuccessStatusCode);
                var body = await this.ReadAsJsonAsync<AuthResult>(response).ConfigureAwait(false);
                string accessToken = body.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return client;
        }

        /// <summary>
        /// Reads the body of a response using the json converter associated with the project.
        /// </summary>
        /// <typeparam name="T">The expected result type.</typeparam>
        /// <param name="response">The http response message to read.</param>
        /// <returns>The deserialization result.</returns>
        public async Task<T> ReadAsJsonAsync<T>(HttpResponseMessage response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var options = this.Application.Services.GetRequiredService<IOptions<JsonOptions>>().Value;
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(json, options.JsonSerializerOptions);
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
