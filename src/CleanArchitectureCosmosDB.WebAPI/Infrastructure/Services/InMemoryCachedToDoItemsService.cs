using CleanArchitectureCosmosDB.Core.Entities;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CleanArchitectureCosmosDB.WebAPI.Infrastructure.Services
{
    /// <summary>
    ///     Non-distributed cache
    /// </summary>
    public class InMemoryCachedToDoItemsService : ICachedToDoItemsService
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="cache"></param>
        public InMemoryCachedToDoItemsService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        ///     Delete
        /// </summary>
        /// <returns></returns>
        public void DeleteCachedToDoItems()
        {
            _cache.Remove(CacheHelpers.GenerateToDoItemsCacheKey());
        }

        /// <summary>
        ///     Get 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ToDoItem> GetCachedToDoItems()
        {
            IEnumerable<ToDoItem> toDoItems;

            _cache.TryGetValue<IEnumerable<ToDoItem>>(CacheHelpers.GenerateToDoItemsCacheKey(), out toDoItems);

            return toDoItems;
        }

        /// <summary>
        ///     Set
        /// </summary>
        /// <param name="entry"></param>
        public void SetCachedToDoItems(IEnumerable<ToDoItem> entry)
        {
            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _cache.Set(CacheHelpers.GenerateToDoItemsCacheKey(), entry, cacheEntryOptions);
        }
    }
}
