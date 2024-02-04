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
        foreach (var currencyRate in currencyRates)
        {
            AddCurrencyRate(currencyRate);
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
        return (!after.FromDate.HasValue || !before.ToDate.HasValue || after.FromDate <= before.ToDate) &&
               (!before.FromDate.HasValue || !after.ToDate.HasValue || before.FromDate <= after.ToDate);
    }

    private bool IsThereOverlapBetweenTimePeriods(ICurrencyRateOptions input)
    {
        var index = 0;
        while (index <= this._currencyRates.Count - 1)
        {
            var currencyRate = this._currencyRates[index];
            //ar overlapped = input.TimePeriod.DoesOverlapWith(currencyRate.TimePeriod);
            var overlapped = IsThereOverlapBetween(input.TimePeriod, currencyRate.TimePeriod);
            if (overlapped) return true;
            index++;
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