// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="UsersController"/> class.
    /// </summary>
    [Collection("api")]
    public class UsersControllerTest : BaseModelControllerTest<UserForList, UserForView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public UsersControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/users", UriKind.Relative);

        /// <inheritdoc />
        public override object CreateContent
            => new { UserName = "test", DisplayName = "test User" };

        /// <inheritdoc />
        public override UserForView CreateExpected
            => new UserForView() { UserName = "test", DisplayName = "test User" };

        /// <inheritdoc />
        public override object UpdateContent
            => new { UserName = "test", DisplayName = "test updated User", Email = "test@example.com" };

        /// <inheritdoc />
        public override UserForView UpdateExpected
            => new UserForView() { UserName = "test", DisplayName = "test updated User", Email = "test@example.com" };
    }
}
