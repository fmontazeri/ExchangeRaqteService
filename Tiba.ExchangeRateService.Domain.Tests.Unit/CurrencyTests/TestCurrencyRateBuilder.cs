using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class TestCurrencyRateBuilder : ICurrencyRateOptions
{
    private CurrencyRateOptionsBuilder _builder;
    public string Currency { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime FromDate => _builder.FromDate;
    public DateTime ToDate => _builder.ToDate;
    public IMoneyOption Money => _builder.Money;


    public TestCurrencyRateBuilder()
    {
        _builder = new CurrencyRateOptionsBuilder();
        this.Currency = CurrencyConsts.SOME_CURRENCY;
        this.Amount = CurrencyConsts.SOME_PRICE;
        _builder.WithMoney(new Money(this.Amount, this.Currency));
        _builder.WithFromDate(TimePeriod.TODAY);
        _builder.WithToDate(TimePeriod.TODAY.AddDays(TimePeriod.SOME_DAYS));
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
        _builder.WithFromDate(fromDate);
        return this;
    }

    public TestCurrencyRateBuilder WithToDate(DateTime toDate)
    {
        _builder.WithToDate(toDate);
        return this;
    }
    public TestCurrencyRateBuilder WithMoney(IMoneyOption money)
    {
        this.Currency = money.Currency;
        this.Amount = money.Amount;
        _builder.WithMoney(money);
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