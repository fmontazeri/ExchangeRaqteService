namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ICurrencyRateOptions 
{
    public IMoneyOptions Money { get; }
    public ITimePeriodOptions TimePeriod { get; }
}

public interface ICurrencyRateOptions1
{
    public decimal Amount { get; set; }
    public ITimePeriodOptions TimePeriod { get; }
}