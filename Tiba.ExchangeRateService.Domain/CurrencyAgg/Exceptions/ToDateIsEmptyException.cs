namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class ToDateIsEmptyException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "ToDate should be defined.";
}