using AutoMapper;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Interfaces.Cache;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Config;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Extensions;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Repository;
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
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
            // AutoMapper, this will scan and register everything that inherits AutoMapper.Profile
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // MediatR, this will scan and register everything that inherits IRequest, IPipelineBehavior
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.ApiExceptions.UnhandledExceptionBehaviour<,>));
            // Fluent Validation, this will scan and register everything that inherits FluentValidation.AbstractValidator
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Bind database-related bindings
            var cosmosDbConfig = Configuration.GetSection("ConnectionStrings:CleanArchitectureCosmosDB").Get<CosmosDbConfig>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

            // Non-distributed in-memory cache services
            services.AddMemoryCache();
            services.AddScoped<ICachedToDoItemsService, InMemoryCachedToDoItemsService>();


            // API controllers
            services.AddControllers(options =>
                        // handle exceptions thrown by an action
                        options.Filters.Add(new ApiExceptionFilterAttribute()))
                    .AddNewtonsoftJson()
                    .AddFluentValidation()
                    .AddMvcOptions(options => {
                        // Clear the default MVC model validations, as we are registering all model validators using FluentValidation
                        options.ModelMetadataDetailsProviders.Clear();
                        options.ModelValidatorProviders.Clear();
                    });

            // swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Clean Architecture API", Version = "v1" });

                // Get xml comments path
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Set xml path
                options.IncludeXmlComments(xmlPath);
            });

            // OData Support
            services.AddOData();
            // In order to make swagger work with OData
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<OutputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<InputFormatter>().Where(x => x.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
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

            // Swagger UI page at /swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API V1");
            });

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
