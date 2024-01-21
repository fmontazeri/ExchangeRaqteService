namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class OverlapTimePeriodException(DateTime startDate) : Exception(string.Format(ErrorMessage, startDate))
{
    public const string ErrorMessage = "The time period should be start from {0}";
}