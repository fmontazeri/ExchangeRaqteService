using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

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

    public CurrencyTestBuilder WithTimePeriod(DateTime? fromDate , DateTime? toDate )
    {
        var currentRate = _builder.WithTimePeriod(new TimePeriodOptionsTest(fromDate,toDate)).BuildOptions();
        this.CurrencyRates.Add(currentRate);
        return this;
    }
    public Currency Build()
    {
        return new Currency(this.Symbol, this.CurrencyRates);
    }
}