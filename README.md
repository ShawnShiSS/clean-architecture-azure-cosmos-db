# Clean Architecture with partitioned repository pattern using Azure Cosmos DB
This solution provides a starting point to build a web API to work with Azure Cosmos DB using ASP.NET Core and Azure Cosmos DB .NET SDK V3, based on Clean Architecture and repository design pattern. 
* Partition key is also implemented through the repository pattern in order to support large scale Cosmos DB.
* A RESTful API application is created with popular architecture features (see full list below).

Clean Architecture is promoted by Microsoft on their .NET application architecture guide page. The e-book written by Steve "ardalis" Smith ([@ardalis](https://github.com/ardalis)) is beautifully written and well explains the beauty and benefits of using Clean Architecture. For more details, please see [**Architect Modern Web Applications with ASP.NET Core and Azure**](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

This project uses the newer Cosmos DB .NET SDK V3, because it adds support for stream APIs and Cosmos DB Change Feed processor APIs, as well as performance improvements.


**(NEW) Azure Functions starter project**
* An Azure Functions project is also created with popular architecture features (see full list below), in order to demonstrate how it can be used as a serverless computer service to work with Cosmos DB.

**(NEW) Auditing feature**
* A default Audit container is created at API startup.
* Auditing is done through repository automatically when updating an item, with no code change required in the API project.
* An example API endpoint is added to demonstrate how to retrieve full history of an entity.
* Audit container use the entity id as the partition key, so that each entity record technically should have 20G worth of storage size avaiable to store audit log.

**(NEW) Web Application (work in progress)**
* Built using React, Typescript, Material UI

# Give a star
:star: If you enjoy this project, or are using this project to start your exciting new project, please give it a star. Thanks! :star: 

# Goals
The primary goal of the project is to provide a basic solution structure for anyone who is building a new ASP.NET Core web or API project using Cosmos DB.
**Please watch for this repository to get notification when I make updates to the repo.**

**For a detailed discussion, please see my recent article, Clean Architecture — ASP.NET Core API using Partitioned Repository Pattern and Azure Cosmos DB** (link: https://medium.com/swlh/clean-architecture-with-partitioned-repository-pattern-using-azure-cosmos-db-62241854cbc5)

# Getting Started
1. Download the Azure CosmosDB emulator in order to run the API project locally. Here is a download link: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator-release-notes#download.
2. Start the emulator
3. Set the API project as your Startup project in Visual Studio
4. The swagger UI page should be loaded at: https://localhost:5001/swagger/index.html

# Features supported
* ASP.NET Core 3.1
* Azure Cosmos DB .NET SDK V3
* **Azure Functions V3 (new)**
* Repository Design Pattern
* Horizontal Partitioning
* Partition Key Design
* REST API
* Swagger UI
* OData support
* IMemoryCache Cache service (Non-distributed in-memory cache)
* Serilog for structured logging
* MediatR Command/Query pattern
* MediatR pipeline behaviour for exception handling 
* FluentValidation for validation
* AutoMapper to mapping
* Email Sender using SendGrid 
* Cosmos DB Database initial creation
* Cosmos DB initial sample data seeding
* CRUD endpoints using Cosmos DB
* Cosmos DB point-read using partition key and ID
* Cosmos DB single-partition read and cross-partition read
* Search data in Cosmos DB using SQL query directly (demonstration purpose, not recommended in production)
* Search data in Cosmos DB using Parameterized Query
* Search data in Cosmos DB using LINQ and IQueryable
* Search data in Cosmos DB using Specification Pattern to abstract out query-specific logic
* Audit container and automatic auditing 


# New Features Under Development
* ASP.NET Core Identity  (TODO)
* Identity Service (TODO)
* Cosmos DB Change Feed (TODO)
* Cosmos DB Stored Procedure for Transaction (TODO)
* SPA Client Application using React + Typescript (TODO)
* other TODOs

# Additional Resources
I have published some short articles to cover different aspects of this project. Please feel free to give them a read.
* [Clean Architecture — ASP.NET Core API using Partitioned Repository Pattern and Azure Cosmos DB ](https://medium.com/swlh/clean-architecture-with-partitioned-repository-pattern-using-azure-cosmos-db-62241854cbc5)
* [Clean Architecture — ASP.NET Core API Starter Project using In-memory Cache Service](https://shawn-shi.medium.com/clean-architecture-using-in-memory-cache-service-ab376fe226eb)
* [Clean Architecture — ASP.NET Core REST API with OData and Swagger UI](https://shawn-shi.medium.com/clean-architecture-rest-api-with-odata-and-swagger-ui-406f7df896c)
* [Azure Cosmos DB — SQL Injection Attack and Defense](https://medium.com/swlh/azure-cosmos-db-sql-injection-attack-and-defense-17b32ef95b9)
* [Audit Log using Partitioned Repository Pattern with Cosmos DB](https://shawn-shi.medium.com/audit-log-using-partitioned-repository-pattern-with-cosmos-db-99b63de97e35)

# Acknowledgement
Special thanks to Steve Smith ([@ardalis](https://github.com/ardalis)) for sharing the CleanArchitecture repository and the e-book. I absolutely love it!

Thanks to [Azure-Samples/PartitionedRepository](https://github.com/Azure-Samples/PartitionedRepository) for sample code using Cosmos DB .NET SDK V2, which helped me understand Cosmos DB .NET SDK V2 and compare SDK V2 against V3.
