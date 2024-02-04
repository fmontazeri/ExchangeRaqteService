using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class CurrencyTestBuilder : ICurrencyOptions
{
    public string Symbol { get; private set; }
    public List<ICurrencyRateOptions> CurrencyRates { get; private set; }
    private CurrencyRateTestBuilder _builder;

    public CurrencyTestBuilder()
    {
        this._builder = new CurrencyRateTestBuilder();
        this.Symbol = CurrencyConsts.SOME_CURRENCY;
        this.CurrencyRates = new List<ICurrencyRateOptions>(){};
    }

    public CurrencyTestBuilder WithSymbol(string symbol)
    {
        this.Symbol = symbol;
        return this;
    }

    // public CurrencyTestBuilder WithTimePeriod(ITimePeriodOptions timePeriod)
    // {
    //     var currencyRate = _builder
    //         .WithTimePeriod(timePeriod)
    //         .Build();
    //     this.CurrencyRates.Add(currencyRate);
    //     return this;
    // }

    public CurrencyTestBuilder WithCurrencyRates(params ICurrencyRateOptions[] currencyRates)
    {
        this.CurrencyRates.AddRange(currencyRates);
        return this;
    }
    
    public CurrencyTestBuilder WithCurrencyRate(ITimePeriodOptions timePeriod)
    {
        var currentRate = _builder.WithTimePeriod(timePeriod).Build();
        this.CurrencyRates.Add(currentRate);
        return this;
    }
    public Currency Build()
    {
        return new Currency(this.Symbol, this.CurrencyRates);
    }
}