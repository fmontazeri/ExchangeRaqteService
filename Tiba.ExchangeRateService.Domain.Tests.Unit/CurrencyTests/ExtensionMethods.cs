using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public static class ExtensionMethods
{
    public static void AssertCurrencyRates(this Currency actual, params ICurrencyRateOptions[] currencyRates)
    {
        foreach (var expectation in currencyRates)
        {
            actual.CurrencyRates.Should().ContainEquivalentOf(expectation);
        }
    }
}