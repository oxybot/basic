// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Defines the standard tests for the API linked to model management.
/// </summary>
/// <typeparam name="TForList">The DTO associated with list.</typeparam>
/// <typeparam name="TForView">The DTO associated with view.</typeparam>
[Collection("api")]
public abstract class BaseModelControllerTest<TForList, TForView>
    where TForList : BaseEntityDTO
    where TForView : BaseEntityDTO
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseModelControllerTest{TForList, TForView}"/> class.
    /// </summary>
    /// <param name="testServer">The current test server manager.</param>
    /// <param name="logger">The logger instance for the test execution.</param>
    protected BaseModelControllerTest(TestServer testServer, ITestOutputHelper logger)
    {
        this.TestServer = testServer ?? throw new ArgumentNullException(nameof(testServer));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the current test server manager.
    /// </summary>
    public TestServer TestServer { get; }

    /// <summary>
    /// Gets the logger instance for the test execution.
    /// </summary>
    public ITestOutputHelper Logger { get; }

    /// <summary>
    /// Gets the base url associated with the tested API.
    /// </summary>
    public abstract Uri BaseUrl { get; }

    /// <summary>
    /// Runs a Create/Update/Delete scenario.
    /// </summary>
    /// <param name="testModel">The detail about the executed test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [SuppressMessage(
        "Usage",
        "CA2234:Pass system uri objects instead of strings",
        Justification = "Usage of relative uri")]
    protected async Task CreateReadUpdateDeleteTestAsync(TestCRUDModel<TForView> testModel)
    {
        if (testModel is null)
        {
            throw new ArgumentNullException(nameof(testModel));
        }

        // Global initialization and authentication
        using var client = await this.TestServer.CreateAuthenticatedClientAsync().ConfigureAwait(false);

        // Read all - should be empty
        using (var response = await client.GetAsync(this.BaseUrl).ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call GET {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<ListResult<TForList>>(response).ConfigureAwait(false);
            Assert.NotNull(body);
        }

        // Create one entry
        Guid? identifier = null;
        using (var response = await client.PostAsJsonAsync(this.BaseUrl, testModel.CreateContent).ConfigureAwait(false))
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errors = await this.TestServer.ReadAsJsonAsync<InvalidResult>(response).ConfigureAwait(false);
                foreach (var error in errors)
                {
                    this.Logger.WriteLine($"{error.Key}: {string.Join(", ", error.Value)}");
                }

                Assert.Fail("Error in provided values for the creation");
            }

            Assert.True(response.IsSuccessStatusCode, $"Can't call POST {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<TForList>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            identifier = this.RetrieveIdentifier(body);
            Assert.NotNull(identifier);
        }

        // Read one entry
        using (var response = await client.GetAsync($"{this.BaseUrl}/{identifier}").ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call GET {this.BaseUrl}/{identifier}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<TForView>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            var expected = this.UpdateIdentifier(testModel.CreateExpected, (Guid)identifier);
            Assert.Equivalent(expected, body, strict: true);
        }

        // Update entry
        if (testModel.UpdateContent != null)
        {
            using (var response = await client.PutAsJsonAsync($"{this.BaseUrl}/{identifier}", testModel.UpdateContent).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errors = await this.TestServer.ReadAsJsonAsync<InvalidResult>(response).ConfigureAwait(false);
                    foreach (var error in errors)
                    {
                        this.Logger.WriteLine($"{error.Key}: {string.Join(", ", error.Value)}");
                    }

                    Assert.Fail("Error in provided values for the update");
                }

                Assert.True(response.IsSuccessStatusCode, $"Can't call PUT {this.BaseUrl}/{identifier}, status code: {(int)response.StatusCode} {response.StatusCode}");
                var body = await this.TestServer.ReadAsJsonAsync<TForList>(response).ConfigureAwait(false);
                Assert.NotNull(body);
                Assert.Equal(identifier, this.RetrieveIdentifier(body));
            }

            // Read one entry
            using (var response = await client.GetAsync($"{this.BaseUrl}/{identifier}").ConfigureAwait(false))
            {
                Assert.True(response.IsSuccessStatusCode, $"Can't call GET {this.BaseUrl}/{identifier}, status code: {(int)response.StatusCode} {response.StatusCode}");
                var body = await this.TestServer.ReadAsJsonAsync<TForView>(response).ConfigureAwait(false);
                Assert.NotNull(body);
                var expected = this.UpdateIdentifier(testModel.UpdateExpected, (Guid)identifier);
                Assert.Equivalent(expected, body, strict: true);
            }
        }

        // Delete entry
        using (var response = await client.DeleteAsync($"{this.BaseUrl}/{identifier}").ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call DELETE {this.BaseUrl}/{identifier}, status code: {(int)response.StatusCode} {response.StatusCode}");
        }

        // Read one entry
        using (var response = await client.GetAsync($"{this.BaseUrl}/{identifier}").ConfigureAwait(false))
        {
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    /// <summary>
    /// Retrieves the identifier of a specific entity.
    /// </summary>
    /// <param name="body">The reference entity.</param>
    /// <returns>The identifier of the entiy; or <c>null</c>.</returns>
    protected virtual Guid? RetrieveIdentifier(TForList body)
    {
        if (body is null)
        {
            return null;
        }

        var property = body.GetType().GetProperty("Identifier");
        if (property != null)
        {
            return (Guid)property.GetValue(body);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Updates a reference identifier with its expected value, if possible.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="identifier">The identifier associated with the entity.</param>
    /// <returns>The updated entity.</returns>
    protected virtual TForView UpdateIdentifier(TForView entity, Guid identifier)
    {
        if (entity is null)
        {
            return entity;
        }

        var property = entity.GetType().GetProperty("Identifier");
        if (property != null)
        {
            property.SetValue(entity, identifier);
        }

        return entity;
    }
}
