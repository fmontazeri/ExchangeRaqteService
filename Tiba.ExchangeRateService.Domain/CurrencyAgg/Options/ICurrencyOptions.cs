namespace Tiba.ExchangeRateService.Domain.CurrencyAgg.Options;

public interface ICurrencyOptions
{ 
    string Symbol { get; }
    List<ICurrencyRateOptions> CurrencyRates { get; }
}