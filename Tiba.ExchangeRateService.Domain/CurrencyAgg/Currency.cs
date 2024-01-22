using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency
{
    public Currency(ICurrencyRateOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Money.Currency))
            throw new CurrencyIsNotDefinedException();

        GuardAgainstOverlappingTheTimePeriod(options.FromDate, options.ToDate);

        this.Name = options.Money.Currency;
        var currencyRateOptions = new CurrencyRateOptionsBuilder()
            .WithFromDate(options.FromDate)
            .WithToDate(options.ToDate)
            .WithMoney(options.Money).Build();
        this._currencyRates.Add(currencyRateOptions);
    }

    private void GuardAgainstOverlappingTheTimePeriod(DateTime fromDate, DateTime toDate)
    {
        if (this._currencyRates.Any(currencyRate => fromDate <= currencyRate.ToDate && currencyRate.FromDate <= toDate))
            throw new OverlapTimePeriodException();
    }

    public string Name { get; private set; }

    private List<ICurrencyRateOptions> _currencyRates = new();
    public IReadOnlyCollection<ICurrencyRateOptions> CurrencyRates => _currencyRates;

    public void Add(DateTime fromDate, DateTime toDate, decimal price)
    {
        GuardAgainstOverlappingTheTimePeriod(fromDate, toDate);
        
        var currencyRateOptions = new CurrencyRateOptionsBuilder()
            .WithFromDate(fromDate)
            .WithToDate(toDate)
            .WithMoney(new Money(price, this.Name))
            .Build();
        this._currencyRates.Add(currencyRateOptions);
    }
}