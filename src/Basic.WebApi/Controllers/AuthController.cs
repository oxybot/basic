// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides authentication services.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="configuration">The associated configuration.</param>
        /// <param name="context">The datasource context.</param>
        /// <param name="logger">The associated logger.</param>
        public AuthController(IConfiguration configuration, Context context, ILogger<AuthController> logger)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the associated configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the datasource context.
        /// </summary>
        protected Context Context { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        protected ILogger<AuthController> Logger { get; }

        /// <summary>
        /// Sign in a specific user.
        /// </summary>
        /// <param name="externalAuthenticator">The service to manage external authentications.</param>
        /// <param name="signIn">The sign in information.</param>
        /// <returns>The JWT token to be used for this connection.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InvalidResult), StatusCodes.Status400BadRequest)]
        public AuthResult SignIn([FromServices] ExternalAuthenticatorService externalAuthenticator, [FromBody] AuthRequest signIn)
        {
            if (externalAuthenticator is null)
            {
                throw new ArgumentNullException(nameof(externalAuthenticator));
            }
            else if (signIn is null)
            {
                throw new ArgumentNullException(nameof(signIn));
            }

            var user = this.Context.Set<User>()
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Username == signIn.Username);
            if (user == null)
            {
                // The user doesn't exists
                this.ModelState.AddModelError(string.Empty, "Invalid credentials");
                throw new InvalidModelStateException(this.ModelState);
            }

            if (user.ExternalIdentifier != null)
            {
                // Use the external authenticator
                if (!externalAuthenticator.ValidateUser(user.ExternalIdentifier, signIn.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid credentials");
                    throw new InvalidModelStateException(this.ModelState);
                }
            }
            else if (user.Password != null)
            {
                // Use local password
                if (user.HashPassword(signIn.Password) != user.Password)
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid credentials");
                    throw new InvalidModelStateException(this.ModelState);
                }
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid credentials");
                throw new InvalidModelStateException(this.ModelState);
            }

            // The provided credential are valid - checking that the user is active
            if (!user.IsActive)
            {
                this.ModelState.AddModelError(string.Empty, "This account is inactive");
                throw new InvalidModelStateException(this.ModelState);
            }

            // All good - create and provide a token for the session
            var token = this.BuildJWTToken(user);
            return new AuthResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expire = new DateTimeOffset(token.ValidTo).ToUnixTimeMilliseconds(),
            };
        }

        /// <summary>
        /// Renew the access token.
        /// </summary>
        /// <returns>The JWT token to be used for this connection.</returns>
        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [Route("renew")]
        public AuthResult Renew()
        {
            var userIdClaim = this.User.Claims.SingleOrDefault(c => c.Type == "sid:guid");
            var userId = Guid.Parse(userIdClaim.Value);
            var user = this.Context.Set<User>()
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Identifier == userId);

            if (user == null)
            {
                throw new UnauthorizedRequestException();
            }

            var token = this.BuildJWTToken(user);
            return new AuthResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expire = new DateTimeOffset(token.ValidTo).ToUnixTimeMilliseconds(),
            };
        }

        private JwtSecurityToken BuildJWTToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JwtToken:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = this.Configuration["BaseUrl"];
            var audience = this.Configuration["BaseUrl"];
            var jwtValidity = DateTime.Now.AddSeconds(Convert.ToInt32(this.Configuration["JwtToken:ExpireIn"], CultureInfo.InvariantCulture));

            var claims = new List<Claim>
            {
                new Claim(type: "sid:guid", user.Identifier.ToString("D")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
            };

            foreach (Role role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Code));
            }

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims: claims,
                expires: jwtValidity,
                signingCredentials: creds);

            var tokenDb = new Token { Expiration = jwtValidity, User = user };
            this.Context.Add<Token>(tokenDb);
            this.Context.SaveChanges();

            return token;
        }
    }
}
