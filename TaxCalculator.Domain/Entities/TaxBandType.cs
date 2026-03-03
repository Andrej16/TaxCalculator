using TaxCalculator.Domain.Common;

namespace TaxCalculator.Domain.Entities;

public class TaxBandType : Entity
{
    private TaxBandType()
    {
    }

    public string Name { get; private set; } = null!;

    public int MinRange { get; private set; }

    public int MaxRange { get; private set; }

    public int TaxRate { get; private set; }
}
