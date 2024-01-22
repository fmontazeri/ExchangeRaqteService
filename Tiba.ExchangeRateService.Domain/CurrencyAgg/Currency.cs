using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency
{
    public Currency(ICurrencyRateOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Currency))
            throw new CurrencyIsNotDefinedException();

        GuardAgainstOverlappingTheTimePeriod(options.FromDate, options.ToDate);

        this.Name = options.Currency;
        var currencyRate = new CurrencyRateOptionsBuilder()
            .WithFromDate(options.FromDate)
            .WithToDate(options.ToDate)
            .WithPrice(options.Price)
            .WithCurrency(options.Currency).Build();
        this._currencyRates.Add(currencyRate);
    }

    private void GuardAgainstOverlappingTheTimePeriod(DateTime fromDate, DateTime toDate)
    {
        if (this._currencyRates.Any(currencyRate => fromDate <= currencyRate.ToDate && currencyRate.FromDate <= toDate))
            throw new OverlapTimePeriodException();
    }

    public string Name { get; private set; }

    private List<ICurrencyRateOptions> _currencyRates = new();
    public IReadOnlyCollection<ICurrencyRateOptions> CurrencyRates => _currencyRates;

    public void Add(ICurrencyRateOptions options)
    {
        GuardAgainstOverlappingTheTimePeriod(options.FromDate, options.ToDate);

        this._currencyRates
            .Add(new CurrencyRate(this.Name, options.FromDate, options.ToDate, options.Price));
    }
}