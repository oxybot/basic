// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
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

        /// <inheritdoc />
        public override object CreateContent
            => new
            {
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                ActiveFrom = "2023-01-01",
                WorkingSchedule = new[] { 8, 8, 8, 8, 8, 0, 0 },
            };

        /// <inheritdoc />
        public override ScheduleForView CreateExpected
            => new()
            {
                User = this.TestServer.TestReferences.User,
                ActiveFrom = new DateOnly(2023, 1, 1),
                WorkingSchedule = new decimal[] { 8, 8, 8, 8, 8, 0, 0 },
            };

        /// <inheritdoc />
        public override object UpdateContent
            => new
            {
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                ActiveFrom = "2023-01-01",
                WorkingSchedule = new[] { 0, 8, 8, 8, 8, 8, 0 },
            };

        /// <inheritdoc />
        public override ScheduleForView UpdateExpected
            => new()
            {
                User = this.TestServer.TestReferences.User,
                ActiveFrom = new DateOnly(2023, 1, 1),
                WorkingSchedule = new decimal[] { 0, 8, 8, 8, 8, 8, 0 },
            };
    }
}
