using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency : ICurrencyOptions
{
    public Currency(string currency, List<ICurrencyRateOptions> currencyRates)
    {
        this.Symbol = currency;
        if (IsThereOverlapBetweenTimePeriods(currencyRates.ToArray()))
            throw new OverlapTimePeriodException();
        foreach (var currencyRate in currencyRates)
        {
            this._currencyRates.Add(currencyRate);
        }
    }

    public Currency(string currency, ICurrencyRateOptions currencyRate)
    {
        this.Symbol = currency;
        if (IsThereOverlapBetweenTimePeriods(currencyRate))
            throw new OverlapTimePeriodException();
        this._currencyRates.Add(currencyRate);
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

    private bool IsThereOverlapBetweenTimePeriods(params ICurrencyRateOptions[] input)
    {
        var options = this._currencyRates.ToArray().Concat(input).ToArray();
        if (options.Length <= 1) return false;

        options = options.OrderBy(o => o.TimePeriod.FromDate).ToArray(); //remove sort because of performance
        var index = 0;
        ICurrencyRateOptions currencyRate = options[index];
        while (options.Length > index)
        {
            var result = options[++index].TimePeriod.DoesOverlapWith(currencyRate.TimePeriod);
            if (result || index >= options.Length - 1) return result;
            currencyRate = options[index];
        }

        return false;

        // foreach (var currencyRate in input)
        // {
        //     //This has a problem without sorting
        //     var overlapped = IsThereOverlapBetweenTimePeriods(source , currencyRate);
        //     if (overlapped) return true;
        // }
        // TODO: It's not necessary to add a list of currency rates in ctor - It's important to do this without sorting input
        //
        // return false;
    }

    private bool IsThereOverlapBetweenTimePeriods(ICurrencyRateOptions input)
    {
        var index = 0;
        while (index <= this._currencyRates.Count - 1)
        {
            var currencyRate = this._currencyRates[index];
            var overlapped = input.TimePeriod.DoesOverlapWith(currencyRate.TimePeriod);
            if (overlapped) return true;
            index++;
        }

        return false;
    }

    public string Symbol { get; private set; }

    private List<ICurrencyRateOptions> _currencyRates = new();
    public List<ICurrencyRateOptions> CurrencyRates => _currencyRates;

    public void Add(ITimePeriodOptions timePeriod, decimal price)
    {
        var currencyRateOptions = new CurrencyRateBuilder()
            .WithTimePeriod(timePeriod)
            .WithMoney(Money.New(price, this.Symbol))
            .Build();
        if (IsThereOverlapBetweenTimePeriods(currencyRateOptions))
            throw new OverlapTimePeriodException();
        this._currencyRates.Add(currencyRateOptions);
    }
}