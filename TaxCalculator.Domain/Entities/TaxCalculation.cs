using TaxCalculator.Domain.Common;

namespace TaxCalculator.Domain.Entities;


public class TaxCalculation : Entity
{
    private TaxCalculation()
    {
        CreatedDate = DateTime.UtcNow;
    }

    public double GrossAnnualSalary { get; private set; }

    public double GrossMonthlySalary { get; private set; }

    public double NetAnnualSalary { get; private set; }

    public double NetMonthlySalary { get; private set; }

    public double AnnualTaxPaid { get; private set; }

    public double MonthlyTaxPaid { get; private set; }

    public static TaxCalculation Calculate(double grossAnnualSalary, IEnumerable<TaxBandType> bands)
    {
        if (!bands.Any()) 
        {
            throw new ArgumentException("At least one tax band must be provided.", nameof(bands));
        }

        var calculation = new TaxCalculation();
        var annualPaidTax = 0d;

        foreach (var band in bands)
        {
            annualPaidTax += CalculateTaxForBand(grossAnnualSalary, band);
        }

        calculation.GrossAnnualSalary = grossAnnualSalary;
        calculation.GrossMonthlySalary = grossAnnualSalary / 12;
        calculation.NetAnnualSalary = grossAnnualSalary - annualPaidTax;
        calculation.NetMonthlySalary = calculation.NetAnnualSalary / 12;
        calculation.AnnualTaxPaid = annualPaidTax;
        calculation.MonthlyTaxPaid = annualPaidTax / 12;

        return calculation;
    }

    private static double CalculateTaxForBand(double salary, TaxBandType band)
    {
        if (salary <= band.MinRange)
            return 0;

        var taxableAmount = Math.Min(salary, band.MaxRange) - band.MinRange;

        return taxableAmount * band.TaxRate / 100;
    }
}
