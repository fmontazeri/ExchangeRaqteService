namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class FromDateIsEmptyException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "FromDate should be defined!";
}