using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Novell.Directory.Ldap;
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
        /// <param name="options">The options associated with AD authentication.<param>
        /// <param name="configuration">The associated configuration.</param>
        /// <param name="context">The datasource context.</param>
        /// <param name="logger">The associated logger.</param>
        public AuthController(IOptions<ActiveDirectoryOptions> options, IConfiguration configuration, Context context, ILogger<AuthController> logger)
        {
            this.Options = options.Value;
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the options associated with AD authentication.
        /// </summary>
        protected ActiveDirectoryOptions Options { get; }

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
        /// <param name="signIn">The sign in information.</param>
        /// <returns>The JWT token to be used for this connection.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InvalidResult), StatusCodes.Status400BadRequest)]
        public AuthResult SignIn([FromBody] AuthRequest signIn)
        {
            var user = Context.Set<User>()
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Username == signIn.Username);
            if (user == null)
            {
                // The user doesn't exists
                ModelState.AddModelError("", "Invalid credentials");
                throw new InvalidModelStateException(ModelState);
            }

            if (user.Password == null)
            {
                // Use AD connection to authenticate
                if (!ValidateUser(signIn.Username, signIn.Password))
                {
                    ModelState.AddModelError("", "Invalid credentials");
                    throw new InvalidModelStateException(ModelState);
                }
            }
            else
            {
                // Use local password
                if (user.HashPassword(signIn.Password) != user.Password)
                {
                    ModelState.AddModelError("", "Invalid credentials");
                    throw new InvalidModelStateException(ModelState);
                }
            }

            var token = BuildJWTToken(user);
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
            var user = Context.Set<User>()
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Identifier == userId);

            if (user == null)
            {
                throw new UnauthorizedRequestException();
            }

            var token = BuildJWTToken(user);
            return new AuthResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expire = new DateTimeOffset(token.ValidTo).ToUnixTimeMilliseconds(),
            };
        }

        private bool ValidateUser(string username, string password)
        {
            string domainName = Options.DomainName;
            string userDn = $"{username}@{domainName}";
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(Options.Server, Options.Port);
                    connection.Bind(userDn, password);
                    if (connection.Bound)
                    {
                        return true;
                    }
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        private JwtSecurityToken BuildJWTToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = Configuration["BaseUrl"];
            var audience = Configuration["BaseUrl"];
            var jwtValidity = DateTime.Now.AddSeconds(Convert.ToInt32(Configuration["JwtToken:ExpireIn"]));

            var claims = new List<Claim> {
                new Claim(type: "sid:guid", user.Identifier.ToString("D")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
            };

            foreach (Role role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Code));
            }

            var token = new JwtSecurityToken(issuer,
                audience,
                claims: claims,
                expires: jwtValidity,
                signingCredentials: creds);

            return token;
        }

    }
}
