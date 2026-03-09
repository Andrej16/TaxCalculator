using System.Globalization;
using System.Reflection;
using TaxCalculator.Domain.Entities;
using TaxCalculator.Domain.Exceptions;

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
        TaxCalculatorDomainException? ex = null;
        try
        {
            TaxCalculation.Calculate(grossAnnualSalary, bands);
        }
        catch (TaxCalculatorDomainException e)
        {
            ex = e;
        }

        // Assert
        Assert.IsNotNull(ex, "Expected TaxCalculatorDomainException to be thrown when bands is empty.");
    }

    [TestMethod]
    public void Calculate_ValidBands_ReturnsCorrectTaxCalculation()
    {
        // Arrange
        var grossAnnualSalary = 10000d;
        var expectedAnnualTaxPaid = 1000d;
        
        var bands = new List<TaxBandType>
        {
            CreateTaxBandType("Band 1", 0, 5000, 0),
            CreateTaxBandType("Band 2", 5000, 20000, 20),
            CreateTaxBandType("Band 3", 20000, int.MaxValue, 40)
        };

        // Act
        var result = TaxCalculation.Calculate(grossAnnualSalary, bands);

        // Assert
        Assert.AreEqual(grossAnnualSalary, result.GrossAnnualSalary);
        Assert.AreEqual(grossAnnualSalary / 12, result.GrossMonthlySalary);
        Assert.AreEqual(grossAnnualSalary - expectedAnnualTaxPaid, result.NetAnnualSalary);
        Assert.AreEqual(result.NetAnnualSalary / 12, result.NetMonthlySalary);
        Assert.AreEqual(expectedAnnualTaxPaid, result.AnnualTaxPaid);
        Assert.AreEqual(expectedAnnualTaxPaid / 12, result.MonthlyTaxPaid);
    }

    [TestMethod]
    public void Calculate_NegativeGrossAnnualSalary_ThrowsArgumentException()
    {
        // Arrange
        double grossAnnualSalary = -50000d;
        var bands = new List<TaxBandType>
        {
            CreateTaxBandType("Band 1", 0, 5000, 0),
            CreateTaxBandType("Band 2", 5000, 20000, 20),
            CreateTaxBandType("Band 3", 20000, int.MaxValue, 40)
        };

        // Act
        var action = () => TaxCalculation.Calculate(grossAnnualSalary, bands);

        // Assert
        Assert.ThrowsExactly<TaxCalculatorDomainException>(
            action, 
            "Gross annual salary cannot be negative.", 
            "Expected ArgumentException to be thrown when grossAnnualSalary is negative.");
    }

    private static TaxBandType CreateTaxBandType(string name, int minRange, int maxRange, int taxRate)
    {
        Type objectType = typeof(TaxBandType);
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;

        if (Activator.CreateInstance(
            objectType,
            flags,
            null,
            null,
            CultureInfo.CurrentCulture) is not TaxBandType band)
        {
            throw new InvalidOperationException("Failed to create instance of TaxBandType.");
        }

        objectType.GetProperty(nameof(TaxBandType.Name))!.SetValue(band, name);
        objectType.GetProperty(nameof(TaxBandType.MinRange))!.SetValue(band, minRange);
        objectType.GetProperty(nameof(TaxBandType.MaxRange))!.SetValue(band, maxRange);
        objectType.GetProperty(nameof(TaxBandType.TaxRate))!.SetValue(band, taxRate);
        
        return band;
    }
}
