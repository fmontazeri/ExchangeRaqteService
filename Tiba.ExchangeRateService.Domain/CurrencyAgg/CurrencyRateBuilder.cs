namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public sealed class CurrencyRateBuilder : ICurrencyRateOptions
{
    public ITimePeriodOptions TimePeriod { get; private set; }
    public IMoneyOptions Money { get; private set; }

    public CurrencyRateBuilder WithTimePeriod(ITimePeriodOptions timePeriod)
    {
        this.TimePeriod = timePeriod;
        return this;
    }
    public CurrencyRateBuilder WithMoney(IMoneyOptions money)
    {
        this.Money = money;
        return this;
    }

 
    public ICurrencyRateOptions Build()
    {
        return new CurrencyRate(this.Money, TimePeriod);
    }
}