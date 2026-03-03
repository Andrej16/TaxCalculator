using TaxCalculator.Domain.Entities;

namespace TaxCalculator.UnitTests.Domain;

[TestClass]
public sealed class TaxCalculationTests
{
    [TestMethod]
    public void Calculate_EmptyBands_ThrowsArgumentException()
    {
        // Arrange
        double grossAnnualSalary = 50000d;
        IEnumerable<TaxBandType> bands = Enumerable.Empty<TaxBandType>();

        // Act
        ArgumentException? ex = null;
        try
        {
            TaxCalculation.Calculate(grossAnnualSalary, bands);
        }
        catch (ArgumentException e)
        {
            ex = e;
        }

        // Assert
        Assert.IsNotNull(ex, "Expected ArgumentException to be thrown when bands is empty.");
    }
}
