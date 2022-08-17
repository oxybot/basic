using Basic.DataAccess;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Basic.Model;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Overrides the default jwt security token handler to add token validity tests.
    /// </summary>
    public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJwtSecurityTokenHandler"/> class.
        /// </summary>
        /// <param name="services">The application service provider.</param>
        public CustomJwtSecurityTokenHandler(IServiceProvider services) : base()
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the application service provider.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Called to validate a specific token.
        /// </summary>
        /// <param name="securityToken">The received security token.</param>
        /// <param name="validationParameters">The validation parameters.</param>
        /// <param name="validatedToken">The valided and transformed token.</param>
        /// <returns>The extracted claims.</returns>
        /// <exception cref="SecurityTokenValidationException">The security token is not active.</exception>
        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var claims = base.ValidateToken(securityToken, validationParameters, out validatedToken);

            using (var scope = this.Services.CreateScope())
            {
                Context context = scope.ServiceProvider.GetService<Context>();

                claims.FindFirstValue("sid:guid");

                var tokens = context.Set<Token>();
            }

            return claims;
        }
    }
}
