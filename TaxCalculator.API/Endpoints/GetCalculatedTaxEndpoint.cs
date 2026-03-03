using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TaxCalculator.Application.Queries;
using TaxCalculator.Application.Queries.Dtos;

namespace TaxCalculator.API.Endpoints;

public class GetCalculatedTaxEndpoint
{
    internal static async Task<Results<Ok<GetCalculatedTaxQueryResponse>, ProblemHttpResult>> ExecuteAsync(
        int taxCalculationId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetCalculatedTaxQuery(taxCalculationId), cancellationToken);
            
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
    }
}
