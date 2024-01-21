using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency
{
    public Currency(ICurrencyRateOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Currency))
            throw new CurrencyIsNotDefinedException();

        this.Name = options.Currency;
        var currencyRate = new CurrencyRate(options);
        this._currencyRates.Add(currencyRate);
        this.LastCurrencyRate = currencyRate;
    }

    public string Name { get; private set; }

    private List<CurrencyRate> _currencyRates = new();
    public IReadOnlyCollection<ICurrencyRateOptions> CurrencyRates => _currencyRates;
    public ICurrencyRateOptions? LastCurrencyRate { get; private set; }

    public void Add(DateTime fromDate, DateTime toDate, decimal price)
    {
        this._currencyRates.Add(new CurrencyRate(this.Name, fromDate, toDate, price, LastCurrencyRate?.ToDate));
    }
}