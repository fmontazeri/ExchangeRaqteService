using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

internal class ExchangeRate : IExchangeRate
{
    public ExchangeRate(DateTime fromDate, DateTime toDate, decimal price, DateTime? startDate = null)
    {
        if (price <= 0)
            throw new PriceIsNotValidException();

        GuardAgainstInvalidTimePeriod(fromDate, toDate, startDate);

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

    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal Price { get; private set; }
}