// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
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

        /// <inheritdoc />
        public override object CreateContent
            => new { DisplayName = "test", FullName = "test client" };

        /// <inheritdoc />
        public override ClientForView CreateExpected
            => new ClientForView() { DisplayName = "test", FullName = "test client" };

        /// <inheritdoc />
        public override object UpdateContent
            => new { DisplayName = "Test", FullName = "Updated test client", AddressCountry = "Utopia" };

        /// <inheritdoc />
        public override ClientForView UpdateExpected
            => new ClientForView() { DisplayName = "Test", FullName = "Updated test client", AddressCountry = "Utopia" };
    }
}
