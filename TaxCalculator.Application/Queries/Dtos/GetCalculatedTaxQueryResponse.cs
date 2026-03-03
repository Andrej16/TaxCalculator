namespace TaxCalculator.Application.Queries.Dtos;

public record GetCalculatedTaxQueryResponse(
    int Id,
    double GrossAnnualSalary,
    double GrossMonthlySalary,
    double NetAnnualSalary,
    double NetMonthlySalary,
    double AnnualTaxPaid,
    double MonthlyTaxPaid);
