namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

public interface ITimePeriodOptions
{
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }
    bool DoesOverlapWith(ITimePeriodOptions before);
}

