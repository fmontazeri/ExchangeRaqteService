namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ICurrencyOptions
{ 
    string Symbol { get; }
    List<ICurrencyRateOptions> CurrencyRates { get; }
}