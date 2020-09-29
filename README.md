# Clean Architecture with partitioned repository pattern using Azure Cosmos DB
This project provides a starting point to work with Azure Cosmos DB using ASP.NET Core and Azure Cosmos DB .NET SDK V3, based on Clean Architecture and repository design pattern. Partition key is also implemented through the repository pattern in order to support large scale Cosmos DB.

Clean Architecture is promoted by Microsoft on their .NET application architecture guide page. The e-book written by Steve "ardalis" Smith ([@ardalis](https://github.com/ardalis)) is beautifully written and well explains the beauty and benefits of using Clean Architecture. For more details, please see [**Architect Modern Web Applications with ASP.NET Core and Azure**](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

This project uses the newer Cosmos DB .NET SDK V3, because it adds support for stream APIs and Cosmos DB Change Feed processor APIs, as well as performance improvements.

# Goals
The primary goal of the project is to provide a basic solution structure for anyone who is building a new ASP.NET Core web or API project using Cosmos DB.

# Getting Started
1. Download the Azure CosmosDB emulator in order to run the API project locally. Here is a download link: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator-release-notes#download.
2. Start the emulator
3. Set the API project as your Startup project in Visual Studio
4. The swagger UI page should be loaded at: https://localhost:5001/swagger/index.html

**Please watch for this repository to get notification when I updates to the repo.**

# Acknowledgement
Special thanks to Steve Smith ([@ardalis](https://github.com/ardalis)) for sharing the CleanArchitecture repository and the e-book. I absolutely love it!

Thanks to [Azure-Samples/PartitionedRepository](https://github.com/Azure-Samples/PartitionedRepository) for sample code using Cosmos DB .NET SDK V2, which helped me understand Cosmos DB .NET SDK V2 and compare SDK V2 against V3.
