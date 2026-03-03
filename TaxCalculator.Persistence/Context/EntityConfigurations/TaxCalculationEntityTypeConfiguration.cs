using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Persistence.Context.EntityConfigurations;

public class TaxCalculationEntityTypeConfiguration
    : IEntityTypeConfiguration<TaxCalculation>
{
    public void Configure(EntityTypeBuilder<TaxCalculation> builder)
    {
        _ = builder.Property(r => r.GrossAnnualSalary).HasPrecision(18, 2);

        _ = builder.Property(r => r.GrossMonthlySalary).HasPrecision(18, 2);

        _ = builder.Property(r => r.NetAnnualSalary).HasPrecision(18, 2);

        _ = builder.Property(r => r.NetMonthlySalary).HasPrecision(18, 2);

        _ = builder.Property(r => r.AnnualTaxPaid).HasPrecision(18, 2);

        _ = builder.Property(r => r.MonthlyTaxPaid).HasPrecision(18, 2);
    }
}
