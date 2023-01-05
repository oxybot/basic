// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="AgreementsController"/> class.
    /// </summary>
    [Collection("api")]
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

        /// <inheritdoc />
        public override object CreateContent
            => new { ClientIdentifier = this.TestServer.TestReferences.Client.Identifier, InternalCode = "test", Title = "test agreement" };

        /// <inheritdoc />
        public override AgreementForView CreateExpected
            => new() { Client = this.TestServer.TestReferences.Client, InternalCode = "test", Title = "test agreement", Items = new List<AgreementItemForList>() };

        /// <inheritdoc />
        public override object UpdateContent
            => new { ClientIdentifier = this.TestServer.TestReferences.Client.Identifier, InternalCode = "Test", Title = "test updated agreement", PrivateNotes = "test" };

        /// <inheritdoc />
        public override AgreementForView UpdateExpected
            => new() { Client = this.TestServer.TestReferences.Client, InternalCode = "Test", Title = "test updated agreement", PrivateNotes = "test", Items = new List<AgreementItemForList>() };
    }
}
