// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;
using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="EventsController"/> class.
    /// </summary>
    [Collection("api")]
    public class EventsControllerTest : BaseModelControllerTest<EventForList, EventForView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public EventsControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/Events", UriKind.Relative);

        /// <inheritdoc />
        public override object CreateContent
            => new
            {
                CategoryIdentifier = this.TestServer.TestReferences.Category.Identifier,
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                StartDate = "2023-02-01",
                EndDate = "2023-02-03",
                DurationTotal = 16,
                DurationFirstDay = 0,
                DurationLastDay = 0,
            };

        /// <inheritdoc />
        public override EventForView CreateExpected
            => new()
            {
                Category = this.TestServer.TestReferences.Category,
                User = this.TestServer.TestReferences.User,
                StartDate = new DateOnly(2023, 2, 1),
                EndDate = new DateOnly(2023, 2, 3),
                DurationTotal = 16,
                DurationFirstDay = 0,
                DurationLastDay = 0,
                CurrentStatus = this.TestServer.TestReferences.StatusRequested,
            };

        /// <inheritdoc />
        public override object UpdateContent
            => new
            {
                CategoryIdentifier = this.TestServer.TestReferences.Category.Identifier,
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                Comment = "test comment",
                StartDate = "2023-02-01",
                EndDate = "2023-02-03",
                DurationTotal = 16,
                DurationFirstDay = 4,
                DurationLastDay = 4,
            };

        /// <inheritdoc />
        public override EventForView UpdateExpected
            => new()
            {
                Category = this.TestServer.TestReferences.Category,
                User = this.TestServer.TestReferences.User,
                Comment = "test comment",
                StartDate = new DateOnly(2023, 2, 1),
                EndDate = new DateOnly(2023, 2, 3),
                DurationTotal = 16,
                DurationFirstDay = 4,
                DurationLastDay = 4,
            };
    }
}
