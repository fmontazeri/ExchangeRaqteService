namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class TimePeriodIsNotDefinedException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "Time period should be defined.";
}