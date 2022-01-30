using Basic.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Basic.WebApi.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        [TestMethod]
        public async Task SignIn()
        {
            using var client = TestServer.CreateClient();
            using var response = await client.PostAsync("/Auth", JsonContent.Create(new { Username = "demo", Password = "demo" }));
            Assert.IsTrue(response.IsSuccessStatusCode);

            var body = await response.Content.ReadAsJsonAsync<AuthResult>();
            Assert.IsNotNull(body);
            Assert.AreEqual("Bearer", body.TokenType);
            Assert.IsNotNull(body.AccessToken);
            Assert.IsTrue(DateTimeOffset.Now.AddMinutes(59).ToUnixTimeMilliseconds() < body.Expire);
            Assert.IsTrue(DateTimeOffset.Now.AddMinutes(61).ToUnixTimeMilliseconds() > body.Expire);
        }
    }
}
