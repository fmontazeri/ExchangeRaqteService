namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class TheTimePeriodIsEmptyOrDefaultException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "FromDate and ToDate should have a value.";
}