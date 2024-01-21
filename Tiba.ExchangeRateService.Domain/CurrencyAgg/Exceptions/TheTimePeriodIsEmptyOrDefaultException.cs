namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class TheTimePeriodIsEmptyOrDefaultException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "The FromDate and ToDate should not be Null.";
}