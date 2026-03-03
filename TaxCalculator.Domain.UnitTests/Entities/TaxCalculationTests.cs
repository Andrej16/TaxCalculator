#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxCalculator.Domain;
using TaxCalculator.Domain.Common;
using TaxCalculator.Domain.Entities;


namespace TaxCalculator.Domain.Entities.UnitTests;

/// <summary>
/// Tests for TaxCalculator.Domain.Entities.TaxCalculation.
/// </summary>
[TestClass]
public class TaxCalculationTests
{
    /// <summary>
    /// Verifies that Calculate throws an ArgumentException when an empty collection of tax bands is provided.
    /// Condition: bands is an empty IEnumerable{TaxBandType}.
    /// Expected: ArgumentException is thrown with parameter name 'bands' and a message indicating at least one band is required.
    /// </summary>
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
        Assert.AreEqual("bands", ex!.ParamName, "Expected the parameter name for the ArgumentException to be 'bands'.");
        StringAssert.Contains(ex.Message, "At least one tax band must be provided.", "Expected the exception message to mention that at least one band must be provided.");
    }

    /// <summary>
    /// Verifies Calculate with default/unenriched TaxBandType instances (properties have their default values).
    /// Condition: single default TaxBandType in bands. Test a variety of grossAnnualSalary values including edge cases.
    /// Expected: For non-NaN salaries the calculated AnnualTaxPaid is 0 (due to default band ranges/rates), NetAnnualSalary equals grossAnnualSalary,
    /// and monthly fields are gross/12. For NaN salary, tax and resulting salary fields propagate NaN.
    /// </summary>
    [TestMethod]
    public void Calculate_DefaultTaxBand_VariousSalaries_ComputesExpectedSalariesAndTaxes()
    {
        // Arrange
        var band = new TaxBandType(); // properties are default: MinRange=0, MaxRange=0, TaxRate=0
        var bands = new List<TaxBandType> { band };

        double[] testSalaries = new[]
        {
                0d,
                -1000d,
                50000d,
                double.NaN,
                double.PositiveInfinity,
                double.NegativeInfinity,
                double.MaxValue,
                double.MinValue
            };

        foreach (double salary in testSalaries)
        {
            // Act
            TaxCalculation result = TaxCalculation.Calculate(salary, bands);

            // Assert - GrossAnnualSalary always equals input
            if (double.IsNaN(salary))
            {
                // When input is NaN we expect NaN propagation for numeric results that depend on salary or tax arithmetic.
                Assert.IsTrue(double.IsNaN(result.GrossAnnualSalary), "GrossAnnualSalary should be NaN when input salary is NaN.");
                Assert.IsTrue(double.IsNaN(result.GrossMonthlySalary), "GrossMonthlySalary should be NaN when input salary is NaN.");
                Assert.IsTrue(double.IsNaN(result.AnnualTaxPaid), "AnnualTaxPaid should be NaN when input salary is NaN (arithmetic produced NaN).");
                Assert.IsTrue(double.IsNaN(result.MonthlyTaxPaid), "MonthlyTaxPaid should be NaN when input salary is NaN.");
                Assert.IsTrue(double.IsNaN(result.NetAnnualSalary), "NetAnnualSalary should be NaN when input salary is NaN.");
                Assert.IsTrue(double.IsNaN(result.NetMonthlySalary), "NetMonthlySalary should be NaN when input salary is NaN.");
            }
            else
            {
                // For all other numeric inputs, with the default TaxBandType (MinRange=0, MaxRange=0, TaxRate=0),
                // CalculateTaxForBand yields 0 tax contribution, therefore annual tax is 0 and net equals gross.
                Assert.AreEqual(salary, result.GrossAnnualSalary, $"GrossAnnualSalary should equal input salary ({salary}).");

                // Monthly values are computed as division by 12. For infinities or very large numbers, equality still holds for IEEE arithmetic.
                if (double.IsPositiveInfinity(salary))
                {
                    Assert.IsTrue(double.IsPositiveInfinity(result.GrossMonthlySalary), "GrossMonthlySalary should be PositiveInfinity when salary is PositiveInfinity.");
                    Assert.IsTrue(double.IsPositiveInfinity(result.NetAnnualSalary), "NetAnnualSalary should be PositiveInfinity when salary is PositiveInfinity and tax is 0.");
                }
                else if (double.IsNegativeInfinity(salary))
                {
                    Assert.IsTrue(double.IsNegativeInfinity(result.GrossMonthlySalary), "GrossMonthlySalary should be NegativeInfinity when salary is NegativeInfinity.");
                    Assert.IsTrue(double.IsNegativeInfinity(result.NetAnnualSalary), "NetAnnualSalary should be NegativeInfinity when salary is NegativeInfinity and tax is 0.");
                }
                else
                {
                    Assert.AreEqual(salary / 12d, result.GrossMonthlySalary, 1e-9, "GrossMonthlySalary should be salary divided by 12.");
                    Assert.AreEqual(salary, result.NetAnnualSalary, 1e-9, "NetAnnualSalary should equal grossAnnualSalary when annual tax is 0.");
                    Assert.AreEqual(salary / 12d, result.NetMonthlySalary, 1e-9, "NetMonthlySalary should be net annual divided by 12.");
                }

                // Tax assertions: default band yields zero tax.
                Assert.AreEqual(0d, result.AnnualTaxPaid, 0d, "AnnualTaxPaid should be 0 for default band configuration.");
                Assert.AreEqual(0d, result.MonthlyTaxPaid, 0d, "MonthlyTaxPaid should be 0 for default band configuration.");
            }
        }
    }
}