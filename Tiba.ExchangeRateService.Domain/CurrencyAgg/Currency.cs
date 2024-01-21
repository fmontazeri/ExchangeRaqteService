namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency
{
    public Currency(string currency, DateTime fromDate, DateTime toDate, decimal price)
    {
        this.Name = currency;
        var exchangeRate = new ExchangeRate(fromDate, toDate, price);
        //We can add to exchange rates by event
        this._exchangeRates.Add(exchangeRate);
        this.CurrentExchangeRate = exchangeRate;
    }

    public string Name { get; private set; }

    private List<ExchangeRate> _exchangeRates = new ();
    public IReadOnlyCollection<IExchangeRate> ExchangeRates => _exchangeRates;
    public IExchangeRate? CurrentExchangeRate { get; private set; }

    public void Add(DateTime fromDate, DateTime toDate, decimal price)
    {
        this._exchangeRates.Add(new ExchangeRate(fromDate, toDate, price , CurrentExchangeRate?.ToDate));
    }
}