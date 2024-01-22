namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public sealed class CurrencyRateOptionsBuilder : ICurrencyRateOptions
{
    public ITimePeriodOptions TimePeriod { get; set; }
    public IMoneyOptions Money { get; private set; }

    public CurrencyRateOptionsBuilder WithTimePeriod(ITimePeriodOptions timePeriod)
    {
        this.TimePeriod = CurrencyAgg.TimePeriod.New(timePeriod.FromDate, timePeriod.ToDate);
        return this;
    }
    public CurrencyRateOptionsBuilder WithMoney(IMoneyOptions money)
    {
        this.Money = money;
        return this;
    }

    public ICurrencyRateOptions Build()
    {
        return new CurrencyRate(this.Money, TimePeriod);
    }
}