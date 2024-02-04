using FluentAssertions;
using Tiba.ExchangeRateService.Domain.CurrencyAgg;
using Tiba.ExchangeRateService.Domain.CurrencyAgg.Exceptions;
using Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests.Consts;

namespace Tiba.ExchangeRateService.Domain.Tests.Unit.CurrencyTests;

public class MoneyTests
{
    [Fact]
    public void Money_Should_Create_Successfully()
    {
        var actual = Money.New(CurrencyConsts.SOME_PRICE, CurrencyConsts.SOME_CURRENCY);

        actual.Amount.Should().Be(CurrencyConsts.SOME_PRICE);
        actual.Currency.Should().Be(CurrencyConsts.SOME_CURRENCY);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Money_Should_Not_Be_Created_When_Amount_Is_Out_Of_Range(decimal amount)
    {
        var exception = Assert.Throws<AmountIsNotValidException>(() =>
        {
            var actual = Money.New(amount, CurrencyConsts.SOME_CURRENCY);
        });

        exception.Message.Should().Be(AmountIsNotValidException.ErrorMessage);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Money_Should_Not_Be_Created_When_Currency_Is_Not_Defined(string currency)
    {
        var exception = Assert.Throws<CurrencyIsNotDefinedException>(() =>
        {
            var actual = Money.New(CurrencyConsts.SOME_PRICE, currency);
        });

        exception.Message.Should().Be(CurrencyIsNotDefinedException.ErrorMessage);
    }
}