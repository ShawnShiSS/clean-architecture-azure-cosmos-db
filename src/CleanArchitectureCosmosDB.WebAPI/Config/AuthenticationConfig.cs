using CleanArchitectureCosmosDB.Infrastructure.Identity.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     Authentication configuration
    /// </summary>
    public static class AuthenticationConfig
    {
        /// <summary>
        ///     authentication configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            Token token = configuration.GetSection("token").Get<Token>();
            byte[] secret = Encoding.ASCII.GetBytes(token.Secret);

            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.ClaimsIssuer = token.Issuer;
                        options.IncludeErrorDetails = true;
                        options.Validate(JwtBearerDefaults.AuthenticationScheme);
                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ClockSkew = TimeSpan.Zero,
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = token.Issuer,
                                ValidAudience = token.Audience,
                                IssuerSigningKey = new SymmetricSecurityKey(secret),
                                NameClaimType = ClaimTypes.NameIdentifier,
                                RequireSignedTokens = true,
                                RequireExpirationTime = true
                            };
                    });
        }
    }
}
