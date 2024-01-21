namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface IExchangeRate
{
    public string Currency { get;}
    public DateTime FromDate { get; }
    public DateTime ToDate { get; }
    public decimal Price { get; }
}