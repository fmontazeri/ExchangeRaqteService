using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency
{
    public Currency(ICurrencyRateOptions options)
    {
        if (options.Money is null)
            throw new CurrencyIsNotDefinedException();

        GuardAgainstInvalidTimePeriod(options.TimePeriod);

        this.Name = options.Money.Currency;
        var currencyRateOptions = new CurrencyRateBuilder()
            .WithTimePeriod(options.TimePeriod)
            .WithMoney(options.Money)
            .Build();
        this._currencyRates.Add(currencyRateOptions);
    }

    private void GuardAgainstInvalidTimePeriod(ITimePeriodOptions timePeriod)
    {
        if (timePeriod is null)
            throw new TimePeriodIsNotDefinedException();

        if (this._currencyRates.Any(currencyRate =>
                timePeriod.FromDate <= currencyRate.TimePeriod.ToDate &&
                currencyRate.TimePeriod.FromDate <= timePeriod.ToDate))
            throw new OverlapTimePeriodException();
    }

    public string Name { get; private set; }

    private List<ICurrencyRateOptions> _currencyRates = new();
    public IReadOnlyCollection<ICurrencyRateOptions> CurrencyRates => _currencyRates;

    public void Add(ITimePeriodOptions timePeriod, decimal price)
    {
        GuardAgainstInvalidTimePeriod(timePeriod);

        var currencyRateOptions = new CurrencyRateBuilder()
            .WithTimePeriod(timePeriod)
            .WithMoney(Money.New(price, this.Name))
            .Build();
        this._currencyRates.Add(currencyRateOptions);
    }
}