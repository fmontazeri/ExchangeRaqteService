namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

public interface ICurrencyRateOptions 
{ 
    IMoneyOptions Money { get; } 
    ITimePeriodOptions TimePeriod { get; }
}

public interface ICurrencyRate : ICurrencyRateOptions
{
    bool DoesItOverlapWith(ITimePeriodOptions before);
    
    ITimePeriod TimePeriod { get; }
}

