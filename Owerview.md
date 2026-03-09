# TaxCalculator — Overview

A small ASP.NET Core solution (targeting .NET 8) that calculates tax using configurable tax bands and exposes a minimal HTTP API. The codebase follows a layered/clean architecture and uses CQRS-style commands and queries mediated by MediatR.

## Solution structure

- `TaxCalculator.API` — Minimal API project exposing HTTP endpoints and wiring application services.
- `TaxCalculator.Application` — Application/use-case layer containing MediatR requests (commands/queries), DTOs and handlers.
- `TaxCalculator.Persistence` — EF Core persistence implementation and repository implementations used by the application layer.
- `TaxCalculator.Domain` — Domain types and repository interfaces referenced by Application/Persistence.

All projects target `net8.0` and enable nullable reference types and implicit usings.

## Architecture & patterns

- Layered / Clean architecture: API → Application → Persistence, separating responsibilities and limiting dependencies.
- CQRS: Commands (state changes) and Queries (reads) are separated. Examples: `CalculateTaxCommand`, `GetCalculatedTaxQuery`.
- Mediator pattern: `MediatR` is used to decouple HTTP endpoints from business logic handlers.
- Repository pattern: `ITaxCalculationsRepository` (and similar) provide data access abstraction implemented in persistence.
- Minimal APIs: Endpoints are implemented as small classes with `ExecuteAsync` methods and mapped in `Program.cs`.

## Key .NET technologies

- .NET 8 (`net8.0`)
- ASP.NET Core Minimal APIs
- MediatR for in-process messaging
- Entity Framework Core for data access and migrations
- `Asp.Versioning.Mvc.ApiExplorer` for API versioning / OpenAPI integration
- Nullable reference types and implicit usings enabled in project files

## API surface (examples)

- POST `/api/tax/calculate` — Calculate tax for a provided gross annual salary and persist the result. Example request body: `{ "GrossAnnualSalary": 4000 }`.
- GET `/api/tax/{id}` — Retrieve a persisted tax calculation by id. Returns DTO with gross/net annual and monthly values and tax paid.

## Development & local quickstart

1. Update `TaxCalculator.API\appsettings.Development.json` with your connection string (LocalDB is acceptable).
2. Apply EF Core migrations using Package Manager Console (set default project to `TaxCalculator.Persistence`) or `dotnet ef database update --project TaxCalculator.Persistence --startup-project TaxCalculator.API`.
3. Run the API (`dotnet run --project TaxCalculator.API` or from Visual Studio with `TaxCalculator.API` as startup project).

## Notes

- Endpoints return typed `Results<T>` shapes (e.g., `TypedResults.Ok(...)`) for clearer minimal-API responses.
- Handlers use `ILogger` to record failures; endpoints translate exceptions into HTTP problem responses.

If you want this file renamed to `Overview.md` (correcting the spelling) or placed in a specific folder, tell me and I will update it.