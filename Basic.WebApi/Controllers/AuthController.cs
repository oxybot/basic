using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        /// <param name="signIn">The sign in information.</param>
        /// <returns>The JWT token to be used for this connection.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        public AuthResult SignIn([FromBody] AuthRequest signIn)
        {
            if (signIn == null)
            {
                throw new UnauthorizedRequestException();
            }

            var user = Context.Set<User>()
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Username == signIn.Username && u.Password == signIn.Password);

            if (user == null)
            {
                throw new UnauthorizedRequestException();
            }

            var token = BuildJWTToken(user);
            return new AuthResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireIn = Convert.ToInt32(Configuration["JwtToken:ExpireIn"]),
            };
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
