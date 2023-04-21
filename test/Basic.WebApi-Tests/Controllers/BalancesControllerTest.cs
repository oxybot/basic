// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers;

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

    /// <summary>
    /// Executes a Create/Read/Update/Delete test.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public Task CreateReadUpdateDeleteTest()
    {
        var model = new TestCRUDModel<BalanceForList>()
        {
            CreateContent = new
            {
                CategoryIdentifier = this.TestServer.TestReferences.Category.Identifier,
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                Year = DateTime.Today.Year,
                Total = 100,
            },
            CreateExpected = new()
            {
                Category = this.TestServer.TestReferences.Category,
                User = this.TestServer.TestReferences.User,
                Year = DateTime.Today.Year,
                Total = 100,
            },
            UpdateContent = new
            {
                CategoryIdentifier = this.TestServer.TestReferences.Category.Identifier,
                UserIdentifier = this.TestServer.TestReferences.User.Identifier,
                Year = DateTime.Today.Year,
                Total = 130,
            },
            UpdateExpected = new()
            {
                Category = this.TestServer.TestReferences.Category,
                User = this.TestServer.TestReferences.User,
                Year = DateTime.Today.Year,
                Total = 130,
            },
        };

        return this.CreateReadUpdateDeleteTestAsync(model);
    }
}
