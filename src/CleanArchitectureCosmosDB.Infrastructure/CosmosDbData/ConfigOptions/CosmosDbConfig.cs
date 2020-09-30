using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.ConfigOptions
{
    public class CosmosDbConfig
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
        public string Name { get; set; }
        public string PartitionKey { get; set; }
    }
}
