using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Persistence.Context.EntityConfigurations;

public class TaxBandTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<TaxBandType>
{
    public void Configure(EntityTypeBuilder<TaxBandType> builder)
    {
        _ = builder.Property(r => r.Id).ValueGeneratedNever();

        _ = builder.Property(r => r.Name).HasMaxLength(100);

        _ = builder.HasData(new { Id = 1, Name = "Tax Band A", MinRange = 0, MaxRange = 5000, TaxRate = 0, CreatedDate = DateTime.UtcNow });

        _ = builder.HasData(new { Id = 2, Name = "Tax Band B", MinRange = 5000, MaxRange = 20000, TaxRate = 20, CreatedDate = DateTime.UtcNow });

        _ = builder.HasData(new { Id = 3, Name = "Tax Band C", MinRange = 20000, MaxRange = int.MaxValue, TaxRate = 40, CreatedDate = DateTime.UtcNow });
    }
}
