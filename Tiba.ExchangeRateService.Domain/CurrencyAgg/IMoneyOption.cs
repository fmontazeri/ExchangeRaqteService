namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface IMoneyOption
{
    public decimal Amount { get; }
    public string Currency { get; }
}