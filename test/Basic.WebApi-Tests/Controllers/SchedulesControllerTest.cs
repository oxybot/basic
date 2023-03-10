// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Tests the <see cref="SchedulesController"/> class.
/// </summary>
[Collection("api")]
public sealed class SchedulesControllerTest : BaseModelControllerTest<ScheduleForList, ScheduleForView>, IDisposable
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

    /// <summary>
    /// Gets the test data for the <see cref="CreateWithConflictsTest"/> method.
    /// </summary>
    public static IEnumerable<object[]> CreateWithConflictsData => new List<object[]>()
    {
        new object[] { new DateOnly(2023, 01, 01), null, new DateOnly(2023, 01, 01), null },
        new object[] { new DateOnly(2023, 01, 01), null, new DateOnly(2022, 01, 01), null },
        new object[] { new DateOnly(2023, 01, 01), null, new DateOnly(2024, 01, 01), null },
        new object[] { new DateOnly(2023, 01, 01), null, new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), null, new DateOnly(2022, 01, 01), new DateOnly(2023, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), null, new DateOnly(2024, 01, 01), new DateOnly(2024, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 01, 01), null },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2022, 01, 01), null },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 08, 01), null },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2022, 01, 01), new DateOnly(2023, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2022, 01, 01), new DateOnly(2023, 7, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 01, 01), new DateOnly(2024, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 08, 01), new DateOnly(2024, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 01, 01), new DateOnly(2024, 7, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 08, 01), new DateOnly(2024, 12, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2023, 04, 01), new DateOnly(2024, 10, 31) },
        new object[] { new DateOnly(2023, 01, 01), new DateOnly(2023, 12, 31), new DateOnly(2022, 01, 01), new DateOnly(2024, 12, 31) },
    };

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
    /// <param name="referenceFrom">The reference schedule from date.</param>
    /// <param name="referenceTo">The reference schedule to date, if any.</param>
    /// <param name="conflictingFrom">The conflicting schedule from date.</param>
    /// <param name="conflictingTo">The conflicting schedule to date, if any.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [MemberData(nameof(CreateWithConflictsData))]
    public async Task CreateWithConflictsTest(DateOnly referenceFrom, DateOnly? referenceTo, DateOnly conflictingFrom, DateOnly? conflictingTo)
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
            ActiveFrom = referenceFrom,
            ActiveTo = referenceTo,
            WorkingSchedule = new decimal[] { 0, 8, 8, 8, 8, 8, 0 },
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
            ActiveFrom = conflictingFrom,
            ActiveTo = conflictingTo,
            WorkingSchedule = new decimal[] { 0, 8, 8, 8, 8, 8, 0 },
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

    /// <inheritdoc />
    public void Dispose()
    {
        using var scope = this.TestServer.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<Context>();
        context.Set<Schedule>().RemoveRange(context.Set<Schedule>());
        context.SaveChanges();
    }
}
