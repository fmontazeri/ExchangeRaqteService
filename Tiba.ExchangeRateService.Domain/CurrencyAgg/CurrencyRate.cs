using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class CurrencyRate : ICurrencyRateOptions
{
    internal CurrencyRate(IMoneyOption money, DateTime fromDate, DateTime toDate)
    {
        GuardAgainstInvalidTimePeriod(fromDate, toDate);
        this.Money = money;
        this.FromDate = fromDate;
        this.ToDate = toDate;
    }

    private void GuardAgainstInvalidTimePeriod(DateTime fromDate, DateTime toDate)
    {
        if (fromDate == default) throw new FromDateIsEmptyException();
        if (toDate == default)
            throw new ToDateIsEmptyException();
        if (fromDate > toDate)
            throw new FromDateIsNotValidException();
    }

    public IMoneyOption Money { get; private set; }
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
}