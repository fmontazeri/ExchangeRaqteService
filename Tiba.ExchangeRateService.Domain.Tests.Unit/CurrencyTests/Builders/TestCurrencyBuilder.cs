using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class TestCurrencyBuilder : ICurrencyOptions
{
    public string Symbol { get; private set; }
    public List<ICurrencyRateOptions> CurrencyRates { get; private set; }
    private CurrencyRateBuilder _builder;


    public TestCurrencyBuilder()
    {
        this._builder = new CurrencyRateBuilder();
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
        var currencyRate = _builder.WithTimePeriod(timePeriod);
        this.CurrencyRates.Add(currencyRate);
        return this;
    }

    public ICurrencyOptions Build()
    {
        return new Currency(this.Symbol, this.CurrencyRates);
    }
}