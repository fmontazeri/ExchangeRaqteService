using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class TestCurrencyRateBuilder : ICurrencyRate
{
    public string Currency { get; private set; }
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }


    public TestCurrencyRateBuilder()
    {
        WithCurrency(CurrencyRateConsts.SOME_CURRENCY);
        WithFromDate(DateTime.Today);
        WithToDate(DateTime.Today.AddDays(CurrencyRateConsts.SOME_DAYS));
        WithPrice(CurrencyRateConsts.SOME_PRICE);
    }

    public void Assert(ICurrencyRate actual)
    {
        actual.Should().BeEquivalentTo<ICurrencyRate>(this);
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

    public ICurrencyRate Build()
    {
        return new CurrencyRateBuilder()
            .WithCurrency(this.Currency)
            .WithFromDate(this.FromDate)
            .WithToDate(this.ToDate)
            .WithPrice(this.Price).Build();
    }
}