using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.WebAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     Setup caching
    /// </summary>
    public static class CachingConfig
    {
        /// <summary>
        ///     In-memory Caching
        /// </summary>
        /// <param name="services"></param>
        public static void SetupInMemoryCaching(this IServiceCollection services)
        {
            // Non-distributed in-memory cache services
            services.AddMemoryCache();
            services.AddScoped<ICachedToDoItemsService, InMemoryCachedToDoItemsService>();
        }
    }
}
