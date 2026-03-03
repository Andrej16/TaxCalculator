using TaxCalculator.API.Endpoints;

namespace TaxCalculator.API.Extensions;

public static class WebApplicationExtensions
{
    public static void RegisterTaxEndpoints(
        this WebApplication app,
        IConfiguration configuration)
    {
        var taxGroup = app
            .MapGroup("/api/tax")
            .WithTags("Tax");

        taxGroup
            .MapPost("/calculate", CalculateTaxEndpoint.ExecuteAsync);

        taxGroup
            .MapGet("/{taxCalculationId:int}", GetCalculatedTaxEndpoint.ExecuteAsync);
    }
}
