namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class OverlapTimePeriodException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "The time period overlaped with another one.";
}