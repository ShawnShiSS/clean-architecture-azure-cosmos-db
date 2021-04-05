using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Interfaces.Persistence;
using CleanArchitectureCosmosDB.Infrastructure.AppSettings;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Repository;
using CleanArchitectureCosmosDB.Infrastructure.Extensions;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Models;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     Database related configurations
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        ///     Setup Cosmos DB
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:CleanArchitectureCosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();   
            services.AddScoped<IAuditRepository, AuditRepository>();
        }

        /// <summary>
        ///     Setup ASP.NET Core Identity DB, including connection string, Identity options, token providers, and token services, etc..
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupIdentityDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer(configuration.GetConnectionString("CleanArchitectureIdentity"))
                options.UseInMemoryDatabase("CleanArchitectureIdentity")
                );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddSignInManager<SignInManager<ApplicationUser>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<IdentityOptions>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    // Identity : Default password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                });

            // services required using Identity
            services.AddScoped<ITokenService, TokenService>();

        }
    }
}
