using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency : ICurrencyOptions
{
    public Currency(ICurrencyRateOptions options)
    {
        if (options.Money is null)
            throw new CurrencyIsNotDefinedException();
        GuardAgainstInvalidTimePeriod(options.TimePeriod);

        this.Symbol = options.Money.Currency;
        var currencyRateOptions = new CurrencyRateBuilder()
            .WithTimePeriod(options.TimePeriod)
            .WithMoney(options.Money)
            .Build();
        this._currencyRates.Add(currencyRateOptions);
    }

    public Currency(string currency , List<ICurrencyRateOptions> currencyRates)//(ICurrencyOptions options)
    {
        this.Symbol = currency;
        GuardAgainstOverlappingTimePeriods(currencyRates.ToArray());
        foreach (var currencyRate in currencyRates)
        {
            this._currencyRates.Add(currencyRate);
        }
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

    private void GuardAgainstOverlappingTimePeriods(params ICurrencyRateOptions[] options)
    {
        if (options.Length == 1) return;
        var index = 0;
        bool result;
        ICurrencyRateOptions currencyRate = options[index];
        while (!(result = options[++index].TimePeriod.DoesTheTimePeriodOverlapWith(currencyRate.TimePeriod)))
        {
            Console.WriteLine(index);
            currencyRate = options[index];
        }

        if (result) throw new OverlapTimePeriodException();
    }

    public string Symbol { get; private set; }

    private List<ICurrencyRateOptions> _currencyRates = new();
    public List<ICurrencyRateOptions> CurrencyRates => _currencyRates;

    public void Add(ITimePeriodOptions timePeriod, decimal price)
    {
        GuardAgainstInvalidTimePeriod(timePeriod);

        var currencyRateOptions = new CurrencyRateBuilder()
            .WithTimePeriod(timePeriod)
            .WithMoney(Money.New(price, this.Symbol))
            .Build();
        this._currencyRates.Add(currencyRateOptions);
    }
}