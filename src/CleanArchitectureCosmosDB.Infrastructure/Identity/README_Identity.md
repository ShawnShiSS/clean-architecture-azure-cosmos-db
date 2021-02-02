# ASP.NET Core Identity is used to provide token service and user management service.

# Entity Framework Core
## NOTES: 
1. migrations will use the connection string defined in CleanArchitectureCosmosDB.Infrastructure/appsettings.json
      as this is the connection string referenced by DesignTimeDbContextFactory
1. Consult Microsoft documentation on Entity Framework Core Code First migrations for more information on migrations.

## To create migrations for the first time:
* Add-Migration -Name "InitialIdentityDbCreation" -OutputDir "Identity\Migrations" -Context "CleanArchitectureCosmosDB.Infrastructure.Identity.Models.ApplicationDbContext" -Project "CleanArchitectureCosmosDB.Infrastructure"

## To run the migrations:
* Update-Database -Context "CleanArchitectureCosmosDB.Infrastructure.Identity.Models.ApplicationDbContext" -Project "CleanArchitectureCosmosDB.Infrastructure"