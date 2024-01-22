using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public static class ExtensionMethods
{
    public static void AssertCurrencyRates(this Currency currency, params ICurrencyRateOptions[] currencyRates)
    {
        currency.CurrencyRates.Should().BeEquivalentTo(currencyRates);
    }
    
}