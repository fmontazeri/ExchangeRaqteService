using Tiba.ExchangeRateService.Domain.ExchangeRates.Exceptions;

namespace Tiba.ExchangeRateService.Domain.ExchangeRates;

public class ExchangeRate
{
    public ExchangeRate(DateTime fromDate, DateTime toDate, decimal price)
    {
        if (price <= 0)
            throw new PriceIsNotValidException();

        if (fromDate < toDate)
            throw new FromDateIsNotValidException();
        
        this.FromDateDate = fromDate;
        this.ToDateDate = toDate;
        this.Price = price;
    }


    public DateTime FromDateDate { get; private set; }
    public DateTime ToDateDate { get; private set; }
    public decimal Price { get; private set; }
}