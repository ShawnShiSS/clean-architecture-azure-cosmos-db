using CleanArchitectureCosmosDB.Core.Entities;
using CleanArchitectureCosmosDB.Core.Interfaces;
using CleanArchitectureCosmosDB.Infrastructure.CosmosDbData.Interfaces;
using CleanArchitectureCosmosDB.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Infrastructure.Extensions
{
    /// <summary>
    ///     Extension methods for IApplicationBuilder 
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Ensure Cosmos DB is created
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder builder)
        {
            using (IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ICosmosDbContainerFactory factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

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
            using (IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                IToDoItemRepository _repo = serviceScope.ServiceProvider.GetService<IToDoItemRepository>();

                // Check if empty
                string sqlQueryText = "SELECT * FROM c";
                IEnumerable<ToDoItem> todos = await _repo.GetItemsAsync(sqlQueryText);

                if (todos.Count() == 0)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        ToDoItem beer = new ToDoItem()
                        {
                            Category = "Grocery",
                            Title = $"Get {i} beers"
                        };

                        await _repo.AddItemAsync(beer);
                    }
                }
            }
        }

        /// <summary>
        ///     Create Identity DB if not exist
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureIdentityDbIsCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                
                // Ensure the database is created.
                // Note this does not use migrations. If database may be updated using migrations, use DbContext.Database.Migrate() instead.
                dbContext.Database.EnsureCreated();
            }
        }

        /// <summary>
        ///     Seed Identity data
        /// </summary>
        /// <param name="builder"></param>
        public static async Task SeedIdentityDataAsync(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await Infrastructure.Identity.Seed.ApplicationDbContextDataSeed.SeedAsync(userManager, roleManager);
            }
        }
    }
}
