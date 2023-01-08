// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="GlobalDaysOffController"/> class.
    /// </summary>
    [Collection("api")]
    public class GlobalDaysOffControllerTest : BaseModelControllerTest<GlobalDayOffForList, GlobalDayOffForList>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalDaysOffControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public GlobalDaysOffControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/GlobalDaysOff", UriKind.Relative);

        /// <inheritdoc />
        public override object CreateContent
            => new { Date = "2023-01-15", Description = "test date" };

        /// <inheritdoc />
        public override GlobalDayOffForList CreateExpected
            => new() { Date = new DateOnly(2023, 1, 15), Description = "test date" };

        /// <inheritdoc />
        public override object UpdateContent
            => new { Date = "2023-02-15", Description = "test updated date" };

        /// <inheritdoc />
        public override GlobalDayOffForList UpdateExpected
            => new() { Date = new DateOnly(2023, 2, 15), Description = "test updated date" };
    }
}
