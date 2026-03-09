using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TaxCalculator.Application.Commands;
using TaxCalculator.Application.Commands.Dtos;
using TaxCalculator.Domain.Exceptions;

namespace TaxCalculator.API.Endpoints;

public class CalculateTaxEndpoint
{
    internal static async Task<Results<Ok<CalculateTaxCommandResponse>, ProblemHttpResult>> ExecuteAsync(
        CalculateTaxRequest request,
		ISender sender,
		CancellationToken cancellationToken)
    {
		try
		{
            var command = new CalculateTaxCommand(request.GrossAnnualSalary);
            var response = await sender.Send(command, cancellationToken);

			return TypedResults.Ok(response);
		}
		catch (TaxCalculatorDomainException ex)
		{
			return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
		catch (Exception ex)
		{
			return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}