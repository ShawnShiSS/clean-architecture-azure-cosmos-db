using Microsoft.Extensions.DependencyInjection;
using ZymLabs.NSwag.FluentValidation;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     Swagger
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        ///     NSwag for swagger 
        /// </summary>
        /// <param name="services"></param>
        public static void SetupNSwag(this IServiceCollection services)
        {
            // Add the FluentValidationSchemaProcessor as a singleton
            services.AddSingleton<FluentValidationSchemaProcessor>();
            services.AddOpenApiDocument((options, serviceProvider) =>
            {
                options.DocumentName = "v1";
                options.Title = "Clean Architecture Cosmos DB API";
                options.Version = "v1";
                FluentValidationSchemaProcessor fluentValidationSchemaProcessor = serviceProvider.GetService<FluentValidationSchemaProcessor>();
                // Add the fluent validations schema processor
                options.SchemaProcessors.Add(fluentValidationSchemaProcessor);

            });


        }
    }
}
