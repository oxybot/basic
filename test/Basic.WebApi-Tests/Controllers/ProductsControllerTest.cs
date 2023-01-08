// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Tests the <see cref="ProductsController"/> class.
    /// </summary>
    [Collection("api")]
    public class ProductsControllerTest : BaseModelControllerTest<ProductForList, ProductForView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsControllerTest"/> class.
        /// </summary>
        /// <param name="testServer">The current test server manager.</param>
        /// <param name="logger">The logger instance for the test execution.</param>
        public ProductsControllerTest(TestServer testServer, ITestOutputHelper logger)
            : base(testServer, logger)
        {
        }

        /// <inheritdoc />
        public override Uri BaseUrl => new Uri("/Products", UriKind.Relative);

        /// <inheritdoc />
        public override object CreateContent
            => new { DisplayName = "test Product", DefaultQuantity = 1, DefaultUnitPrice = 100 };

        /// <inheritdoc />
        public override ProductForView CreateExpected
            => new() { DisplayName = "test Product", DefaultQuantity = 1, DefaultUnitPrice = 100 };

        /// <inheritdoc />
        public override object UpdateContent
            => new { DisplayName = "test updated Product", DefaultDescription = "updated", DefaultQuantity = 2, DefaultUnitPrice = 50 };

        /// <inheritdoc />
        public override ProductForView UpdateExpected
            => new() { DisplayName = "test updated Product", DefaultDescription = "updated", DefaultQuantity = 2, DefaultUnitPrice = 50 };
    }
}
