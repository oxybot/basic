// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Tests the <see cref="SchedulesController"/> class.
/// </summary>
[Collection("api")]
public class SchedulesControllerTest : BaseModelControllerTest<ScheduleForList, ScheduleForView>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulesControllerTest"/> class.
    /// </summary>
    /// <param name="testServer">The current test server manager.</param>
    /// <param name="logger">The logger instance for the test execution.</param>
    public SchedulesControllerTest(TestServer testServer, ITestOutputHelper logger)
        : base(testServer, logger)
    {
    }

    /// <inheritdoc />
    public override Uri BaseUrl => new Uri("/Schedules", UriKind.Relative);

    /// <summary>
    /// Executes a Create/Read/Update/Delete test.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public Task CreateReadUpdateDeleteTest()
    {
        var model = new TestCRUDModel<ScheduleForView>()
        {
            CreateContent = new
            {
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                ActiveFrom = "2023-01-01",
                WorkingSchedule = new[] { 8, 8, 8, 8, 8, 0, 0 },
            },
            CreateExpected = new()
            {
                User = this.TestServer.TestReferences.User,
                ActiveFrom = new DateOnly(2023, 1, 1),
                WorkingSchedule = new decimal[] { 8, 8, 8, 8, 8, 0, 0 },
            },
            UpdateContent = new
            {
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                ActiveFrom = "2023-01-01",
                WorkingSchedule = new[] { 0, 8, 8, 8, 8, 8, 0 },
            },
            UpdateExpected = new()
            {
                User = this.TestServer.TestReferences.User,
                ActiveFrom = new DateOnly(2023, 1, 1),
                WorkingSchedule = new decimal[] { 0, 8, 8, 8, 8, 8, 0 },
            },
        };

        return this.CreateReadUpdateDeleteTestAsync(model);
    }

    /// <summary>
    /// Tests the creation of a schedule with conflicts.
    /// </summary>
    [Fact]
    public async Task CreateWithConflicts()
    {
        // Global initialization and authentication
        using var client = await this.TestServer.CreateAuthenticatedClientAsync().ConfigureAwait(false);

        // Read all - should be empty
        using (var response = await client.GetAsync(this.BaseUrl).ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call GET {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<ListResult<ScheduleForList>>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            Assert.Equal(0, body.Total);
        }

        // Create a reference
        var reference = new
        {
            UserIdentifier = this.TestServer.TestReferences.User.Identifier,
            ActiveFrom = new DateOnly(2023, 1, 1),
            WorkingSchedule = new decimal[] { 0, 8, 8, 8, 8, 8, 0 }
        };
        Guid referenceId;
        using (var response = await client.PostAsJsonAsync(this.BaseUrl, reference).ConfigureAwait(false))
        {
            Assert.True(response.IsSuccessStatusCode, $"Can't call POST {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
            var body = await this.TestServer.ReadAsJsonAsync<ScheduleForList>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            Assert.Equal(reference.UserIdentifier, body.User.Identifier);
            Assert.Equal(reference.ActiveFrom, body.ActiveFrom);
            Assert.Equal(reference.WorkingSchedule, body.WorkingSchedule);
            referenceId = body.Identifier;
        }

        // Create a conflicting schedule
        var conflicting = new
        {
            UserIdentifier = this.TestServer.TestReferences.User.Identifier,
            ActiveFrom = new DateOnly(2023, 1, 1),
            WorkingSchedule = new decimal[] { 0, 8, 8, 8, 8, 8, 0 }
        };
        using (var response = await client.PostAsJsonAsync(this.BaseUrl, reference).ConfigureAwait(false))
        {
            Assert.Equal((HttpStatusCode)400, response.StatusCode);
            var body = await this.TestServer.ReadAsJsonAsync<InvalidResult>(response).ConfigureAwait(false);
            Assert.NotNull(body);
            Assert.True(body.ContainsKey("activeFrom"));
            Assert.Single(body["activeFrom"], "This schedule conflicts with another schedule");
        }
    }
}
