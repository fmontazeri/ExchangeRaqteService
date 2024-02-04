using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.TestClasses;

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