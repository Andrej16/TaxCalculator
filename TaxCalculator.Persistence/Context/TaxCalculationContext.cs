using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Persistence.Context;

public class TaxCalculationContext(DbContextOptions<TaxCalculationContext> options) 
    : DbContext(options)
{
    public const string DefaultSchema = "tax";

    public DbSet<TaxCalculation> TaxCalculations { get; set; } = null!;

    public DbSet<TaxBandType> TaxBandTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder
            .HasDefaultSchema(DefaultSchema)
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
