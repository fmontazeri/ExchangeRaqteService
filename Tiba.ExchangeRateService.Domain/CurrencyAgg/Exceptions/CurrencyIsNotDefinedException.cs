namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

public  class CurrencyIsNotDefinedException() : Exception(ErrorMessage)
{
    public const string ErrorMessage = "Currency should be defined!";
}