using AutoMapper;
using CleanArchitectureCosmosDB.Infrastructure.Extensions;
using CleanArchitectureCosmosDB.Infrastructure.Identity;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Models.Authentication;
using CleanArchitectureCosmosDB.WebAPI.Config;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            // Strongly-typed configurations using IOptions
            services.Configure<Token>(Configuration.GetSection("token"));
            services.Configure<TokenServiceProvider>(Configuration.GetSection("TokenServiceProvider"));

            // Authentication and Authorization
            services.SetupAuthentication(Configuration);
            services.SetAuthorization();

            // Cosmos DB for application data
            services.SetupCosmosDb(Configuration);
            // Identity DB for Identity data
            services.SetupIdentityDatabase(Configuration);

            // API controllers
            services.SetupControllers();

            // HttpContext
            services.AddHttpContextAccessor();

            // AutoMapper, this will scan and register everything that inherits AutoMapper.Profile
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // MediatR for Command/Query pattern and pipeline behaviours
            services.SetupMediatr();

            // Caching
            services.SetupInMemoryCaching();

            // NSwag Swagger
            services.SetupNSwag();

            // OData 
            services.SetupOData();

            // Blob Storage
            // Since storage can be shared among projects, extension method is defined in Infrastructure project.
            services.SetupStorage(Configuration);
            
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

                // ONLY automatically create development databases
                app.EnsureCosmosDbIsCreated();
                app.SeedToDoContainerIfEmptyAsync().Wait();
                // Optional: auto-create and seed Identity DB
                app.EnsureIdentityDbIsCreated();
                app.SeedIdentityDataAsync().Wait();
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
