using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class CurrencyRate : ICurrencyRate
{
    internal CurrencyRate(IMoneyOptions money, ITimePeriodOptions timePeriod)
    {
        this.Money = money ?? throw new CurrencyIsNotDefinedException();
        this._timePeriod = new TimePeriod(timePeriod);
    }

    public IMoneyOptions Money { get; private set; }

    ITimePeriodOptions ICurrencyRateOptions.TimePeriod => _timePeriod;
    ITimePeriod ICurrencyRate.TimePeriod => _timePeriod;
 
    private readonly TimePeriod _timePeriod;

    public bool DoesItOverlapWith(ITimePeriodOptions before)
    {
        return this._timePeriod.DoesItOverlapWith(before);
    }
}