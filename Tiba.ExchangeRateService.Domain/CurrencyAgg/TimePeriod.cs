using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class TimePeriod : ITimePeriodOptions, IEquatable<TimePeriod>
{
    public DateTime? FromDate { get; private set; }
    public DateTime? ToDate { get; private set; }

    private TimePeriod(DateTime? fromDate, DateTime? toDate)
    {
        GuardAgainstInvalidTimePeriod(fromDate, toDate);
        FromDate = fromDate;
        ToDate = toDate;
    }

    public bool DoesOverlapWith(ITimePeriodOptions before)
    {
        // return  ( this.FromDate.HasValue? this.FromDate <= before.ToDate : this.ToDate <= before.ToDate) &&  
        //         (before.FromDate.HasValue? before.FromDate <= this.ToDate : before.ToDate >= this.ToDate) 
        //         ||  this.FromDate < before.ToDate && before.FromDate < this.ToDate;

        return (!this.FromDate.HasValue || !before.ToDate.HasValue || this.FromDate <= before.ToDate) &&
               (!before.FromDate.HasValue || !this.ToDate.HasValue || before.FromDate <= this.ToDate); 
    }

    public static ITimePeriodOptions New(DateTime? fromDate, DateTime? toDate)
    {
        return new TimePeriod(fromDate, toDate);
    }

    private void GuardAgainstInvalidTimePeriod(DateTime? fromDate, DateTime? toDate)
    {
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