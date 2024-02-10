using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Builders;

public class CurrencyRateTestBuilder : ICurrencyRateOptions
{
    public IMoneyOptions Money { get; private set; }
    public ITimePeriodOptions TimePeriod { get; private set; }

    private readonly CurrencyRateBuilder _builder;

    public CurrencyRateTestBuilder()
    {
        _builder = new CurrencyRateBuilder();
        this.WithMoney(CurrencyConsts.SOME_PRICE, CurrencyConsts.SOME_CURRENCY)
            .WithTimePeriod(Days.TODAY, Days.TODAY.AddDays(Days.SOME_DAYS));
    }

    public void Assert(ICurrencyRateOptions actual)
    {
        actual.Should().BeEquivalentTo<ICurrencyRateOptions>(this);
    }

    public CurrencyRateTestBuilder WithMoney(decimal amount , string currency)
    {
        this.Money = new MoneyOptionsTest(amount , currency);
        return this;
    }

    public CurrencyRateTestBuilder WithTimePeriod(DateTime? fromDate, DateTime? toDate)
    {
        this.TimePeriod = new TimePeriodOptionsTest(fromDate, toDate);
        return this;
    }

    public ICurrencyRateOptions BuildOptions()
    {
        return new CurrencyRateOptionsTest(this.Money, this.TimePeriod);
    }

    public ICurrencyRate Build()
    {
        return _builder.WithMoney(this.Money).WithTimePeriod(this.TimePeriod).Build();
    }

    private class CurrencyRateOptionsTest(IMoneyOptions money, ITimePeriodOptions timePeriod) : ICurrencyRateOptions
    {
        public IMoneyOptions Money { get; } = money;
        public ITimePeriodOptions TimePeriod { get; } = timePeriod;
    }
}






// public class CurrencyRateTestBuilder : ICurrencyRateOptions
// {
//     private CurrencyRateBuilder _builder;
//     public IMoneyOptions Money => _builder.Money;
//     public ITimePeriodOptions TimePeriod => _builder.TimePeriod;
//
//     public CurrencyRateTestBuilder()
//     {
//         _builder = new CurrencyRateBuilder()
//             .WithMoney(new MoneyOptionsTest(CurrencyConsts.SOME_PRICE, CurrencyConsts.SOME_CURRENCY))
//             .WithTimePeriod(new TimePeriodOptionsTest(Days.TODAY, Days.TODAY.AddDays(Days.SOME_DAYS)));
//     }
//
//     public void Assert(ICurrencyRateOptions actual)
//     {
//         actual.Should().BeEquivalentTo<ICurrencyRateOptions>(this);
//     }
//
//     public CurrencyRateTestBuilder WithMoney(IMoneyOptions options)
//     {
//         _builder.WithMoney(options);
//         return this;
//     }
//
//     public CurrencyRateTestBuilder WithTimePeriod(ITimePeriodOptions options)
//     {
//         _builder.WithTimePeriod(options);
//         return this;
//     }
//
//     public ICurrencyRateOptions BuildOptions()
//     {
//         return new CurrencyRateOptionsTest(this.Money, this.TimePeriod);
//     }
//
//     public CurrencyRate Build()
//     {
//         return _builder.Build();
//     }
//
//     private class CurrencyRateOptionsTest(IMoneyOptions money, ITimePeriodOptions timePeriod) : ICurrencyRateOptions
//     {
//         public IMoneyOptions Money { get; } = money;
//         public ITimePeriodOptions TimePeriod { get; } = timePeriod;
//     }
// }