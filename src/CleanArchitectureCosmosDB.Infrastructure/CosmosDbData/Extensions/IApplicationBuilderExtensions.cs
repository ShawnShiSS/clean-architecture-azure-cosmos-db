using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureCosmosDB.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using CleanArchitectureCosmosDB.Core.Interfaces;
using System.Linq;

namespace CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Extensions
{
    /// <summary>
    ///     IApplicationBuilderExtensions 
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Ensure Cosmos DB is created
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

                factory.EnsureDbSetupAsync().Wait();

            }
        }

        /// <summary>
        ///     Seed sample data in the Todo container
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static async Task SeedToDoContainerIfEmptyAsync(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _repo = serviceScope.ServiceProvider.GetService<IToDoItemRepository>();

                // Check if empty
                var sqlQueryText = "SELECT * FROM c";
                var todos = await _repo.GetItemsAsync(sqlQueryText);

                if (todos.Count() == 0)
                {
                    ToDoItem milk = new ToDoItem()
                    {
                        Category = "Grocery",
                        Title = "Get more milk"
                    };
                    ToDoItem beer = new ToDoItem()
                    {
                        Category = "Grocery",
                        Title = "Get 7 beers"
                    };
                    ToDoItem laundry = new ToDoItem()
                    {
                        Category = "Household",
                        Title = "Do laundry"
                    };

                    await _repo.AddItemAsync(milk);
                    await _repo.AddItemAsync(laundry);

                }
            }

        }
    }
}
