// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="AgreementsController"/> class.
    /// </summary>
    public class AgreementsControllerTest : BaseModelControllerTest<AgreementForList, AgreementForView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementsControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public AgreementsControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/agreements", UriKind.Relative);

        /// <summary>
        /// Executes a Create/Read/Update/Delete test.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public Task CreateReadUpdateDeleteTest()
        {
            var model = new TestCRUDModel<AgreementForView>()
            {
                CreateContent = new
                {
                    ClientIdentifier = this.TestServer.TestReferences.Client.Identifier,
                    InternalCode = "test",
                    Title = "test agreement",
                },
                CreateExpected = new()
                {
                    Client = this.TestServer.TestReferences.Client,
                    InternalCode = "test",
                    Title = "test agreement",
                    Items = new List<AgreementItemForList>(),
                },
                UpdateContent = new
                {
                    ClientIdentifier = this.TestServer.TestReferences.Client.Identifier,
                    InternalCode = "Test",
                    Title = "test updated agreement",
                    PrivateNotes = "test",
                },
                UpdateExpected = new()
                {
                    Client = this.TestServer.TestReferences.Client,
                    InternalCode = "Test",
                    Title = "test updated agreement",
                    PrivateNotes = "test",
                    Items = new List<AgreementItemForList>(),
                },
            };

            return this.CreateReadUpdateDeleteTestAsync(model);
        }
    }
}
