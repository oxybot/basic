// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="BalancesController"/> class.
    /// </summary>
    [Collection("api")]
    public class BalancesControllerTest : BaseModelControllerTest<BalanceForList, BalanceForList>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BalancesControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public BalancesControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/Balances", UriKind.Relative);

        /// <inheritdoc />
        public override object CreateContent
            => new
            {
                CategoryIdentifier = this.TestServer.TestReferences.Category.Identifier,
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                Year = DateTime.Today.Year,
                Allowed = 100,
                Transfered = 0,
            };

        /// <inheritdoc />
        public override BalanceForList CreateExpected
            => new()
            {
                Category = this.TestServer.TestReferences.Category,
                User = this.TestServer.TestReferences.User,
                Year = DateTime.Today.Year,
                Allowed = 100,
            };

        /// <inheritdoc />
        public override object UpdateContent
            => new
            {
                CategoryIdentifier = this.TestServer.TestReferences.Category.Identifier,
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                Year = DateTime.Today.Year,
                Allowed = 130,
                Transfered = 30,
            };

        /// <inheritdoc />
        public override BalanceForList UpdateExpected
            => new()
            {
                Category = this.TestServer.TestReferences.Category,
                User = this.TestServer.TestReferences.User,
                Year = DateTime.Today.Year,
                Allowed = 130,
                Transfered = 30,
            };
    }
}
