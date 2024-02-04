using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.TestClasses;

public class CurrencyRateTest : ICurrencyRateOptions
{
    public IMoneyOptions Money { get; }
    public ITimePeriodOptions TimePeriod { get; }

    public CurrencyRateTest(IMoneyOptions money, ITimePeriodOptions timePeriod)
    {
        Money = money;
        TimePeriod = timePeriod;
    }
}