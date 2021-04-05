using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     Configure authorization
    /// </summary>
    public static class AuthorizationConfig
    {
        /// <summary>
        ///     Authorization
        /// </summary>
        /// <param name="services"></param>
        public static void SetAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(
                        JwtBearerDefaults.AuthenticationScheme,
                        new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser()
                            .Build());
                });
        }
    }
}
