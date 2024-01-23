using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class TestCurrencyRateBuilder : ICurrencyRateOptions
{
    private CurrencyRateBuilder _builder;
    public IMoneyOptions Money => _builder.Money;
    public ITimePeriodOptions TimePeriod => _builder.TimePeriod;


    public TestCurrencyRateBuilder()
    {
        _builder = new CurrencyRateBuilder();
        _builder.WithMoney(CurrencyAgg.Money.New(CurrencyConsts.SOME_PRICE, CurrencyConsts.SOME_CURRENCY));
        _builder.WithTimePeriod(CurrencyAgg.TimePeriod.New(DayConsts.TODAY, DayConsts.TODAY.AddDays(DayConsts.SOME_DAYS)));
    }

    public void Assert(ICurrencyRateOptions actual)
    {
        actual.Should().BeEquivalentTo<ICurrencyRateOptions>(this);
    }

    public TestCurrencyRateBuilder WithMoney(IMoneyOptions options)
    {
        _builder.WithMoney(options);
        return this;
    }

    public TestCurrencyRateBuilder WithTimePeriod(ITimePeriodOptions options)
    {
        _builder.WithTimePeriod(options);
        return this;
    }

    public ICurrencyRateOptions Build()
    {
        return _builder.Build();
    }

    public ICurrencyRateOptions BuildOptions()
    {
        return this;
    }
}