namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public class AmountIsNotValidException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "The Price should be greater than zero.";
}