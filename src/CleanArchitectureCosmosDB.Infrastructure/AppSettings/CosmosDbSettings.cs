using System.Collections.Generic;

namespace CleanArchitectureCosmosDB.Infrastructure.AppSettings
{
    public class CosmosDbSettings
    {
        /// <summary>
        ///     CosmosDb Account - The Azure Cosmos DB endpoint
        /// </summary>
        public string EndpointUrl { get; set; }
        /// <summary>
        ///     Key - The primary key for the Azure DocumentDB account.
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        ///     Database name
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        ///     List of containers in the database
        /// </summary>
        public List<ContainerInfo> Containers { get; set; }

    }
    public class ContainerInfo
    {
        /// <summary>
        ///     Container Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Container partition Key
        /// </summary>
        public string PartitionKey { get; set; }
    }

}
