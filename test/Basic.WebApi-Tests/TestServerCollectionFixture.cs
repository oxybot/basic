// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Basic.WebApi;

/// <summary>
/// Defines the test collection for api tests.
/// </summary>
[CollectionDefinition("api")]
public class TestServerCollectionFixture : ICollectionFixture<TestServer>
{
}
