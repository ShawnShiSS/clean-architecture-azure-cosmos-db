using CleanArchitectureCosmosDB.Core.Entities;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Repository
{
    public class ToDoItemRepository : CosmosDbRepository<ToDoItem>, IToDoItemRepository
    {
        /// <summary>
        ///     CosmosDB container name
        /// </summary>
        public override string ContainerName { get; } = "Todo";

        /// <summary>
        ///     Generate Id.
        ///     e.g. "shoppinglist:783dfe25-7ece-4f0b-885e-c0ea72135942"
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string GenerateId(ToDoItem entity) => $"{entity.Category}:{Guid.NewGuid()}";

        /// <summary>
        ///     Returns the value of the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public ToDoItemRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        // For demonstration purpose and try to use better alternatives in production if possible.
        // Just an example of how to use Cosmos DB Parameterized Query to avoid SQL Injection.
        // Get by Category is also an example of single partition read, where get by title will be a cross partition read
        public async Task<IEnumerable<ToDoItem>> GetItemsAsyncByCategory(string category)
        {
            List<ToDoItem> results = new List<ToDoItem>();
            string query = @$"SELECT c.Name FROM c 
                                    WHERE c.Category = @Category";

            QueryDefinition queryDefinition = new QueryDefinition(query)
                                                    .WithParameter("@Category", category);
            string queryString = queryDefinition.ToString();

            var entities = await this.GetItemsAsync(queryString);

            return results;
        }

        // For demonstration purpose and try to use better alternatives in production if possible.
        // Just an example of how to use Cosmos DB Parameterized Query to avoid SQL Injection.
        // Get by Title is also an example of cross partition read, where Get by Category will be single partition read
        public async Task<IEnumerable<ToDoItem>> GetItemsAsyncByTitle(string title)
        {
            List<ToDoItem> results = new List<ToDoItem>();
            string query = @$"SELECT c.Name FROM c 
                                    WHERE c.Title = @Title";

            QueryDefinition queryDefinition = new QueryDefinition(query)
                                                    .WithParameter("@Title", title);
            string queryString = queryDefinition.ToString();

            var entities = await this.GetItemsAsync(queryString);

            return results;
        }
    }
}
