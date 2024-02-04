using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public sealed class CurrencyRateBuilder : ICurrencyRateOptions
{
    public ITimePeriodOptions TimePeriod { get; private set; }
    public IMoneyOptions Money { get; private set; }

    public CurrencyRateBuilder WithTimePeriod(ITimePeriodOptions options)
    {
        this.TimePeriod = new TimePeriod(options);
        return this;
    }

    public CurrencyRateBuilder WithMoney(IMoneyOptions options)
    {
        this.Money = new Money(options);
        return this;
    }

    public CurrencyRate Build()
    {
        return new CurrencyRate(this.Money, TimePeriod);
    }
}