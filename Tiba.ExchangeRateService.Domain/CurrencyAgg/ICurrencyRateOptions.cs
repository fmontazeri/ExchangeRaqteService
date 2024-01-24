namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ICurrencyRateOptions 
{
    public IMoneyOptions Money { get; }
    public ITimePeriodOptions TimePeriod { get; }
}

