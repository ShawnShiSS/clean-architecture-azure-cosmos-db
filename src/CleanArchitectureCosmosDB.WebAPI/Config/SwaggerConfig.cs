using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
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
            // Register the Swagger services
            services.AddOpenApiDocument((options, serviceProvider) =>
            {
                options.DocumentName = "v1";
                options.Title = "Clean Architecture Cosmos DB API";
                options.Version = "v1";

                FluentValidationSchemaProcessor fluentValidationSchemaProcessor = serviceProvider.GetService<FluentValidationSchemaProcessor>();
                // Add the fluent validations schema processor
                options.SchemaProcessors.Add(fluentValidationSchemaProcessor);

                // Add JWT token authorization
                options.OperationProcessors.Add(new OperationSecurityScopeProcessor("auth"));
                options.DocumentProcessors.Add(new SecurityDefinitionAppender("auth", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Scheme = "bearer",
                    BearerFormat = "jwt"
                }));

            });

            // Add the FluentValidationSchemaProcessor as a singleton
            services.AddSingleton<FluentValidationSchemaProcessor>();
        }
    }
}
