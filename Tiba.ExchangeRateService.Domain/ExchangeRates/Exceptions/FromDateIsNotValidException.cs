namespace Tiba.ExchangeRateService.Domain.ExchangeRates.Exceptions;

public class FromDateIsNotValidException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "FromDate should be smaller or equal than ToDate.";
}