namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class CurrencyRateBuilder : ICurrencyRate
{
    public virtual string Currency { get; private set; }
    public  virtual DateTime FromDate { get; private set; }
    public virtual DateTime ToDate { get; private set; }
    public  virtual decimal Price { get; private set; }


    public CurrencyRateBuilder WithCurrency(string currency)
    {
        this.Currency = currency;
        return this;
    }
    public CurrencyRateBuilder WithFromDate(DateTime fromDate)
    {
        this.FromDate = fromDate;
        return this;
    }

    public CurrencyRateBuilder WithToDate(DateTime toDate)
    {
        this.ToDate = toDate;
        return this;
    }

    public CurrencyRateBuilder WithPrice(decimal price)
    {
        this.Price = price;
        return this;
    }

    public  ICurrencyRate Build()
    {
        return new CurrencyRate(Currency, FromDate, ToDate, Price);
    }
}