# Clean Architecture with partitioned repository pattern using Azure Cosmos DB
This project provides a starting point to build a web API to work with Azure Cosmos DB using ASP.NET Core and Azure Cosmos DB .NET SDK V3, based on Clean Architecture and repository design pattern. 
* Partition key is also implemented through the repository pattern in order to support large scale Cosmos DB.
* A RESTful API application is created with popular architecture features (see list below)

Clean Architecture is promoted by Microsoft on their .NET application architecture guide page. The e-book written by Steve "ardalis" Smith ([@ardalis](https://github.com/ardalis)) is beautifully written and well explains the beauty and benefits of using Clean Architecture. For more details, please see [**Architect Modern Web Applications with ASP.NET Core and Azure**](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

This project uses the newer Cosmos DB .NET SDK V3, because it adds support for stream APIs and Cosmos DB Change Feed processor APIs, as well as performance improvements.

# Give a star
:star: If you enjoy this project, or are using this project to kick start your next project, please give a star to support it. Thanks! :star: 

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
* Repository Design Pattern
* Partition Key
* REST API
* Swagger UI
* OData support
* IMemoryCache Cache service (Non-distributed in-memory cache)
* Serilog for structured logging
* MediatR Command/Query pattern
* MediatR pipeline behaviour for exception handling 
* FluentValidation for validation
* AutoMapper to mapping
* Database initial creation
* Sample data seeding
* Email Sender using SendGrid 
* CRUD endpoints using Cosmos DB
* Cosmos DB Point read using partition key and ID
* Search data in Cosmos DB using SQL query directly (demonstration purpose, not recommended in production)
* Search data in Cosmos DB using Parameterized Query
* Search data in Cosmos DB using LINQ and IQueryable
* Search data in Cosmos DB using Specification Pattern to abstract out query-specific logic
* ASP.NET Core Identity  (TODO)
* Identity Service (TODO)
* Cosmos DB Change Feed (TODO)
* Cosmos DB Stored Procedure for Transaction (TODO)
* Data Modeling and Partitioning patterns (TODO)

# Acknowledgement
Special thanks to Steve Smith ([@ardalis](https://github.com/ardalis)) for sharing the CleanArchitecture repository and the e-book. I absolutely love it!

Thanks to [Azure-Samples/PartitionedRepository](https://github.com/Azure-Samples/PartitionedRepository) for sample code using Cosmos DB .NET SDK V2, which helped me understand Cosmos DB .NET SDK V2 and compare SDK V2 against V3.
