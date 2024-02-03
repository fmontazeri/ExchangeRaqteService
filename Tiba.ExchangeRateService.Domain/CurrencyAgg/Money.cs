using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;

namespace Tiba.ExchangeRateService.Domain.CurrencyAgg;

public class Money :IMoneyOptions, IEquatable<Money>
{
    public string Currency { get; private set; }
    public decimal Amount { get; private set; }

    protected Money(decimal amount, string currency)
    {
        if (amount <= 0)
            throw new AmountIsNotValidException();
        if (string.IsNullOrWhiteSpace(currency))
            throw new CurrencyIsNotDefinedException();
        this.Currency = currency;
        this.Amount = amount;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Different currencies cannot be added.");
        return new Money(Amount + other.Amount, Currency);
    }

    public static Money New(decimal amount, string currency)
    {
        return new Money(amount, currency);
    }
    public bool Equals(Money? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Currency == other.Currency && Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Money)obj);
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode() ^ Currency.GetHashCode(); //HashCode.Combine(Currency, Amount);
    }
}