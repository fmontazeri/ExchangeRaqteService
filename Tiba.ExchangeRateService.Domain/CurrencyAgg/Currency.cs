using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Currency : ICurrencyOptions
{
    public string Symbol { get; private set; }

    private List<ICurrencyRateOptions> _currencyRates = new();
    public List<ICurrencyRateOptions> CurrencyRates => _currencyRates;

    public Currency(string currency, ICurrencyRateOptions currencyRateOptions)
    {
        this.Symbol = currency;
        AddCurrencyRate(currencyRateOptions);
    }

    public Currency(string currency, List<ICurrencyRateOptions> currencyRates)
    {
        this.Symbol = currency;
        // foreach (var currencyRate in currencyRates)
        // {
        //     AddCurrencyRate(currencyRate);
        // }

        if (IsThereOverlapBetweenTimePeriods(currencyRates.ToArray()))
            throw new OverlapTimePeriodException();

        foreach (var options in currencyRates)
        {
            var currencyRate = new CurrencyRateBuilder()
                .WithMoney(options.Money)
                .WithTimePeriod(options.TimePeriod)
                .Build();
            this._currencyRates.Add(currencyRate);
        }
    }

    private void AddCurrencyRate(ICurrencyRateOptions options)
    {
        var currencyRate = new CurrencyRateBuilder()
            .WithMoney(options.Money)
            .WithTimePeriod(options.TimePeriod)
            .Build();
        GuardAgainstOverlapTimePeriods(currencyRate);
        this._currencyRates.Add(currencyRate);
    }

    private void GuardAgainstOverlapTimePeriods(CurrencyRate currencyRate)
    {
        if (IsThereOverlapBetweenTimePeriods(currencyRate))
            throw new OverlapTimePeriodException();
    }

    private bool IsThereOverlapBetween(ITimePeriodOptions before, ITimePeriodOptions after)
    {
        return (after.FromDate ?? DateTime.MinValue) <= (before.ToDate ?? DateTime.MaxValue) &&
               (before.FromDate ?? DateTime.MinValue) <= (after.ToDate ?? DateTime.MaxValue);
    }

    private bool IsThereOverlapBetweenTimePeriods(CurrencyRate input)
    {
        var index = 0;
        while (index <= this._currencyRates.Count - 1)
        {
            var currencyRate = this._currencyRates[index];
            var overlapped = input.DoesItOverlapWith(currencyRate.TimePeriod);
            //var overlapped = IsThereOverlapBetween(input.TimePeriod, currencyRate.TimePeriod);
            if (overlapped) return true;
            index++;
        }

        return false;
    }


    private bool IsThereOverlapBetweenTimePeriods(params ICurrencyRateOptions[] options)
    {
        if (options.Length <= 1) return false;
        var index = 0;
        ICurrencyRateOptions currencyRate = options[index];
        var next = 0;
        foreach (var item in options)
        {
            while (options.Length - index > 1)
            {
                //var result = options[++index].TimePeriod.DoesOverlapWith(currencyRate.TimePeriod);
                var result = IsThereOverlapBetween(options[++index].TimePeriod, currencyRate.TimePeriod);
                if (result) return result;
                if (index >= options.Length - 1) break;
                currencyRate = options[index];
            }

            index = ++next;
            currencyRate = item;
        }

        return false;
    }

    public void Add(ITimePeriodOptions timePeriod, decimal price)
    {
        var currencyRate = new CurrencyRateBuilder()
            .WithTimePeriod(timePeriod)
            .WithMoney(Money.New(price, this.Symbol))
            .Build();
        GuardAgainstOverlapTimePeriods(currencyRate);
        this._currencyRates.Add(currencyRate);
    }
}