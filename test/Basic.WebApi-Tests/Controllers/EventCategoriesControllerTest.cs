// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;
using Basic.WebApi.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="EventCategoriesController"/> class.
    /// </summary>
    [Collection("api")]
    public class EventCategoriesControllerTest : BaseModelControllerTest<EventCategoryForList, EventCategoryForList>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCategoriesControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public EventCategoriesControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/EventCategories", UriKind.Relative);

        /// <summary>
        /// Executes a Create/Read/Update/Delete test.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public Task CreateReadUpdateDeleteTest()
        {
            var model = new TestCRUDModel<EventCategoryForList>()
            {
                CreateContent = new
                {
                    DisplayName = "test EventCategory",
                    Mapping = "timeoff",
                    RequireBalance = false,
                },
                CreateExpected = new()
                {
                    DisplayName = "test EventCategory",
                    Mapping = EventTimeMapping.TimeOff,
                    RequireBalance = false,
                },
                UpdateContent = new
                {
                    DisplayName = "test updated EventCategory",
                    ColorClass = "ddffff",
                    Mapping = "active",
                    RequireBalance = true,
                },
                UpdateExpected = new()
                {
                    DisplayName = "test updated EventCategory",
                    ColorClass = "ddffff",
                    Mapping = EventTimeMapping.Active,
                    RequireBalance = true,
                },
            };

            return this.CreateReadUpdateDeleteTestAsync(model);
        }
    }
}
