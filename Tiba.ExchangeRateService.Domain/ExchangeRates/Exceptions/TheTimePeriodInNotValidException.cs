namespace Tiba.ExchangeRateService.Domain.ExchangeRates.Exceptions;

public class TheTimePeriodInNotValidException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "The FromDate and ToDate should not be Null.";
}