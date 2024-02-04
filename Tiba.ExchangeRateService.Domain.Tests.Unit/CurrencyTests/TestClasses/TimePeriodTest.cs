using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.TestClasses;

internal class TimePeriodTest : ITimePeriodOptions
{
    public DateTime? FromDate { get;  }
    public DateTime? ToDate { get;  }

    public TimePeriodTest(DateTime? fromDate, DateTime? toDate)
    {
        FromDate = fromDate;
        ToDate = toDate;
    }

    public bool DoesOverlapWith(ITimePeriodOptions before)
    {
        return (!this.FromDate.HasValue || !before.ToDate.HasValue || this.FromDate <= before.ToDate) &&
               (!before.FromDate.HasValue || !this.ToDate.HasValue || before.FromDate <= this.ToDate); 
    }
}