using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class TestCurrencyBuilder : ICurrencyOptions
{
    public string Symbol { get; private set; }
    public List<ICurrencyRateOptions> CurrencyRates { get; private set; }
    private TestCurrencyRateBuilder _builder;


    public TestCurrencyBuilder()
    {
        this._builder = new TestCurrencyRateBuilder();
        this.Symbol = CurrencyConsts.SOME_CURRENCY;
        this.CurrencyRates = new List<ICurrencyRateOptions>();
    }

    public TestCurrencyBuilder WithSymbol(string symbol)
    {
        this.Symbol = symbol;
        return this;
    }

    public TestCurrencyBuilder WithTimePeriod(ITimePeriodOptions timePeriod)
    {
        var currencyRate = _builder
            .WithTimePeriod(timePeriod)
            .Build();
        this.CurrencyRates.Add(currencyRate);
        return this;
    }

    public Currency Build()
    {
        return new Currency(this.Symbol, this.CurrencyRates);
    }
}