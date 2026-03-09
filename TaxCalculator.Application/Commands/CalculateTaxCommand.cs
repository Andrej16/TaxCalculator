using Azure.Core;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TaxCalculator.Application.Commands.Dtos;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Repositories;

namespace TaxCalculator.Application.Commands;

public record CalculateTaxCommand(double GrossAnnualSalary) : IRequest<CalculateTaxCommandResponse>;

public class CalculateTaxCommandHandler : IRequestHandler<CalculateTaxCommand, CalculateTaxCommandResponse>
{
    private readonly ITaxCalculationsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CalculateTaxCommandHandler> _logger;

    public CalculateTaxCommandHandler(
        ITaxCalculationsRepository repository,
        IUnitOfWork unitOfWork,
        IMemoryCache memoryCache,
        ILogger<CalculateTaxCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<CalculateTaxCommandResponse> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var bands = await GetCachedBandsAsync(cancellationToken);

            var taxCalculation = TaxCalculation.Calculate(request.GrossAnnualSalary, bands);

            _repository.Add(taxCalculation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CalculateTaxCommandResponse(taxCalculation.Id);
        }
        catch (TaxCalculatorDomainException ex)
        {
            _logger.LogError(ex, "An error occurred while calculating tax.");
            throw;
        }
    }

    private async Task<IReadOnlyList<TaxBandType>> GetCachedBandsAsync(CancellationToken cancellationToken)
    {
        var cacheKey = $"TaxBands";

        var result = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(2);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                return await _repository.GetAllBandsAsync(cancellationToken);
            });

        return result ?? [];
    }
}
