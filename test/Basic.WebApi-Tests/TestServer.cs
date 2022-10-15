using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Basic.WebApi
{
    /// <summary>
    /// Manages the web test server.
    /// </summary>
    [TestClass]
    internal class TestServer
    {
        /// <summary>
        /// The web test server factory.
        /// </summary>
        public static WebApplicationFactory<Program> Application { get; private set; }

        /// <summary>
        /// Initializes the web test server.
        /// </summary>
        /// <param name="context">The parameter is not used.</param>
        /// <exception cref="ApplicationException">
        /// The method has been already called.
        /// </exception>
        [AssemblyInitialize]
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Will be disposed on AssemblyCleanup")]
        [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Used for testing purpose only")]
        public static void AssemblyInitialize(TestContext context)
        {
            if (Application != null)
            {
                throw new ApplicationException("Test web server already initialized");
            }

            Application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });
        }

        public static HttpClient CreateClient()
        {
            return Application.CreateClient();
        }

        public static HttpClient CreateClient(WebApplicationFactoryClientOptions options)
        {
            return Application.CreateClient(options);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (Application != null)
            {
                Application.Dispose();
            }
        }
    }
}
