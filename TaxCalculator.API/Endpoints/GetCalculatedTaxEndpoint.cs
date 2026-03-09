using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TaxCalculator.Application.Queries;
using TaxCalculator.Application.Queries.Dtos;
using TaxCalculator.Domain.Exceptions;

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
        catch (TaxCalculatorDomainException ex)
        {
            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
