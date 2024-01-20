namespace Tiba.ExchangeRateService.Domain.ExchangeRates;

public class ExchangeRateBuilder : IExchangeRate
{
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }

    // public ExchangeRate Build()
    // {
    //     return new ExchangeRate(FromDate, ToDate, Price);
    // }
}