// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="EventsController"/> class.
    /// </summary>
    [Collection("api")]
    public class EventsControllerTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public EventsControllerTest(TestServer testServer, ITestOutputHelper logger)
        {
            this.TestServer = testServer ?? throw new ArgumentNullException(nameof(testServer));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the base url for the tested controller.
        /// </summary>
        public Uri BaseUrl => new Uri("/Events", UriKind.Relative);

        /// <summary>
        /// Gets the associated configured test server.
        /// </summary>
        public TestServer TestServer { get; }

        /// <summary>
        /// Gets the logger used for the test.
        /// </summary>
        public ITestOutputHelper Logger { get; }

        /// <summary>
        /// Provides the properties that could be used as sorting options for the test controller.
        /// </summary>
        /// <returns>The list of possible sorting properties.</returns>
        public static IEnumerable<object[]> GetSortingTestData()
        {
            return typeof(EventForList).GetProperties().Select(p => new[] { p.Name });
        }

        /// <summary>
        /// Tests the <see cref="EventsController.GetAll"/> method with sorting.
        /// </summary>
        /// <param name="propertyName">The sorting option for the test.</param>
        /// <returns>A task associated with the unit test execution.</returns>
        [Theory]
        [MemberData(nameof(GetSortingTestData))]
        [SuppressMessage(
            "Usage",
            "CA2234:Pass system uri objects instead of strings",
            Justification = "Usage of relative uri")]
        public async Task SortingTest(string propertyName)
        {
            // Global initialization and authentication
            using var client = await this.TestServer.CreateAuthenticatedClientAsync().ConfigureAwait(false);

            // Read all - should be empty
            using (var response = await client.GetAsync($"{this.BaseUrl}?sortKey={propertyName}").ConfigureAwait(false))
            {
                Assert.True(response.IsSuccessStatusCode, $"Can't call GET {this.BaseUrl}, status code: {(int)response.StatusCode} {response.StatusCode}");
                var body = await this.TestServer.ReadAsJsonAsync<IEnumerable<EventForList>>(response).ConfigureAwait(false);
                Assert.NotNull(body);
            }
        }
    }
}
