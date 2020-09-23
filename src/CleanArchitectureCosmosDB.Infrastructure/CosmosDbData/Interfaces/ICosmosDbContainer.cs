using Microsoft.Azure.Cosmos;

namespace CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces
{
    public interface ICosmosDbContainer
    {
        Container _container { get; }
    }
}
