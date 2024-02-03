using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class CurrencyRateTestBuilder : ICurrencyRateOptions
{
    private CurrencyRateBuilder _builder;
    public IMoneyOptions Money => _builder.Money;
    public ITimePeriodOptions TimePeriod => _builder.TimePeriod;


    public CurrencyRateTestBuilder()
    {
        _builder = new CurrencyRateBuilder();
        _builder.WithMoney(CurrencyAgg.Money.New(CurrencyConsts.SOME_PRICE, CurrencyConsts.SOME_CURRENCY))
            .WithTimePeriod(CurrencyAgg.TimePeriod.New(Days.TODAY, Days.TODAY.AddDays(Days.SOME_DAYS)));
    }

    public void Assert(ICurrencyRateOptions actual)
    {
        actual.Should().BeEquivalentTo<ICurrencyRateOptions>(this);
    }

    public CurrencyRateTestBuilder WithMoney(IMoneyOptions options)
    {
        _builder.WithMoney(options);
        return this;
    }

    public CurrencyRateTestBuilder WithTimePeriod(ITimePeriodOptions options)
    {
        _builder.WithTimePeriod(options);
        return this;
    }

    public CurrencyRate Build()
    {
        return _builder.Build();
    }

    public ICurrencyRateOptions BuildOptions()
    {
        return this;
    }
}