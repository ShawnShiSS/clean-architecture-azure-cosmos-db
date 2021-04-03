using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Infrastructure.AppSettings;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Repository;
using CleanArchitectureCosmosDB.Infrastructure.Extensions;
using CleanArchitectureCosmosDB.Infrastructure.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;

// add the FunctionsStartup assembly attribute that specifies the type name used during startup
[assembly: FunctionsStartup(typeof(CleanArchitectureCosmosDB.AzureFunctions.Startup))]

namespace CleanArchitectureCosmosDB.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configurations
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Use a singleton Configuration throughout the application
            services.AddSingleton<IConfiguration>(configuration);

            // Singleton instance. See example usage in SendGridEmailService: inject IOptions<SendGridEmailSettings> in SendGridEmailService constructor
            services.Configure<SendGridEmailSettings>(configuration.GetSection("SendGridEmailSettings"));

            // if default ILogger is desired instead of Serilog
            //services.AddLogging();

            // configure serilog
            Serilog.Core.Logger logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("C:\\Logs\\AzureFunctions.Starter\\log-StaterFunction.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            services.AddLogging(lb => lb.AddSerilog(logger));

            //Register SendGrid Email
            services.AddScoped<IEmailService, SendGridEmailService>();

            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:CleanArchitectureCosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

        }
    }
}
