namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class TestExchangeRate : IExchangeRate
{
    public string Currency { get; }
    public DateTime FromDate { get; }
    public DateTime ToDate { get; }
    public decimal Price { get; }

    public TestExchangeRate(DateTime fromDate, DateTime toDate, decimal price)
    {
        FromDate = fromDate;
        ToDate = toDate;
        Price = price;
    }
}