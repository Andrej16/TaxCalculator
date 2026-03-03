using TaxCalculator.Domain.Repositories;
using TaxCalculator.Persistence.Context;

namespace TaxCalculator.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaxCalculationContext _context;

    public UnitOfWork(TaxCalculationContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => 
        await _context.SaveChangesAsync(cancellationToken);
}