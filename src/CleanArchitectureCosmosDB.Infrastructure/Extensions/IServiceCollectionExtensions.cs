using CleanArchitectureCosmosDB.Core.Interfaces.Storage;
using CleanArchitectureCosmosDB.Infrastructure.AppSettings;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces;
using CleanArchitectureCosmosDB.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Net;
using System.Collections.Generic;

namespace CleanArchitectureCosmosDB.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Register a singleton instance of Cosmos Db Container Factory, which is a wrapper for the CosmosClient.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="primaryKey"></param>
        /// <param name="databaseName"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string databaseName,
                                                     List<ContainerInfo> containers)
        {
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(endpointUrl, primaryKey);
            CosmosDbContainerFactory cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);

            // Microsoft recommends a singleton client instance to be used throughout the application
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.cosmosclient?view=azure-dotnet#definition
            // "CosmosClient is thread-safe. Its recommended to maintain a single instance of CosmosClient per lifetime of the application which enables efficient connection management and performance"
            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;
        }

        /// <summary>
        ///     Setup Azure Blob storage
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupStorage(this IServiceCollection services, IConfiguration configuration)
        {
            StorageFactory.Modules.UseAzureBlobStorage();

            // Register IBlobStorage, which is used in AzureBlobStorageService
            // Avoid using IBlobStorage directly outside of AzureBlobStorageService.
            services.AddScoped<Storage.Net.Blobs.IBlobStorage>(
                factory => StorageFactory.Blobs.FromConnectionString(configuration.GetConnectionString("StorageConnectionString")));

            services.AddScoped<IStorageService, AzureBlobStorageService>();
        }

    }
}
