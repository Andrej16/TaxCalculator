using MediatR;
using Microsoft.Extensions.Logging;
using TaxCalculator.Application.Queries.Dtos;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Repositories;

namespace TaxCalculator.Application.Queries;

public record GetCalculatedTaxQuery(int TaxCalculationId) : IRequest<GetCalculatedTaxQueryResponse>;

public class GetCalculatedTaxQueryHandler : IRequestHandler<GetCalculatedTaxQuery, GetCalculatedTaxQueryResponse>
{
    private readonly ITaxCalculationsRepository _repository;
    private readonly ILogger<GetCalculatedTaxQueryHandler> _logger;

    public GetCalculatedTaxQueryHandler(ITaxCalculationsRepository repository, ILogger<GetCalculatedTaxQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<GetCalculatedTaxQueryResponse> Handle(GetCalculatedTaxQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var taxCalculation = await _repository.GetByIdAsync(query.TaxCalculationId, cancellationToken);

            return new GetCalculatedTaxQueryResponse(
                taxCalculation.Id,
                taxCalculation.GrossAnnualSalary,
                taxCalculation.GrossMonthlySalary,
                taxCalculation.NetAnnualSalary,
                taxCalculation.NetMonthlySalary,
                taxCalculation.AnnualTaxPaid,
                taxCalculation.MonthlyTaxPaid);
        }
        catch (TaxCalculatorDomainException ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tax calculation with ID {TaxCalculationId}", query.TaxCalculationId);
            throw;
        }
    }
}
