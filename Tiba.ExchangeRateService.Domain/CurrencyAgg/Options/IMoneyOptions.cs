namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

public interface IMoneyOptions
{
    public decimal Amount { get; }
    public string Currency { get; }
}