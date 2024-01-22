namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public sealed class CurrencyRateOptionsBuilder : ICurrencyRateOptions
{
    public string Currency { get; private set; }
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }


    public CurrencyRateOptionsBuilder WithCurrency(string currency)
    {
        this.Currency = currency;
        return this;
    }
    public CurrencyRateOptionsBuilder WithFromDate(DateTime fromDate)
    {
        this.FromDate = fromDate;
        return this;
    }

    public CurrencyRateOptionsBuilder WithToDate(DateTime toDate)
    {
        this.ToDate = toDate;
        return this;
    }

    public CurrencyRateOptionsBuilder WithPrice(decimal price)
    {
        this.Price = price;
        return this;
    }

    public  CurrencyRate Build()
    {
        return new CurrencyRate(Currency, FromDate, ToDate, Price);
    }
}