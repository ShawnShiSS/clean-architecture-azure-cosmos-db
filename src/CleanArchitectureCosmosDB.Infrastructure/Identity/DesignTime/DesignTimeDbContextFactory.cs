using CleanArchitectureCosmosDB.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CleanArchitectureCosmosDB.Infrastructure.Identity.DesignTime
{
    /// <summary>
    ///     Used for design time migrations.  Will look to the appsettings.json file in this project
    ///     for the connection string.
    ///     EF Core tools scans the assembly containing the dbcontext for an implementation
    ///     of IDesignTimeDbContextFactory.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("CleanArchitectureIdentity");

            Console.WriteLine($"DesignTimeDbContextFactory: using base path = {path}");
            Console.WriteLine($"DesignTimeDbContextFactory: using connection string = {connectionString}");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find connection string named 'CleanArchitectureIdentity'");
            }

            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<ApplicationDbContext>();

            ApplicationDbContext.AddBaseOptions(dbContextOptionsBuilder, connectionString);

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
