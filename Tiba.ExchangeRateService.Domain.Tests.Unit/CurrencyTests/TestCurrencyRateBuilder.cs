using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class TestCurrencyRateBuilder : ICurrencyRateOptions
{
    public string Currency { get; private set; }
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }


    public TestCurrencyRateBuilder()
    {
        WithCurrency(CurrencyConsts.SOME_CURRENCY);
        WithFromDate(DateTime.Today);
        WithToDate(DateTime.Today.AddDays(TimePeriod.SOME_DAYS));
        WithPrice(CurrencyConsts.SOME_PRICE);
    }

    public void Assert(ICurrencyRateOptions actual)
    {
        actual.Should().BeEquivalentTo<ICurrencyRateOptions>(this);
    }

    public TestCurrencyRateBuilder WithCurrency(string currency)
    {
        this.Currency = currency;
        return this;
    }

    public TestCurrencyRateBuilder WithFromDate(DateTime fromDate)
    {
        this.FromDate = fromDate;
        return this;
    }

    public TestCurrencyRateBuilder WithToDate(DateTime toDate)
    {
        this.ToDate = toDate;
        return this;
    }

    public TestCurrencyRateBuilder WithPrice(decimal price)
    {
        this.Price = price;
        return this;
    }


    public CurrencyRate Build()
    {
        return new CurrencyRateOptionsBuilder()
            .WithCurrency(this.Currency)
            .WithFromDate(this.FromDate)
            .WithToDate(this.ToDate)
            .WithPrice(this.Price).Build();
    }

    public ICurrencyRateOptions BuildOptions()
    {
        return this;
    }
}