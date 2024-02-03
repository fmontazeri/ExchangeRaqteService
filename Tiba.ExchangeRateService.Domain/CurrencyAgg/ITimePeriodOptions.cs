namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ITimePeriodOptions
{
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }
    bool DoesOverlapWith(ITimePeriodOptions before);
}

