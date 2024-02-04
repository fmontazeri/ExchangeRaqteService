namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

public interface ITimePeriodOptions
{
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }
}

public interface ITimePeriod : ITimePeriodOptions
{
    bool DoesOverlapWith(ITimePeriodOptions before);
}

