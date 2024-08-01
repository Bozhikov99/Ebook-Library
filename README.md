# Ebook-Library
This is an ASP.NET Core Web application project that is an online subscription based book reading service.
The web application will allow you to read books online for a periodic subscription, you can place reviews on books and search specific titles, genres and authors.

The solution is built upon ASP.NET Core and features a MVC application and an API. We've used the MedatR with CQRS approach to achieve Clean Architecture for its realization. 

Cooperation of Bozhikov99 and Heathen-Source.

# Changelog
Version 1.0.0 - Restructured to Domain-driven Clean Architecture (2024-08-01)

Summary:
- In this release, the project has been restructured from a Service-oriented layered architecture to a Domain-driven Clean architecture. This change aims to improve the modularity, maintainability, and testability of the codebase.

Changes:

- Changing Application Layer's File Structure: The Application layer's file structure has been reorganized into feature folders. The IRequests and their respective handlers are now nested within a single file for better organization and easier navigation.

- Removing the Generic Repository Pattern: The Generic Repository pattern has been removed as it added unnecessary complexity without providing significant value, thereby simplifying the codebase.

- Refactoring the UserIdHelper: The UserIdHelper has been refactored into an Application service that is now used by both Presentational layers (API and MVC), promoting code reuse and consistency.

- Introducing Static Seeding: Static seeding has been introduced by seeding an Administrator role and account on startup, eliminating the need for magic endpoints for this purpose.

- Decoupling HATEOAS from the Application Layer: The HATEOAS functionality has been decoupled from the Application layer through Dependency Inversion towards the API, as it is only used there, thereby adhering to the Single -Responsibility Principle.

- Reducing the Usage of Automapper: The usage of Automapper has been reduced in favor of better-performing Select projections, enhancing performance and readability of the code.

Improvements:

- Improved separation of concerns.

- Enhanced testability with clear boundaries between layers.

- Better adherence to SOLID principles and clean architecture guidelines.

- More organized and maintainable codebase through feature folders and reduced clutter.

- Increased performance with optimized data mapping.
