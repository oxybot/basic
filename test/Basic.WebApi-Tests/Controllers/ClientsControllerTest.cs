// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Tests the <see cref="ClientsController"/> class.
/// </summary>
[Collection("api")]
public class ClientsControllerTest : BaseModelControllerTest<ClientForList, ClientForView>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientsControllerTest"/> class.
    /// </summary>
    /// <param name="testServer">The current test server manager.</param>
    /// <param name="logger">The logger instance for the test execution.</param>
    public ClientsControllerTest(TestServer testServer, ITestOutputHelper logger)
        : base(testServer, logger)
    {
    }

    /// <inheritdoc />
    public override Uri BaseUrl => new Uri("/clients", UriKind.Relative);

    /// <summary>
    /// Executes a Create/Read/Update/Delete test.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public Task CreateReadUpdateDeleteTest()
    {
        var model = new TestCRUDModel<ClientForView>()
        {
            CreateContent = new
            {
                DisplayName = "test",
                FullName = "test client",
            },
            CreateExpected = new()
            {
                DisplayName = "test",
                FullName = "test client",
            },
            UpdateContent = new
            {
                DisplayName = "Test",
                FullName = "Updated test client",
                AddressCountry = "Utopia",
            },
            UpdateExpected = new()
            {
                DisplayName = "Test",
                FullName = "Updated test client",
                AddressCountry = "Utopia",
            },
        };

        return this.CreateReadUpdateDeleteTestAsync(model);
    }
}
