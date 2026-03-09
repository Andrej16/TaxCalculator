using Microsoft.EntityFrameworkCore;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Repositories;
using TaxCalculator.Persistence.Context;

namespace TaxCalculator.Persistence.Repositories;

public class TaxCalculationsRepository(TaxCalculationContext context) : ITaxCalculationsRepository
{
    private readonly TaxCalculationContext _context = context;

    public async Task<TaxCalculation> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var dbItem = await _context.TaxCalculations.FindAsync([id], cancellationToken);

        return dbItem ?? throw new TaxCalculatorDomainException($"Tax calculation with id {id} not found.");
    }

    public async Task<IReadOnlyList<TaxBandType>> GetAllBandsAsync(CancellationToken cancellationToken)
    {
        return await _context.TaxBandTypes.ToListAsync(cancellationToken);
    }

    public void Add(TaxCalculation taxCalculation)
    {
        _ = _context.TaxCalculations.Add(taxCalculation);
    }
}
