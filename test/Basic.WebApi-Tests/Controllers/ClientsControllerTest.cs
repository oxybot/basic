// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Tests the <see cref="ClientsController"/> class.
/// </summary>
[Collection("api")]
public sealed class ClientsControllerTest : BaseModelControllerTest<ClientForList, ClientForView>, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientsControllerTest"/> class.
    /// </summary>
    /// <param name="testServer">The current test server manager.</param>
    /// <param name="logger">The logger instance for the test execution.</param>
    public ClientsControllerTest(TestServer testServer, ITestOutputHelper logger)
        : base(testServer, logger)
    {
    }

    /// <inheritdoc />
    public override Uri BaseUrl => new Uri("/clients", UriKind.Relative);

    /// <summary>
    /// Executes a Create/Read/Update/Delete test.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public Task CreateReadUpdateDeleteTest()
    {
        var model = new TestCRUDModel<ClientForView>()
        {
            CreateContent = new
            {
                DisplayName = "test",
                FullName = "test client",
            },
            CreateExpected = new()
            {
                DisplayName = "test",
                FullName = "test client",
            },
            UpdateContent = new
            {
                DisplayName = "Test",
                FullName = "Updated test client",
                AddressCountry = "Utopia",
            },
            UpdateExpected = new()
            {
                DisplayName = "Test",
                FullName = "Updated test client",
                AddressCountry = "Utopia",
            },
        };

        return this.CreateReadUpdateDeleteTestAsync(model);
    }

    /// <summary>
    /// Tests the Create method with a conflicting entry.
    /// </summary>
    /// <param name="referenceName">The name of the reference client.</param>
    /// <param name="conflictingName">The name of the conflicting client.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData("test", "test")]
    [InlineData("test", "Test")]
    [InlineData("test", "TEST")]
    [InlineData("test", "test ")]
    [InlineData("test", " test ")]
    public async Task CreateWithConflictTest(string referenceName, string conflictingName)
    {
        // Global initialization and authentication
        using var client = await this.TestServer.CreateAuthenticatedClientAsync().ConfigureAwait(false);

        // Read all - should be empty
        using (var response = await client.GetAsync(this.BaseUrl).ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call GET {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<ListResult<ClientForList>>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            Assert.Equal(1, body.Total);
        }

        // Create a reference
        var reference = new
        {
            DisplayName = referenceName,
            FullName = "test client",
        };

        Guid referenceId;
        using (var response = await client.PostAsJsonAsync(this.BaseUrl, reference).ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call POST {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<ClientForList>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            Assert.Equal(reference.DisplayName, body.DisplayName);
            Assert.Equal(reference.FullName, body.FullName);
            referenceId = body.Identifier;
        }

        // Create a conflicting client
        var conflicting = new
        {
            DisplayName = conflictingName,
            FullName = "test client",
        };
        using (var response = await client.PostAsJsonAsync(this.BaseUrl, reference).ConfigureAwait(false))
        {
            Assert.Equal((HttpStatusCode)400, response.StatusCode);
            var body = await this.TestServer.ReadAsJsonAsync<InvalidResult>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            Assert.True(body.ContainsKey("displayName"));
            Assert.Single(body["displayName"], "A client with the same Display Name is already registered");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        using var scope = this.TestServer.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<Context>();
        context.Set<Client>().RemoveRange(context.Set<Client>().Where(c => c.Identifier != this.TestServer.TestReferences.Client.Identifier));
        context.SaveChanges();
    }
}
