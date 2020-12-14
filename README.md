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

**(NEW) React Client Web Application**
* A web application dashboard built using React, Typescript, Material UI.
* NSwagStudio is used to generate TypeScript client code to interact with the API.
* NSwagStudio file is added to the ClientApp project.
* FluentValidation is registered to define validation rules in Swagger/OpenAPI schema

# Give a star
:star: If you enjoy this project, or are using this project to start your exciting new project, please give it a star. Thanks! :star: 

# Goals
The primary goal of the project is to provide a basic solution structure for anyone who is building a new ASP.NET Core web or API project using Cosmos DB.
**Please watch for this repository to get notification when I make updates to the repo.**

**For a detailed discussion, please see my recent article, Clean Architecture — ASP.NET Core API using Partitioned Repository Pattern and Azure Cosmos DB** (link: https://medium.com/swlh/clean-architecture-with-partitioned-repository-pattern-using-azure-cosmos-db-62241854cbc5)

# Getting Started - API
1. Download the Azure CosmosDB emulator in order to run the API project locally. Here is a download link: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator-release-notes#download.
2. Start the emulator
3. Set the API project as your Startup project in Visual Studio
4. The swagger UI page should be loaded at: https://localhost:5001/swagger/index.html

# Getting Started - Client Application
Because the client web application is built using React and TypeScript, you need a couple of things below installed on your machine.

Prerequisites:
* Download and install [node.js](https://nodejs.org/en/download/)
* Download and install [Yarn](https://classic.yarnpkg.com/en/docs/install/#windows-stable)
* (optional) Download and install [Visual Studio Code](https://code.visualstudio.com/download)

1. Open folder src/CleanArchitectureCosmosDB.ClientApp in Visual Studio Code 
1. Open Terminal
1. Run command "yarn install"
1. Run command "yarn start"
1. You should see the web app running on localhost:3000

# Features supported
* ASP.NET Core 3.1
* Azure Cosmos DB .NET SDK V3
* **Azure Functions V3 (new)**
* **Client Web Application using React + Typescript + Material UI(new)**
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
* FluentValidation for model validation (with support for database query)
* FluentValidation to define validation rules in Swagger Schema
* AutoMapper to mapping
* Email Sender using SendGrid 
* Cosmos DB Database initial creation
* Cosmos DB initial sample data seeding
* CRUD endpoints using Cosmos DB
* Cosmos DB point-read using partition key and ID
* Cosmos DB single-partition read and cross-partition read
* Cosmos DB SQL query (demonstration purpose, not recommended in production)
* Cosmos DB query with Parameterized Query, LINQ and IQueryable
* Cosmos DB data searching using Specification Pattern to abstract out query-specific logic
* Audit container and automatic auditing 
* NSwagStudio file to generate TypeScript client code 
* DataTables and Pagination support using LINQ and IQueryable in API
* Material UI DataTable in React Client App
* Client side validation using Formik and Yup in React Client App
* Map and render server-side errors in React Client App

# New Features Under Development
* ASP.NET Core Identity  (TODO)
* Token Service Provider using Identity (TODO)
* Cosmos DB Change Feed (TODO)
* Cosmos DB Stored Procedure for Transaction (TODO)
* other TODOs

# Additional Resources
I have published some short articles to cover different aspects of this project. Please feel free to give them a read.
* [Clean Architecture — ASP.NET Core API using Partitioned Repository Pattern and Azure Cosmos DB ](https://medium.com/swlh/clean-architecture-with-partitioned-repository-pattern-using-azure-cosmos-db-62241854cbc5)
* [Clean Architecture — ASP.NET Core API Starter Project using In-memory Cache Service](https://shawn-shi.medium.com/clean-architecture-using-in-memory-cache-service-ab376fe226eb)
* [Clean Architecture — ASP.NET Core REST API with OData and Swagger UI](https://shawn-shi.medium.com/clean-architecture-rest-api-with-odata-and-swagger-ui-406f7df896c)
* [Azure Cosmos DB — SQL Injection Attack and Defense](https://medium.com/swlh/azure-cosmos-db-sql-injection-attack-and-defense-17b32ef95b9)
* [Audit Log using Partitioned Repository Pattern with Cosmos DB](https://shawn-shi.medium.com/audit-log-using-partitioned-repository-pattern-with-cosmos-db-99b63de97e35)
* [Clean Architecture from a Technical Interview Perspective](https://shawn-shi.medium.com/clean-architecture-from-a-technical-interview-perspective-7b79d86d6155)
* [DataTables Support using Partitioned Repository with Cosmos DB](https://shawn-shi.medium.com/pagination-and-searching-in-asp-net-core-api-using-cosmos-db-869384a59f5)
* [How to Generate API Client Code Using NSwag With FluentValidation Rules](https://medium.com/swlh/how-to-generate-api-client-code-using-nswag-with-fluentvalidation-rules-9428ae65c10e)
* [Mapping and Displaying Server-Side Errors with Formik in React](https://shawn-shi.medium.com/mapping-and-displaying-server-side-errors-with-formik-in-react-bbe7ae696895)
* [Partitioning Best Practice in Partitioned Repository Pattern with Cosmos DB](https://shawn-shi.medium.com/partition-key-design-best-practice-in-partitioned-repository-pattern-with-cosmos-db-ef9318007fc1)
* Specification Pattern with Partitioned Repository Pattern (TODO)

# Acknowledgement
Special thanks to Steve Smith ([@ardalis](https://github.com/ardalis)) for sharing the CleanArchitecture repository and the e-book. I absolutely love it!

Thanks to [Azure-Samples/PartitionedRepository](https://github.com/Azure-Samples/PartitionedRepository) for sample code using Cosmos DB .NET SDK V2, which helped me understand Cosmos DB .NET SDK V2 and compare SDK V2 against V3.
