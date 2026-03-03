using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Domain.Repositories;

public interface ITaxCalculationsRepository
{
    Task<TaxCalculation> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaxBandType>> GetAllBandsAsync(CancellationToken cancellationToken);
    void Add(TaxCalculation taxCalculation);
}