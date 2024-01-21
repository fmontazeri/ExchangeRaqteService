namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ICurrencyRate
{
    public string Currency { get;}
    public DateTime FromDate { get; }
    public DateTime ToDate { get; }
    public decimal Price { get; }
}