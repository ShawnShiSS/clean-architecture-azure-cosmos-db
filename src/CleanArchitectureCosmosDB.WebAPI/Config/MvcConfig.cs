using CleanArchitectureCosmosDB.WebAPI.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ZymLabs.NSwag.FluentValidation.AspNetCore;

namespace CleanArchitectureCosmosDB.WebAPI.Config
{
    /// <summary>
    ///     Configure MVC options
    /// </summary>
    public static class MvcConfig
    {
        /// <summary>
        ///     Configure controllers
        /// </summary>
        /// <param name="services"></param>
        public static void SetupControllers(this IServiceCollection services)
        {
            // API controllers
            services.AddControllers(options =>
                        // handle exceptions thrown by an action
                        options.Filters.Add(new ApiExceptionFilterAttribute()))
                    .AddNewtonsoftJson(options =>
                    {
                        // Serilize enum in string
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    })
                    .AddFluentValidation(options =>
                    {
                        // In order to register FluentValidation to define Swagger schema
                        // https://github.com/RicoSuter/NSwag/issues/1722#issuecomment-544202504
                        // https://github.com/zymlabs/nswag-fluentvalidation
                        options.RegisterValidatorsFromAssemblyContaining<Startup>();

                        // Optionally set validator factory if you have problems with scope resolve inside validators.
                        options.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                    })
                    .AddMvcOptions(options =>
                    {
                        // Clear the default MVC model validations, as we are registering all model validators using FluentValidation
                        // https://github.com/jasontaylordev/NorthwindTraders/issues/76
                        options.ModelMetadataDetailsProviders.Clear();
                        options.ModelValidatorProviders.Clear();
                    });
        }

    }
}
