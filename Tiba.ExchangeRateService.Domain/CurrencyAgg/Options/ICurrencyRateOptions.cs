namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

public interface ICurrencyRateOptions 
{
    public IMoneyOptions Money { get; }
    public ITimePeriodOptions TimePeriod { get; }
}

