namespace Tiba.ExchangeRateService.Domain.ExchangeRates.Exceptions;

public class PriceIsNotValidException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "The Price should be greater than zero.";
}