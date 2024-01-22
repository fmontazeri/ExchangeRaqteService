using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class CurrencyRate : ICurrencyRateOptions
{
    internal CurrencyRate(IMoneyOptions money, ITimePeriodOptions timePeriod)
    {
        this.Money = money;
        this.TimePeriod = timePeriod;
    }

    public IMoneyOptions Money { get; private set; }
    public ITimePeriodOptions TimePeriod { get; private set; }
}