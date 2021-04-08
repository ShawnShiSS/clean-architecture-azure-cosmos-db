# Core Project

This project includes business entities and business logics in abstraction.


* Application core defines the domain models and abstractions, such as interfaces, entities, domain services that do not belong to any specific entity, exceptions, domain events and handlers, specifications, etc.
* Core project should have minimal dependencies, see dependencies in the project, 
particularly, application core should NOT depend on things like EF Core, SQL client, etc., since application core is only about high level business level logic, and should NOT care how things are implemented.
* Infrastructure has dependency on application core, but not vice versa!