// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.Models;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Tests the <see cref="AuthController"/> class.
/// </summary>
[Collection("api")]
public class AuthControllerTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthControllerTest"/> class.
    /// </summary>
    /// <param name="testServer">The current test server manager.</param>
    public AuthControllerTest(TestServer testServer)
    {
        this.TestServer = testServer ?? throw new ArgumentNullException(nameof(testServer));
    }

    /// <summary>
    /// Gets the current test server manager.
    /// </summary>
    public TestServer TestServer { get; }

    /// <summary>
    /// Tests the <see cref="AuthController.SignIn"/> method.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task SignIn()
    {
        using var client = this.TestServer.CreateClient();
        using var content = JsonContent.Create(new { Username = "demo", Password = "demo" });
        using var response = await client.PostAsync(new Uri("/Auth", UriKind.Relative), content).ConfigureAwait(false);
        Assert.True(response.IsSuccessStatusCode);

        var body = await this.TestServer.ReadAsJsonAsync<AuthResult>(response).ConfigureAwait(false);
        Assert.NotNull(body);
        Assert.Equal("Bearer", body.TokenType);
        Assert.NotNull(body.AccessToken);
        Assert.True(DateTimeOffset.Now.AddMinutes(59).ToUnixTimeMilliseconds() < body.Expire);
        Assert.True(DateTimeOffset.Now.AddMinutes(61).ToUnixTimeMilliseconds() > body.Expire);
    }
}
