namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface IExchangeRate
{
    public DateTime FromDate { get; }
    public DateTime ToDate { get; }
    public decimal Price { get; }
}