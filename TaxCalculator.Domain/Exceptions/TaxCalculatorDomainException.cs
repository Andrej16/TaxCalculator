namespace TaxCalculator.Domain.Exceptions;

public class TaxCalculatorDomainException : Exception
{
    public TaxCalculatorDomainException()
    {
    }

    public TaxCalculatorDomainException(string message)
        : base(message)
    {
    }

    public TaxCalculatorDomainException(string message, string paramName)
        : base(message)
    {
        ParamName = paramName;
    }

    public string? ParamName { get; init; }
}