namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public interface ICurrencyRateOptions 
{
    public IMoneyOption Money { get; }
    public DateTime FromDate { get; }
    public DateTime ToDate { get; }
}