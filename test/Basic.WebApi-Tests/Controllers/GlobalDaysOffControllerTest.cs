// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Executes a Create/Read/Update/Delete test.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public Task CreateReadUpdateDeleteTest()
        {
            var model = new TestCRUDModel<GlobalDayOffForList>()
            {
                CreateContent = new { Date = "2023-01-15", Description = "test date" },
                CreateExpected = new() { Date = new DateOnly(2023, 1, 15), Description = "test date" },
                UpdateContent = new { Date = "2023-02-15", Description = "test updated date" },
                UpdateExpected = new() { Date = new DateOnly(2023, 2, 15), Description = "test updated date" },
            };

            return this.CreateReadUpdateDeleteTestAsync(model);
        }
    }
}
