using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Options;

internal class TimePeriodOptionsTest(DateTime? fromDate, DateTime? toDate) : ITimePeriodOptions
{
    public DateTime? FromDate { get; } = fromDate;
    public DateTime? ToDate { get; } = toDate;
}