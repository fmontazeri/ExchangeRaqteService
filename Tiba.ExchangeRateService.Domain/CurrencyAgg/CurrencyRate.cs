using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class CurrencyRate : ICurrencyRate
{
    internal CurrencyRate(string currency, DateTime fromDate, DateTime toDate, decimal price, DateTime? startDate = null)
    {
        if (price <= 0)
            throw new PriceIsNotValidException();
        if (string.IsNullOrWhiteSpace(currency))
            throw new CurrencyIsNotDefinedException();

        GuardAgainstInvalidTimePeriod(fromDate, toDate, startDate);

        this.Currency = currency;
        this.FromDate = fromDate;
        this.ToDate = toDate;
        this.Price = price;
    }

    private void GuardAgainstInvalidTimePeriod(DateTime fromDate, DateTime toDate, DateTime? startDate = null)
    {
        if (fromDate == default || toDate == default)
            throw new TheTimePeriodIsEmptyOrDefaultException();
        if (startDate.HasValue && fromDate <= startDate || toDate <= startDate)
            throw new OverlapTimePeriodException(startDate.Value);
        if (fromDate > toDate)
            throw new FromDateIsNotValidException();
    }

    public string Currency { get; private set; }
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }
}