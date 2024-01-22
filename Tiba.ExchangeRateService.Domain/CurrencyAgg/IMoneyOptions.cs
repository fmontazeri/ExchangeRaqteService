namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface IMoneyOptions
{
    public decimal Amount { get; }
    public string Currency { get; }
}