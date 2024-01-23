using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ITimePeriodOptions
{
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }

    bool DoesTheTimePeriodOverlapWith(ITimePeriodOptions period);
}

public class TimePeriod : ITimePeriodOptions, IEquatable<TimePeriod>
{
    public DateTime? FromDate { get; private set; }
    public DateTime? ToDate { get; private set; }

    public TimePeriod(DateTime fromDate, DateTime toDate)
    {
        GuardAgainstInvalidTimePeriod(fromDate, toDate);
        FromDate = fromDate;
        ToDate = toDate;
    }

    public bool DoesTheTimePeriodOverlapWith(ITimePeriodOptions other)
    {
        return this.FromDate <= other.ToDate && other.FromDate <= this.ToDate;
    }

    public static ITimePeriodOptions New(DateTime fromDate, DateTime toDate)
    {
        return new TimePeriod(fromDate, toDate);
    }
    private void GuardAgainstInvalidTimePeriod(DateTime fromDate, DateTime toDate)
    {
        if (fromDate == default) throw new FromDateIsEmptyException();
        if (toDate == default)
            throw new ToDateIsEmptyException();
        if (fromDate > toDate)
            throw new FromDateIsNotValidException();
    }

    public bool Equals(TimePeriod? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FromDate.Equals(other.FromDate) && ToDate.Equals(other.ToDate);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TimePeriod)obj);
    }

    public override int GetHashCode()
    {
        return FromDate.GetHashCode() ^ ToDate.GetHashCode(); //HashCode.Combine(FromDate, ToDate);
    }
}