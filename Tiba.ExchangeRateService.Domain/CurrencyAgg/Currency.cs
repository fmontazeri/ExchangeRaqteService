using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency
{
    public Currency(string name, DateTime fromDate, DateTime toDate, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CurrencyIsNotDefinedException();
        
        this.Name = name;
        var exchangeRate = new CurrencyRate(name, fromDate, toDate, price);
        //We can add to exchange rates by event
        this._currencyRates.Add(exchangeRate);
        this.LastCurrencyRate = exchangeRate;
    }

    public string Name { get; private set; }

    private List<CurrencyRate> _currencyRates = new();
    public IReadOnlyCollection<ICurrencyRate> CurrencyRates => _currencyRates;
    public ICurrencyRate? LastCurrencyRate { get; private set; }

    public void Add(DateTime fromDate, DateTime toDate, decimal price)
    {
        this._currencyRates.Add(new CurrencyRate(this.Name, fromDate, toDate, price, LastCurrencyRate?.ToDate));
    }
}