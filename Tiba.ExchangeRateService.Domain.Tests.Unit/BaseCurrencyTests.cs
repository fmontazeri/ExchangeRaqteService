using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit;

public abstract class BaseCurrencyTests
{
    protected (DateTime? from1, DateTime? to1, DateTime? from2, DateTime? to2) GetTimePeriod(int? fromDate1, int? toDate1,
        int? fromDate2, int? toDate2)
    {
        DateTime? from1 = fromDate1.HasValue ? DayConsts.TODAY.AddDays(fromDate1.Value) : null;
        DateTime? to1 = toDate1.HasValue ? DayConsts.TODAY.AddDays(toDate1.Value) : null;
        DateTime? from2 = fromDate2.HasValue ? DayConsts.TODAY.AddDays(fromDate2.Value) : null;
        DateTime? to2 = toDate2.HasValue ? DayConsts.TODAY.AddDays(toDate2.Value) : null;
        return (from1, to1, from2, to2);
    }

    protected (DateTime? from1, DateTime? to1, DateTime? from2, DateTime? to2, DateTime? from3, DateTime? to3)
        GetTimePeriod(int? fromDate1, int? toDate1,
            int? fromDate2, int? toDate2, int? fromDate3, int? toDate3)
    {
        DateTime? from1 = fromDate1.HasValue ? DayConsts.TODAY.AddDays(fromDate1.Value) : null;
        DateTime? to1 = toDate1.HasValue ? DayConsts.TODAY.AddDays(toDate1.Value) : null;
        DateTime? from2 = fromDate2.HasValue ? DayConsts.TODAY.AddDays(fromDate2.Value) : null;
        DateTime? to2 = toDate2.HasValue ? DayConsts.TODAY.AddDays(toDate2.Value) : null;

        DateTime? from3 = fromDate3.HasValue ? DayConsts.TODAY.AddDays(fromDate3.Value) : null;
        DateTime? to3 = toDate3.HasValue ? DayConsts.TODAY.AddDays(toDate3.Value) : null;
        return (from1, to1, from2, to2, from3, to3);
    }
}