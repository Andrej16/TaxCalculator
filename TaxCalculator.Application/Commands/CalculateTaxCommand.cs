using MediatR;
using Microsoft.Extensions.Logging;
using TaxCalculator.Application.Commands.Dtos;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Repositories;

namespace TaxCalculator.Application.Commands;

public record CalculateTaxCommand(double GrossAnnualSalary) : IRequest<CalculateTaxCommandResponse>;

public class CalculateTaxCommandHandler : IRequestHandler<CalculateTaxCommand, CalculateTaxCommandResponse>
{
    private readonly ITaxCalculationsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CalculateTaxCommandHandler> _logger;

    public CalculateTaxCommandHandler(
        ITaxCalculationsRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<CalculateTaxCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CalculateTaxCommandResponse> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var bands = await _repository.GetAllBandsAsync(cancellationToken);

            var taxCalculation = TaxCalculation.Calculate(request.GrossAnnualSalary, bands);

            _repository.Add(taxCalculation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CalculateTaxCommandResponse(taxCalculation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calculating tax.");
            throw;
        }
    }
}
