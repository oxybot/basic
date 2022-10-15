using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Overrides after configuration a jwt token validator that can access services.
    /// </summary>
    public class CustomJwtBearerPostConfigure : IPostConfigureOptions<JwtBearerOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJwtBearerPostConfigure"/> class.
        /// </summary>
        /// <param name="services">The service provider associated with the application.</param>
        public CustomJwtBearerPostConfigure(IServiceProvider services)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the service provider associated with the application.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Called after the configuration of the jwt bearer options.
        /// </summary>
        /// <param name="name">The name of the configurated options.</param>
        /// <param name="options">The current options.</param>
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Remove the default implementation
            options.SecurityTokenValidators.Clear();

            // Add the custom implementation
            options.SecurityTokenValidators.Add(new CustomJwtSecurityTokenHandler(Services));
        }
    }
}
