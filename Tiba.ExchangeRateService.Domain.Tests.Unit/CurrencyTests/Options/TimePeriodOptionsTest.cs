using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

internal class TimePeriodOptionsTest(DateTime? fromDate, DateTime? toDate) : ITimePeriodOptions
{
    public DateTime? FromDate { get;  } = fromDate;
    public DateTime? ToDate { get;  } = toDate;

    public bool DoesOverlapWith(ITimePeriodOptions before)
    {
        return (!this.FromDate.HasValue || !before.ToDate.HasValue || this.FromDate <= before.ToDate) &&
               (!before.FromDate.HasValue || !this.ToDate.HasValue || before.FromDate <= this.ToDate); 
    }
}