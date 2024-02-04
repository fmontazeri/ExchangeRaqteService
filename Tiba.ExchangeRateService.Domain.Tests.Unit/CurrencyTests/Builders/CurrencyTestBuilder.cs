using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class CurrencyTestBuilder : ICurrencyOptions
{
    public string Symbol { get; private set; }
    public List<ICurrencyRateOptions> CurrencyRates { get; private set; } = new();
    private CurrencyRateTestBuilder _builder;

    public CurrencyTestBuilder()
    {
        this._builder = new CurrencyRateTestBuilder();
        this.Symbol = CurrencyConsts.SOME_CURRENCY;
    }

    public CurrencyTestBuilder WithTimePeriod(DateTime? fromDate, DateTime? toDate)
    {
        var currentRate = _builder.WithTimePeriod(fromDate, toDate).BuildOptions();
        this.CurrencyRates.Add(currentRate);
        return this;
    }
    public Currency Build()
    {
        return new Currency(this.Symbol, this.CurrencyRates);
    }
}