using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

internal class MoneyTest : IMoneyOptions
{
    public decimal Amount { get; }
    public string Currency { get; }


    public MoneyTest(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}