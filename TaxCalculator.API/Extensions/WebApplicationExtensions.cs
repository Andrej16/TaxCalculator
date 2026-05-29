using Asp.Versioning;
using TaxCalculator.API.Endpoints;

namespace TaxCalculator.API.Extensions;

public static class WebApplicationExtensions
{
    public static void RegisterTaxEndpoints(this WebApplication app)
    {
        var taxGroup = app
            .MapGroup("/api/tax")
            .WithTags("Tax");

        taxGroup
            .MapPost("/calculate", CalculateTaxEndpoint.ExecuteAsync);

        taxGroup
            .MapGet("/{taxCalculationId:int}", GetCalculatedTaxEndpoint.ExecuteAsync);
    }

    public static void RegisterUsersEndpoints(this WebApplication app)
    {
        var usersGroup = app
            .NewVersionedApi("Users")
            .MapGroup("/api/v{version:apiVersion}/users")
            .WithTags("Users");

        usersGroup.MapGet("", () => TypedResults.Ok(new[]
        {
            new UserV1(1, "John Doe"),
            new UserV1(2, "Alice Dewett"),
        }))
        .HasApiVersion(new ApiVersion(1, 0));

        usersGroup.MapGet("", () => TypedResults.Ok(new[]
        {
            new UserV2(1, "John Doe", new DateOnly(1990, 1, 1)),
            new UserV2(2, "Alice Dewett", new DateOnly(1992, 2, 2)),
        }))
        .HasApiVersion(new ApiVersion(2, 0));
    }

    record UserV1(int Id, string Name);
    record UserV2(int Id, string Name, DateOnly BirthDate);
}
