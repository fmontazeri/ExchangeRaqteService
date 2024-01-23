using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class CurrencyRate : ICurrencyRateOptions
{
    internal CurrencyRate(IMoneyOptions money, ITimePeriodOptions timePeriod)
    {
        this.Money = money ?? throw new CurrencyIsNotDefinedException();
        this.TimePeriod = timePeriod ?? throw new TimePeriodIsNotDefinedException();
    }

    public IMoneyOptions Money { get; private set; }
    public ITimePeriodOptions TimePeriod { get; private set; }
}