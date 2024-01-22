using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class TestCurrencyRateBuilder : ICurrencyRateOptions
{
    private CurrencyRateOptionsBuilder _builder;
    public string Currency { get; private set; }
    public decimal Amount { get; private set; }
    public IMoneyOptions Money => _builder.Money;
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public ITimePeriodOptions TimePeriod => _builder.TimePeriod;


    public TestCurrencyRateBuilder()
    {
        _builder = new CurrencyRateOptionsBuilder();
        this.Currency = CurrencyConsts.SOME_CURRENCY;
        this.Amount = CurrencyConsts.SOME_PRICE;
        _builder.WithMoney(new Money(this.Amount, this.Currency));
        this.FromDate = TestTimePeriod.TODAY;
        this.ToDate = TestTimePeriod.TODAY.AddDays(TestTimePeriod.SOME_DAYS);
        _builder.WithTimePeriod(CurrencyAgg.TimePeriod.New(this.FromDate, this.ToDate));
    }

    public void Assert(ICurrencyRateOptions actual)
    {
        actual.Should().BeEquivalentTo<ICurrencyRateOptions>(this);
    }

    public TestCurrencyRateBuilder WithCurrency(string currency)
    {
        this.Currency = currency;
        _builder.WithMoney(new Money(_builder.Money.Amount, currency));
        return this;
    }

    public TestCurrencyRateBuilder WithAmount(decimal price)
    {
        this.Amount = price;
        _builder.WithMoney(new Money(price, _builder.Money.Currency));
        return this;
    }

    public TestCurrencyRateBuilder WithFromDate(DateTime fromDate)
    {
        this.FromDate = fromDate;
        _builder.WithTimePeriod(CurrencyAgg.TimePeriod.New(fromDate, _builder.TimePeriod.ToDate));
        return this;
    }

    public TestCurrencyRateBuilder WithToDate(DateTime toDate)
    {
        this.ToDate = toDate;
        _builder.WithTimePeriod(CurrencyAgg.TimePeriod.New(_builder.TimePeriod.FromDate, ToDate));
        return this;
    }

    public TestCurrencyRateBuilder WithMoney(IMoneyOptions options)
    {
        this.Currency = options.Currency;
        this.Amount = options.Amount;
        _builder.WithMoney(options);
        return this;
    }

    public TestCurrencyRateBuilder WithTimePeriod(ITimePeriodOptions options)
    {
        this.FromDate = options.FromDate;
        this.ToDate = options.ToDate;
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