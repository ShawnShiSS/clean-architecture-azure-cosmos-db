using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Interfaces.Persistence;
using CleanArchitectureCosmosDB.Core.Interfaces.Storage;
using CleanArchitectureCosmosDB.Infrastructure.AppSettings;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Extensions;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Repository;
using CleanArchitectureCosmosDB.Infrastructure.Identity;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Models.Authentication;
using CleanArchitectureCosmosDB.Infrastructure.Services;
using CleanArchitectureCosmosDB.WebAPI.Infrastructure.Filters;
using CleanArchitectureCosmosDB.WebAPI.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Storage.Net;
using System.Linq;
using System.Reflection;
using ZymLabs.NSwag.FluentValidation;
using ZymLabs.NSwag.FluentValidation.AspNetCore;

namespace CleanArchitectureCosmosDB.WebAPI
{
    /// <summary>
    ///     Start up
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Authentication and Authorization
            services.Configure<TokenServiceProvider>(Configuration.GetSection("TokenServiceProvider"));
            services.Configure<Token>(Configuration.GetSection("token"));

            // HttpContextServiceProviderValidatorFactory requires access to HttpContext
            services.AddHttpContextAccessor();

            // AutoMapper, this will scan and register everything that inherits AutoMapper.Profile
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // MediatR, this will scan and register everything that inherits IRequest, IPipelineBehavior
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.ApiExceptions.UnhandledExceptionBehaviour<,>));
            // Fluent Validation, this will scan and register everything that inherits FluentValidation.AbstractValidator
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = Configuration.GetSection("ConnectionStrings:CleanArchitectureCosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

            // Non-distributed in-memory cache services
            services.AddMemoryCache();
            services.AddScoped<ICachedToDoItemsService, InMemoryCachedToDoItemsService>();
            services.AddScoped<IAuditRepository, AuditRepository>();


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

            // NSwag Swagger
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


            // OData Support
            services.AddOData();
            // In order to make swagger work with OData
            services.AddMvcCore(options =>
            {
                foreach (OutputFormatter outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (InputFormatter inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            // Storage
            StorageFactory.Modules.UseAzureBlobStorage();

            // Register IBlobStorage, which is used in AzureBlobStorageService
            // Avoid using IBlobStorage directly outside of AzureBlobStorageService.
            services.AddScoped<Storage.Net.Blobs.IBlobStorage>(
                factory => StorageFactory.Blobs.FromConnectionString(Configuration.GetConnectionString("StorageConnectionString")));

            services.AddScoped<IStorageService, AzureBlobStorageService>();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // create CosmosDB database
                app.EnsureCosmosDbIsCreated();
                app.SeedToDoContainerIfEmptyAsync().Wait();
            }

            // NSwag Swagger
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllers();

                // OData configuration
                endpointRouteBuilder.EnableDependencyInjection();
                endpointRouteBuilder.Filter().Select().Count().OrderBy();
            });
        }
    }
}
