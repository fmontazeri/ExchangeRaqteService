namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class ExchangeRateBuilder : IExchangeRate
{
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }


    public ExchangeRateBuilder WithFromDate(DateTime fromDate)
    {
        this.FromDate = fromDate;
        return this;
    }

    public ExchangeRateBuilder WithToDate(DateTime toDate)
    {
        this.ToDate = toDate;
        return this;
    }

    public ExchangeRateBuilder WithPrice(decimal price)
    {
        this.Price = price;
        return this;
    }

    public IExchangeRate Build()
    {
        return new ExchangeRate(FromDate, ToDate, Price);
    }
}