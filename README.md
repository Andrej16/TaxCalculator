# TaxCalculator

A small ASP.NET Core application that calculates tax based on configurable tax bands and exposes a simple HTTP API.

## Requirements
- .NET 8 SDK
- SQL Server (LocalDB is fine for development)
- Visual Studio or any editor that supports .NET 8

## Quickstart

1. Update the development connection string
   - Edit `TaxCalculator.API\appsettings.Development.json` and change the connection string to point to your SQL Server instance (or LocalDB).

2. Apply EF Core migrations to create the database
   - In Visual Studio open __Package Manager Console__
   - Set __Default Project__ to `TaxCalculator.Persistence`
   - Run:
     ```powershell
     Update-Database
     ```
5. Set the startup project
- In Visual Studio set the startup project to `TaxCalculator.API`

## API

### POST /api/tax/calculate
Calculate tax for the provided gross annual salary and persist the result.

- URL
  - `POST http://localhost:5129/api/tax/calculate`

- Request body (JSON) Example using `curl`
curl -X POST http://localhost:5129/api/tax/calculate 
-H "Content-Type: application/json" 
-d '{"GrossAnnualSalary":4000}'

Response
  - Status: `201 Created`
  - Body: Id of the created tax calculation record in the database


### GET /api/tax/{id}
Retrieve a previously calculated tax result.

- URL
  - `GET http://localhost:5129/api/tax/1`

- Sample response

	{
		"id": 1,
		"grossAnnualSalary": 4000,
		"grossMonthlySalary": 333.33,
		"netAnnualSalary": 4000,
		"netMonthlySalary": 333.33,
		"annualTaxPaid": 0,
		"monthlyTaxPaid": 0
	}