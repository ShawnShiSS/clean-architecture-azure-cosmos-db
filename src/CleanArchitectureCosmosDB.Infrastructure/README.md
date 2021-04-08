# Infrastructure

This project has plumbing code that implements the abstractions defined in Core project.

* Infrastructure has plumbing code, such as repositories, EF Core DbContext if used, Cached repositories, third party APIs, file systems, email service implementations, logging adapters, third party SDKs like S3 or Azure Blob Storage SDKs.
* Infrastructure has dependency on application core, but not vice versa!