using Microsoft.Azure.Cosmos;

namespace CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces
{
    public interface ICosmosDbContainer
    {
        /// <summary>
        ///     Instance of Azure Cosmos DB Container class
        /// </summary>
        Container _container { get; }
    }
}
