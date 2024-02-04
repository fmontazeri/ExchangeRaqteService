using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

internal class MoneyOptionsTest(decimal amount, string currency) : IMoneyOptions
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency;
}