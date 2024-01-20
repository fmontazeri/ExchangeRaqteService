namespace Tiba.ExchangeRateService.Domain.ExchangeRates;

public class Currency
{
    public Currency(string currency, DateTime fromDate, DateTime toDate, decimal price)
    {
        this.Name = currency;
        var exchangeRate = new ExchangeRate(fromDate, toDate, price);
        this._exchangeRates.Add(exchangeRate);
        this.LastExchangeRate = exchangeRate;
    }

    public string Name { get; set; }

    private List<ExchangeRate> _exchangeRates = new ();
    public IReadOnlyCollection<IExchangeRate> ExchangeRates => _exchangeRates;

    public IExchangeRate? LastExchangeRate { get; private set; }

    public void Add(DateTime fromDate, DateTime toDate, decimal price)
    {
        this._exchangeRates.Add(new ExchangeRate(fromDate, toDate, price , LastExchangeRate?.ToDate));
    }
}