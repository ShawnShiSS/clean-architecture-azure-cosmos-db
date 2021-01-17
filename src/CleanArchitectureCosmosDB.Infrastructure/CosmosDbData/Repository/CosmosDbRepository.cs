using Ardalis.Specification;
using CleanArchitectureCosmosDB.Core.Entities;
using CleanArchitectureCosmosDB.Core.Entities.Base;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Core.Specifications.Base;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Repository
{
    public abstract class CosmosDbRepository<T> : IRepository<T>, IContainerContext<T> where T : BaseEntity
    {
        /// <summary>
        ///     Name of the CosmosDB container
        /// </summary>
        public abstract string ContainerName { get; }

        /// <summary>
        ///     Generate id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GenerateId(T entity);

        /// <summary>
        ///     Resolve the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public abstract PartitionKey ResolvePartitionKey(string entityId);

        /// <summary>
        ///     Generate id for the audit record.
        ///     All entities will share the same audit container,
        ///     so we can define this method here with virtual default implementation.
        ///     Audit records for different entities will use different partition key values,
        ///     so we are not limited to the 20G per logical partition storage limit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual string GenerateAuditId(Audit entity) => $"{entity.EntityId}:{Guid.NewGuid()}";

        /// <summary>
        ///     Resolve the partition key for the audit record.
        ///     All entities will share the same audit container,
        ///     so we can define this method here with virtual default implementation.
        ///     Audit records for different entities will use different partition key values,
        ///     so we are not limited to the 20G per logical partition storage limit.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual PartitionKey ResolveAuditPartitionKey(string entityId) => new PartitionKey($"{entityId.Split(':')[0]}:{entityId.Split(':')[1]}");

        /// <summary>
        ///     Cosmos DB factory
        /// </summary>
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;

        /// <summary>
        ///     Cosmos DB container
        /// </summary>
        private readonly Microsoft.Azure.Cosmos.Container _container;
        /// <summary>
        ///     Audit container that will store audit log for all entities.
        /// </summary>
        private readonly Microsoft.Azure.Cosmos.Container _auditContainer;

        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
            this._auditContainer = this._cosmosDbContainerFactory.GetContainer("Audit")._container;
        }

        public async Task AddItemAsync(T item)
        {
            item.Id = GenerateId(item);
            await _container.CreateItemAsync<T>(item, ResolvePartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // Search data using SQL query string
        // This shows how to use SQL string to read data from Cosmos DB for demonstration purpose.
        // For production, try to use safer alternatives like Parameterized Query and LINQ if possible.
        // Using string can expose SQL Injection vulnerability, e.g. select * from c where c.id=1 OR 1=1. 
        // String can also be hard to work with due to special characters and spaces when advanced querying like search and pagination is required.
        public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
        {
            FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <inheritdoc cref="IRepository{T}.GetItemsAsync(Ardalis.Specification.ISpecification{T})"/>
        public async Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification)
        {
            IQueryable<T> queryable = ApplySpecification(specification);
            FeedIterator<T> iterator = queryable.ToFeedIterator<T>();

            List<T> results = new List<T>();
            while (iterator.HasMoreResults)
            {
                FeedResponse<T> response = await iterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <inheritdoc cref="IRepository{T}.GetItemsCountAsync(ISpecification{T})"/>
        public async Task<int> GetItemsCountAsync(ISpecification<T> specification)
        {
            IQueryable<T> queryable = ApplySpecification(specification);
            return await queryable.CountAsync();
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            // Audit
            await Audit(item);
            // Update
            await this._container.UpsertItemAsync<T>(item, ResolvePartitionKey(id));
        }

        /// <summary>
        ///     Evaluate specification and return IQueryable
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            CosmosDbSpecificationEvaluator<T> evaluator = new CosmosDbSpecificationEvaluator<T>();
            return evaluator.GetQuery(_container.GetItemLinqQueryable<T>(), specification);
        }

        /// <summary>
        ///     Audit a item by adding it to the audit container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task Audit(T item)
        {
            Audit auditItem = new Core.Entities.Audit(item.GetType().Name,
                                                    item.Id,
                                                    Newtonsoft.Json.JsonConvert.SerializeObject(item));
            auditItem.Id = GenerateAuditId(auditItem);
            await _auditContainer.CreateItemAsync<Audit>(auditItem, ResolveAuditPartitionKey(auditItem.Id));
        }


    }
}
